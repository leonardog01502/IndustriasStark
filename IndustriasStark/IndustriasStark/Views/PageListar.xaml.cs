using IndustriasStark.Sevies;
using IndustriasStark.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;

namespace IndustriasStark.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageListar : ContentPage
    {
        public PageListar()
        {
            InitializeComponent();
            AtualizaLista();
        }

        public void AtualizaLista()
        {
            String cpf = "";
            if (txtCPF.Text != null) cpf = txtCPF.Text;
            BdIndustrias dBNotas = new BdIndustrias(App.DbPath);
            if (swFavorito.IsToggled)
            {
                ListaFuncionarios.ItemsSource = dBNotas.Localizar(cpf, true);
            }
            else
            {
                ListaFuncionarios.ItemsSource = dBNotas.Localizar(cpf);
            }
        }

        private void swFavorito_Toggled(object sender, ToggledEventArgs e)
        {
            AtualizaLista();
        }

        private void btnLocalizar_Clicked(object sender, EventArgs e)
        {
            AtualizaLista();
        }

        private void txtCPF_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            var newText = e.NewTextValue;

            var cleanText = Regex.Replace(newText, "[^0-9]", "");

            if (cleanText.Length == 11)
            {
                var formattedText = String.Format("{0:000\\.000\\.000\\-00}", long.Parse(cleanText));

                entry.Text = formattedText;
            }
            else if (cleanText.Length > 11)
            {
                entry.Text = cleanText.Substring(0, 11);
            }
            else
            {
                entry.Text = newText;
            }
            entry.CursorPosition = entry.Text.Length;
        }

        private void ListaFuncionarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ModIndustrias funcionarios = (ModIndustrias)ListaFuncionarios.SelectedItem;
            MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
            p.Detail = new NavigationPage(new PageCadastrar(funcionarios, funcionarios.Foto));
        }
    }
}