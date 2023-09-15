using System;
namespace Birjanews.Api.Extentions
{
	public static class FileExtentions
	{
        public static bool IsImageOkay(this IFormFile file)
        {
            return file.ContentType.Contains("image/");
        }
        public static bool İsSizeOk(this IFormFile file, int mb)
        {
            return file.Length / 1024 / 1024 < mb;
        }
        public static string SaveImage(this IFormFile FirmFile, string root, string folder)
        {
            string RootPath = Path.Combine(root, folder);
            string FileName = Guid.NewGuid().ToString() + FirmFile.FileName;
            string FullPath = Path.Combine(RootPath, FileName);
            using (FileStream fileStream = new FileStream(FullPath, FileMode.Create))
            {
                FirmFile.CopyTo(fileStream);
            }
            return FileName;
        }
    }
}

