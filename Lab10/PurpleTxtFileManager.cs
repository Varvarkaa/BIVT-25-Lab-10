using Lab9.Purple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10.Purple
{
    public class PurpleTxtFileManager<T> : PurpleFileManager<T> where T : Lab9.Purple.Purple
    {
        public PurpleTxtFileManager(string name) : base(name)
        { }

        public PurpleTxtFileManager(string name, string folderPath, string fileName, string fileExtension = "txt")
            : base(name, folderPath, fileName, fileExtension)
        { }

        public override void Serialize(T obj)
        {
            if (obj == null || FullPath == null) return;

            var pairs = new Dictionary<string, string>();
            pairs["Type"] = obj.GetType().Name;
            pairs["Input"] = obj.Input;

            if (obj is Task3 task3)
            {
                pairs["Count"] = task3.Codes.Length.ToString();
                for (int i = 0; i < task3.Codes.Length; i++)
                {
                    pairs[$"Code{i}"] = task3.Codes[i].Item1 + "|" + task3.Codes[i].Item2;
                }
            }
            else if (obj is Task4 task4)
            {
                pairs["Count"] = task4.Codes.Length.ToString();
                for (int i = 0; i < task4.Codes.Length; i++)
                {
                    pairs[$"Code{i}"] = task4.Codes[i].Item1 + "|" + task4.Codes[i].Item2;
                }
            }

            string[] lines = new string[pairs.Count];
            int index = 0;
            foreach (var pair in pairs)
                lines[index++] = pair.Key + ":" + pair.Value;

            File.WriteAllLines(FullPath, lines);
        }

        public override T Deserialize()
        {
            if (string.IsNullOrEmpty(FullPath) || !File.Exists(FullPath)) return null;

            var pairs = new Dictionary<string, string>();
            bool enc = false;

            using (StreamReader reader = new StreamReader(FullPath))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    int ind = line.IndexOf(':');
                    if (ind >= 0)
                        pairs[line.Substring(0, ind)] = line.Substring(ind + 1);

                }
            }

            if (!pairs.ContainsKey("Type") || !pairs.ContainsKey("Input")) return null;

            string typeName = pairs["Type"];
            string input = pairs["Input"];

            (string, char)[] codes = null;
            if (pairs.ContainsKey("Count"))
            {
                int count = int.Parse(pairs["Count"]);
                codes = new (string, char)[count];
                for (int i = 0; i < count; i++)
                {
                    string value = pairs[$"Code{i}"];

                    codes[i] = (value.Split("|")[0], value[value.Length - 1]);
                }
            }

            Lab9.Purple.Purple obj;
            if (typeName == "Task1") obj = new Task1(input);

            else if (typeName == "Task2") obj = new Task2(input);

            else if (typeName == "Task3") obj = new Task3(input);

            else if (typeName == "Task4") obj = new Task4(input, codes);

            else return null;

            obj.Review();
            return (T)obj;
        }


        public override void EditFile(string input)
        {
            if (string.IsNullOrEmpty(FullPath) || !File.Exists(FullPath)) return;
            T obj = Deserialize();
            if (obj == null) return;
            obj.ChangeText(input);
            Serialize(obj);
        }

        public override void ChangeFileExtension(string ext)
        {
            if (ext == "txt") ChangeFileFormat(ext);
        }
    }
}

