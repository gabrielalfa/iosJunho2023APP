
using TornfyApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ICT.ViewModel
{
    public class DetailsViewModel : BaseViewModel
    {
        public const string url = "http://ict.web-ded-180026a.kinghost.net/";

        private Task<ObservableCollection<Etapa>> etapas;
        public Config_Home config = new Config_Home();
        public ObservableCollection<Etapa> etapa_recente1;

        public DetailsViewModel()
        {

            etapas = ObterTorneiosRecentes(5, 1);

        }


        public static async Task<ObservableCollection<Etapa>> ObterTorneiosRecentes(int qtd, int id_master)
        {

            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("/api/Torneios/{0}/{1}/proximos_eventos", qtd, id_master);
                string response = await client.GetStringAsync(Url);
                ObservableCollection<Etapa> etapa_recente = JsonConvert.DeserializeObject<ObservableCollection<Etapa>>(response);

                foreach (var item in etapa_recente)
                {

                    var nome_mes_inicio = "";

                    if (Convert.ToDateTime(item.Inicio).Month == 1) { nome_mes_inicio = "Jan"; }
                    if (Convert.ToDateTime(item.Inicio).Month == 2) { nome_mes_inicio = "Fev"; }
                    if (Convert.ToDateTime(item.Inicio).Month == 3) { nome_mes_inicio = "Mar"; }
                    if (Convert.ToDateTime(item.Inicio).Month == 4) { nome_mes_inicio = "Abr"; }
                    if (Convert.ToDateTime(item.Inicio).Month == 5) { nome_mes_inicio = "Mai"; }
                    if (Convert.ToDateTime(item.Inicio).Month == 6) { nome_mes_inicio = "Jun"; }
                    if (Convert.ToDateTime(item.Inicio).Month == 7) { nome_mes_inicio = "Jul"; }
                    if (Convert.ToDateTime(item.Inicio).Month == 8) { nome_mes_inicio = "Ago"; }
                    if (Convert.ToDateTime(item.Inicio).Month == 9) { nome_mes_inicio = "Set"; }
                    if (Convert.ToDateTime(item.Inicio).Month == 10) { nome_mes_inicio = "Out"; }
                    if (Convert.ToDateTime(item.Inicio).Month == 11) { nome_mes_inicio = "Nov"; }
                    if (Convert.ToDateTime(item.Inicio).Month == 12) { nome_mes_inicio = "Dez"; }

                    var nome_mes_fim = "";

                    if (Convert.ToDateTime(item.Fim).Month == 1) { nome_mes_fim = "Jan"; }
                    if (Convert.ToDateTime(item.Fim).Month == 2) { nome_mes_fim = "Fev"; }
                    if (Convert.ToDateTime(item.Fim).Month == 3) { nome_mes_fim = "Mar"; }
                    if (Convert.ToDateTime(item.Fim).Month == 4) { nome_mes_fim = "Abr"; }
                    if (Convert.ToDateTime(item.Fim).Month == 5) { nome_mes_fim = "Mai"; }
                    if (Convert.ToDateTime(item.Fim).Month == 6) { nome_mes_fim = "Jun"; }
                    if (Convert.ToDateTime(item.Fim).Month == 7) { nome_mes_fim = "Jul"; }
                    if (Convert.ToDateTime(item.Fim).Month == 8) { nome_mes_fim = "Ago"; }
                    if (Convert.ToDateTime(item.Fim).Month == 9) { nome_mes_fim = "Set"; }
                    if (Convert.ToDateTime(item.Fim).Month == 10) { nome_mes_fim = "Out"; }
                    if (Convert.ToDateTime(item.Fim).Month == 11) { nome_mes_fim = "Nov"; }
                    if (Convert.ToDateTime(item.Fim).Month == 12) { nome_mes_fim = "Dez"; }

                    string data = @Convert.ToDateTime(item.Inicio).Day + "-" + nome_mes_inicio + "/" + @Convert.ToDateTime(item.Fim).Day + "-" + nome_mes_fim;
                    string limite = @Convert.ToDateTime(item.Data_Limite).Day + "-" + @nome_mes_fim;

                    item.data = data;
                    item.limite = limite;

                    if (item.publico)
                    {
                        if (item.andamento)
                        {
                            item.color = "#64B5F6";
                            item.situacao = "Andamento";
                            item.nome_botao = "INSCREVA-SE";
                            item.opacity = "1";
                        }
                        else
                        {
                            item.color = "#64B5F6";
                            item.situacao = "Aberto";
                            item.nome_botao = "INSCREVA-SE";
                            item.opacity = "1";
                        }
                    }
                    else
                    {
                        item.color = "#F06292";
                        item.situacao = "Fechado";
                        item.nome_botao = "INFORMAÇÕES";
                        item.opacity = "0.5";
                    }

                }

                return etapa_recente;
            }
            catch (Exception)
            {
                throw;
            }

        }



        public Task<ObservableCollection<Etapa>> Etapas
        {
            get { return etapas; }
            set
            {
                etapas = value;
                OnPropertyChanged();
            }
        }

        private Etapa selectedEtapas;
        public Etapa SelectedEtapas
        {
            get { return selectedEtapas; }
            set
            {
                selectedEtapas = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectionCommand => new Command(DisplayEtapas);

        private void DisplayEtapas()
        {
            //if (selectedEtapas != null)
            //{

            //    config.master = selectedEtapas.id_master;
            //    config.path = selectedEtapas.path;
            //    config.circuito = selectedEtapas.circuito;
            //    config.color = selectedEtapas.color;
            //    config.obs = selectedEtapas.obs;
            //    config.info = selectedEtapas.info;

            //    var id = selectedEtapas.id;
            //    var detailsPage = new TB_TorneiosPage(config, id, 1, selectedEtapas.id_tipo);

            //    var navigation = Application.Current.MainPage as NavigationPage;
            //    navigation.PushAsync(detailsPage, true);

            //}
        }

        public ICommand Pesquisarommand => new Command(Pesquisar);

        private void Pesquisar()
        {
            //var detailsPage = new ListaTorneios(config, 1);
            //var navigation = Application.Current.MainPage as NavigationPage;
            //navigation.PushAsync(detailsPage, true);
        }


        //private int position;
        //public int Position
        //{
        //    get
        //    {
        //        if (position != etapas.Result.IndexOf(selectedEtapas))
        //            return etapas.Result.IndexOf(selectedEtapas);

        //        return position;
        //    }
        //    set
        //    {
        //        position = value;
        //        selectedEtapas = etapas.Result[position];



        //        OnPropertyChanged();
        //        OnPropertyChanged(nameof(selectedEtapas));

        //    }
        //}

        //public ICommand ChangePositionCommand => new Command(ChangePosition);

        //private void ChangePosition(object obj)
        //{
        //    string direction = (string)obj;

        //    if (direction == "L")
        //    {
        //        if (position == 0)
        //        {
        //            Position = etapas.Result.Count - 1;
        //            return;
        //        }

        //        Position -= 1;
        //    }
        //    else if (direction == "R")
        //    {
        //        if (position == etapas.Result.Count - 1)
        //        {
        //            Position = 0;
        //            return;
        //        }

        //        position += 1;
        //        selectedEtapas = etapas.Result[position];



        //        OnPropertyChanged();
        //        OnPropertyChanged(nameof(selectedEtapas));

        //    }

        //}





    }
}
