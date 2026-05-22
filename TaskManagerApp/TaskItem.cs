using System;

namespace TaskManagerApp
{
    // Пріоритет завдання - від низького до критичного
    public enum Priority
    {
        Low,
        Medium,
        High,
        Critical
    }

    // Статус завдання - що зараз відбувається з ним
    public enum WorkStatus
    {
        Pending,     // чекає виконання
        InProgress,  // виконується
        Completed,   // завершено
        Cancelled    // скасовано
    }

    // Клас, що описує одне завдання
    public class TaskItem
    {
        // Унікальний номер завдання
        public int Id { get; set; }

        // Назва завдання
        public string Title { get; set; }

        // Детальний опис
        public string Description { get; set; }

        // Пріоритет (наскільки важливе завдання)
        public Priority Priority { get; set; }

        // Поточний статус завдання
        public WorkStatus Status { get; set; }

        // Дата створення завдання
        public DateTime CreatedAt { get; set; }

        // Конструктор - заповнює всі поля при створенні
        public TaskItem(int id, string title, string description, Priority priority)
        {
            Id = id;
            Title = title;
            Description = description;
            Priority = priority;
            Status = WorkStatus.Pending; // нові завдання завжди "чекають"
            CreatedAt = DateTime.Now;
        }

        // Виводить завдання у зручному вигляді для консолі
        public override string ToString()
        {
            return string.Format("[{0}] {1} | Пріоритет: {2} | Статус: {3} | Створено: {4}",
                Id, Title, Priority, Status, CreatedAt.ToString("dd.MM.yyyy HH:mm"));
        }
    }
}