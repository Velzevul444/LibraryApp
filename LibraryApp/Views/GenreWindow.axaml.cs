using Avalonia.Controls;

namespace LibraryApp.Views
{
    public partial class GenreWindow : Window
    {
        public GenreWindow()
        {
            InitializeComponent();
            DataContext = new LibraryApp.ViewModels.GenreViewModel();
        }
    }
}