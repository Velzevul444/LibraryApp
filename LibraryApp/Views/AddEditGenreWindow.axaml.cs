using Avalonia.Controls;
using LibraryApp.ViewModels;

namespace LibraryApp.Views
{
    public partial class AddEditGenreWindow : Window
    {
        public AddEditGenreWindow()
        {
            InitializeComponent();
            var vm = new AddEditGenreViewModel();
            vm.CloseRequested += () => Close();
            DataContext = vm;
        }

        public AddEditGenreWindow(Models.Genre genre) : this()
        {
            if (DataContext is AddEditGenreViewModel vm)
                vm.Genre = genre;
        }
    }
}