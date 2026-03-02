using LibraryApp.Data;
using LibraryApp.Models;
using System.ComponentModel;
using System.Windows.Input;
using System;

namespace LibraryApp.ViewModels
{
    public class AddEditAuthorViewModel : INotifyPropertyChanged
    {
        private readonly LibraryContext _context;
        public Author Author { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action? CloseRequested;

        public AddEditAuthorViewModel(Author? author = null)
        {
            _context = App.ServiceProvider.GetService(typeof(LibraryContext)) as LibraryContext
                       ?? throw new InvalidOperationException("LibraryContext not available");
            Author = author ?? new Author { BirthDate = DateTime.Today };
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseRequested?.Invoke());
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Author.FirstName) || string.IsNullOrWhiteSpace(Author.LastName))
                return;

            if (Author.Id == 0)
                _context.Authors.Add(Author);
            else
                _context.Authors.Update(Author);

            _context.SaveChanges();
            CloseRequested?.Invoke();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}