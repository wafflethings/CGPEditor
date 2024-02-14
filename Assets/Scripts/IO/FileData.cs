namespace CgpEditor.IO
{
    public class FileData
    {
        public static FileData CurrentFile;
        public string FileName;
        public string Extension;

        public FileData(string fileName, string extension)
        {
            FileName = fileName;
            Extension = extension;
        }
    }
}