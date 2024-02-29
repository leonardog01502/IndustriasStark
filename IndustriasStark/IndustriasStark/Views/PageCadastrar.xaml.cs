using IndustriasStark.Models;
using IndustriasStark.Sevies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.MaskedEntry;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace IndustriasStark.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCadastrar : ContentPage
    {
        ModIndustrias funcionario = new ModIndustrias();

        public PageCadastrar(ModIndustrias funcionario, byte[] foto)
        {
            InitializeComponent();

            txtCodigo.IsVisible = true;
            btnExcluir.IsVisible = true;

            txtCodigo.Text = funcionario.Id.ToString();
            txtNome.Text = funcionario.Nome;
            txtCPF.Text = funcionario.CPF;
            txtTelefone.Text = funcionario.Telefone;
            txtEndereco.Text = funcionario.Endereco;
            txtBairro.Text = funcionario.Bairro;
            txtComplemento.Text = funcionario.Complemento;
            txtEmail.Text = funcionario.Email;
            pkrSetor.SelectedItem = funcionario.Setor;
            pkrTurno.SelectedItem = funcionario.Turno;
            txtDados.Text = funcionario.Dados;
            swFavorito.IsToggled = funcionario.Favorito;

            var nomeArquivo = Path.Combine(FileSystem.AppDataDirectory, "foto.jpg");
            if (File.Exists(nomeArquivo))
            {
                using (var stream = File.OpenRead(nomeArquivo))
                {
                    imgFoto.Source = ImageSource.FromStream(() => stream);
                }
            }
            else
            {
                imgFoto.Source = ImageSource.FromFile("placeholder.jpg");
            }

            btnSalvar.Text = "Alterar";
        }

        public PageCadastrar()
        {
            InitializeComponent();
        }

        private async void btnTirarFoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                var foto = await TirarFotoAsync();

                if (foto != null)
                {
                    var nomeArquivo = Path.Combine(FileSystem.AppDataDirectory, "foto.jpg");
                    using (var destino = File.OpenWrite(nomeArquivo))
                    {
                        await destino.WriteAsync(foto, 0, foto.Length);
                    }
                    imgFoto.Source = ImageSource.FromStream(() => new MemoryStream(foto));
                }
                else
                {
                    imgFoto.Source = ImageSource.FromFile("placeholder.jpg");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async Task<byte[]> TirarFotoAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Erro", "A permissão para acessar a câmera não foi concedida.", "OK");
                return null;
            }

            var foto = await MediaPicker.CapturePhotoAsync();

            if (foto == null)
            {
                await DisplayAlert("Erro", "Nenhuma foto foi capturada.", "OK");
                return null;
            }

            using (var ms = new MemoryStream())
            {
                var stream = await foto.OpenReadAsync();
                await stream.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        private async void btnSalvar_Clicked(object sender, EventArgs e)
        {
            try
            {
                ModIndustrias funcionario = new ModIndustrias();
                funcionario.Nome = txtNome.Text;
                funcionario.CPF = txtCPF.Text;
                funcionario.Telefone = txtTelefone.Text;
                funcionario.Endereco = txtEndereco.Text;
                funcionario.Bairro = txtBairro.Text;
                funcionario.Complemento = txtComplemento.Text;
                funcionario.Email = txtEmail.Text;
                funcionario.Setor = pkrSetor.SelectedItem.ToString();
                funcionario.Turno = pkrTurno.SelectedItem.ToString();
                funcionario.Dados = txtDados.Text;
                funcionario.Favorito = swFavorito.IsToggled;

                byte[] foto = null;
                if (imgFoto.Source != null)
                {
                    foto = await GetFotoBytes(imgFoto.Source);
                    if (foto != null)
                    {
                        var nomeArquivo = Path.Combine(FileSystem.AppDataDirectory, "foto.jpg");
                        using (var destino = File.OpenWrite(nomeArquivo))
                        {
                            await destino.WriteAsync(foto, 0, foto.Length);
                        }
                        imgFoto.Source = ImageSource.FromStream(() => new MemoryStream(foto));
                    }
                    else
                    {
                        imgFoto.Source = ImageSource.FromFile("placeholder.jpg");
                    }
                }

                BdIndustrias dBIndustrias = new BdIndustrias(App.DbPath);
                if (btnSalvar.Text == "Inserir")
                {
                    dBIndustrias.Inserir(funcionario, foto);
                    await DisplayAlert("Resultado", dBIndustrias.StatusMessage, "OK");
                }
                else
                {
                    funcionario.Id = Convert.ToInt32(txtCodigo.Text);
                    dBIndustrias.Alterar(funcionario);
                    await DisplayAlert("Funcionário alterado com sucesso !!!", dBIndustrias.StatusMessage, "OK");
                }
                MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
                p.Detail = new NavigationPage(new PageHome());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task<byte[]> GetFotoBytes(ImageSource source)
        {
            byte[] foto = null;
            try
            {
                var stream = await ((StreamImageSource)source).Stream(CancellationToken.None);
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    foto = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return foto;
        }

        private void btnCancelar_Clicked(object sender, EventArgs e)
        {
            MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
            p.Detail = new NavigationPage(new PageHome());
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

        private void txtTelefone_TextChanged(object sender, TextChangedEventArgs e)
        {
            string texto = txtTelefone.Text;

            string numeros = new string(texto.Where(char.IsDigit).ToArray());

            if (numeros.Length >= 11)
            {
                texto = string.Format("({0}) {1}-{2}", numeros.Substring(0, 2), numeros.Substring(2, 5), numeros.Substring(7, 4));
            }

            txtTelefone.Text = texto;
        }

        private async void btnExcluir_Clicked(object sender, EventArgs e)
        {
            var resp = await DisplayAlert("Excluir Notas", "Deseja excluir a nota selecionada ?", "Sim", "Não");
            if (resp == true)
            {
                BdIndustrias dBNotas = new BdIndustrias(App.DbPath);
                int id = Convert.ToInt32(txtCodigo.Text);
                dBNotas.Excluir(id);
                await DisplayAlert("Nota excluida com sucesso ", dBNotas.StatusMessage, "OK");
                MasterDetailPage p = (MasterDetailPage)Application.Current.MainPage;
                p.Detail = new NavigationPage(new PageHome());
            }
        }
    }
}
