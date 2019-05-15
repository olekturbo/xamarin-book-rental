using Books.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Books.ViewModels
{
    class MainPageViewModel : NotifyBase
    {
        private readonly MainPage _mainPage;
        private string _date;
        private string _dateLabel;
        private bool _isWorker;

        public bool IsWorker
        {
            get => _isWorker;
            set
            {
                _isWorker = value;
                NotifyPropertyChange(nameof(IsWorker));
            }
        }

        public string Date
        {
            get => _date;
            set
            {
                _date = value;
                NotifyPropertyChange(nameof(Date));
            }
        }

        public string DateLabel
        {
            get => _dateLabel;
            set
            {
                _dateLabel = value;
                NotifyPropertyChange(nameof(DateLabel));
            }
        }

        private async Task SetRole()
        {
            var foundUser = await App.Database.GetUserByUsernameAsync(Preferences.Get("Username", "default"));
            if (foundUser.Role.Contains("Pracownik"))
            {
                IsWorker = true;
            }
            else
            {
                IsWorker = false;
            }
        }



        public MainPageViewModel(MainPage mainPage)
        {
            _mainPage = mainPage;
            Task.Run(async () => { await SetRole(); });
        }

        public MainPageViewModel(MainPage mainPage, string date)
        {
            _mainPage = mainPage;
            _date = date;
            _dateLabel = "Ostatnia zmiana danych: ";
            Task.Run(async () => { await SetRole(); });
        }

        public ICommand CheckData => new Command(OnClickCheckData);
        public ICommand AddBook => new Command(OnClickAddBook);
        public ICommand CheckUsers => new Command(OnClickCheckUsers);

        public ICommand CheckBooks => new Command(OnClickCheckBooks);
        public ICommand CheckBorrowedBooks => new Command(OnClickCheckBorrowedBooks);

        private void OnClickCheckData(object obj)
        {
            _mainPage.Navigation.PushAsync(new DataPage());
        }

        private void OnClickCheckUsers(object obj)
        {
            _mainPage.Navigation.PushAsync(new CheckUsers());
        }

        private async void OnClickAddBook(object obj)
        {
            _mainPage.Navigation.PushAsync(new AddBook());
        }

        private void OnClickCheckBooks(object obj)
        {
            _mainPage.Navigation.PushAsync(new CheckBooks());
        }

        private void OnClickCheckBorrowedBooks(object obj)
        {
            _mainPage.Navigation.PushAsync(new CheckBorrowedBooks());
        }
    }
}
