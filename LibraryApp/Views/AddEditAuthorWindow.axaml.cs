using Avalonia.Controls;
using LibraryApp.ViewModels;

namespace LibraryApp.Views
{
    public partial class AddEditAuthorWindow : Window
    {
        public AddEditAuthorWindow()
        {
            InitializeComponent();
            var vm = new AddEditAuthorViewModel();
            vm.CloseRequested += () => Close();
            DataContext = vm;
        }

        public AddEditAuthorWindow(Models.Author author) : this()
        {
            if (DataContext is AddEditAuthorViewModel vm)
                vm.Author = author;
        }
    }
}