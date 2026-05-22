using System;
using System.IO;

namespace TaskManagerApp
{
    public interface ITaskObserver
    {
        void Update(TaskItem task, string action);
    }

    // Спостерігач №1 - виводить повідомлення в консоль
    public class ConsoleNotifier : ITaskObserver
    {
        public void Update(TaskItem task, string action)
        {
            Console.WriteLine("[Сповіщення] {0}: {1}", action, task.Title);
        }
    }

    // Спостерігач №2 - записує зміни у лог-файл
    public class FileLogger : ITaskObserver
    {
        private readonly string _logPath;

        public FileLogger(string logPath) { _logPath = logPath; }

        public void Update(TaskItem task, string action)
        {
            string logLine = string.Format("{0} | {1} | ID: {2} | {3} | {4}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                action, task.Id, task.Title, task.Status);
            File.AppendAllText(_logPath, logLine + Environment.NewLine);
        }
    }

    // Спостерігач №3 - попереджає про критичні завдання
    public class PriorityAlertNotifier : ITaskObserver
    {
        public void Update(TaskItem task, string action)
        {
            if (task.Priority == Priority.Critical || task.Priority == Priority.High)
                Console.WriteLine("[!] {0} завдання з {1} пріоритетом: {2}", action, task.Priority, task.Title);
        }
    }
}