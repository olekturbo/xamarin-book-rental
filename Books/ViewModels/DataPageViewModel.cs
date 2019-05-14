using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Input;
using Books.Models;
using Books.Validation;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace Books.ViewModels
{
    class DataPageViewModel : NotifyBase
    {
        private readonly DataPage _dataPage;

        private ValidatableObject<string> _username = new ValidatableObject<string>();
        private ValidatableObject<string> _password = new ValidatableObject<string>();
        private ValidatableObject<string> _firstName = new ValidatableObject<string>();
        private ValidatableObject<string> _lastName = new ValidatableObject<string>();
        private ValidatableObject<string> _email = new ValidatableObject<string>();
        private ValidatableObject<string> _role = new ValidatableObject<string>();

        public ValidatableObject<string> Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyPropertyChange(nameof(Username));
            }
        }

        public ValidatableObject<string> Role
        {
            get => _role;
            set
            {
                _role = value;
                NotifyPropertyChange(nameof(Role));
            }
        }

        public ValidatableObject<string> Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyPropertyChange(nameof(Password));
            }
        }

        public ValidatableObject<string> FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                NotifyPropertyChange(nameof(FirstName));
            }
        }

        private async Task GetCurrentUser()
        {
            var foundUser = await App.Database.GetUserByUsernameAsync(Preferences.Get("Username", "default"));
            _firstName.Value = foundUser.FirstName;
            _lastName.Value = foundUser.LastName;
            _username.Value = foundUser.Username;
            _password.Value = foundUser.Password;
            _email.Value = foundUser.Email;
            _role.Value = foundUser.Role;

        }

        public ValidatableObject<string> LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                NotifyPropertyChange(nameof(LastName));
            }
        }

        public ValidatableObject<string> Email
        {
            get => _email;
            set
            {
                _email = value;
                NotifyPropertyChange(nameof(Email));
            }
        }

        public ICommand Change => new Command(OnClickChange);

        public ICommand Back => new Command(OnClickBack);

        public DataPageViewModel(DataPage dataPage)
        {
            _dataPage = dataPage;
            AddValidationRules();
            Task.Run(async () => { await GetCurrentUser(); });
        }

        private void AddValidationRules()
        {

            _firstName.Rules.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Imie jest wymagane."
            });

            _lastName.Rules.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Nazwisko jest wymagane."
            });


        }

        private void OnClickBack(object obj)
        {
            _dataPage.Navigation.PopAsync();
        }

        async private void OnClickChange(object obj)
        {
            ValidateEntries();

            var isError = _firstName.Errors.Any() || _lastName.Errors.Any();

            if (isError)
                return;

            var foundUser = await App.Database.GetUserByUsernameAsync(Preferences.Get("Username", "default"));

            foundUser.FirstName = _firstName.Value;
            foundUser.LastName = _lastName.Value;
            await App.Database.SaveUserAsync(foundUser);

            _dataPage.DisplayAlert("Komunikat", "Dziękujemy - dane zmienione.", "OK");
            _dataPage.Navigation.PushAsync(new MainPage(DateTime.Now.ToString("u")));

        }

        private void ValidateEntries()
        {
            _firstName.Validate();
            _lastName.Validate();
        }
    }
}
