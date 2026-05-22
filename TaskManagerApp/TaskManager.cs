using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagerApp
{
    // Головний клас, що зберігає всі завдання і сповіщає спостерігачів
    public class TaskManager
    {
        // Список усіх завдань
        private List<TaskItem> _tasks = new List<TaskItem>();

        // Список спостерігачів, яких треба сповіщати про зміни
        private List<ITaskObserver> _observers = new List<ITaskObserver>();

        // Лічильник для генерації унікальних ID
        private int _nextId = 1;

        // --- Методи для роботи зі спостерігачами (Observer патерн) ---

        // Додаємо нового спостерігача
        public void AddObserver(ITaskObserver observer)
        {
            _observers.Add(observer);
        }

        // Видаляємо спостерігача
        public void RemoveObserver(ITaskObserver observer)
        {
            _observers.Remove(observer);
        }

        // Сповіщаємо всіх спостерігачів про подію
        private void Notify(TaskItem task, string action)
        {
            foreach (var observer in _observers)
            {
                observer.Update(task, action);
            }
        }

        // --- Методи для роботи із завданнями ---

        // Додаємо нове завдання і сповіщаємо всіх
        public void AddTask(TaskItem task)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
            Notify(task, "Додано");
        }

        // Змінюємо статус завдання на "Виконується"
        public void StartTask(int id)
        {
            TaskItem task = FindTask(id);
            if (task != null)
            {
                task.Status = WorkStatus.InProgress;
                Notify(task, "Розпочато");
            }
        }

        // Позначаємо завдання як виконане
        public void CompleteTask(int id)
        {
            TaskItem task = FindTask(id);
            if (task != null)
            {
                task.Status = WorkStatus.Completed;
                Notify(task, "Завершено");
            }
        }

        // Скасовуємо завдання
        public void CancelTask(int id)
        {
            TaskItem task = FindTask(id);
            if (task != null)
            {
                task.Status = WorkStatus.Cancelled;
                Notify(task, "Скасовано");
            }
        }

        // Видаляємо завдання зі списку
        public bool DeleteTask(int id)
        {
            TaskItem task = FindTask(id);
            if (task != null)
            {
                _tasks.Remove(task);
                Notify(task, "Видалено");
                return true;
            }
            return false;
        }

        // Шукаємо завдання за ID
        public TaskItem FindTask(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        // Повертаємо всі завдання
        public List<TaskItem> GetAllTasks()
        {
            return _tasks;
        }

        // Повертаємо завдання з потрібним пріоритетом
        public List<TaskItem> GetByPriority(Priority priority)
        {
            return _tasks.Where(t => t.Priority == priority).ToList();
        }

        // Виводимо всі завдання посортовані за пріоритетом (критичні першими)
        public void PrintAllTasks()
        {
            if (_tasks.Count == 0)
            {
                Console.WriteLine("Завдань немає.");
                return;
            }

            // Сортуємо від найважливішого до найменш важливого
            var sorted = _tasks.OrderByDescending(t => t.Priority).ToList();
            Console.WriteLine("\n--- Список завдань ---");
            foreach (var task in sorted)
            {
                Console.WriteLine(task);
            }
            Console.WriteLine("----------------------\n");
        }
    }
}