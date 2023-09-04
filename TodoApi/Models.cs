namespace TodoApiV7
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public record TodoItemDto(long Id, string Name, bool IsComplete);
}
