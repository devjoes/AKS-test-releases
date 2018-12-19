using Canary.Kubernetes.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Canary.Kubernetes.Models;
using Canary.Kubernetes.Queries.Handlers;
using Moq;
using Xunit;

namespace Canary.Kubernetes.Tests.Queries
{
    public class GetIngressPathsInNamespaceQueryTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validate_Fails_WhenNamespaceIsMissing(string k8SNamespace)
        {
            var query = new GetIngressPathsInNamespaceQuery(null)
            {
                Namespace = k8SNamespace
            };
            Assert.Throws<ArgumentException>(() => query.Validate());
        }
        
        [Fact]
        public async Task Execute_PassesArgsToHandler_WhenCalled()
        {
            const string ns = "foo";
            var handler = new Mock<IGetIngressPathsInNamespaceQueryHandler>();
            handler.Setup(h => h.GetIngressPathsInNamespace(ns))
                .ReturnsAsync(() => 
                    new Dictionary<string, IEnumerable<Backend>>(){
                        {"host", new []{new Backend{Path = "/"}} }})
                .Verifiable();

            var query = new GetIngressPathsInNamespaceQuery(handler.Object)
            {
                Namespace = ns
            };
            var config = await query.Execute();

            handler.Verify();
            Assert.NotEmpty(config);
        }
    }
}
