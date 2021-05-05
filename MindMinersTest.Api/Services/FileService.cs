using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MindMinersTest.Interfaces.Services;
using MindMinersTest.Models;

namespace MindMinersTest.Services
{
    public class FileService : IFileService
    {
        public static IWebHostEnvironment _webHostEnv;

        public FileService(IWebHostEnvironment webHostEnv)
        {
            _webHostEnv = webHostEnv;
        }

        public void SaveFileWithOffset(FileModel model)
        {
            string path = _webHostEnv.WebRootPath + "\\uploads\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (StreamWriter sw = new StreamWriter(path + model.SrtFile.FileName))
            {
                sw.Write(model.OffsetResult);
                sw.Flush();
            }
        }
    }
}   