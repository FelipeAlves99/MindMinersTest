using MindMinersTest.Models;

namespace MindMinersTest.Interfaces.Services
{
    public interface IFileService
    {
        void SaveFileWithOffset(FileModel model);
    }
}
