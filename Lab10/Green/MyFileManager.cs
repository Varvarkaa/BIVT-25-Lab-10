using System;
using System.IO;

namespace Lab10.Green
{
    public abstract class MyFileManager : IFileManager, IFileLifeController
    {
        private string _name;
        private string _folderPath;
        private string _fileName;
        private string _fileExtension;

        public string FolderPath => _folderPath;
        public string FileName => _fileName;
        public string FileExtension => _fileExtension;
        public string FullPath => Path.Combine(_folderPath, $"{_fileName}.{_fileExtension}");

        protected MyFileManager(string name)
        {
            _name = name;
            _folderPath = Directory.GetCurrentDirectory();
            _fileName = name;
            _fileExtension = "txt";
        }

        protected MyFileManager(string name, string folder, string file, string extension = "txt")
        {
            _name = name;
            _folderPath = folder ?? Directory.GetCurrentDirectory();
            _fileName = file ?? name;
            _fileExtension = extension ?? "txt";
        }

        public void SelectFolder(string folder)
        {
            if (!string.IsNullOrWhiteSpace(folder))
                _folderPath = folder;
        }

        public void ChangeFileName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                _fileName = name;
        }

        public void ChangeFileFormat(string extension)
        {
            if (!string.IsNullOrWhiteSpace(extension))
                _fileExtension = extension;
        }

        public virtual void CreateFile()
        {
            Directory.CreateDirectory(_folderPath);
            if (!File.Exists(FullPath))
                File.WriteAllText(FullPath, string.Empty);
        }

        public virtual void DeleteFile()
        {
            if (File.Exists(FullPath))
                File.Delete(FullPath);
        }

        public abstract void EditFile(string text);

        public abstract void ChangeFileExtension(string ext);
    }
}
