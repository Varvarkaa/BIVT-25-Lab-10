using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab10.Purple
{
    public class Purple<T> where T : Lab9.Purple.Purple
    {
        private T[] _tasks;
        private PurpleFileManager<T> _manager;

        public PurpleFileManager<T> Manager => _manager;
        public T[] Tasks => _tasks?.ToArray() ?? Array.Empty<T>();

        public Purple(T[] tasks = null)
        {
            _tasks = tasks ?? Array.Empty<T>();
            _manager = null;
        }

        public Purple(PurpleFileManager<T> manager, T[] tasks = null)
        {
            _manager = manager;
            _tasks = tasks ?? Array.Empty<T>();
        }

        public Purple(T[] tasks, PurpleFileManager<T> manager)
        {
            _manager = manager;
            _tasks = tasks ?? Array.Empty<T>();
        }

        public void Add(T item)
        {
            if (item == null) return;
            Array.Resize(ref _tasks, _tasks.Length + 1);
            _tasks[^1] = item;  // Использование индекса от конца массива
        }

        public void Add(T[] tasks)
        {
            if (tasks == null || tasks.Length == 0) return;
            foreach (T item in tasks)
            {
                Add(item);
            }
        }

        public void Remove(T item)
        {
            int index = Array.IndexOf(_tasks, item);
            if (index == -1) return;

            T[] newTasks = new T[_tasks.Length - 1];
            Array.Copy(_tasks, 0, newTasks, 0, index);
            Array.Copy(_tasks, index + 1, newTasks, index, _tasks.Length - index - 1);
            _tasks = newTasks;
        }

        public void Clear()
        {
            _tasks = Array.Empty<T>();
            if (_manager != null && !string.IsNullOrEmpty(_manager.FolderPath) && Directory.Exists(_manager.FolderPath))
                Directory.Delete(_manager.FolderPath, true);
        }

        public void LoadTasks()
        {
            if (_manager == null) return;

            for (int i = 0; i < _tasks.Length; i++)
            {
                string nameFile = $"task{i}";
                _manager.ChangeFileName(nameFile);
                _tasks[i] = _manager.Deserialize();
            }
        }

        public void SaveTasks()
        {
            if (_manager == null || _tasks == null || _tasks.Length == 0) return;

            for (int i = 0; i < _tasks.Length; i++)
            {
                _manager.ChangeFileName($"task{i}");
                _manager.Serialize(_tasks[i]);
            }
        }

        public void ChangeManager(PurpleFileManager<T> newManager)
        {
            if (newManager == null) return;

            _manager = newManager;
            if (!string.IsNullOrEmpty(_manager.Name))
            {
                string folderPath = _manager.Name;

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                _manager.SelectFolder(folderPath);
            }
        }
    }
}
