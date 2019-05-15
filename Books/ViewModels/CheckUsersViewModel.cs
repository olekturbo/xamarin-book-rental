using Books.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Books.ViewModels
{
    public class CheckUsersViewModel : NotifyBase
    {
        private readonly CheckUsers _checkUsers;

        private ObservableCollection<User> _listOfUsers = new ObservableCollection<User>();
        public ObservableCollection<User> ListOfUsers
        {
            get => _listOfUsers;
            set
            {
                _listOfUsers = value;
                NotifyPropertyChange(nameof(ListOfUsers));
            }
        }

        private async Task GetAllUsers()
        {
            var foundUsers = await App.Database.GetUsersAsync();

            foreach (var item in foundUsers)
            {
                _listOfUsers.Add(item);
            }

        }

        public CheckUsersViewModel(CheckUsers checkUsers)
        {
            _checkUsers = checkUsers;
            Task.Run(async () => { await GetAllUsers(); });
        }

        public ICommand Edit => new Command<int>(OnClickEdit);
        public ICommand Remove => new Command<int>(OnClickRemove);

        private async void OnClickEdit(int id)
        {
            _checkUsers.Navigation.PushAsync(new EditUser(id));
        }

        private async void OnClickRemove(int id)
        {
            var foundUser = await App.Database.GetUserAsync(id);
            await App.Database.DeleteUserAsync(foundUser);

            _checkUsers.DisplayAlert("Komunikat", "Uzytkownik zostal usuniety.", "OK");
        }
        
    }
}
