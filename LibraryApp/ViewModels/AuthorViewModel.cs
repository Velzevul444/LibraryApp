using LibraryApp.Data;
using LibraryApp.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using Avalonia.Controls;

namespace LibraryApp.ViewModels
{
    public class AuthorViewModel
    {
        private readonly LibraryContext _context;
        public ObservableCollection<Author> Authors { get; } = new();
        public Author? SelectedAuthor { get; set; }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public AuthorViewModel()
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
            Authors.Clear();
            foreach (var a in _context.Authors)
                Authors.Add(a);
        }

        private void Add()
        {
            var win = new LibraryApp.Views.AddEditAuthorWindow();
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
            if (SelectedAuthor == null) return;
            var win = new LibraryApp.Views.AddEditAuthorWindow(SelectedAuthor);
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
            if (SelectedAuthor == null) return;
            _context.Authors.Remove(SelectedAuthor);
            _context.SaveChanges();
            Load();
        }
    }
}