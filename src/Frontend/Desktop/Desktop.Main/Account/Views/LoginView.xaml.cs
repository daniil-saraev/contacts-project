using System.Windows;
using System.Windows.Controls;
using Desktop.Main.Account.ViewModels;

namespace Desktop.Main.Account.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { ((LoginViewModel)DataContext).Password = ((PasswordBox)sender).Password; }
        }
    }
}
