using Lab9.Green;
using System;
using System.IO;

namespace Lab10.Green
{
    public class Green
    {
        private GreenFileManager _manager;
        private Lab9.Green.Green[] _tasks;

        public GreenFileManager Manager => _manager;
        public Lab9.Green.Green[] Tasks => (Lab9.Green.Green[])_tasks.Clone();

        public Green(Lab9.Green.Green[] tasks = null)
        {
            _tasks = tasks != null ? (Lab9.Green.Green[])tasks.Clone() : new Green[0];
        }

        public Green(GreenFileManager manager, Lab9.Green.Green[] tasks = null) : this(tasks)
        {
            _manager = manager;
        }

        public Green(Lab9.Green.Green[] tasks, GreenFileManager manager) : this(tasks)
        {
            _manager = manager;
        }

        public void Add(Green task)
        {
            if (task == null) return;
            Array.Resize(ref _tasks, _tasks.Length + 1);
            _tasks[_tasks.Length - 1] = task;
        }

        public void Add(Lab9.Green.Green[] tasks)
        {
            if (tasks == null) return;
            foreach (var t in tasks) Add(t);
        }

        public void Remove(Lab9.Green.Green task)
        {
            if (task == null || _tasks.Length == 0) return;
            var list = new System.Collections.Generic.List<Green>(_tasks);
            list.Remove(task);
            _tasks = list.ToArray();
        }

        public void Clear()
        {
            _tasks = new Green[0];
            if (_manager != null && Directory.Exists(_manager.FolderPath))
                Directory.Delete(_manager.FolderPath, true);
        }

        public void SaveTasks()
        {
            if (_manager == null || _tasks.Length == 0) return;
            for (int i = 0; i < _tasks.Length; i++)
            {
                _manager.ChangeFileName($"Task{i + 1}");
                _manager.Serialize(_tasks[i]);
            }
        }

        public void LoadTasks()
        {
            if (_manager == null || _tasks.Length == 0) return;
            for (int i = 0; i < _tasks.Length; i++)
            {
                _manager.ChangeFileName($"Task{i + 1}");
                _tasks[i] = _manager.Deserialize<Green>();
            }
        }

        public void ChangeManager(GreenFileManager manager)
        {
            _manager = manager;
            if (!Directory.Exists(manager.FolderPath))
                Directory.CreateDirectory(manager.FolderPath);
            manager.SelectFolder(manager.FolderPath);
        }
    }
}
