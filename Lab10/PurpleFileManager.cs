using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10.Purple
{
    public abstract class PurpleFileManager<T> : MyFileManager, ISerializer<T>
        where T : Lab9.Purple.Purple
    {
        public PurpleFileManager(string name) : base(name) { }
        public PurpleFileManager(string name, string folderPath, string fileName, string fileExtension = "") :
             base(name, folderPath, fileName, fileExtension)
        { }
        public override void ChangeFileExtension(string ext)
        {
            if (string.IsNullOrEmpty(FullPath) || !File.Exists(FullPath)) return;
            base.ChangeFileExtension(ext);
        }
        public override void EditFile(string newcont)
        {
            if (string.IsNullOrEmpty(FullPath) || !File.Exists(FullPath)) return;

            base.EditFile(newcont);
        }
        public abstract T Deserialize();


        public abstract void Serialize(T obj);


    }
}
