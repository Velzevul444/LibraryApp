using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System;

namespace LibraryApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly LibraryContext _context;

        public ObservableCollection<Book> Books { get; } = new();
        public ObservableCollection<Author> Authors { get; } = new();
        public ObservableCollection<Genre> Genres { get; } = new();

        private int _bookCount;
        public int BookCount
        {
            get => _bookCount;
            private set { _bookCount = value; OnPropertyChanged(nameof(BookCount)); }
        }

        public string DatabasePath { get; }

        private Author? _selectedAuthor;
        public Author? SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                OnPropertyChanged(nameof(SelectedAuthor));
                LoadBooks();
            }
        }

        private Genre? _selectedGenre;
        public Genre? SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                OnPropertyChanged(nameof(SelectedGenre));
                LoadBooks();
            }
        }

        private string _searchTitle = string.Empty;
        public string SearchTitle
        {
            get => _searchTitle;
            set
            {
                _searchTitle = value;
                OnPropertyChanged(nameof(SearchTitle));
                LoadBooks();
            }
        }

        public MainWindowViewModel()
        {
            _context = App.ServiceProvider.GetService(typeof(LibraryContext)) as LibraryContext
                       ?? throw new InvalidOperationException("LibraryContext not available");
            _context.Database.EnsureCreated();
            DatabasePath = _context.Database.GetDbConnection().DataSource;
            DatabasePath = _context.Database.GetDbConnection().DataSource;
            SeedData();
            LoadAuthors();
            LoadGenres();
            LoadBooks();
        }

        private void SeedData()
        {
            if (!_context.Authors.Any() && !_context.Genres.Any())
            {
                var a1 = new Author { FirstName = "Лев", LastName = "Толстой", BirthDate = new DateTime(1828, 9, 9), Country = "Россия" };
                var a2 = new Author { FirstName = "Федор", LastName = "Достоевский", BirthDate = new DateTime(1821, 11, 11), Country = "Россия" };
                _context.Authors.AddRange(a1, a2);

                var g1 = new Genre { Name = "Роман", Description = "Художественная литература" };
                var g2 = new Genre { Name = "Классика", Description = "Классическая литература" };
                _context.Genres.AddRange(g1, g2);

                _context.SaveChanges();

                _context.Books.Add(new Book
                {
                    Title = "Война и мир",
                    AuthorId = a1.Id,
                    GenreId = g2.Id,
                    PublishYear = 1869,
                    ISBN = "9785171534600",
                    QuantityInStock = 3
                });
                _context.SaveChanges();
            }
        }

        public void DeleteBook(Book book)
        {
            if (book == null) return;
            _context.Books.Remove(book);
            _context.SaveChanges();
            LoadBooks();
        }

        public void LoadAuthors()
        {
            Authors.Clear();
            foreach (var a in _context.Authors.OrderBy(a => a.LastName))
                Authors.Add(a);
        }

        public void LoadGenres()
        {
            Genres.Clear();
            foreach (var g in _context.Genres.OrderBy(g => g.Name))
                Genres.Add(g);
        }

        public void LoadBooks()
        {
            var query = _context.Books.Include(b => b.Author).Include(b => b.Genre).AsQueryable();
            if (!string.IsNullOrWhiteSpace(SearchTitle))
                query = query.Where(b => b.Title.Contains(SearchTitle));
            if (SelectedAuthor != null)
                query = query.Where(b => b.AuthorId == SelectedAuthor.Id);
            if (SelectedGenre != null)
                query = query.Where(b => b.GenreId == SelectedGenre.Id);

            Books.Clear();
            foreach (var b in query)
                Books.Add(b);

            BookCount = Books.Count;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
