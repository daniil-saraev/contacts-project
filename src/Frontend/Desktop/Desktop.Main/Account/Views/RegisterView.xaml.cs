using System.Windows;
using System.Windows.Controls;
using Desktop.Main.Account.ViewModels;

namespace Desktop.Main.Account.Views
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { ((RegisterViewModel)DataContext).Password = ((PasswordBox)sender).Password; }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { ((RegisterViewModel)DataContext).ConfirmPassword = ((PasswordBox)sender).Password; }
        }
    }
}
