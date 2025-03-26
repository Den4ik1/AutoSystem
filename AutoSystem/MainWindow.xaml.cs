using AutoSystem.Contexts;
using AutoSystem.Forms;
using AutoSystem.Models;
using System.Linq;
using System.Windows;

namespace AutoSystem
{
    public partial class MainWindow : Window
    {
        ApplicationContext db;
        public MainWindow()
        {
            InitializeComponent();
            InitDB();
        }

        private void InitDB()
        {
            using (db = new ApplicationContext())
            {
                db.Database.EnsureCreated();
                if (!db.Users.Any())
                {
                    db.Users.Add(new UserDataModel()
                    {
                        Login = "a",
                        Password = "1"
                    });
                    db.SaveChanges();
                }
            };
        }

        private void Button_Auth(object sender, RoutedEventArgs e)
        {
            using (db = new ApplicationContext())
            {
                if (db.Users.Any(x => x.Login == textBoxLogin.Text.Trim() && x.Password == textBoxPassword.Text.Trim()))
                {
                    DataWindow dateWindow = new DataWindow();
                    dateWindow.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Не верный логин или пороль. Попробуйте \nLogin = a\nPass = 1");
                }
            };
        }

        private void Button_New_Account(object sender, RoutedEventArgs e)
        {
            RegistrationWindow regWindow = new RegistrationWindow();
            regWindow.Show();
            Hide();
        }
    }
}
