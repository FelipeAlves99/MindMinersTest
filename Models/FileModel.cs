using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace MindMinersTest.Models
{
    [DataContract]
    public class FileModel
    {
        public FileModel(IFormFile srtFile, string offset)
        {
            SrtFile = srtFile;
            Offset = offset;       

            Validate();     
        }

        private void Validate()
        {
            throw new NotImplementedException();
        }

        [DataMember]
        public IFormFile SrtFile { get; private set; }
        [DataMember]
        public string Offset { get; private set; }

        [JsonIgnore]
        public StringBuilder OffsetResult { get; private set; }

        public async void UpdateFileOffset()
        {
            OffsetResult = new StringBuilder();
            using (var reader = new StreamReader(SrtFile.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    string lineText = await reader.ReadLineAsync();

                    if (lineText.Contains("-->"))
                    {
                        Offset = Offset.Replace(",", ".");
                        TimeSpan.TryParseExact(Offset, @"hh\:mm\:ss\.fff", null, out TimeSpan offsetTime);

                        string oldText = lineText.Substring(0, 16);
                        DateTime time = DateTime.Parse(lineText.Replace(",", ".").Substring(17));
                        time = time.Add(offsetTime);

                        lineText = $"{oldText} {time.ToString("HH:mm:ss,fff")}";
                    }

                    OffsetResult.AppendLine(lineText);
                }
            }
        }

    }
}
