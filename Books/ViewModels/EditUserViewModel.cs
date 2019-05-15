using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Books.Models;
using Books.Validation;
using Xamarin.Forms;

namespace Books.ViewModels
{
    public class EditUserViewModel : NotifyBase
    {

        private readonly EditUser _editUser;
        private ValidatableObject<string> _username = new ValidatableObject<string>();
        private ValidatableObject<string> _firstName = new ValidatableObject<string>();
        private ValidatableObject<string> _lastName = new ValidatableObject<string>();
        private ValidatableObject<string> _email = new ValidatableObject<string>();
        private string _chooseRole;
        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                NotifyPropertyChange(nameof(Id));
            }
        }

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

        public ICommand Update => new Command(OnClickUpdate);

        public ICommand Back => new Command(OnClickBack);

        public EditUserViewModel(EditUser editUser, int id)
        {
            _editUser = editUser;
            _id = id;
            AddValidationRules();
            Task.Run(async () => { await GetUser(); });
        }

        private void AddValidationRules()
        {
            _username.Rules.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Nazwa użytkownika jest wymagana."
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

        }

        private void OnClickBack(object obj)
        {
            _editUser.Navigation.PopAsync();
        }

        private async Task GetUser()
        {
            var foundUser = await App.Database.GetUserAsync(_id);
            _firstName.Value = foundUser.FirstName;
            _lastName.Value = foundUser.LastName;
            _username.Value = foundUser.Username;
            _email.Value = foundUser.Email;
            _chooseRole = foundUser.Role;

        }

        async private void OnClickUpdate(object obj)
        {
            ValidateEntries();

            var isError = _username.Errors.Any() || _firstName.Errors.Any() ||
                _lastName.Errors.Any() || _email.Errors.Any();

            if (isError)
                return;

            User user = await App.Database.GetUserAsync(_id);
            user.Username = _username.Value;
            user.FirstName = _firstName.Value;
            user.LastName = _lastName.Value;
            user.Email = _email.Value;
            user.Role = _chooseRole;
            await App.Database.SaveUserAsync(user);

            _editUser.DisplayAlert("Komunikat", "Uzytkownik zaktualizowany.", "OK");
            _editUser.Navigation.PopAsync();

        }

        private void ValidateEntries()
        {
            _username.Validate();
            _firstName.Validate();
            _lastName.Validate();
            _email.Validate();

        }
    }
}
