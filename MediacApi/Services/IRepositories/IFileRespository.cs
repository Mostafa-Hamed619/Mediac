namespace MediacBack.Services.IRepositories
{
    public interface IFileRespository
    {
        public Tuple<int,string> SaveImage(IFormFile image, string FolderName);

        public bool DeleteImage(string fileName, string FolderName);
    }
}
