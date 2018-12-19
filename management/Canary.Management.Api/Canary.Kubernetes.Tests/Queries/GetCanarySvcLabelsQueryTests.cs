using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Canary.Kubernetes.Queries;
using Canary.Kubernetes.Queries.Handlers;
using Moq;
using Xunit;

namespace Canary.Kubernetes.Tests.Queries
{
    public class GetCanarySvcLabelsQueryTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validate_Fails_WhenNamespaceIsMissing(string k8SNamespace)
        {
            var query = new GetCanarySvcLabelsQuery(null)
            {
                Namespace = k8SNamespace
            };
            Assert.Throws<ArgumentException>(() => query.Validate());
        }

        [Fact]
        public async Task Execute_PassesArgsToHandler_WhenCalled()
        {
            const string ns = "foo";
            var handler = new Mock<IGetSvcLabelsQueryHandler>();
            handler.Setup(h => h.GetSvcLabels(ns))
                .ReturnsAsync(() => new Dictionary<string, IDictionary<string, string>>())
                .Verifiable();

            var query = new GetCanarySvcLabelsQuery(handler.Object)
            {
                Namespace = ns
            };
            await query.Execute();

            handler.Verify();
        }

        [Fact]
        public async Task Execute_FiltersNonCanaryLabels_WhenCalled()
        {
            const string ns = "foo";
            const string canarySvc = "has-canary-route";
            const string canaryLabel = "canary-foo";
            const string canaryValue = "bar";

            var handler = new Mock<IGetSvcLabelsQueryHandler>();
            handler.Setup(h => h.GetSvcLabels(ns))
                .ReturnsAsync(() =>
                {
                    var labels = new Dictionary<string, IDictionary<string, string>>
                    {
                        {canarySvc, new Dictionary<string, string>() {{"foo", "bar"}, {canaryLabel, canaryValue}}},
                        {"blah", new Dictionary<string, string>() {{"foo", "bar"}}}
                    };
                    return labels;
                })
                .Verifiable();

            var query = new GetCanarySvcLabelsQuery(handler.Object)
            {
                Namespace = ns
            };
            var result = await query.Execute();

            handler.Verify();
            Assert.Single(result);
            Assert.Single(result[canarySvc]);
            Assert.Equal(canaryValue, result[canarySvc][canaryLabel]);
        }
    }
}
