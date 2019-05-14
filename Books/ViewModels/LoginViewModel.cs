using System;
using System.Collections.Generic;
using System.Text;
using Books.Validation;
using Books.Models;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Books.InterfacesDS;

namespace Books.ViewModels
{
    public class LoginViewModel : NotifyBase
    {
        private readonly LoginPage _loginPage;
        private ValidatableObject<string> _username = new ValidatableObject<string>();
        private ValidatableObject<string> _password = new ValidatableObject<string>();
        private string _orientation;

        public ValidatableObject<string> Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyPropertyChange(nameof(Username));
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

        public string Orientation => GetOrientation();
        
        public ICommand Register => new Command(OnClickRegisterForm);

        public ICommand Login => new Command(OnClickLogin);

        private string GetOrientation()
        {
            var orientation = DependencyService.Get<IDeviceOrientation>().GetOrientation();
            string orient = "[DependencyService] Orientacja: ";

            switch (orientation)
            {
                case DeviceOrientations.Undefined:
                    orient += "niezdefiniowano";
                    break;
                case DeviceOrientations.Landscape:
                    orient += "Horyzontalna";
                    break;
                case DeviceOrientations.Portrait:
                    orient += "Wertykalna";
                    break;
            }

            return orient;
        }

        async private void OnClickLogin(object obj)
        {
            ValidateEntries();
            var foundUser = await App.Database.GetUserByUsernameAsync(_username.Value);
            if(foundUser == null)
            {
                _loginPage.DisplayAlert("Błąd", "Brak użytkownika o nazwie " + _username.Value, "OK");
            } else
            {
                if (foundUser.Password.Equals(_password.Value))
                {
                    Preferences.Set("Username", _username.Value);
                    _loginPage.DisplayAlert("Komunikat", "Udało ci się zalogować", "OK");
                    _loginPage.Navigation.PushAsync(new MainPage());
                }
                else
                {
                    _loginPage.DisplayAlert("Błąd", "Nieprawidłowe dane", "OK");
                }
            }
        }

        private void OnClickRegisterForm(object obj)
        {
            _loginPage.Navigation.PushAsync(new RegisterPage());
        }

        public LoginViewModel(LoginPage loginPage)
        {
            _loginPage = loginPage;
            AddValidationRules();
        }

        private void AddValidationRules()
        {
            _username.Rules.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Nazwa użytkownika jest wymagana."
            });

            _password.Rules.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Hasło jest wymagane."
            });
        }

        private void ValidateEntries()
        {
            _username.Validate();
            _password.Validate();
        }
    }
}
