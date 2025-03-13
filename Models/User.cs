using System;
using System.Collections.Generic;

namespace ToDo.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public string Nationalid { get; set; } = null!;
        public string? Firstname { get; set; }
        public string Title { get; set; } = null!;
        public string Lastname { get; set; } = null!;
    }
}
