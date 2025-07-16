using EY.CapitalEdge.ChatOrchestrator.Models;
using EY.CapitalEdge.ChatOrchestrator.Utils;
using EY.CapitalEdge.ChatOrchestrator.Utils.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using Suggestion = EY.CapitalEdge.ChatOrchestrator.Utils.Dtos.Suggestion;

namespace EY.CapitalEdge.ChatOrchestrator.Tests
{
    public class CommonTests
    {
        [Fact]
        public void Deserialize_WhenJsonStringIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var common = new Common();

            // Act
            Action act = () => common.Deserialize<object>(null);

            // Assert
            ArgumentException exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Contains("The JSON string is empty or null.", exception.Message);
        }

        [Fact]
        public void Deserialize_WhenFailedDeserialization_ThrowsJsonException()
        {
            // Arrange
            var common = new Common();

            // Act
            Action act = () => common.Deserialize<object>("null");

            // Assert
            JsonException exception = Assert.Throws<JsonException>(act);
            Assert.Equal("The deserialization from JSON to the specified type has failed.", exception.Message);
        }

        [Fact]
        public void Deserialize_WhenSuccessDeserialization_ReturnObject()
        {
            // Arrange
            var common = new Common();

            // Act
            ProjectContextResponse act = common.Deserialize<ProjectContextResponse>("{\"id\":1,\"title\":\"\",\"key\":\"PROJECT_CONTEXT\",\"value\":\"Let's Test\",\"isEnabled\":false}");

            // Assert
            Assert.NotNull(act);
            Assert.Equal("PROJECT_CONTEXT", act.Key);
        }

        [Fact]
        public void SerializeToCamelCase_WhenSuccessSerialization_ReturnJsonString()
        {
            // Arrange
            var common = new Common();
            var projectContextResponse = new ProjectContextResponse
            {
                Id = 1,
                Title = "Project Context",
                Key = "PROJECT_CONTEXT",
                Value = "Let's Test",
                IsEnabled = false
            };

            // Act
            string act = common.SerializeToCamelCase(projectContextResponse);

            // Assert
            Assert.NotNull(act);
            Assert.Contains("title", act);
        }

        [Fact]
        public void ValidateToken_WhenMismatchedPublicKey_ReturnFalse()
        {
            // Arrange
            var common = new Common();
            var logger = new Mock<ILogger>();
            string token = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkRhbmllbC5SaXZlcmFAZXkuY29tIiwiZW1haWwiOiJEYW5pZWwuUml2ZXJhQGV5LmNvbSIsImZhbWlseV9uYW1lIjoiUml2ZXJhIiwiZ2l2ZW5fbmFtZSI6IkRhbmllbCIsInVzZXJfdHlwZSI6IkludGVybmFsIiwib2lkIjoiMWI4YjNjMmQtODI3NC00N2MxLTljMDctNTI0NTkxYzRiNzIxIiwiY2Vfb2lkIjoiMzMyYmU0NjYtYTEyZS00ZDNlLTk5YzEtZTY3ZTJkNzljOWRkIiwidXBuIjoiRGFuaWVsLlJpdmVyYUBleS5jb20iLCJzcF91cmwiOiJodHRwczovL2V5dXMuc2hhcmVwb2ludC5jb20vc2l0ZXMvY2U1LXByb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28tZGV2IiwicG9fYXBwX3VybCI6Imh0dHBzOi8vZXljYXBpdGFsZWRnZS1kZXYuZXkuY29tL3Byb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28vY2U0IiwicG9fYXBpX3VybCI6Imh0dHBzOi8vZXljYXBpdGFsZWRnZS1kZXYuZXkuY29tL3Byb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28tY2U0L2FwaSIsImNvcGlsb3RfYXBwX3VybCI6Imh0dHBzOi8vZXljYXBpdGFsZWRnZS1kZXYuZXkuY29tL3Byb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28vY29waWxvdCIsImNvcGlsb3RfYXBpX3VybCI6Imh0dHBzOi8vZXljYXBpdGFsZWRnZS1kZXYuZXkuY29tL3Byb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28vY29waWxvdC9hcGkiLCJkYXRhX2luZGV4X25hbWUiOiIiLCJkb2NzX2luZGV4X25hbWUiOiIiLCJnbG9zc19pbmRleF9uYW1lIjoiIiwicHJvamVjdF9pZCI6ImQwYjcxZjhlLWVhZDQtNGUxZC1iZDcwLTc2ODFjM2RkNDkxZCIsInByb2plY3RfZnJpZW5kbHlfaWQiOiJwcm9kdWN0ZGV2ZWxvcC1jZWRldnByamNvIiwic2NvcGUiOlsiZXktZ3VpZGFuY2UiLCJpbnRlcm5ldCIsInByb2plY3QtZG9jcyIsInByb2plY3QtZGF0YSJdLCJuYmYiOjE3MTU3MDE5MzcsImV4cCI6MTcxNTcwNTUzNywiaWF0IjoxNzE1NzAxOTM3LCJpc3MiOiJodHRwczovL2V5Y2FwaXRhbGVkZ2UtZGV2LmV5LmNvbSIsImF1ZCI6ImQwYjcxZjhlLWVhZDQtNGUxZC1iZDcwLTc2ODFjM2RkNDkxZCJ9.C3yWWjJC_ntLCkv2nLCyxmySmNsRWDWKe6YoDax_g8voV9x84mGQdn7UJOivJunU-LtCm8UR41IWihYZnjYIvx_A--gDGHP3GVwoztCVYCuVf4y3gtcbXDLlTjSVkkVLJl3st3NYtEZD7kYV1jMn5g7ACqk-EbEVFWVFqvB3YR2GND9VfwxSDzeNAIaFp9M2elDR9DwVb7lle3P7lE-0vxIGiKbLF7VsLKYFcZKl2w7ZHz0COdySGCUJCO5EV3b15hvUCKG-GAmG5oK484ZxL00hYCmLI0JC3U4NXAXocgF7k2NzcdOxfmBDxoYsNKYrGBFZ7BNyxLIiskIFdrvE9WX0_SNWPJeWN0wG4kdqS2NMZfIjq49bXPQ7XDq4R8Ud0i0aN0llQ6r2jKCKeb3viHj7diif_F22CWrJzI1eX6R1m6fC4VhiS40pNhhs-aXepeEWoe1bo0EwiSIkIKfYQexFGk6wvelzj3gzpRNFGfy9UpRGOmKY3AP9y4B73_3qLDUrbzhFIALuCDJ7MjAyOXRkJFTMlzBvuRDElbXTQfXAWmhtXyuZSFq6N2pZKZADGAmSlzHKNjkvphlOQ6Qiml-nI1PN23jBTKA5Q64cDQhgPTmGsvyRbkY7o7i1jEUNg72sca9qV5_GsSznMPgSEMuu-1jj0VysdB5nR4zpVAU";
            string issuer = "https://eycapitaledge-dev.ey.com";
            string mismatchedPublicKey = "MIIBCgKCAQEA+xGZ/wcz9ugFpP07Nspo6U17l0YhFiFpxxU4pTk3Lifz9R3zsIsu\r\nERwta7+fWIfxOo208ett/jhskiVodSEt3QBGh4XBipyWopKwZ93HHaDVZAALi/2A\r\n+xTBtWdEo7XGUujKDvC2/aZKukfjpOiUI8AhLAfjmlcD/UZ1QPh0mHsglRNCmpCw\r\nmwSXA9VNmhz+PiB+Dml4WWnKW/VHo2ujTXxq7+efMU4H2fny3Se3KYOsFPFGZ1TN\r\nQSYlFuShWrHPtiLmUdPoP6CV2mML1tk+l7DIIqXrQhLUKDACeM5roMx0kLhUWB8P\r\n+0uj1CNlNN4JRZlC7xFfqiMbFRU9Z4N6YwIDAQAB";

            // Act
            bool act = common.ValidateToken(logger.Object, token, issuer, mismatchedPublicKey);

            // Assert
            Assert.False(act);
        }

        [Fact]
        public void GetTokenData_WhenTokenIsValid_ReturnToken()
        {
            // Arrange
            var common = new Common();
            var logger = new Mock<ILogger>();
            string token = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkRhbmllbC5SaXZlcmFAZXkuY29tIiwiZW1haWwiOiJEYW5pZWwuUml2ZXJhQGV5LmNvbSIsImZhbWlseV9uYW1lIjoiUml2ZXJhIiwiZ2l2ZW5fbmFtZSI6IkRhbmllbCIsInVzZXJfdHlwZSI6IkludGVybmFsIiwib2lkIjoiMWI4YjNjMmQtODI3NC00N2MxLTljMDctNTI0NTkxYzRiNzIxIiwiY2Vfb2lkIjoiMzMyYmU0NjYtYTEyZS00ZDNlLTk5YzEtZTY3ZTJkNzljOWRkIiwidXBuIjoiRGFuaWVsLlJpdmVyYUBleS5jb20iLCJzcF91cmwiOiJodHRwczovL2V5dXMuc2hhcmVwb2ludC5jb20vc2l0ZXMvY2U1LXByb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28tZGV2IiwicG9fYXBwX3VybCI6Imh0dHBzOi8vZXljYXBpdGFsZWRnZS1kZXYuZXkuY29tL3Byb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28vY2U0IiwicG9fYXBpX3VybCI6Imh0dHBzOi8vZXljYXBpdGFsZWRnZS1kZXYuZXkuY29tL3Byb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28tY2U0L2FwaSIsImNvcGlsb3RfYXBwX3VybCI6Imh0dHBzOi8vZXljYXBpdGFsZWRnZS1kZXYuZXkuY29tL3Byb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28vY29waWxvdCIsImNvcGlsb3RfYXBpX3VybCI6Imh0dHBzOi8vZXljYXBpdGFsZWRnZS1kZXYuZXkuY29tL3Byb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28vY29waWxvdC9hcGkiLCJkYXRhX2luZGV4X25hbWUiOiIiLCJkb2NzX2luZGV4X25hbWUiOiIiLCJnbG9zc19pbmRleF9uYW1lIjoiIiwicHJvamVjdF9pZCI6ImQwYjcxZjhlLWVhZDQtNGUxZC1iZDcwLTc2ODFjM2RkNDkxZCIsInByb2plY3RfZnJpZW5kbHlfaWQiOiJwcm9kdWN0ZGV2ZWxvcC1jZWRldnByamNvIiwic2NvcGUiOlsiZXktZ3VpZGFuY2UiLCJpbnRlcm5ldCIsInByb2plY3QtZG9jcyIsInByb2plY3QtZGF0YSJdLCJuYmYiOjE3MTU3MDE5MzcsImV4cCI6MTcxNTcwNTUzNywiaWF0IjoxNzE1NzAxOTM3LCJpc3MiOiJodHRwczovL2V5Y2FwaXRhbGVkZ2UtZGV2LmV5LmNvbSIsImF1ZCI6ImQwYjcxZjhlLWVhZDQtNGUxZC1iZDcwLTc2ODFjM2RkNDkxZCJ9.C3yWWjJC_ntLCkv2nLCyxmySmNsRWDWKe6YoDax_g8voV9x84mGQdn7UJOivJunU-LtCm8UR41IWihYZnjYIvx_A--gDGHP3GVwoztCVYCuVf4y3gtcbXDLlTjSVkkVLJl3st3NYtEZD7kYV1jMn5g7ACqk-EbEVFWVFqvB3YR2GND9VfwxSDzeNAIaFp9M2elDR9DwVb7lle3P7lE-0vxIGiKbLF7VsLKYFcZKl2w7ZHz0COdySGCUJCO5EV3b15hvUCKG-GAmG5oK484ZxL00hYCmLI0JC3U4NXAXocgF7k2NzcdOxfmBDxoYsNKYrGBFZ7BNyxLIiskIFdrvE9WX0_SNWPJeWN0wG4kdqS2NMZfIjq49bXPQ7XDq4R8Ud0i0aN0llQ6r2jKCKeb3viHj7diif_F22CWrJzI1eX6R1m6fC4VhiS40pNhhs-aXepeEWoe1bo0EwiSIkIKfYQexFGk6wvelzj3gzpRNFGfy9UpRGOmKY3AP9y4B73_3qLDUrbzhFIALuCDJ7MjAyOXRkJFTMlzBvuRDElbXTQfXAWmhtXyuZSFq6N2pZKZADGAmSlzHKNjkvphlOQ6Qiml-nI1PN23jBTKA5Q64cDQhgPTmGsvyRbkY7o7i1jEUNg72sca9qV5_GsSznMPgSEMuu-1jj0VysdB5nR4zpVAU";

            // Act
            Token act = common.GetTokenData(logger.Object, token);

            // Assert
            Assert.NotNull(act);
            Assert.Equal("https://eycapitaledge-dev.ey.com", act.Iss);
        }

        [Fact]
        public void GetTokenData_WhenIsInvalid_ThrowsException()
        {
            // Arrange
            var common = new Common();
            var logger = new Mock<ILogger>();
            string token = "Bearer xyz";

            // Act
            Token act () => common.GetTokenData(logger.Object, token);

            // Assert
            CommonException exception = Assert.Throws<CommonException>(act);
            Assert.Contains("Error getting token data", exception.Message);
        }

        [Fact]
        public void IsThereAnySuggestedQuestion_WhenContextIsNull_ReturnFalse()
        {
            // Arrange
            var common = new Common();

            // Act
            bool act = common.IsThereAnySuggestedQuestion(null);

            // Assert
            Assert.False(act);
        }

        [Fact]
        public void IsThereAnySuggestedQuestion_WhenNoSuggestion_ReturnFalse()
        {
            // Arrange
            var common = new Common();
            string requestBody = "{\"messageId\":1,\"chatId\":\"c0fa8cbf-daeb-4539-ae0e-7220bb92a498\",\"question\":\"Which project team has the most overdue risks?\",\"sources\":[\"project-data\"],\"context\":{}}";

            ChatQuestionInput? chatQuestionInput = JsonSerializer.Deserialize<ChatQuestionInput>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act
            bool act = common.IsThereAnySuggestedQuestion(chatQuestionInput?.Context);

            // Assert
            Assert.False(act);
        }

        [Fact]
        public void IsThereAnySuggestedQuestion_WhenValidSuggestion_ReturnTrue()
        {
            // Arrange
            var common = new Common();
            string requestBody = "{\"messageId\":1,\"chatId\":\"c0fa8cbf-daeb-4539-ae0e-7220bb92a498\",\"question\":\"Which project team has the most overdue risks?\",\"sources\":[\"project-data\"],\"context\":{\"suggestion\":{\"id\":\"11\",\"sqlQuery\":\"select * from workplan\",\"source\":\"project-data\"}}}";

            ChatQuestionInput? chatQuestionInput = JsonSerializer.Deserialize<ChatQuestionInput>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act
            bool act = common.IsThereAnySuggestedQuestion(chatQuestionInput?.Context);

            // Assert
            Assert.True(act);
        }

        [Fact]
        public void GetSuggestedQuestion_WhenContextIsNull_ReturnEmptySuggestion()
        {
            // Arrange
            var common = new Common();

            // Act
            Suggestion act = common.GetSuggestedQuestion(null);

            // Assert
            Assert.NotNull(act);
            Assert.Null(act.Source);
        }

        [Fact]
        public void GetSuggestedQuestion_WhenInvalidSuggestion_ReturnEmptySuggestion()
        {
            // Arrange
            var common = new Common();
            string requestBody = "{\"messageId\":1,\"chatId\":\"c0fa8cbf-daeb-4539-ae0e-7220bb92a498\",\"question\":\"Which project team has the most overdue risks?\",\"sources\":[\"project-data\"],\"context\":{}}";

            ChatQuestionInput? chatQuestionInput = JsonSerializer.Deserialize<ChatQuestionInput>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act
            Suggestion act = common.GetSuggestedQuestion(chatQuestionInput?.Context);

            // Assert
            Assert.NotNull(act);
            Assert.Null(act.Id);
        }

        [Fact]
        public void GetSuggestedQuestion_WhenValidSuggestion_ReturnSuggestion()
        {
            // Arrange
            var common = new Common();
            string requestBody = "{\"messageId\":1,\"chatId\":\"c0fa8cbf-daeb-4539-ae0e-7220bb92a498\",\"question\":\"Which project team has the most overdue risks?\",\"sources\":[\"project-data\"],\"context\":{\"suggestion\":{\"id\":\"11\",\"sqlQuery\":\"select * from workplan\",\"source\":\"project-data\"}}}";

            ChatQuestionInput? chatQuestionInput = JsonSerializer.Deserialize<ChatQuestionInput>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act
            Suggestion act = common.GetSuggestedQuestion(chatQuestionInput?.Context);

            // Assert
            Assert.NotNull(act);
            Assert.Equal("11", act.Id);
        }

        [Theory]
        [InlineData("http://example.com", true)]
        [InlineData("https://example.com", true)]
        [InlineData("ftp://example.com", false)]
        [InlineData("example", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsValidUrl_ReturnsExpectedResult(string url, bool expectedResult)
        {
            var common = new Common();

            // Act
            bool result = common.IsValidUrl(url);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("null", false)]
        [InlineData("{}", false)]
        [InlineData("{\"suggestion\":null}", false)]
        [InlineData("{\"appInfo\":{}}", false)]
        [InlineData("{\"appInfo\":{\"name\":\"PMO\",\"teamTypeIds\":[]}}", false)]
        [InlineData("{\"appInfo\":{\"key\":null,\"name\":\"PMO\",\"teamTypeIds\":null}}", false)]
        [InlineData("{\"appInfo\":{\"key\": \"CE4-PMO\",\"name\":\"PMO\",\"teamTypeIds\":[100001,100002]}}", true)]
        [InlineData("{\"appInfo\":{\"key\": \"CE4-PMO\",\"name\":null,\"teamTypeIds\":[100001]}}", true)]
        public void IsThereAnyAppInfo_ReturnsExpectedResult(string? context, bool expectedResult)
        {
            // Arrange
            var common = new Common();
            string requestBody = "{\"messageId\":1,\"chatId\":\"74a5431b-a514-4d70-8d1e-a23a810d664a\",\"question\":\"How many milestones does each team have?\",\"sources\":[\"project-data\"],\"context\":"+context+"}";
            ChatQuestionInput? chatQuestionInput = JsonSerializer.Deserialize<ChatQuestionInput>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act
            bool result = common.IsThereAnyAppInfo(chatQuestionInput?.Context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetAppInfo_WhenContextIsNull_ReturnEmptyAppInfo()
        {
            // Arrange
            var common = new Common();

            // Act
            AppInfo act = common.GetAppInfo(null);

            // Assert
            Assert.NotNull(act);
            Assert.Null(act.Key);
        }

        [Fact]
        public void GetAppInfo_WhenInvalid_ReturnEmptyAppInfo()
        {
            // Arrange
            var common = new Common();
            string requestBody = "{\"messageId\":1,\"chatId\":\"c0fa8cbf-daeb-4539-ae0e-7220bb92a498\",\"question\":\"Which project team has the most overdue risks?\",\"sources\":[\"project-data\"],\"context\":{}}";

            ChatQuestionInput? chatQuestionInput = JsonSerializer.Deserialize<ChatQuestionInput>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act
            AppInfo act = common.GetAppInfo(chatQuestionInput?.Context);

            // Assert
            Assert.NotNull(act);
            Assert.Null(act.Key);
        }

        [Fact]
        public void GetAppInfo_WhenValid_ReturnAppInfo()
        {
            // Arrange
            var common = new Common();
            string requestBody = "{\"messageId\":1,\"chatId\":\"c0fa8cbf-daeb-4539-ae0e-7220bb92a498\",\"question\":\"Which project team has the most overdue risks?\",\"sources\":[\"project-data\"],\"context\":{\"appInfo\":{\"key\":\"CE4-PMO\",\"name\":\"PMO\",\"teamTypeIds\":[1]}}}";

            ChatQuestionInput? chatQuestionInput = JsonSerializer.Deserialize<ChatQuestionInput>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act
            AppInfo act = common.GetAppInfo(chatQuestionInput?.Context);

            // Assert
            Assert.NotNull(act);
            Assert.Equal("CE4-PMO", act.Key);
        }

        [Theory]
        [InlineData("null", false)]
        [InlineData("{}", false)]
        [InlineData("{\"suggestion\":null}", false)]
        [InlineData("{\"activePOApps\":[]}", false)]
        [InlineData("{\"activePOApps\":null}", false)]
        [InlineData("{\"activePOApps\":[{\"key\":\"\",\"name\":\"PMO\"}]}", false)]
        [InlineData("{\"activePOApps\":[{\"key\":\"CE4-PMO\",\"name\":\"PMO\"}]}", true)]
        [InlineData("{\"activePOApps\":[{\"key\":\"CE4-PMO\",\"name\":\"PMO\"},{\"key\":\"CE4-VC\",\"name\":\"Value Capture\"}]}", true)]
        [InlineData("{\"activePOApps\":[{\"key\":null,\"name\":\"PMO\"},{\"key\":\"CE4-VC\",\"name\":\"Value Capture\"}]}", true)]
        public void IsThereActivePOApps_ReturnsExpectedResult(string? context, bool expectedResult)
        {
            // Arrange
            var common = new Common();
            string requestBody = "{\"messageId\":1,\"chatId\":\"74a5431b-a514-4d70-8d1e-a23a810d664a\",\"question\":\"How many milestones does each team have?\",\"sources\":[\"project-data\"],\"context\":" + context + "}";
            ChatQuestionInput? chatQuestionInput = JsonSerializer.Deserialize<ChatQuestionInput>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act
            bool result = common.IsThereAnyActivePOApps(chatQuestionInput?.Context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetActivePOApps_WhenContextIsNull_ReturnEmptyAppInfo()
        {
            // Arrange
            var common = new Common();

            // Act
            List<ActivePOApps> act = common.GetActivePOApps(null);

            // Assert
            Assert.NotNull(act);
            Assert.Empty(act);
        }

        [Fact]
        public void GetActivePOApps_WhenInvalid_ReturnEmptyAppInfo()
        {
            // Arrange
            var common = new Common();
            string requestBody = "{\"messageId\":1,\"chatId\":\"c0fa8cbf-daeb-4539-ae0e-7220bb92a498\",\"question\":\"Which project team has the most overdue risks?\",\"sources\":[\"project-data\"],\"context\":{}}";

            ChatQuestionInput? chatQuestionInput = JsonSerializer.Deserialize<ChatQuestionInput>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act
            List<ActivePOApps> act = common.GetActivePOApps(chatQuestionInput?.Context);

            // Assert
            Assert.NotNull(act);
            Assert.Empty(act);
        }

        [Fact]
        public void GetActivePOApps_WhenValid_ReturnAppInfo()
        {
            // Arrange
            var common = new Common();
            string requestBody = "{\"messageId\":1,\"chatId\":\"c0fa8cbf-daeb-4539-ae0e-7220bb92a498\",\"question\":\"Which project team has the most overdue risks?\",\"sources\":[\"project-data\"],\"context\":{\"activePOApps\":[{\"key\":\"CE4-PMO\",\"name\":\"PMO\"}]}}";

            ChatQuestionInput? chatQuestionInput = JsonSerializer.Deserialize<ChatQuestionInput>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Act
            List<ActivePOApps> act = common.GetActivePOApps(chatQuestionInput?.Context);

            // Assert
            Assert.NotNull(act);
            Assert.Equal("CE4-PMO", act[0].Key);
        }
    }
}
