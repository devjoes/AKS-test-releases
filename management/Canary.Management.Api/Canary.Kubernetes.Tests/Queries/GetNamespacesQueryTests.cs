using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canary.Kubernetes.Queries;
using Canary.Kubernetes.Queries.Handlers;
using Moq;
using Xunit;

namespace Canary.Kubernetes.Tests.Queries
{
    public class GetNamespacesQueryTests
    {
        [Fact]
        public async Task Execute_InvokesHandler_WhenCalled()
        {
            var handler = new Mock<IGetNamespacesQueryHandler>();
            handler.Setup(h => h.GetNamespaces())
                .ReturnsAsync(() => new[] {"foo"})
                .Verifiable();
            var query = new GetNamespacesQuery(handler.Object);

            query.Validate();
            var ns = await query.Execute();

            Assert.Single(ns);
            Assert.Equal("foo",ns.Single());
            handler.Verify();
        }
    }
}
