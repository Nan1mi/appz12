using System;

namespace TaskManagerApp
{
    // головний клас для запуску програми та меню
    internal class Program
    {
        static TaskManager manager = new TaskManager();
        static CommandHistory history = new CommandHistory();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            AddTask("Оновити документацію", "Дописати README", Priority.Low);
            AddTask("Виправити критичний баг", "Помилка при авторизації", Priority.High);
            AddTask("Сервер не відповідає", "Продакшн упав", Priority.Critical);
            AddTask("Зробити рев'ю коду", "Переглянути пул-реквест", Priority.Medium);

            manager.AddObserver(new ConsoleNotifier());
            manager.AddObserver(new PriorityAlertNotifier());
            manager.AddObserver(new FileLogger("task_log.txt"));

            string input;
            do
            {
                Console.WriteLine("\n1. Додати завдання");
                Console.WriteLine("2. Переглянути всі");
                Console.WriteLine("3. Розпочати завдання");
                Console.WriteLine("4. Завершити завдання");
                Console.WriteLine("5. Видалити завдання");
                Console.WriteLine("6. Фільтр за пріоритетом");
                Console.WriteLine("7. Скасувати дію (Undo)");
                Console.WriteLine("0. Вихід");
                Console.Write("> ");
                input = Console.ReadLine()?.Trim();


                switch (input)
                {
                    case "1":
                        Console.Write("Назва: "); string title = Console.ReadLine();
                        Console.Write("Опис: "); string desc = Console.ReadLine();
                        Console.Write("Пріоритет (1-Low 2-Medium 3-High 4-Critical): ");
                        AddTask(title, desc, ParsePriority(Console.ReadLine()));
                        break;
                    case "2": 
                        manager.PrintAllTasks(); break;
                    case "3":
                        manager.PrintAllTasks();
                        Console.Write("ID: ");
                        if (int.TryParse(Console.ReadLine(), out int sid))
                            history.ExecuteCommand(new StartTaskCommand(manager, sid));
                        break;
                    case "4": 
                        manager.PrintAllTasks();
                        Console.Write("ID: ");
                        if (int.TryParse(Console.ReadLine(), out int cid))
                            history.ExecuteCommand(new CompleteTaskCommand(manager, cid));
                        break;
                    case "5": 
                        manager.PrintAllTasks();
                        Console.Write("ID: ");
                        if (int.TryParse(Console.ReadLine(), out int did))
                            history.ExecuteCommand(new DeleteTaskCommand(manager, did));
                        break;
                    case "6": 
                        Console.Write("Пріоритет (1-Low 2-Medium 3-High 4-Critical): ");
                        var list = manager.GetByPriority(ParsePriority(Console.ReadLine()));
                        if (list.Count == 0) Console.WriteLine("Немає завдань.");
                        else foreach (var t in list) Console.WriteLine(t);
                        break;
                    case "7":
                        history.Undo(); break;
                    case "0": 
                        break;
                    default: 
                        Console.WriteLine("Невідома команда."); break;
                }
            }
            while (input != "0");
        }
        static void AddTask(string title, string desc, Priority p)
        {
            TaskItem task = TaskCreatorFactory.GetCreator(p).CreateTask(title, desc ?? "");
            history.ExecuteCommand(new AddTaskCommand(manager, task));
        }
        static Priority ParsePriority(string s)
        {
            switch (s?.Trim())
            {
                case "1": return Priority.Low;
                case "3": return Priority.High;
                case "4": return Priority.Critical;
                default: return Priority.Medium;
            }
        }
    }
}