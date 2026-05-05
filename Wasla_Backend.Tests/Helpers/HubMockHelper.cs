using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Wasla_Backend.Tests.Helpers
{
    public static class HubMockHelper
    {
        public static Mock<IHubContext<THub>> CreateHubContextMock<THub>(
            out Mock<IClientProxy> clientProxyMock)
            where THub : Hub
        {
            clientProxyMock = new Mock<IClientProxy>();

            clientProxyMock
                .Setup(x => x.SendCoreAsync(
                    It.IsAny<string>(),
                    It.IsAny<object[]>(),
                    default))
                .Returns(Task.CompletedTask);

            var clientsMock = new Mock<IHubClients>();

            clientsMock
                .Setup(c => c.User(It.IsAny<string>()))
                .Returns(clientProxyMock.Object);

            clientsMock
                .Setup(c => c.Group(It.IsAny<string>()))
                .Returns(clientProxyMock.Object);

            clientsMock
                .Setup(c => c.All)
                .Returns(clientProxyMock.Object);

            var hubContextMock = new Mock<IHubContext<THub>>();

            hubContextMock
                .Setup(h => h.Clients)
                .Returns(clientsMock.Object);

            return hubContextMock;
        }
    }
}

