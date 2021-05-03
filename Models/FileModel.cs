using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Flunt.Notifications;
using Microsoft.AspNetCore.Http;

namespace MindMinersTest.Models
{
    [DataContract]
    public class FileModel : Notifiable
    {
        [DataMember]
        public IFormFile SrtFile { get; set; }

        [DataMember]
        public string Offset { get; set; }

        public string OffsetResult { get; set; }

        public async void Validate()
        {
            if (Path.GetExtension(SrtFile.FileName).ToLowerInvariant() != ".srt")
                AddNotification("SrtFile", "O arquivo deve ser .srt");
            if (SrtFile.Length > 200000)
                AddNotification("SrtFile", "O arquivo não deve ultrapassar os 200Kb");
            if (await FileSignatureIsTrusted())
                AddNotification("SrtFile", "O arquivo possui conteudo diferente do esperado");            
        }

        private async Task<bool> FileSignatureIsTrusted()
        {

            using (var reader = new StreamReader(SrtFile.OpenReadStream()))
            {
                int loopCount = 1;
                int tries = 5;

                while (!reader.EndOfStream && loopCount < 4)
                {
                    string lineText = await reader.ReadLineAsync();

                    if (int.TryParse(lineText, out int subtitleOrder))
                    {
                        if (subtitleOrder == loopCount)
                        {
                            loopCount++;
                            tries = 5;
                            continue;
                        }
                        else
                            continue;
                    }
                    else if (loopCount == 1)
                    {
                        return false;
                    }
                    else if (tries == 0)
                    {
                        return false;
                    }
                    else
                    {
                        tries--;
                        continue;
                    }
                }

                return true;
            }
        }

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
