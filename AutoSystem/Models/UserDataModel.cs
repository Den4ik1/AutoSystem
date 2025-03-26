using System.Windows.Input;

namespace AutoSystem.Models
{
    public class UserDataModel
    {
        public UserDataModel() { }

        public UserDataModel(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
