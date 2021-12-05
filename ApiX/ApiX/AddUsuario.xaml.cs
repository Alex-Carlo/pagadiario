using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ApiX
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddUsuario : ContentPage
    {
        Usuario detalles;
        public AddUsuario(Usuario usuario=null)
        {
            InitializeComponent();
            if (usuario != null)
            {
                btnGuardar.Text = "Editar";
                txtNombre.Text = usuario.nombre;
                txtEmail.Text = usuario.email;
                txtUsuario.Text = usuario.usuario;
                txtContra.Text = usuario.contra;
                detalles = usuario;
            }
        }

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            if (btnGuardar.Text == "Guardar")
            {
                btnGuardar.IsEnabled = false;
                var nombre = txtNombre.Text ?? "";
                var email = txtEmail.Text ?? "";
                var usuario = txtUsuario.Text ?? "";
                var contra = txtContra.Text ?? "";
                if (string.IsNullOrEmpty(nombre))
                {
                    await DisplayAlert("Advertencia", "Ingresa un nombre", "Aceptar");
                    return;
                }
                if (string.IsNullOrEmpty(email))
                {
                    await DisplayAlert("Advertencia", "Ingresa un Email", "Aceptar");
                    return;
                }
                if (string.IsNullOrEmpty(usuario))
                {
                    await DisplayAlert("Advertencia", "Ingresa un Usuario", "Aceptar");
                    return;
                }
                if (string.IsNullOrEmpty(contra))
                {
                    await DisplayAlert("Advertencia", "Ingresa una Contrasena", "Aceptar");
                    return;
                }
                UserRequest request = new UserRequest(App.RestClient);
                if (await request.Add(new Usuario
                {
                    nombre = nombre,
                    email = email,
                    usuario = usuario,
                    contra = contra
                }))
                {
                    await DisplayAlert("Mensaje", "Se ha creado un usuario", "Ok");
                }
                else
                {
                    await DisplayAlert("Mensaje", "No se ha podido crear el usuario ", "Ok");
                }
            }
            else
            {
                btnGuardar.IsEnabled = false;
                var nombre = txtNombre.Text ?? "";
                var email = txtEmail.Text ?? "";
                var usuario = txtUsuario.Text ?? "";
                var contra = txtContra.Text ?? "";
                detalles.nombre = txtNombre.Text;
                detalles.email = txtEmail.Text;
                detalles.usuario = txtUsuario.Text;
                detalles.contra = txtContra.Text;

                var result = await new UserRequest(App.RestClient).Update(detalles, detalles.id);
                if (result)
                {
                    await DisplayAlert("Mensaje", "Se ha Actualizado un usuario", "Ok");
                }
                else
                {
                    await DisplayAlert("Mensaje", "No se ha podido Actualizar el usuario ", "Ok");
                }
                
            }
            
            txtContra.Text = "";
            txtEmail.Text = "";
            txtNombre.Text = "";
            txtUsuario.Text = "";
            btnGuardar.IsEnabled = true;
        }
    }
}