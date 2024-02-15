using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Models
{
    public interface IFileUploadService
    {
        Task<IActionResult> Upload(IFormFile file);
    }

}
