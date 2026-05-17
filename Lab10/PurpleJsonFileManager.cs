using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10.Purple
{
    public abstract class PurpleFileManager<T> : MyFileManager, ISerializer<T> where T : Lab9.Purple.Purple
    {
        public PurpleFileManager(string name) : base(name)
        {
        }

        public PurpleFileManager(string name, string folder, string fileName, string extension = "txt") : base(name, folder, fileName, extension)
        {
        }

        public override void ChangeFileExtension(string fileExtention)
        {
            if (File.Exists(FullPath))
                base.ChangeFileExtension(fileExtention);
        }
        public override void EditFile(string fileContent)
        {
            if (File.Exists(FullPath))
                base.EditFile(fileContent);
        }
        public abstract T Deserialize();

        public abstract void Serialize(T purple);
    }
}