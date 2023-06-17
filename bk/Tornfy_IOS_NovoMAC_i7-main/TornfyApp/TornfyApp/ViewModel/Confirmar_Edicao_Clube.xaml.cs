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
    public partial class Confirmar_Edicao_Clube : ContentPage
    {
        public Etapa info_etapa = new Etapa();
        int id_jogador = 0;
        public Config_Home config;
        public List<Temporada> lista_circuitos;
        int tipo_clube = 0;
        int id_clube = 0;


        public Confirmar_Edicao_Clube(Config_Home Config, int Id_jogador, int Tipo_clube, string nome_clube, int Id_clube)
        {
            InitializeComponent();

            id_jogador = Id_jogador;
            config = Config;
            tipo_clube = Tipo_clube;
            id_clube = Id_clube;

            txt_nome_clube.Text = nome_clube;
        }

        private async void btnCancelar_edicao_Clicked(object sender, EventArgs e)
        {
            int master = config.master;

            if (master == 4)
            {
                master = 1;
            }

            List<Temporada> lista_circuitos1;

            lista_circuitos1 = await API_Service.ObterCircuitos_Menu(master);
            lista_circuitos = lista_circuitos1.ToList();


            await Navigation.PushAsync(new MasterPage(config, id_jogador, 1));
        }

        private async void btnConfirmar_edicao_Clicked(object sender, EventArgs e)
        {

            int master = config.master;

            if (master == 4)
            {
                master = 1;
            }

            Jogador dados = new Jogador();

            dados.id_jogador = id_jogador;
            dados.id_Clube = id_clube;
            dados.tipo_clube = tipo_clube;

            int result = await API_Service.EditarClube(dados);

            if (result == 500)
            {
                await DisplayAlert("Edição não realizada", "Falha ao editar clube!!!", "Fechar");
            }
            else
            {
                await DisplayAlert("Edição realizada", "Clube Editado com sucesso!!!", "Fechar");
            }


            await Navigation.PushAsync(new MasterPage(config, id_jogador, 1));
        }
    }
}