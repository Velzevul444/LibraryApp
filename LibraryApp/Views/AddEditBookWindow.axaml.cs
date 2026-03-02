using Avalonia.Controls;
using LibraryApp.ViewModels;
using LibraryApp.Models;

namespace LibraryApp.Views
{
    public partial class AddEditBookWindow : Window
    {
        public AddEditBookWindow()
        {
            InitializeComponent();
            var vm = new AddEditBookViewModel();
            vm.CloseRequested += () => Close();
            DataContext = vm;
        }

        public AddEditBookWindow(Book book) : this()
        {
            if (DataContext is AddEditBookViewModel vm)
                vm.Book = book;
        }
    }
}