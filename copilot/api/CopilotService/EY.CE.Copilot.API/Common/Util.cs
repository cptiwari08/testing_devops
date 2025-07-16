using EY.CE.Copilot.API.Extensions;
using EY.CE.Copilot.API.Static;
using Markdig;
using Newtonsoft.Json.Linq;

namespace EY.CE.Copilot.API.Common
{
    public class Util
    {
        /// <summary>
        /// Converts the given base64 image to a circular image.
        /// </summary>
        /// <param name="base64String">The base64 string representation of the image.</param>
        /// <returns>The base64 string representation of the circular image.</returns>
        public static string ConvertToCircle(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using var inputImage = SkiaSharp.SKBitmap.Decode(imageBytes);
            int size = Math.Min(inputImage.Width, inputImage.Height);

            using var circleBitmap = new SkiaSharp.SKBitmap(size, size);
            using var canvas = new SkiaSharp.SKCanvas(circleBitmap);
            canvas.Clear(SkiaSharp.SKColors.White);

            using var paint = new SkiaSharp.SKPaint
            {
                IsAntialias = true,
                FilterQuality = SkiaSharp.SKFilterQuality.High
            };

            using var path = new SkiaSharp.SKPath();
            path.AddOval(new SkiaSharp.SKRect(0, 0, size, size));
            canvas.ClipPath(path);

            int xOffset = (size - inputImage.Width) / 2;
            int yOffset = (size - inputImage.Height) / 2;
            canvas.DrawBitmap(inputImage, xOffset, yOffset, paint);

            using var image = SkiaSharp.SKImage.FromBitmap(circleBitmap);
            using var data = image.Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
            return Convert.ToBase64String(data.ToArray());
        }

        public static string? GetDocumentName(string sourceName, object source)
        {
            var documentName = string.Empty;
            var documentId = string.Empty;
            if (source != null)
            {
                if (source is string)
                {
                    documentName = source.ToString();
                }
                else if (source is JToken)
                {
                    documentName = ((JToken)source)["documentName"]?.ToString();
                    documentId = ((JToken)source)["documentId"]?.ToString();
                }

                if (sourceName == Constants.Chats.ProjectDocsSource)
                    documentName = documentName.Replace($"_{documentId}", "");
            }

            return documentName;
        }

        public static string ConvertMarkupToHtml(string markup)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var result = Markdown.ToHtml(markup, pipeline);
            result = ReplaceNewLine(result,HTMLTag.tableStart, HTMLTag.tableEnd);
            result = ReplaceNewLine(result, HTMLTag.olStart, HTMLTag.olEnd);
            result = ReplaceNewLine(result, HTMLTag.ulStart, HTMLTag.ulEnd);
            result = AddClassToTag(HTMLTag.tableStart, result, "contentTable");
            return result.ReplaceLast("<p>", "").ReplaceLast("</p>", "");
        }

        public static string ReplaceNewLine(string text, string startTag, string endTag)
        {
            int start = text.IndexOf(startTag);
            int end = text.IndexOf(endTag) + endTag.Length;
            if (start < 0) return text;
            string final = text.Substring(0, start) + text.Substring(start, end - start).Replace("\n", "") + text.Substring(end, text.Length - end);
            return final;
        }

        static string AddClassToTag(string tag,string text, string className)
        {
            int start = text.IndexOf(tag);
            int end = start + tag.Length;
            if (start < 0) return text;
            string final = text.Substring(0, start) + AddClassName(text.Substring(start, tag.Length), className) + text.Substring(end, text.Length - end);
            return final;
        }

        static string AddClassName(string input, string className)
        {
            return input.Substring(0, input.Length - 1) + " class=\"" + className + "\" >";
        }
    }

    public static class HTMLTag
    {
        public const string tableStart = "<table>";
        public const string tableEnd = "</table>";
        public const string olStart = "<ol>";
        public const string olEnd = "</ol>";
        public const string ulStart = "<ul>";
        public const string ulEnd = "</ul>";
    }
}
