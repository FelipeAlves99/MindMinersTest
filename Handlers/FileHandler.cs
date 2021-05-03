using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MindMinersTest.Models;

namespace MindMinersTest.Handlers
{
    public class FileHandler
    {
       public static IWebHostEnvironment _webHostEnv;

        public FileHandler(IWebHostEnvironment webHostEnv)
        {
            _webHostEnv = webHostEnv;
        }

        public async void UpdateFileOffset(FileModel model)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(model.SrtFile.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    string lineText = await reader.ReadLineAsync();

                    if (lineText.Contains("-->"))
                    {
                        TimeSpan.TryParseExact(model.Offset, @"hh\:mm\:ss.fff", null, out TimeSpan offsetTime);

                        string oldText = lineText.Substring(0, 16);
                        DateTime time = DateTime.Parse(lineText.Replace(",", ".").Substring(17, 28));
                        time.Add(offsetTime);

                        lineText = $"{oldText}{time.ToString("HH:mm:ss,fff")}";
                    }

                    result.AppendLine(await reader.ReadLineAsync());
                }
            }
        }

        private void SaveFileWithOffset(FileModel model)
        {
            string path = _webHostEnv.WebRootPath + "\\uploads\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (StreamWriter sw = new StreamWriter(path + model.SrtFile.FileName))
            {
                sw.WriteAsync(model.OffsetResult);                
                sw.Flush();
            }
        }
    }
}
