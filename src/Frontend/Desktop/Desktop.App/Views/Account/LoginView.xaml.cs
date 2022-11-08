using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Desktop.App.Views.Account
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
