namespace EY.CE.Copilot.API.Models
{
    public class FileStreamData
    {
        public string Name { get; }

        public string ContentType { get; }

        public Stream Stream { get; }

        public FileStreamData(string contentType, Stream stream, string name)
        {
            ContentType = contentType;

            Stream = stream;

            Name = name;
        }
    }
}
