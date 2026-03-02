using LibraryApp.Data;
using LibraryApp.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using Avalonia.Controls;

namespace LibraryApp.ViewModels
{
    public class GenreViewModel
    {
        private readonly LibraryContext _context;
        public ObservableCollection<Genre> Genres { get; } = new();
        public Genre? SelectedGenre { get; set; }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public GenreViewModel()
        {
            _context = App.ServiceProvider.GetService(typeof(LibraryContext)) as LibraryContext
                       ?? throw new InvalidOperationException("LibraryContext not available");
            Load();

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
        }

        private void Load()
        {
            Genres.Clear();
            foreach (var g in _context.Genres)
                Genres.Add(g);
        }

        private void Add()
        {
            var win = new LibraryApp.Views.AddEditGenreWindow();
            var owner = Avalonia.Application.Current.ApplicationLifetime
                is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow
                : null;
            if (owner is Window ow)
                win.ShowDialog(ow);
            else
                win.Show();
            Load();
        }

        private void Edit()
        {
            if (SelectedGenre == null) return;
            var win = new LibraryApp.Views.AddEditGenreWindow(SelectedGenre);
            var owner = Avalonia.Application.Current.ApplicationLifetime
                is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow
                : null;
            if (owner is Window ow)
                win.ShowDialog(ow);
            else
                win.Show();
            Load();
        }

        private void Delete()
        {
            if (SelectedGenre == null) return;
            _context.Genres.Remove(SelectedGenre);
            _context.SaveChanges();
            Load();
        }
    }
}