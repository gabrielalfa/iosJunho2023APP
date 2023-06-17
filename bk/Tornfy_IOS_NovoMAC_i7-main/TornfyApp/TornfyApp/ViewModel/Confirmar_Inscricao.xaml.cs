using Acr.UserDialogs.Extended;
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
    public partial class Confirmar_Inscricao : ContentPage
    {

        public Etapa info_etapa = new Etapa();
        int id_etapa = 0;
        int id_jogador = 0;
        string imagem_cartaz = "";
        string nome_etapa = "";
        string tamanho = "";
        string tamanho_dupla = "";
        int categoria = 0;
        string nome_categoria = "";
        int id_dupla = 0;
        public List<Temporada> lista_circuitos;
        public string nome_jogador;
        public int id_tipo;
        public int id_master;

        public Confirmar_Inscricao(int id_master, int Id_etapa, int Id_jogador, int Categoria, string Categoria_Nome, string Tamanho, string _nome_jogador, int _id_tipo)
        {
            InitializeComponent();

            id_etapa = Id_etapa;
            id_jogador = Id_jogador;
            tamanho = Tamanho;
            nome_categoria = Categoria_Nome;
            this.id_master = id_master;
            categoria = Categoria;
            nome_jogador = _nome_jogador;
            id_tipo = _id_tipo;

            CarregarInfoEtapa();

        }

        public async void CarregarInfoEtapa()
        {
            info_etapa = await API_Service.ObterInfoEtapa(id_etapa);

            path_img.Source = info_etapa.path_clube;
            image_cartaz.Source = info_etapa.img_path;

            imagem_cartaz = info_etapa.img_path;

            txt_nome_etapa.Text = "Etapa: " + info_etapa.Nome_Etapa;
            txt_NomeClube.Text = "(" + info_etapa.Nome_clube + ") " + info_etapa.string_clube;
            txt_tamanho.Text = tamanho;
            nome_etapa = info_etapa.Nome_Etapa;
            txt_nome_categoria.Text = nome_categoria;
            txtNome_jogador.Text = nome_jogador;

        }

        private async void btnCancelar_Inscrição_Clicked(object sender, EventArgs e)
        {
            Config_Home config = new Config_Home();
            config.master = id_master;
            await Navigation.PushAsync(new MasterPage(config, id_jogador, 1));
        }

        private async void btnConfirmar_Inscrição_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(title: "Finalizando Inscrição");
            MessageBox pop;

            var form = new Formulario_Inscricao
            {
                tamanho = tamanho,
                tamanho_dupla = tamanho_dupla,
                id_etapa = id_etapa,
                id_jogador = id_jogador,
                id_cat = categoria,
                id_dupla = 1,
            };

            int response = await API_Service.AdicionarInscricao(form);

            //200: sucesso
            //1: bloqueio de letra
            //2: redundante
            //8 inscrição sem parceiro
            //3: corum total
            //4: córum parcial



            switch (response)
            {
                case 200:
                    Config_Home config = new Config_Home();
                    config.master = id_master;
                    config.id_etapa = id_etapa;
                    config.id_tipo = id_tipo;

                    await Navigation.PushAsync(new MasterPage(config, id_jogador, 3), true);


                    break;
                case 1:
                    pop = new MessageBox("Bloqueio de Letra associado a inscrição!", "Esta categoria não permite que seja realizado incrições em letras diferentes.");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                    break;
                case 2:
                    pop = new MessageBox("Inscrição Redundante", "Esta inscrição já foi realizada nesta etapa.");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                    break;
                case 8:
                    pop = new MessageBox("Inscrição de Duplas sem parceiro", "Escolha um parceiro para jogar.");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                    break;
                case 3:
                    pop = new MessageBox("Córun total de inscritos atingido!", "O limite de inscrição desta etapa foi atingido!");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                    break;
                case 4:
                    pop = new MessageBox("Córum por categoria atingido!", "Limite por categoria atingido.");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                    break;
                case 550:
                    pop = new MessageBox("Falha na inscrição", "A incrição não foi realizada por falha no servidor, tente novamente mais tarde.");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                    break;
                default:
                    break;
            }



            UserDialogs.Instance.HideLoading();
        }
    }
}