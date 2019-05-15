using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Books.Models;
using Books.Validation;
using Xamarin.Forms;

namespace Books.ViewModels
{
    public class RegisterViewModel : NotifyBase
    {

        private readonly RegisterPage _registerPage;
        private ValidatableObject<string> _username = new ValidatableObject<string>();
        private ValidatableObject<string> _password = new ValidatableObject<string>();
        private ValidatableObject<string> _repeatPassword = new ValidatableObject<string>();
        private ValidatableObject<string> _firstName = new ValidatableObject<string>();
        private ValidatableObject<string> _lastName = new ValidatableObject<string>();
        private ValidatableObject<string> _email = new ValidatableObject<string>();
        private string _chooseRole;

        public ValidatableObject<string> Username
        {
            get => _username;
            set
            {
                _username = value;
                NotifyPropertyChange(nameof(Username));
            }
        }

        public string ChooseRole
        {
            get => _chooseRole;
            set
            {
                _chooseRole = value;
                NotifyPropertyChange(nameof(ChooseRole));
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

        public ValidatableObject<string> RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                _repeatPassword = value;
                NotifyPropertyChange(nameof(RepeatPassword));
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

        public ICommand Register => new Command(OnClickRegister);

        public ICommand Back => new Command(OnClickBack);

        public RegisterViewModel(RegisterPage registerPage)
        {
            _registerPage = registerPage;
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

            _repeatPassword.Rules.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Powtórzenie hasła jest wymagane."
            });

            _firstName.Rules.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Imie jest wymagane."
            });

            _lastName.Rules.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Nazwisko jest wymagane."
            });

            _email.Rules.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Email jest wymagany."
            });

            _email.Rules.Add(new EmailRule<string>
            {
                ValidationMessage = "Email jest niepoprawny."
            });

            _password.Rules.Add(new PasswordRule<string>()
            {
                ValidationMessage = "Haslo musi miec przynajmniej 6 znakow."
            });

            _repeatPassword.Rules.Add(new PasswordRule<string>()
            {
                ValidationMessage = "Haslo musi miec przynajmniej 6 znakow."
            });

        }

        private void OnClickBack(object obj)
        {
            _registerPage.Navigation.PopAsync();
        }

        async private void OnClickRegister(object obj)
        {
            ValidateEntries();

            var isError = _username.Errors.Any() || _password.Errors.Any() ||
                _repeatPassword.Errors.Any() || _firstName.Errors.Any() ||
                _lastName.Errors.Any() || _email.Errors.Any();

            if (isError)
                return;

            User user = new User();
            user.Username = _username.Value;
            user.Password = _password.Value;
            user.FirstName = _firstName.Value;
            user.LastName = _lastName.Value;
            user.Email = _email.Value;
            user.Role = _chooseRole;
            await App.Database.SaveUserAsync(user);

            _registerPage.DisplayAlert("Komunikat", "Dziękujemy " + _username.Value + " - zostałeś zarejestrowany.", "OK");
            _registerPage.Navigation.PopAsync();

        }

        private void ValidateEntries()
        {
            _username.Validate();
            _password.Validate();
            _repeatPassword.Validate();
            _firstName.Validate();
            _lastName.Validate();
            _email.Validate();

            if (string.IsNullOrEmpty(_password.Value) || string.IsNullOrEmpty(_repeatPassword.Value))
                return;

            if (!_password.Value.Equals(_repeatPassword.Value))
                _repeatPassword.Errors.Add("Hasła są różne.");
        }
    }
}
