using Acr.UserDialogs.Extended;
using Rg.Plugins.Popup.Extensions;
using System;
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
    public class lista_favoritas
    {

    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Favoritos : ContentPage
    {
        private lista_favoritas newRecipe;
        public Config_Home _config;
        public int id_jogador;
        public List<Salvos> Lista_torneios;
        List<Salvos> lista_circuitos;
        public bool etapa_path = true;

        public List<etapas_favoritas> Directions { get; set; }
        public List<circuitos_favoritos> Ingredients { get; set; }

        public Favoritos(Config_Home config, int _id_jogador)
        {

            InitializeComponent();


            _config = config;
            id_jogador = _id_jogador;
            if (TestarInternet()) CarregarTorneiosFavoritos();

            directionsButton.IsEnabled = false;
            ingredientsButton.IsEnabled = true;

        }



        protected override void OnAppearing()
        {
            if (TestarInternet()) CarregarTorneiosFavoritos();
            directionsButton.IsEnabled = false;
            ingredientsButton.IsEnabled = true;
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

        public async void CarregarTorneiosFavoritos()
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            Lista_torneios = await API_Service.ObterSalvosEtapas(id_jogador);
            foreach (var item in Lista_torneios)
            {
                item.path_grupo = item.img_path;
                item.label_inscricao = "Inscrição: ";
                item.label_circuito = "Circuito: ";
            }

            theList.ItemsSource = Lista_torneios.ToList();
            UserDialogs.Instance.HideLoading();
        }

        public async void CarregarCircuitosFavoritos()
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Circuitos");
            lista_circuitos = await API_Service.ObterSalvosCircuitos(id_jogador);
            foreach (var item in lista_circuitos)
            {
                item.data = "ativo";
                item.nome_mes_inicio = "--";
                item.color = "#3e494f";
                item.nome_etapa = item.nome_grupo;
                item.limite = "jan/dez " + DateTime.Today.Year;
                item.situacao = "ativo";
                item.nome_grupo = item.Nome_Ranking;
                item.path_grupo = item.img_path;
                item.label_inscricao = "Status: ";
                item.label_circuito = "Esquema: ";

                item.circuito = item.circuito;
                item.info = item.info;
                item.favorito = true;

            }
            theList.ItemsSource = lista_circuitos.ToList().Where(x => x.favorito);
            UserDialogs.Instance.HideLoading();
        }


        protected void Ingredients_Clicked(object sender, EventArgs e)
        {
            //theList.ItemsSource = newRecipe.Ingredients;

            if (TestarInternet()) CarregarCircuitosFavoritos();

            etapa_path = false;
            directionsButton.IsEnabled = true;
            ingredientsButton.IsEnabled = false;
        }

        protected void Directions_Clicked(object sender, EventArgs e)
        {
            //theList.ItemsSource = newRecipe.Directions;
            if (TestarInternet()) CarregarTorneiosFavoritos();

            etapa_path = true;
            directionsButton.IsEnabled = false;
            ingredientsButton.IsEnabled = true;
        }

        private void theList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }


        private async void theList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (etapa_path)
            {
                Salvos etapa = ((ListView)sender).SelectedItem as Salvos;
                if (etapa == null)
                {
                    return;
                }

                _config.nome_etapa = etapa.nome_etapa;
                await Navigation.PushAsync(new TB_TorneiosPage(etapa.nome_etapa, etapa.id_etapa, id_jogador, etapa.id_tipo, true, etapa.publico_programacao, etapa.anexo, etapa.nome_anexo, etapa.nome_principal));
            }
            else
            {

                Salvos temporada = ((ListView)sender).SelectedItem as Salvos;
                if (temporada == null)
                {
                    return;
                }
                _config.info = temporada.info;
                _config.circuito = temporada.circuito;


                Temporada temporada_concat = new Temporada
                {
                    grupo_nome = temporada.nome_grupo,
                    Nome_Ranking = temporada.Nome_Ranking,
                    id_ranking = temporada.id_ranking,
                    id_master = 1,
                    tipo_etapa = temporada.id_tipo
                };


                await Navigation.PushAsync(new Circuito(id_jogador, temporada.id_grupo, "", "", "", temporada.path_grupo, temporada.nome_grupo, temporada_concat, temporada.favorito, temporada.id_circuito, temporada.id_tipo));


            }







        }
    }
}