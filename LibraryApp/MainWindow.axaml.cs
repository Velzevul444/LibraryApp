using Avalonia.Controls;
using LibraryApp.ViewModels;
using LibraryApp.Views;
using LibraryApp.Models;

namespace LibraryApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    private MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;

    private async void OnAddBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var win = new AddEditBookWindow();
        await win.ShowDialog(this);
        if (ViewModel != null)
        {
            ViewModel.SearchTitle = string.Empty;
            ViewModel.SelectedAuthor = null;
            ViewModel.SelectedGenre = null;
            ViewModel.LoadBooks();
        }
    }

    private async void OnEditBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ViewModel?.Books == null || ViewModel.Books.Count == 0) return;
        var selected = BooksDataGrid.SelectedItem as Book;
        if (selected == null) return;
        var win = new AddEditBookWindow(selected);
        await win.ShowDialog(this);
        if (ViewModel != null)
        {
            ViewModel.SearchTitle = string.Empty;
            ViewModel.SelectedAuthor = null;
            ViewModel.SelectedGenre = null;
            ViewModel.LoadBooks();
        }
    }

    private void OnDeleteBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var selected = BooksDataGrid.SelectedItem as Book;
        if (selected == null) return;
        ViewModel?.DeleteBook(selected);
    }

    private async void OnManageAuthors(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var win = new Views.AuthorWindow();
        await win.ShowDialog(this);
        ViewModel?.LoadAuthors();
        ViewModel?.LoadBooks();
    }

    private async void OnManageGenres(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var win = new Views.GenreWindow();
        await win.ShowDialog(this);
        ViewModel?.LoadGenres();
        ViewModel?.LoadBooks();
    }
}
