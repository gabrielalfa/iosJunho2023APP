using Acr.UserDialogs.Extended;
using API_Inteleco.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Faq_perguntas : ContentPage
    {
        public int item;
        public List<FAQ> Lista_perguntas;
        public string url;
        public List<Clube> Lista_Clubes;
        public int id_master = 1;

        public Faq_perguntas(int item)
        {
            InitializeComponent();

            switch (item)
            {
                case 3:
                    Title = "Buscar um Organizador";
                    this.item = item;
                    perguntas.IsVisible = false;
                    resposta.IsVisible = false;
                    link_text.IsVisible = false;
                    btn_revisao.IsVisible = false;
                    area_form.IsVisible = false;
                    area_clubes.IsVisible = true;
                    CarregarClubes(id_master, "vazio");
                    break;
                case 6:
                    Title = "Formulario de BUGs";
                    this.item = item;
                    if (TestarInternet()) CarregarPerguntas(item);

                    //abertutra de chamado
                    perguntas.IsVisible = false;
                    resposta.IsVisible = false;
                    link_text.IsVisible = false;
                    btn_revisao.IsVisible = false;
                    area_form.IsVisible = true;
                    area_clubes.IsVisible = false;
                    break;
                default:
                    this.item = item;
                    if (TestarInternet()) CarregarPerguntas(item);
                    Title = "Perguntas e Respostas";
                    //chamada para as perguntas e respostas
                    perguntas.IsVisible = true;
                    resposta.IsVisible = false;
                    link_text.IsVisible = false;
                    btn_revisao.IsVisible = false;
                    area_form.IsVisible = false;
                    area_clubes.IsVisible = false;
                    break;
            }





        }


        public async void CarregarClubes(int id_master, string busca)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            Lista_Clubes = await API_Service.ObterClubes_qtd(id_master, busca);
            UserDialogs.Instance.HideLoading();

            listagem_clubes.ItemsSource = Lista_Clubes.ToList();

        }

        public async void CarregarPerguntas(int item)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Questões");
            Lista_perguntas = await API_Service.ObterQuestoes_FAQ(item);
            UserDialogs.Instance.HideLoading();
            carroucel_days.ItemsSource = Lista_perguntas.ToList();
        }

        private void carroucel_days_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void carroucel_days_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            perguntas.IsVisible = false;
            resposta.IsVisible = true;

            FAQ item = ((ListView)sender).SelectedItem as FAQ;
            if (item == null)
            {
                return;
            }
            else
            {
                txt_resposta.Text = item.resposta;
                link_text.IsVisible = false;
                if (item.url_bool)
                {
                    link_text.IsVisible = true;
                    link_text.Text = item.mascara_url;
                    url = "https://tornfy.com/" + item.url;
                }
            }
        }

        private void btn_revisao_Clicked(object sender, EventArgs e)
        {
            perguntas.IsVisible = true;
            resposta.IsVisible = false;
            link_text.IsVisible = false;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Browser.OpenAsync(new Uri(url), BrowserLaunchMode.SystemPreferred);
        }

        private void btnEnviarEmail_Clicked(object sender, EventArgs e)
        {
            var nome = txt_nomeJogador.Text;
            var email = txt_email.Text;
            var telefone = txt_telefone.Text;
            var mensagem = txt_msg.Text;
            bool vazio = false;

            if (string.IsNullOrEmpty(nome)) vazio = true;
            if (string.IsNullOrEmpty(email)) vazio = true;
            if (string.IsNullOrEmpty(telefone)) vazio = true;
            if (string.IsNullOrEmpty(mensagem)) vazio = true;

            if (!vazio)
            {
                EnviarEmail(nome, email, telefone, mensagem);
            }
            else
            {
                var pop2 = new MessageBox("Preencha os Campos!",
                    "Preeencha todos os campos obrigatórios.");
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
            }

        }

        public async void EnviarEmail(string nome, string email, string telefone, string mensagem)
        {
            FAQ dados = new FAQ
            {
                email = email,
                telefone = telefone,
                nome = nome,
                mensagem = mensagem
            };

            UserDialogs.Instance.ShowLoading(title: "Enviando Email");
            int retorno = await API_Service.EnviarEmail(dados);
            UserDialogs.Instance.HideLoading();
            if (retorno == 200)
            {
                var pop2 = new MessageBox("Mensagem Enviada!",
                    "Sua mensagem foi enviada com sucesso para nosso time de suporte. Lembrando que se ela estiver dentro dos parametros de envios corretos, ela será respondida o mais breve possível.");
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
            }
            else
            {
                var pop2 = new MessageBox("Mensagem não enviada!",
                    "Sua mensagem não foi enviada. Favor, tente mais tarde!");
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
            }

            await Navigation.PopToRootAsync();

        }

        public static bool TestarInternet()
        {
            bool coneccao = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                coneccao = false;
                var pop2 = new MensagemComConfirmacao("Conecção Caiu!",
                    "Algo deu errado! Não há conecção de interenet.", "#E10555", "Fechar Mensagem?", false, "", false, "", 0);
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
            }
            return coneccao;
        }

        private void listagem_clubes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void listagem_clubes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Clube clube = ((ListView)sender).SelectedItem as Clube;
            if (clube == null)
            {
                return;
            }

            UserDialogs.Instance.ShowLoading(title: "Buscando Informações");
            await Navigation.PushAsync(new Clubes_Contato(clube.email, clube.Telefone, clube.Responsavel));
            UserDialogs.Instance.HideLoading();
        }

        private void txt_busca_SearchButtonPressed(object sender, EventArgs e)
        {
            CarregarClubes(id_master, txt_busca.Text);
        }

        private void listagem_clubes_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var itens = this.Lista_Clubes as IList;

            if (API_Service.qtd_chamada <= itens.Count)
            {
                if (itens != null && e.Item == itens[itens.Count - 1])
                {
                    CarregarClubes(id_master, txt_busca.Text);
                }
            }
        }
    }
}