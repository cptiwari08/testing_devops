CREATE OR ALTER VIEW [dbo].[ViewAssistantQuestionAnswer] AS
select 
    l.id as ID
    ,x.instanceId as InstanceId
    ,x.chatId as ChatId
    ,x.messageId as QuestionId
    ,x.inputSources as InputSources
    ,x.question as Question
    ,y.content as Answer
    ,y.sourceName as OutputSourceName
    ,y.status as OutputStatus
    ,y.sqlQuery as OutputSqlQuery
    ,y.citingSources as OutputCitingSources

from AssistantChatHistory l

cross apply openjson(l.additionalinfo, '$')  with(   
	instanceId nvarchar(100) '$.instanceId',
    messageId int '$.input.messageId',
	chatId nvarchar(100) '$.input.chatId',
    question nvarchar(max) '$.input.question',
	inputSources nvarchar(max) '$.input.inputSources' as json
) x

cross apply openjson(l.additionalinfo, '$.output.response') with (
    sourceName      nvarchar(100),
    content      nvarchar(100),
	status      nvarchar(100),
    sqlQuery      nvarchar(max),
    citingSources nvarchar(max) as json
) y


