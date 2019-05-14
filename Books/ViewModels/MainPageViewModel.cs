using Books.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Books.ViewModels
{
    class MainPageViewModel : NotifyBase
    {
        private readonly MainPage _mainPage;
        private string _date;
        private string _dateLabel;

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

        public MainPageViewModel(MainPage mainPage)
        {
            _mainPage = mainPage;
        }

        public MainPageViewModel(MainPage mainPage, string date)
        {
            _mainPage = mainPage;
            _date = date;
            _dateLabel = "Ostatnia zmiana danych: ";
        }

        public ICommand CheckData => new Command(OnClickCheckData);
        public ICommand AddBook => new Command(OnClickAddBook);

        public ICommand CheckBooks => new Command(OnClickCheckBooks);
        public ICommand CheckBorrowedBooks => new Command(OnClickCheckBorrowedBooks);

        private void OnClickCheckData(object obj)
        {
            _mainPage.Navigation.PushAsync(new DataPage());
        }

        private void OnClickAddBook(object obj)
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
