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
    public partial class CheckUsers : ContentPage
    {
        public CheckUsers()
        {
            InitializeComponent();
            var checkUsersViewModel = new CheckUsersViewModel(this);
            BindingContext = checkUsersViewModel;
        }
    }
}