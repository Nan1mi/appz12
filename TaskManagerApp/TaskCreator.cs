using System;

namespace TaskManagerApp
{
    // Абстрактний клас-фабрика - описує як створювати завдання
    // Це і є патерн Factory Method
    public abstract class TaskCreator
    {
        // Метод-фабрика - підкласи самі вирішують, яке завдання створити
        public abstract TaskItem CreateTask(string title, string description);

        // Додатковий метод - створює завдання і одразу виводить повідомлення
        public TaskItem CreateAndAnnounce(string title, string description)
        {
            TaskItem task = CreateTask(title, description);
            Console.WriteLine("[Фабрика] Створено нове завдання: \"{0}\" | Пріоритет: {1}",
                task.Title, task.Priority);
            return task;
        }
    }

    // Фабрика для завдань з LOW пріоритетом
    public class LowPriorityTaskCreator : TaskCreator
    {
        public override TaskItem CreateTask(string title, string description)
        {
            return new TaskItem(0, title, description, Priority.Low);
        }
    }

    // Фабрика для завдань з MEDIUM пріоритетом
    public class MediumPriorityTaskCreator : TaskCreator
    {
        public override TaskItem CreateTask(string title, string description)
        {
            return new TaskItem(0, title, description, Priority.Medium);
        }
    }

    // Фабрика для завдань з HIGH пріоритетом
    public class HighPriorityTaskCreator : TaskCreator
    {
        public override TaskItem CreateTask(string title, string description)
        {
            return new TaskItem(0, title, description, Priority.High);
        }
    }

    // Фабрика для КРИТИЧНИХ завдань
    public class CriticalTaskCreator : TaskCreator
    {
        public override TaskItem CreateTask(string title, string description)
        {
            return new TaskItem(0, title, description, Priority.Critical);
        }
    }

    // Допоміжний клас - повертає потрібну фабрику за назвою пріоритету
    public static class TaskCreatorFactory
    {
        public static TaskCreator GetCreator(Priority priority)
        {
            switch (priority)
            {
                case Priority.Low:
                    return new LowPriorityTaskCreator();
                case Priority.Medium:
                    return new MediumPriorityTaskCreator();
                case Priority.High:
                    return new HighPriorityTaskCreator();
                case Priority.Critical:
                    return new CriticalTaskCreator();
                default:
                    throw new ArgumentException("Невідомий пріоритет: " + priority);
            }
        }
    }
}