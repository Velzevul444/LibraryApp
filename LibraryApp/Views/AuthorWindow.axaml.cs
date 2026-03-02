using Avalonia.Controls;

namespace LibraryApp.Views
{
    public partial class AuthorWindow : Window
    {
        public AuthorWindow()
        {
            InitializeComponent();
            DataContext = new LibraryApp.ViewModels.AuthorViewModel();
        }
    }
}