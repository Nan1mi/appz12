using System;
using System.Collections.Generic;

namespace TaskManagerApp
{
    public interface ICommand
    {
        void Execute();
        void Undo();
        string Description { get; }
    }

    public class AddTaskCommand : ICommand
    {
        private TaskManager _manager;
        private TaskItem _task;

        public string Description { get { return "Додати: " + _task.Title; } }

        public AddTaskCommand(TaskManager manager, TaskItem task)
        {
            _manager = manager;
            _task = task;
        }

        public void Execute() { _manager.AddTask(_task); }

        public void Undo()
        {
            _manager.DeleteTask(_task.Id);
            Console.WriteLine("[Undo] Видалено завдання: " + _task.Title);
        }
    }

    public class CompleteTaskCommand : ICommand
    {
        private TaskManager _manager;
        private int _taskId;
        private WorkStatus _previousStatus;

        public string Description { get { return "Завершити ID=" + _taskId; } }

        public CompleteTaskCommand(TaskManager manager, int taskId)
        {
            _manager = manager;
            _taskId = taskId;
        }

        public void Execute()
        {
            TaskItem task = _manager.FindTask(_taskId);
            if (task != null) { _previousStatus = task.Status; _manager.CompleteTask(_taskId); }
        }

        public void Undo()
        {
            TaskItem task = _manager.FindTask(_taskId);
            if (task != null)
            {
                task.Status = _previousStatus;
                Console.WriteLine("[Undo] Повернено статус ID={0} -> {1}", _taskId, _previousStatus);
            }
        }
    }

    public class DeleteTaskCommand : ICommand
    {
        private TaskManager _manager;
        private TaskItem _taskBackup;

        public string Description { get { return "Видалити ID=" + _taskBackup.Id; } }

        public DeleteTaskCommand(TaskManager manager, int taskId)
        {
            _manager = manager;
            _taskBackup = manager.FindTask(taskId);
        }

        public void Execute()
        {
            if (_taskBackup != null) _manager.DeleteTask(_taskBackup.Id);
        }

        public void Undo()
        {
            if (_taskBackup != null)
            {
                _manager.AddTask(_taskBackup);
                Console.WriteLine("[Undo] Відновлено: " + _taskBackup.Title);
            }
        }
    }

    public class StartTaskCommand : ICommand
    {
        private TaskManager _manager;
        private int _taskId;
        private WorkStatus _previousStatus;

        public string Description { get { return "Розпочати ID=" + _taskId; } }

        public StartTaskCommand(TaskManager manager, int taskId)
        {
            _manager = manager;
            _taskId = taskId;
        }

        public void Execute()
        {
            TaskItem task = _manager.FindTask(_taskId);
            if (task != null) { _previousStatus = task.Status; _manager.StartTask(_taskId); }
        }

        public void Undo()
        {
            TaskItem task = _manager.FindTask(_taskId);
            if (task != null)
            {
                task.Status = _previousStatus;
                Console.WriteLine("[Undo] Скасовано початок ID={0}", _taskId);
            }
        }
    }

    public class CommandHistory
    {
        private Stack<ICommand> _history = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _history.Push(command);
        }

        public void Undo()
        {
            if (_history.Count > 0)
                _history.Pop().Undo();
            else
                Console.WriteLine("[Undo] Немає команд для скасування.");
        }

        public void PrintHistory()
        {
            Console.WriteLine("\nІсторія команд:");
            if (_history.Count == 0) { Console.WriteLine("  (порожня)"); return; }
            foreach (var cmd in _history)
                Console.WriteLine("  - " + cmd.Description);
        }
    }
}