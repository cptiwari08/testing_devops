using EY.CE.Copilot.API.Static;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;

namespace EY.CE.Copilot.API.Middleware
{
    public class AntiXSS
    {
        private readonly RequestDelegate _next;

        public AntiXSS(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, HtmlEncoder htmlEncoder)
        {
            // enable buffering so that the request can be read by the model binders next
            httpContext.Request.EnableBuffering();

            // leaveOpen: true to leave the stream open after disposing,
            // so it can be read by the model binders
            using (var streamReader = new StreamReader
                  (httpContext.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                var raw = await streamReader.ReadToEndAsync();
                if (!string.IsNullOrWhiteSpace(raw))
                {
                    string input = excludeProperties(raw, httpContext.Request.Path, httpContext.Request.Method);
                    var sanitised = removeXSSCharacters(input);
                    if (input != sanitised)
                    {
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        httpContext.Response.Headers[HeaderNames.ContentType] = Constants.ContentType.ApplicationJson;
                        await httpContext.Response.WriteAsJsonAsync(new
                        {
                            error_code = "XSS_CONTENT_DETECTED",
                            message = "XSS Content not allowed"
                        });
                        return;
                    }
                }
            }

            // rewind the stream for the next middleware
            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            await _next.Invoke(httpContext);
        }

        string removeXSSCharacters(string input)
        {
            List<string> specialChar = new List<string> { "<", ">", "alert", "onload" };
            foreach (string s in specialChar) {
                input = input.Replace(s,string.Empty);
            }
            return input;
        }
        string excludeProperties(string input, string url, string type)
        {
            if ((url.ToLower() == "/suggestions/update" || url.ToLower() == "/suggestions") && type.ToLower() == "post")
            {
                JArray jObj =JArray.Parse(input);
                foreach (JObject item in jObj)
                {
                    if (item.ContainsKey("AnswerSQL"))
                        item.Remove("AnswerSQL");
                    if (item.ContainsKey("answerSQL"))
                        item.Remove("answerSQL");
                }
                return jObj.ToString();
            }else if((url.ToLower() == "/configuration/update" || url.ToLower() == "/configuration") && type.ToLower() == "post")
            {
                JObject jobj = JObject.Parse(input);
                if (jobj.ContainsKey("Key") && jobj.ContainsKey("Value") && jobj.SelectToken("Key").Value<string>() == "PROJECT_CONTEXT")
                    jobj.Remove("Value");
                return jobj.ToString();
            }
            else if(url.ToLower() == "/chat" && type.ToLower() == "post")
            {
                JObject jobj = JObject.Parse(input);
                if (jobj.ContainsKey("question"))
                    jobj.Remove("question");
                return jobj.ToString();
            }
            else if (url.ToLower() == "/chat/feedback" && type.ToLower() == "post")
            {
                JObject jobj = JObject.Parse(input);
                if (jobj.ContainsKey("FeedbackText"))
                    jobj.Remove("FeedbackText");
                return jobj.ToString();
            }
            else if (url.ToLower() == "/chat/copilot-feedback" && type.ToLower() == "post")
            {
                JObject jobj = JObject.Parse(input);
                if (jobj.ContainsKey("FeedbackText"))
                    jobj.Remove("FeedbackText");
                return jobj.ToString();
            }
            return input;
        }
    }
}
