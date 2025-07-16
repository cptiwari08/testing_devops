using EY.CE.Copilot.API.Contracts;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantAPITest.DependencyMockups
{
    internal class AppLoggerServiceMockup
    {
        internal Mock<IAppLoggerService> appinsightMock;
        public AppLoggerServiceMockup()
        {
            appinsightMock = new Mock<IAppLoggerService>();
            appinsightMock.Setup(a => a.Log(It.IsAny<AppLogLevel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<Exception>(), It.IsAny<string>())).Callback((AppLogLevel logLevel, string className, string message, string method,
                                            Exception ex, string error) =>
                { Console.WriteLine("Testing : " + message); });
        }
    }
}
