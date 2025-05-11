using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using MusicStoreApp.Core.Services;
using MusicStoreApp.Core.Models.MusicStoreApp.Core.Models;
using MusicStoreApp.Views;


namespace MusicStoreApp
{
    public partial class MainWindow : Window
    {
        private readonly UserService _userService = new();
        private readonly OrderService _orderService = new(); // общий OrderService

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var user = _userService.Authenticate(LoginUsername.Text.Trim(), LoginPassword.Password);

            if (user != null)
            {
                Window nextWindow = user.Role switch
                {
                    UserRole.Customer => new CustomerWindow(), // можно также передавать user при необходимости
                    UserRole.Manager => new ManagerWindow(_orderService),
                    UserRole.Admin => new AdminWindow(),
                    _ => null
                };

                if (nextWindow != null)
                {
                    nextWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка: неизвестная роль пользователя.");
                }
            }
            else
            {
                MessageBox.Show("Неверные имя пользователя или пароль.");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var username = RegisterUsername.Text.Trim();
            var password = RegisterPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                RegisterStatus.Text = "Имя пользователя и пароль не должны быть пустыми.";
                return;
            }

            var success = _userService.Register(username, password);
            RegisterStatus.Text = success
                ? "Успешная регистрация! Теперь вы можете войти."
                : "Пользователь с таким именем уже существует.";
        }
    }
}
