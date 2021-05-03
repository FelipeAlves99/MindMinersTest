using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MindMinersTest.Models;

namespace MindMinersTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnv;

        public FileController(IWebHostEnvironment webHostEnv)
        {
            _webHostEnv = webHostEnv;
        }

        [HttpGet]
        public ActionResult<List<string>> GetFiles()
        {
            string path = _webHostEnv.WebRootPath + "\\uploads\\";
            var list = Directory.GetFiles(path).Select(file => Path.GetFileName(file)).ToList();
            return Ok(list);
        }

        [HttpGet("Download")]
        public FileResult DownloadFile(string fileName)
            => File($"/uploads/{fileName}", "application/x-subrip", fileName);

        [HttpPost]
        public ActionResult<FileModel> ReceiveFile([FromForm] FileModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var a = model.SrtFile.Length;

            model.Validate();
            if(model.Invalid)
                return BadRequest(model.Notifications.FirstOrDefault());

            model.UpdateFileOffset();
            SaveFileWirhOffset(model);

            return DownloadFile(model.SrtFile.FileName);
        }        

        private void SaveFileWirhOffset(FileModel model)
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
