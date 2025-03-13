namespace ToDo.DTOs
{
    public class Activity
    {
        public required string Name { get; set; }
        public required DateTime When { get; set; }
        public required string UserId { get; set; }
    }
}