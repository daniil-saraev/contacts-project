using Desktop.ViewModels.Account;
using System.Windows;
using System.Windows.Controls;

namespace Desktop.Views.Account
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
            if (this.DataContext != null)
            { ((RegisterViewModel)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((RegisterViewModel)this.DataContext).ConfirmPassword = ((PasswordBox)sender).Password; }
        }
    }
}
