using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canary.HaProxy;
using Canary.HaProxy.Models;
using Canary.Kubernetes;
using Canary.Kubernetes.Models;
using Canary.Management.Queries;
using Canary.Management.Queries.Handlers;
using Moq;
using Xunit;

namespace Canary.Management.Tests.Queries
{
    public class GetSitesQueryTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validate_Errors_WhenNamespaceMissing(string ns)
        {
            var query = new GetSitesQuery(null)
            {
                Namespace = ns
            };

            Assert.Throws<ArgumentException>(() => query.Validate());
        }

        [Fact]
        public async Task Execute_GetsIngresses_WhenCalled()
        {
            const string ns = "foo";
            var backends = new Dictionary<string, IEnumerable<Backend>>()
            {
                {"test.com", new[]
                {
                    new Backend{Path ="/"},
                    new Backend{Path = "/api"}
                }}
            };

            var kubeActions = new Mock<IKubernetesActions>();
            var haProxyActions = new Mock<IHaProxyActions>();

            kubeActions
                .Setup(k => k.GetIngressPathsInNamespace(ns))
                .ReturnsAsync(() => backends)
                .Verifiable();
            var query = new GetSitesQuery(new GetSitesQueryHandler(kubeActions.Object, haProxyActions.Object))
            {
                Namespace = ns
            };

            query.Validate();
            var sites = (await query.Execute()).ToList();

            Assert.Single(sites);
            var site = sites.Single();
            Assert.Equal(backends.Keys.Single(), site.Host);
            Assert.Collection(site.Routes, p => Assert.Equal(backends.Values.Single().First().Path, p.Url), p => Assert.Equal(backends.Values.Single().Last().Path, p.Url));
            Assert.Collection(site.Routes, p => Assert.Equal(backends.Values.Single().First().Path, p.Name), p => Assert.Equal(backends.Values.Single().Last().Path, p.Name));
            Assert.All(site.Routes, p => Assert.Empty(p.CookieMustContain));
        }


        [Fact]
        public async Task Execute_LooksUpConditionalRoutes_WhenCalled()
        {
            const string ns = "foo";
            var paths = new Dictionary<string, IEnumerable<Backend>>()
            {
                {
                    "test.com", new[]
                    {
                        new Backend {Path = "/" + Constants.ConditionalRouteSuffix + "_test"},
                        new Backend {Path = "/"},
                        new Backend {Path = "/api"},
                        new Backend {Path = "/api" + Constants.ConditionalRouteSuffix + "_test"}
                    }
                }
            };

            var kubeActions = new Mock<IKubernetesActions>();
            kubeActions
                .Setup(k => k.GetIngressPathsInNamespace(ns))
                .ReturnsAsync(() => paths)
                .Verifiable();
            var haProxyActions = new Mock<IHaProxyActions>();
            haProxyActions.Setup(h => h.GetConditionalRoutes())
                .ReturnsAsync(new[]
                {
                    new ConditionalRoute
                    {
                        Hosts = new[] {"test.com"}, Cookies = new[] {"foo"}, RouteAlias = "/"+Constants.ConditionalRouteSuffix+"_test",
                        Url = "/"
                    },
                    new ConditionalRoute
                    {
                        Hosts = new[] {"test.com"}, Cookies = new[] {"foo"}, RouteAlias = "/api"+Constants.ConditionalRouteSuffix+"_test",
                        Url = "/api"
                    }
                });

            var query = new GetSitesQuery(new GetSitesQueryHandler(kubeActions.Object, haProxyActions.Object))
            {
                Namespace = ns
            };

            query.Validate();
            var sites = (await query.Execute()).ToList();


            var site = sites.Single();
            var conditionalRoutes = site.Routes.Where(r => r.Name != r.Url).ToArray();
            var nonConditionalRoutes = site.Routes.Where(r => r.Name == r.Url).ToArray();
            Assert.Single(sites);
            Assert.Equal(paths.Keys.Single(), site.Host);
            Assert.Collection(conditionalRoutes, 
                p => Assert.Equal(paths.Values.Single().ElementAt(1).Path, p.Url), 
                p => Assert.Equal(paths.Values.Single().ElementAt(2).Path, p.Url));
            Assert.Collection(conditionalRoutes, 
                p => Assert.Equal(paths.Values.Single().First().Path, p.Name),
                p => Assert.Equal(paths.Values.Single().Last().Path, p.Name));
            Assert.All(conditionalRoutes, p => Assert.Equal("foo", p.CookieMustContain.Single()));
            Assert.All(nonConditionalRoutes, p => Assert.Empty(p.CookieMustContain));
        }
    }
}
