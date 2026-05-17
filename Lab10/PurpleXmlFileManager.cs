using Lab9.Purple;
using System.Reflection.Emit;
using System.Xml.Serialization;

namespace Lab10.Purple;

public class PurpleXmlFileManager<T> : PurpleFileManager<T> where T : Lab9.Purple.Purple
{
    public PurpleXmlFileManager(string name) : base(name) { }
    public PurpleXmlFileManager(string name, string folder, string fileName, string extension)
        : base(name, folder, fileName, extension="") { }
    public override void ChangeFileExtension(string extension)
    {
        if (extension == "xml") ChangeFileFormat("xml");
    }

    public override void EditFile(string content)
    {
        if (string.IsNullOrEmpty(FullPath) || !File.Exists(FullPath)) return;
        T obj = Deserialize();
        if (obj == null) return;
        obj.ChangeText(content);
        Serialize(obj);
    }
    public override void Serialize(T obj)
    {
        if (obj == null || FullPath == null) return;

        var dto = new DTOPurple(obj);
        var ser = new XmlSerializer(typeof(DTOPurple));
        using (var sw = new StreamWriter(FullPath))
            ser.Serialize(sw, dto);
    }

    public override T Deserialize()
    {
        var ser = new XmlSerializer(typeof(DTOPurple));
        if (string.IsNullOrEmpty(FullPath) || !File.Exists(FullPath)) return null;
        using (var sr = new StreamReader(FullPath))
        {
            var dto = (DTOPurple)ser.Deserialize(sr);
            Lab9.Purple.Purple result;
            if (dto.TypeName == "Task1") result = new Task1(dto.Input);
            else if (dto.TypeName == "Task2") result = new Task2(dto.Input);            
            else if (dto.TypeName == "Task3") result = new Task3(dto.Input);
            else if (dto.TypeName == "Task4") 
                result = new Task4(dto.Input, dto.Codes ?? Array.Empty<(string, char)>());
            
            else return null;
            result.Review();
            return (T)result;
        }
    }
}

public class DTOPurple
{
    public string TypeName { get; set; }
    public string Input { get; set; }
    public (string, char)[] Codes { get; set; }

    public DTOPurple() { }

    public DTOPurple(Lab9.Purple.Purple obj)
    {
        TypeName = obj.GetType().Name;
        Input = obj.Input;
        if (obj is Lab9.Purple.Task4 task4 && task4.Codes != null)
        {
            Codes = new (string, char)[task4.Codes.Length];
            for (int i = 0; i < Codes.Length; i++)
                Codes[i] = (task4.Codes[i].Item1, task4.Codes[i].Item2);
        }
    }
}
