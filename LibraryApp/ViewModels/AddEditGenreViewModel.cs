using LibraryApp.Data;
using LibraryApp.Models;
using System.ComponentModel;
using System.Windows.Input;
using System;

namespace LibraryApp.ViewModels
{
    public class AddEditGenreViewModel : INotifyPropertyChanged
    {
        private readonly LibraryContext _context;
        public Genre Genre { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action? CloseRequested;

        public AddEditGenreViewModel(Genre? genre = null)
        {
            _context = App.ServiceProvider.GetService(typeof(LibraryContext)) as LibraryContext
                       ?? throw new InvalidOperationException("LibraryContext not available");
            Genre = genre ?? new Genre();
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(() => CloseRequested?.Invoke());
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Genre.Name))
                return;

            if (Genre.Id == 0)
                _context.Genres.Add(Genre);
            else
                _context.Genres.Update(Genre);

            _context.SaveChanges();
            CloseRequested?.Invoke();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}