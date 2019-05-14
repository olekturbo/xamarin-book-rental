using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Books.ViewModels;

namespace Books
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var mainPageViewModel = new MainPageViewModel(this);
            BindingContext = mainPageViewModel;
        }

        public MainPage(string date)
        {
            InitializeComponent();
            var mainPageViewModel = new MainPageViewModel(this, date);
            BindingContext = mainPageViewModel;
        }
    }
}