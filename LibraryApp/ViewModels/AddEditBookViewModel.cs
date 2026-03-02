using LibraryApp.Models;
using LibraryApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace LibraryApp.ViewModels
{
    public class AddEditBookViewModel : INotifyPropertyChanged
    {
        private readonly LibraryContext _context;
        public Book Book { get; set; }
        public ObservableCollection<Author> Authors { get; } = new();
        public ObservableCollection<Genre> Genres { get; } = new();

        public string? ErrorMessage { get; private set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action? CloseRequested;

        public AddEditBookViewModel(Book? book = null)
        {
            _context = App.ServiceProvider.GetService(typeof(LibraryContext)) as LibraryContext
                       ?? throw new InvalidOperationException("LibraryContext not available");
            LoadLists();
            Book = book ?? new Book();
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseRequested?.Invoke());
        }

        private void LoadLists()
        {
            Authors.Clear();
            foreach (var a in _context.Authors) Authors.Add(a);
            Genres.Clear();
            foreach (var g in _context.Genres) Genres.Add(g);
        }

        private void Save()
        {
            if (Book.Author != null)
            {
                Book.AuthorId = Book.Author.Id;
                _context.Entry(Book).Reference(b => b.Author).CurrentValue = null;
            }
            if (Book.Genre != null)
            {
                Book.GenreId = Book.Genre.Id;
                _context.Entry(Book).Reference(b => b.Genre).CurrentValue = null;
            }

            if (string.IsNullOrWhiteSpace(Book.Title) || Book.AuthorId == 0 || Book.GenreId == 0 || string.IsNullOrWhiteSpace(Book.ISBN))
            {
                ErrorMessage = "Title, author, genre and ISBN are required.";
                OnPropertyChanged(nameof(ErrorMessage));
                return;
            }

            if (Book.Id == 0)
                _context.Books.Add(Book);
            else
                _context.Books.Update(Book);

            _context.SaveChanges();
            CloseRequested?.Invoke();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
