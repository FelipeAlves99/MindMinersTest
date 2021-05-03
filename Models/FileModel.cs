using System;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace MindMinersTest.Models
{
    public class FileModel
    {
        public IFormFile SrtFile { get; set; }

        public string Offset { get; set; }

        [JsonIgnore]
        public string OffsetResult { get; set; }

        public async void UpdateFileOffset()
        {
            var result = new StringBuilder();
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

                    result.AppendLine(lineText);
                }
                OffsetResult = result.ToString();
            }
        }

    }
}
