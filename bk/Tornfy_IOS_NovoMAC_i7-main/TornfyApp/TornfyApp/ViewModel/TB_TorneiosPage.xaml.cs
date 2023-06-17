
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TB_TorneiosPage : TabbedPage
    {
        public Config_Home config;
        public List<etapas_favoritas> etp_favs;
        public int _id_etapa;
        public int _id;
        public string _nome_etapa;
        public int id_tipo;
        public int _id_jogador;
        public bool anexo;

        public string nome_anexo;
        public string nome_principal;


        public TB_TorneiosPage(string nome_etapa, int id, int id_jogador, int _id_tipo, bool favorito, bool publico_programacao, bool anexo, string nome_anexo, string nome_principal)
        {
            InitializeComponent();

            ToolbarItems.Remove(no_fav);
            ToolbarItems.Remove(fav);

            if (favorito)
            {
                ToolbarItems.Remove(no_fav);
                ToolbarItems.Add(fav);
            }
            else
            {
                ToolbarItems.Add(no_fav);
                ToolbarItems.Remove(fav);
            }

            _id_etapa = id;
            _nome_etapa = nome_etapa;
            id_tipo = _id_tipo;
            _id_jogador = id_jogador;
            int id_master = 1;
            this.anexo = anexo;

            this.nome_anexo = nome_anexo;
            this.nome_principal = nome_principal;

            this.Children.Add(new Incricao_Torneio(nome_etapa, id, id_jogador, _id_tipo, id_master) { Title = "Inscrição", IconImageSource = "menu_regulamento.png"});
            this.Children.Add(new Informacao_torneio(id) { Title = "Info", IconImageSource = "menu_court.png" });
            this.Children.Add(new Lista_JogadoresTorneio(id) { Title = "Jogadores", IconImageSource = "menu_carduser.png" });
            this.Children.Add(new RelatoriosPage(id, id_jogador, _id_tipo, nome_etapa, publico_programacao, anexo, nome_anexo, nome_principal) { Title = "Chaves", IconImageSource = "menu_regulamento.png" });

           

        }

       
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //DisplayAlert("Id Etapa Selecionada", etp.id_etapa.ToString(), "OK");
        }

        private async void fav_Clicked(object sender, EventArgs e)
        {
            if (_id_etapa != 0)
            {
                var dados = new Etapa
                {
                    id_etapa = _id_etapa,
                    etapa_bool = false,
                    id_jogador = _id_jogador
                };

                int result = await API_Service.EditarFavorito(dados);
                if (result == 200)
                {
                    ToolbarItems.Add(no_fav);
                    ToolbarItems.Remove(fav);
                    var pop = new MessageBox("Etapa Desfavoritada!", "Etapa foi retirada da sua lista de etapas favoritas!");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                }
                else
                {
                    var pop = new MessageBox("Falha na requisição!", "Houve uma falha no momento na solicitação, aguarde uns instantes e tente novamente!");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                }

            }




        }

        private async void no_fav_Clicked(object sender, EventArgs e)
        {
            if (_id_etapa != 0)
            {
                var dados = new Etapa
                {
                    id_etapa = _id_etapa,
                    etapa_bool = true,
                    id_jogador = _id_jogador
                };

                int result = await API_Service.EditarFavorito(dados);
                if (result == 200)
                {
                    ToolbarItems.Add(fav);
                    ToolbarItems.Remove(no_fav);
                    var pop = new MessageBox("Etapa Favoritada!", "Agora esta etapa está disponível em suas etapas favoritas para rápido acesso!");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                }
                else
                {
                    var pop = new MessageBox("Falha na requisição!", "Houve uma falha no momento na solicitação, aguarde uns instantes e tente novamente!");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                }

            }
        }


        private void meus_dados_Clicked(object sender, EventArgs e)
        {

        }
    }
}