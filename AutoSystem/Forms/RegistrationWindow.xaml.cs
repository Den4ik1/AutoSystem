using AutoSystem.Contexts;
using AutoSystem.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace AutoSystem.Forms
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private ApplicationContext db ;

        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void Button_New_Account(object sender, RoutedEventArgs e)
        {
            using (db = new ApplicationContext())
            {
                if (db.Users.Any(x => x.Login == textBoxLogin.Text.Trim()))
                {
                    MessageBox.Show("Пользователь с таким именем уже существует");
                }
                else if (textBoxPassword.Text != textBoxRePassword.Text
                    || textBoxPassword.Text.Length <= 6
                    || !(textBoxPassword.Text.Any(char.IsLetter) 
                        && textBoxPassword.Text.Any(char.IsDigit)))
                {
                    MessageBox.Show("Проблема с поролем");
                }
                else
                {
                    db.Users.Add(new UserDataModel
                    {
                        Login = textBoxLogin.Text.Trim(),
                        Password = textBoxPassword.Text.Trim(),
                    });
                    db.SaveChanges();
                    DataWindow dateWindow = new DataWindow();
                    dateWindow.Show();
                    Hide();
                }
            };
        }
    }
}
