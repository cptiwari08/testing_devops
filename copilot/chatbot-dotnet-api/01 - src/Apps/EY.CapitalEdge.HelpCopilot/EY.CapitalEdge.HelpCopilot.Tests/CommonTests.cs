using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using EY.CapitalEdge.HelpCopilot.Utils;

namespace EY.CapitalEdge.HelpCopilot.Tests
{
    public class CommonTests
    {
        private readonly Mock<ILogger<Common>> _loggerMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Common _common;

        public CommonTests()
        {
            _loggerMock = new Mock<ILogger<Common>>();
            _configMock = new Mock<IConfiguration>();
            _common = new Common(_loggerMock.Object, _configMock.Object);
        }

        [Fact]
        public void ExtractValueFromContext_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var context = new Dictionary<string, object>
            {
                { "conversationReferenceId", "41ca88b7-f4cd-4bbf-822b-d77fed83ca6b" }
            };

            // Act
            string? result = _common.ExtractValueFromContext(context, "conversationReferenceId");

            // Assert
            Assert.Equal("41ca88b7-f4cd-4bbf-822b-d77fed83ca6b", result);
        }
    }
}