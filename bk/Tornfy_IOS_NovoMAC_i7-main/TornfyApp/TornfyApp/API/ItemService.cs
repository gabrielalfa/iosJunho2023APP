using API_Inteleco.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TornfyApp.Model;
using TornfyApp.ViewModel;

namespace TornfyApp.API
{
    public class ItemService
    {
        public static int _tipo_etapa_sel;
        private static ObservableCollection<MasterPageItem> menuLista { get; set; }

        public static ObservableCollection<MasterPageItem> GetMenuItens(int id_jogador, Config_Home config)
        {
            menuLista = new ObservableCollection<MasterPageItem>();
            var pagina1 = new MasterPageItem() { Title = "Tornfy", Icon = "menu_diagrama.png", TargetType = typeof(HomePage), args = new object[] { config, id_jogador } };
            var pagina9 = new MasterPageItem() { Title = "Dados Pessoais", Icon = "ball_tennis.png", TargetType = typeof(Dados_Jogador), args = new object[] { config, id_jogador } };
            var pagina10 = new MasterPageItem() { Title = "MInhas Inscrições", Icon = "ball_tennis.png", TargetType = typeof(Minhas_Inscricoes), args = new object[] { config, id_jogador } };
            var pagina11 = new MasterPageItem() { Title = "Meus Rankings", Icon = "ball_tennis.png", TargetType = typeof(Etapas_Pontuadas) };
            var main_page = new MasterPageItem() { Title = "Torneios & Circuitos", Icon = "ball_tennis.png", TargetType = typeof(MainPage), args = new object[] { config, id_jogador } };

            var pg_locacao = new MasterPageItem() { Title = "Locação Quadras", Icon = "menu_calendario.png", TargetType = typeof(Locacoes) };
            var pg_tenis = new MasterPageItem() { Title = "Tênis", Icon = "menu_tenis.png", TargetType = typeof(Circuitos_Modalidade), args = new object[] { id_jogador, _tipo_etapa_sel = 1 } };
            var pg_beach = new MasterPageItem() { Title = "Beach Tênis", Icon = "menu_beach.png", TargetType = typeof(Circuitos_Modalidade), args = new object[] { id_jogador, _tipo_etapa_sel = 2 } };
            var pg_padel = new MasterPageItem() { Title = "Padel", Icon = "menu_padel.png", TargetType = typeof(Circuitos_Modalidade), args = new object[] { id_jogador, _tipo_etapa_sel = 3 } };
            var pg_squash = new MasterPageItem() { Title = "Squash", Icon = "menu_squash.png", TargetType = typeof(Circuitos_Modalidade), args = new object[] { id_jogador, _tipo_etapa_sel = 4 } };
            var pd_volei = new MasterPageItem() { Title = "Volei & Fut", Icon = "menu_volei.png", TargetType = typeof(Circuitos_Modalidade), args = new object[] { id_jogador, _tipo_etapa_sel = 5 } }; ;


            var pagina41 = new MasterPageItem() { Title = "Ranking", Icon = "menu_ranking.png", TargetType = typeof(Main_ranking) };
            var pagina61 = new MasterPageItem() { Title = "Jogadores", Icon = "menu_user.png", TargetType = typeof(Historico_Jogadores), args = new object[] { config, id_jogador } };
            var pagina71 = new MasterPageItem() { Title = "Clubes & Arenas", Icon = "menu_home.png", TargetType = typeof(Clubes) };
            var pagina81 = new MasterPageItem() { Title = "Regulamentos", Icon = "menu_regulamento.png", TargetType = typeof(Regulamentos) };

            var pagina_ajuda = new MasterPageItem() { Title = "Central de Ajuda", Icon = "menu_faq.png", TargetType = typeof(CentralAjuda) };

            var configuracao = new MasterPageItem() { Title = "Preferências", Icon = "cog.png", TargetType = typeof(ConfigPage) };
            var planos = new MasterPageItem() { Title = "Planos e Assinaturas", Icon = "ic_cart_black_24dp.png", TargetType = typeof(Planos), args = new object[] { config, id_jogador } };

            var logout = new MasterPageItem() { Title = "Logout", Icon = "menu_logout.png", TargetType = typeof(Login_2) };

            menuLista.Add(pagina1);
            menuLista.Add(pg_locacao);
            menuLista.Add(pg_tenis);
            menuLista.Add(pg_beach);
            menuLista.Add(pg_padel);
            menuLista.Add(pg_squash);
            menuLista.Add(pd_volei);
            menuLista.Add(pagina41);
            menuLista.Add(pagina61);
            menuLista.Add(pagina71);
            menuLista.Add(pagina81);
            menuLista.Add(pagina_ajuda);

            menuLista.Add(planos);
            menuLista.Add(configuracao);
            menuLista.Add(logout);


            return menuLista;
        }

        private static ObservableCollection<DadosJogador_item> menuDadosJogador { get; set; }

        public static ObservableCollection<DadosJogador_item> GetDadosJogador(Jogador info_jogador)
        {
            menuDadosJogador = new ObservableCollection<DadosJogador_item>();

            var clubes_defesa = new DadosJogador_item() { label = "Minhas associações", color = "Black", atribute = "Bold", size = "Medium", clube_edit = true, tipo_clube = 0 };
            var clube_beach = new DadosJogador_item() { label = string.Format("Beach: {0}", info_jogador.clube_beach_string), color = "Gray", atribute = "None", size = "Medium", clube_edit = true, tipo_clube = 1 };
            var clube_tenis = new DadosJogador_item() { label = string.Format("Tênis: {0}", info_jogador.String_Clube), color = "Gray", atribute = "None", size = "Medium", clube_edit = true, tipo_clube = 2 };
            var clube_valei = new DadosJogador_item() { label = string.Format("Volei: {0}", info_jogador.string_clube_volei), color = "Gray", atribute = "None", size = "Medium", clube_edit = true, tipo_clube = 3 };
            var clube_fut = new DadosJogador_item() { label = string.Format("FutVolei: {0}", info_jogador.string_clube_fut), color = "Gray", atribute = "None", size = "Medium", clube_edit = true, tipo_clube = 4 };
            var dados_pessoais = new DadosJogador_item() { label = "Dados Pessoais", color = "Black", atribute = "Bold", size = "Medium", clube_edit = false, tipo_clube = 0 };
            var email = new DadosJogador_item() { label = string.Format("Email: {0}", info_jogador.Email), color = "Gray", atribute = "None", size = "Medium", clube_edit = false, tipo_clube = 0 };
            var telefone = new DadosJogador_item() { label = string.Format("Telefone: {0}", info_jogador.Telefone), color = "Gray", atribute = "None", size = "Medium", clube_edit = false, tipo_clube = 0 };
            var data_nasc = new DadosJogador_item() { label = string.Format("Data Nasc: {0}", info_jogador.Data_Nascimento), color = "Gray", atribute = "None", size = "Medium", clube_edit = false, tipo_clube = 0 };

            menuDadosJogador.Add(clubes_defesa);

            menuDadosJogador.Add(clube_beach);
            menuDadosJogador.Add(clube_tenis);
            menuDadosJogador.Add(clube_valei);
            menuDadosJogador.Add(clube_fut);

            menuDadosJogador.Add(dados_pessoais);

            menuDadosJogador.Add(email);
            menuDadosJogador.Add(telefone);
            menuDadosJogador.Add(data_nasc);

            return menuDadosJogador;

        }




    }
}
