using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfAdminPanel.Views
{
    public partial class LoginPasswordWindow : Window
    {
        private bool isPasswordVisible = false;

        public LoginPasswordWindow()
        {
            InitializeComponent();



            LoginTextBox.Text = "Admin";
            PasswordBox_Check.Password = "1234";
        }

        private void ToggleVisibilityClick(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                // Show password
                TextBox_PasswordCheck.Text = PasswordBox_Check.Password;
                PasswordBox_Check.Visibility = Visibility.Collapsed;
                TextBox_PasswordCheck.Visibility = Visibility.Visible;
            }
            else
            {
                // Hide password
                PasswordBox_Check.Password = TextBox_PasswordCheck.Text;
                TextBox_PasswordCheck.Visibility = Visibility.Collapsed;
                PasswordBox_Check.Visibility = Visibility.Visible;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (isPasswordVisible)
            {
                TextBox_PasswordCheck.Text = PasswordBox_Check.Password;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isPasswordVisible)
            {
                PasswordBox_Check.Password = TextBox_PasswordCheck.Text;
            }
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox_Check.Password;

            //string login = "Admin";
            //string password = "1234";

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (login == "Admin" && password == "1234")
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) LoginClick(this, new RoutedEventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Выход из приложения
        }


    }
}
