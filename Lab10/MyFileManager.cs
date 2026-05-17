using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10.Purple
{
    public abstract class MyFileManager : IFileManager, IFileLifeController
    {

        private string _name;
        private string _filename;
        private string _folderpath;
        private string _fileextension;
        private string _fileformat;

        public string FullPath
        {
            get
            {
                if (string.IsNullOrEmpty(_fileextension))
                {
                    return Path.Combine(FolderPath, FileName);
                }
                else
                {
                    return Path.Combine(FolderPath, $"{FileName}.{_fileextension}");
                }

            }
        }

        public string Name => _name;
        public string FolderPath => _folderpath;
        public string FileName => _filename;
        public string FileExtension => _fileextension;

        public MyFileManager(string name)
        {
            _name = name;
            _filename = "";
            _fileformat = "";
            _folderpath = "";
            _fileextension = "";
        }
        public MyFileManager(string name, string folder, string file, string extension = "")
        {
            _name = name;
            _folderpath = folder;
            _fileextension = extension;
            _filename = file;

        }
        public virtual void ChangeFileExtension(string newExt)
        {
            if (File.Exists(FullPath))
            {
                string newpath = Path.Combine(FolderPath, $"{FileName}.{newExt}");
                File.Move(FullPath, newpath);
            }
            _fileextension = newExt;


        }

        public virtual void ChangeFileFormat(string format)
        {
            if (string.IsNullOrEmpty(format)) return;
            _fileextension = format;
            if (!File.Exists(FullPath)) CreateFile(); 

        }

        public virtual void ChangeFileName(string fname)
        {
            _filename = fname;
        }

        public virtual void CreateFile()
        {
            if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
            if (!File.Exists(FullPath)) File.Create(FullPath).Close();
        }
        public virtual void SelectFolder(string folder)
        {
            _folderpath = folder;
        }
        public virtual void DeleteFile()
        {
            if (File.Exists(FullPath)) File.Delete(FullPath);
        }

        public virtual void EditFile(string text)
        {

            if (File.Exists(FullPath)) File.WriteAllText(FullPath, text);
        }
    }
}
