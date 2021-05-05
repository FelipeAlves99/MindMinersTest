using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MindMinersTest.Interfaces.Services;
using MindMinersTest.Models;

namespace MindMinersTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnv;
        private IFileService _fileService { get; set; }

        public FileController(IWebHostEnvironment webHostEnv, IFileService fileService)
        {
            _webHostEnv = webHostEnv;
            _fileService = fileService;
        }

        [HttpGet]
        public ActionResult<List<string>> GetFiles()
        {
            try
            {
                string path = _webHostEnv.WebRootPath + "/uploads/";
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
                _fileService.SaveFileWithOffset(model);

                return DownloadFile(model.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro inesperado, contate o administrador do site. Erro: " + ex.Message });
            }
        }
    }
}
