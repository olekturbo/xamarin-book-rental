using System;
using System.IO;
using Xamarin.Forms;
using Books.ViewModels;

namespace Books
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            var loginViewModel = new LoginViewModel(this);
            BindingContext = loginViewModel;

        }
    }
}