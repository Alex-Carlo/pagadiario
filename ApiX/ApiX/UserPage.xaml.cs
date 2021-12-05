using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ApiX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        ObservableCollection<Usuario> _items;
        public UserPage(List<Usuario> usuarios)
        {
            InitializeComponent();
            _items = new ObservableCollection<Usuario>();

            lista.ItemsSource = _items;
            //para refrescar, true
            lista.IsPullToRefreshEnabled = true;

            lista.Refreshing += Lista_Refreshing;
            lista.ItemSelected += Lista_ItemSelected;
            lista.ItemTapped += Lista_ItemTapped;
            //Iteramos la lista
            
            foreach (var usuario in usuarios)
            {
                _items.Add(usuario);
            }
            
        }

        private void Lista_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Usuario details = e.Item as Usuario;
            if (details != null)
            {
                Navigation.PushAsync(new AddUsuario(details));
            }
        }

        private void Lista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //para eviatar que se coloree
            lista.SelectedItem = null;
        }

        private async void Lista_Refreshing(object sender, EventArgs e)
        {
           await Refrescar();
            lista.EndRefresh();
        }
       
        private  async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddUsuario(), true);
        }

        private async Task Refrescar()
        {
            _items.Clear();
            var listusers = await new UserRequest(App.RestClient).All();
            foreach (var item in listusers)
            {
                _items.Add(item);
            }
            lista.EndRefresh();
        }

        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            bool res = await DisplayAlert("Message", "Seguro que quieres eliminar el usuario?", "Ok", "Cancel");
            if (res)
            {
                var menu = sender as MenuItem;
                Usuario details = menu.CommandParameter as Usuario;

                var result = await new UserRequest(App.RestClient).Delete(details.id);
                await Refrescar();
            }
        }

        
    }
}