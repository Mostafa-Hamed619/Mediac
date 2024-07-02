using MediacBack.HelperClasses;
using MediacBack.Services.IRepositories;

namespace MediacBack.Services.MockRepositories
{
    public class FileMockRepository : IFileRespository
    {
        private readonly IWebHostEnvironment _env;

        public FileMockRepository(IWebHostEnvironment environment)
        {
            this._env = environment;
        }

        public Tuple<int, string> SaveImage(IFormFile image, string FolderName)
        {
            try
            {
                var contentPath = this._env.ContentRootPath;
                var path = Path.Combine(contentPath, $"Images/{FolderName}");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var ext = Path.GetExtension(image.FileName);

                var AllowedExtension = new string[] { ".jpg, .png, .jpeg" };

                if (!AllowedExtension.Contains(ext))
                {
                    string Message = string.Format("Only {0} extensions are allowed", string.Join(',', AllowedExtension));
                }

                string uniqueString = Guid.NewGuid().ToString();
                var newFileName = uniqueString + ext;

                var filePath = Path.Combine(path, newFileName);
                var stream = new FileStream(filePath, FileMode.Create);
                image.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int,string>(0,"Error has occured");
            }
        }

        public bool DeleteImage(string fileName, string FolderName)
        {
            try
            {
                var wwwPath = this._env.ContentRootPath;
                var path = Path.Combine(wwwPath, $"Images/{FolderName}\\", fileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }

                return false;
            }
            catch(Exception ex) { return false; }
        }
    }
}
