namespace ToDo.DTOs
{
    public class Register
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Title { get; set; }
        public required string NationalId { get; set; }
        public required string Password { get; set; }
    }
}