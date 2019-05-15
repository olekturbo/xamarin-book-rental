using Books.Models;
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
    public partial class EditUser : ContentPage
    {
        public EditUser(int id)
        {
            InitializeComponent();
            var editUserViewModel = new EditUserViewModel(this, id);
            BindingContext = editUserViewModel;
        }
    }
}