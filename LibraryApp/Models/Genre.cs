using System.Collections.Generic;

namespace LibraryApp.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}