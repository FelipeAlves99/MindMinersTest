using System;
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
            try
            {
                string path = _webHostEnv.WebRootPath + "\\uploads\\";
                var list = Directory.GetFiles(path).Select(file => Path.GetFileName(file)).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro inesperado, contate o administrador do site. Erro: " + ex.Message });
            }
        }

        [HttpGet("Download")]
        public FileResult DownloadFile(string fileName)
            => File($"/uploads/{fileName}", "application/x-subrip", fileName);

        [HttpPost]
        public ActionResult<FileModel> ReceiveFile([FromForm] FileModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                model.Validate();
                if (model.Invalid)
                    return BadRequest(model.Notifications.FirstOrDefault());

                model.UpdateFileOffset();
                SaveFileWirhOffset(model);

                return DownloadFile(model.SrtFile.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro inesperado, contate o administrador do site. Erro: " + ex.Message });
            }
        }

        private void SaveFileWirhOffset(FileModel model)
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
