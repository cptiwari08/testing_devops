using EY.CE.Copilot.API.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantAPITest.DependencyMockups
{
    internal class SessionClientMockup
    {
        internal Mock<ISessionClient> sessionClient;
        public SessionClientMockup()
        {
            sessionClient = new Mock<ISessionClient>();
            sessionClient.Setup(Setup => Setup.GetRedisCache(It.IsAny<string>())).ReturnsAsync("[{\"messageId\":1,\"role\":\"user\",\"content\":\"How do you add a new field to the workplan?\",\"sources\":[\"ey-guidance\",\"internet\"],\"inputSources\":[\"ey-guidance\",\"internet\"],\"context\":{\"suggestion\":{\"id\":\"133\",\"sqlQuery\":null,\"source\":\"ey-guidance\"},\"documents\":null},\"createdTime\":\"2024-06-25T06:14:49.8646372Z\",\"response\":null,\"lastUpdatedTime\":\"2024-06-25T06:14:49.8646378Z\",\"Status\":null},{\"messageId\":2,\"role\":\"assistant\",\"content\":\"<b>Summary</b>\\n\\\"To add a new field to the WorkPlan, modify the WorkPlan table structure by adding a new column, define its name and data type, then update the table schema to include this column.\\\"\\n\\n<b>EY Guidance</b>\\n\\\"To add a new field to the WorkPlan, modify the structure of the WorkPlan table by adding a new column. Define the new column's name and data type, and then update the table schema to include this new column.\\\"\\n\\n\",\"sources\":[\"ey-guidance\"],\"inputSources\":[\"ey-guidance\",\"internet\"],\"context\":{\"suggestion\":{\"id\":\"133\",\"sqlQuery\":null,\"source\":\"ey-guidance\"},\"documents\":null},\"createdTime\":\"2024-06-25T06:14:50.3852815Z\",\"response\":[{\"sourceName\":\"summary\",\"content\":\"\\\"To add a new field to the WorkPlan, modify the WorkPlan table structure by adding a new column, define its name and data type, then update the table schema to include this column.\\\"\",\"status\":\"200\",\"sqlQuery\":null,\"citingSources\":null},{\"sourceName\":\"ey-guidance\",\"content\":\"\\\"To add a new field to the WorkPlan, modify the structure of the WorkPlan table by adding a new column. Define the new column's name and data type, and then update the table schema to include this new column.\\\"\",\"status\":\"200\",\"sqlQuery\":null,\"citingSources\":null},{\"sourceName\":\"ey-guidance-help-copilot\",\"content\":\"The contexts provided do not contain specific information on how to add a new field to the workplan.\",\"status\":\"200\",\"sqlQuery\":null,\"citingSources\":[{\"sourceName\":\"ey-guidance-help-copilot\",\"sourceType\":\"documents\",\"sourceValue\":[]}]},{\"sourceName\":\"ey-guidance-ey-ip\",\"content\":\"To add a new field to the WorkPlan, you would need to modify the structure of the WorkPlan table by adding a new column. This process typically involves defining the new column's name and data type, and then updating the table schema to include this new column.\",\"status\":\"200\",\"sqlQuery\":null,\"citingSources\":[]}],\"lastUpdatedTime\":\"2024-06-25T06:15:29.5814289Z\",\"Status\":\"pending\"}]");
            sessionClient.Setup(Setup => Setup.SetRedisCache(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("");
        }
    }
}
