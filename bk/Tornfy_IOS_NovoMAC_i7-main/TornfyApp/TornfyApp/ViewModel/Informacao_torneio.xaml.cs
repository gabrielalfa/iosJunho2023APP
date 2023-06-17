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
    public partial class Informacao_torneio : ContentPage
    {

        public Etapa info_etapa;
        int id_etapa = 0;

        public Informacao_torneio(int id)
        {
            InitializeComponent();

            id_etapa = id;
            CarregarInfoEtapa();
        }

        public async void CarregarInfoEtapa()
        {
            info_etapa = await API_Service.ObterInfoEtapa(id_etapa);
            string valor_compilado = "";

            valor_compilado = string.Format("R$: {0}/{1}/{2}", info_etapa.valor_inscricao_1, info_etapa.valor_inscricao_2, info_etapa.valor_inscricao_3);

            txt_NomeEtapa.Text = info_etapa.Nome_Etapa;
            txt_Grupo.Text = info_etapa.nome_grupo;
            txt_esquemaPT.Text = info_etapa.Nome_Ranking;
            txt_datalimite.Text = info_etapa.Data_Limite;
            txt_Inicio.Text = info_etapa.Inicio;
            txt_Fim.Text = info_etapa.Fim;
            txt_Arbitragem.Text = info_etapa.Arbitragem;
            txt_ArbitragemContato.Text = info_etapa.Telefone_Contato;
            txt_CompilagemValores.Text = valor_compilado;
            txt_ValorInfantil.Text = info_etapa.valor_inscricao_inf;
            txt_NomeClube.Text = "(" + info_etapa.Nome_clube + ") " + info_etapa.string_clube;


            image_cartaz.Source = info_etapa.img_path;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarInfoEtapa();

        }


    }
}