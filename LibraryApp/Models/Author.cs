using System;
using System.Collections.Generic;

namespace LibraryApp.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Country { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}