using API_Inteleco.Models;

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.Model;
using Newtonsoft.Json;
using Acr.UserDialogs.Extended.Infrastructure;
using System.Linq;
using Xamarin.Forms;
using TornfyApp.ViewModel;
using System.Globalization;

namespace TornfyApp.API
{
    public static class API_Service
    {

        public const string url = "https://www.ictapi.com.br/";

        public static async Task<List<Quadra>> ObterParceiros(int id_jogador, int id_master, bool recentes, string busca, List<Quadra> lista_rateio)
        {
            try
            {
                HttpClient client = new HttpClient();
                if (string.IsNullOrEmpty(busca)) { busca = "vazio"; }
                string Url = url + string.Format("api/Locacao/{0}/{1}/{2}/{3}/obterparceiros", id_jogador, id_master, recentes, busca);
                string response = await client.GetStringAsync(Url);
                List<Quadra> parceiros = JsonConvert.DeserializeObject<List<Quadra>>(response);

                // Filtra os parceiros removendo os que já estão na lista_rateio
                parceiros = parceiros.Where(p => !lista_rateio.Any(q => q.id == p.id)).ToList();

                return parceiros;
            }
            catch (Exception ex)
            {
                Log.Info("ObterInfoEtapa", "error:" + ex.StackTrace);
                throw;
            }

        }

        public static async Task<List<Quadra>> ObterHistorico_Locacao(string inicio, string fim, int id_master, int id_jogador, int status)
        {
            try
            {
                //status 0 = todos pagamentos
                //status 1 = pagamento true
                //status 2 = pagamento false

                inicio = Tratar_data(inicio);
                fim = Tratar_data(fim);

                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Locacao/{0}/{1}/{2}/{3}/{4}/historicoUsuario", inicio.Substring(0, 10), fim.Substring(0, 10), id_master, id_jogador, status);
                string response = await client.GetStringAsync(Url);
                List<Quadra> historico = JsonConvert.DeserializeObject<List<Quadra>>(response);

                foreach (var item in historico)
                {
                    if (item.status_pagamento)
                    {
                        item.color_botao = "#dedede";
                        item.texto_locar = "pago";
                    }
                    else
                    {
                        item.color_botao = "#ffd900";
                        item.texto_locar = "pagar";
                    }

                }

                decimal total_pgo_final = 0;
                decimal soma_quadra = 0;
                decimal soma_iluminacao = 0;
                decimal soma_parcial = 0;

                decimal total_pgo_final_n = 0;
                decimal soma_quadra_n = 0;
                decimal soma_iluminacao_n = 0;

                foreach (var item in historico)
                {
                    if (item.status_pagamento)
                    {
                        if (!item.pgo_quadra) { soma_quadra = decimal.Parse(item.valor_quadra); }
                        if (!item.pgo_luz) { soma_iluminacao = decimal.Parse(item.valor_iluminacao); }
                        soma_parcial = (soma_quadra + soma_iluminacao);
                        total_pgo_final += soma_parcial;
                        item.valor_cobranca = soma_parcial.ToString();
                    }
                    else
                    {
                        if (!item.pgo_quadra) { soma_quadra_n = decimal.Parse(item.valor_quadra); }
                        if (!item.pgo_luz) { soma_iluminacao_n = decimal.Parse(item.valor_iluminacao); }
                        soma_parcial = (soma_quadra_n + soma_iluminacao_n);
                        total_pgo_final_n += soma_parcial;
                        item.valor_cobranca = soma_parcial.ToString();
                    }

                    soma_parcial = 0;

                }


                return historico;
            }
            catch (Exception ex)
            {
                Log.Info("ObterInfoEtapa", "error:" + ex.StackTrace);
                throw;
            }

        }

        public static async Task<List<Credito>> ObterLista_CreditosLocacao(int id_jogador)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Locacao/{0}/listarCreditoLocacaoTodos", id_jogador);
                string response = await client.GetStringAsync(Url);
                List<Credito> creditos = JsonConvert.DeserializeObject<List<Credito>>(response);

                return creditos;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Agenda>> Obter_Agendas(int id_jogador)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/agenda/{0}/listar_agendas", id_jogador);
                string response = await client.GetStringAsync(Url);
                List<Agenda> agendas = JsonConvert.DeserializeObject<List<Agenda>>(response);

                return agendas;
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
                return null;
            }
        }

        public static async Task<Credito> ObterCreditoLocacao(int id_master, int id_jogador)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Locacao/{0}/{1}/listarCreditoLocacao", id_master, id_jogador);
                string response = await client.GetStringAsync(Url);
                Credito creditos = JsonConvert.DeserializeObject<Credito>(response);


                return creditos;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<int> DescontarCreditoLocacao(int id_master, int id_jogador)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Locacao/{0}/{1}/descontarCreditoLocacao", id_master, id_jogador);
                string response = await client.GetStringAsync(Url);
                int retorno = JsonConvert.DeserializeObject<int>(response);

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<int> EnviarEmail(Quadra dados)
        {
            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Locacao/enviar_email/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                int result = JsonConvert.DeserializeObject<int>(resultString);

                return result;


            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Quadra>> ObterPagamentosQuadra(string inicio, string fim, int id_master, int id_jogador)
        {
            try
            {
                if (string.IsNullOrEmpty(inicio)) inicio = DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year; else inicio = inicio.Replace("/", "-");
                if (string.IsNullOrEmpty(fim)) fim = DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year; else fim = fim.Replace("/", "-");

                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Locacao/{0}/{1}/{2}/{3}/listarPagamentos", inicio, fim, id_master, id_jogador);
                string response = await client.GetStringAsync(Url);
                List<Quadra> horarios = JsonConvert.DeserializeObject<List<Quadra>>(response);


                return horarios;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<bool> Liberar_Quadra(int id_locacao, int id_quadra, int id_master, string data)
        {
            try
            {
                Quadra quadra = new Quadra
                {
                    id = id_locacao,
                    id_master = id_master,
                    id_quadra = id_quadra,
                    data = data
                };

                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(quadra);
                string Url = url + "api/Locacao/liberar_quadra/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);
                var resultString = await response.Content.ReadAsStringAsync();
                bool result = JsonConvert.DeserializeObject<bool>(resultString);

                return result;

            }
            catch (Exception ex)
            {
                Log.Info("ObterInfoEtapa", "error:" + ex.StackTrace);
                throw;
            }
        }


        public static async Task<int> Locar_Quadra(int id_locacao, int id_jogador, int id_master, string valor_cobranca, int id_proprietario)
        {
            try
            {
                Quadra quadra = new Quadra
                {
                    id = id_locacao,
                    id_master = id_master,
                    id_jogador = id_jogador,
                    valor_cobranca = valor_cobranca,
                    id_proprietario = id_proprietario
                };

                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(quadra);
                string Url = url + "api/Locacao/locar_quadra/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);
                var resultString = await response.Content.ReadAsStringAsync();
                int result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                return 550;
            }
        }



        public static async Task<List<Temporada>> ObterAnosTemporada(int id_master)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("/api/Temporadas/{0}/listar_anos_temporada", id_master);
                string response = await client.GetStringAsync(Url);
                List<Temporada> temporadas = JsonConvert.DeserializeObject<List<Temporada>>(response);

                return temporadas;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<bool> ConferirPagamento(int id_jogador)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("/api/Temporadas/{0}/conferirPagamento", id_jogador);
                string response = await client.GetStringAsync(Url);
                bool pago = JsonConvert.DeserializeObject<bool>(response);

                return pago;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<bool> ConferirPagamentoUnico(string codigo, string charge_codigo)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("/api/Temporadas/{0}/{1}/conferirPagamentoUnico", codigo, charge_codigo);
                string response = await client.GetStringAsync(Url);
                bool pago = JsonConvert.DeserializeObject<bool>(response);

                return pago;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<bool> ConferirPagamentoLocacao(string codigo, int id_master)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("/api/Locacao/{0}/{1}/conferirPagamentoLocacao", codigo, id_master);
                string response = await client.GetStringAsync(Url);
                bool pago = JsonConvert.DeserializeObject<bool>(response);

                return pago;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public static async Task<List<Quadra>> ObterPagamentoLocacao(int id_quadra, int id_locacao, int id_master, string lista_pagamentos, int id_jogador_sel)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Locacao/{0}/{1}/{2}/{3}/{4}/pagamentolocacao_unico", id_quadra, id_locacao, id_master, lista_pagamentos, id_jogador_sel.ToString());
                string response = await client.GetStringAsync(Url);
                List<Quadra> pagamentos = JsonConvert.DeserializeObject<List<Quadra>>(response);

                return pagamentos;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Quadra>> ObterHorarios(string data, int id_master, int id_quadra, int id_jogador, string nome_local, string nome_quadra)
        {
            try
            {
                             
                data = Tratar_data(data);
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Locacao/{0}/{1}/{2}/listarHorarios", id_master, data, id_quadra);
                string response = await client.GetStringAsync(Url);
                List<Quadra> horarios = JsonConvert.DeserializeObject<List<Quadra>>(response);


                foreach (var item in horarios)
                {
                    item.nome_locacoes = nome_local;
                    item.Nome_Quadra = nome_quadra;

                    if (item.status)
                    {
                        if (!string.IsNullOrEmpty(item.periodo))
                        {
                            //constante
                            item.bounce_1 = "#ff7a25";
                            item.bounce_2 = "#ff7a25";
                        }
                        else
                        {
                            //locado
                            item.bounce_1 = "#E7305E";
                            item.bounce_2 = "#E7305E";
                        }

                        if (item.id_jogador == id_jogador)
                        {
                            item.color_botao = "#dedede";
                            item.texto_locar = "liberar";
                        }
                        else
                        {
                            item.color_botao = "#dedede";
                            item.texto_locar = "locado";
                        }

                    }
                    else
                    {
                        //livre
                        item.bounce_1 = "#3c62aa";
                        item.bounce_2 = "#3c62aa";

                        item.color_botao = "#ff7a25";
                        item.nome_jogador = "livre";
                        item.texto_locar = "livre";
                    }

                    item.string_pgo = "--";
                    if (item.status_pagamento) { item.string_pgo = "pago"; }

                }


                return horarios;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public static async Task<List<Quadra>> ObterQuadras(int id_master)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Locacao/{0}/listarQuadras", id_master);
                string response = await client.GetStringAsync(Url);
                List<Quadra> quadras = JsonConvert.DeserializeObject<List<Quadra>>(response);

                return quadras;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Quadra>> ObterLocaisLocacao(int id_jogador)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Locacao/{0}/listarComplexos", id_jogador);
                string response = await client.GetStringAsync(Url);
                List<Quadra> locais = JsonConvert.DeserializeObject<List<Quadra>>(response);

                return locais;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Quadra>> ObterLocaisLocacao_outros(int id_jogador)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Locacao/{0}/listarComplexos_outros", id_jogador);
                string response = await client.GetStringAsync(Url);
                List<Quadra> locais = JsonConvert.DeserializeObject<List<Quadra>>(response);

                return locais;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public static async Task<List<Etapa>> ObterTamanhosCamiseta()
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + "api/Torneios/camistas";
                string response = await client.GetStringAsync(Url);
                List<Etapa> camisetas = JsonConvert.DeserializeObject<List<Etapa>>(response);

                foreach (var item in camisetas)
                {
                    item.tamanho = "Tamanho " + item.tamanho;
                }


                return camisetas;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Jogador>> ObterCategoriaJogadores(int id_master, string busca)
        {
            try
            {
                if (busca == "" || busca == null)
                {
                    busca = "vazio";
                }
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/{1}/lista_todos_jogadores", id_master, busca.Trim());
                string response = await client.GetStringAsync(Url);
                List<Jogador> jogador = JsonConvert.DeserializeObject<List<Jogador>>(response);
                return jogador;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Jogador>> ObterJogador()
        {

            try
            {
                HttpClient client = new HttpClient();
                string Url = url + "api/jogadores";
                string response = await client.GetStringAsync(Url);
                List<Jogador> jogador = JsonConvert.DeserializeObject<List<Jogador>>(response);
                return jogador;
            }
            catch (Exception)
            {
                throw;
            }

        }


        //public static async Task<List<Clube>> ObterClubes(int id_master, string busca)
        //{
        //    try
        //    {
        //        if (busca == "" || busca == null)
        //        {
        //            busca = "vazio";
        //        }
        //        HttpClient client = new HttpClient();
        //        string Url = url + string.Format("/api/Temporadas/{0}/{1}/listar_clubes", id_master, busca) ;
        //        string response = await client.GetStringAsync(Url);
        //        List<Clube> clubes = JsonConvert.DeserializeObject<List<Clube>>(response);
        //        return clubes;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}


        public static async Task<List<FAQ>> ObterQuestoes_FAQ(int id_tipo)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/FAQ/{0}/buscar_questoes_faq", id_tipo);
                string response = await client.GetStringAsync(Url);
                List<FAQ> questoes = JsonConvert.DeserializeObject<List<FAQ>>(response);

                return questoes;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static int offset = 0;

        public static async Task<List<Feed>> ObterEtapas_Feed(int id_jogador, int qtd)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Feed/{0}/{1}/{2}/listar_feed", id_jogador, offset, qtd);
                string response = await client.GetStringAsync(Url);
                List<Feed> feed = JsonConvert.DeserializeObject<List<Feed>>(response);

                offset += 1;

                return feed;
            }
            catch (Exception ex)
            {
                return null;
            }



        }

        public static async Task<List<Feed>> ObterComentarios(int id_post)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/feed/{0}/listar_comentarios", id_post);
                string response = await client.GetStringAsync(Url);
                List<Feed> feed = JsonConvert.DeserializeObject<List<Feed>>(response);

                return feed;
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
                return null;
            }
        }




        public static async Task<List<Etapa>> ObterEtapas_Home(List<int> lista_esportes)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + "api/torneios/obterEtapasHome";
                string response = await client.GetStringAsync(Url);
                List<Etapa> etapas = JsonConvert.DeserializeObject<List<Etapa>>(response);

              
                if (lista_esportes.Count > 0)
                {
                    // Filtrar etapas baseado nos id_tipo presentes na lista_esportes
                    etapas = etapas.Where(etapa => lista_esportes.Contains(etapa.id_tipo)).ToList();
                }


                return etapas;
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
                return null;
            }



        }

        public static async Task<List<Etapa>> ObterEtapas(string info, string busca)
        {
            try
            {
                if (busca == "" || busca == null)
                {
                    busca = "vazio";
                }

                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/torneios/{0}/{1}/obterEtapas", info, busca);
                string response = await client.GetStringAsync(Url);
                List<Etapa> etapas = JsonConvert.DeserializeObject<List<Etapa>>(response);

                return etapas;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Programacao>> ObterJogosTela(int id_etapa, string busca)
        {
            try
            {
                if (busca == "" || busca == null)
                {
                    busca = "vazio";
                }
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Temporadas/{0}/{1}/programacao_tela", id_etapa, busca);
                string response = await client.GetStringAsync(Url);
                List<Programacao> jogos = JsonConvert.DeserializeObject<List<Programacao>>(response);

                foreach (var item in jogos)
                {



                    var nome_mes_limite = "";

                    if (item.mes == 1) { nome_mes_limite = "Jan"; }
                    if (item.mes == 2) { nome_mes_limite = "Fev"; }
                    if (item.mes == 3) { nome_mes_limite = "Mar"; }
                    if (item.mes == 4) { nome_mes_limite = "Abr"; }
                    if (item.mes == 5) { nome_mes_limite = "Mai"; }
                    if (item.mes == 6) { nome_mes_limite = "Jun"; }
                    if (item.mes == 7) { nome_mes_limite = "Jul"; }
                    if (item.mes == 8) { nome_mes_limite = "Ago"; }
                    if (item.mes == 9) { nome_mes_limite = "Set"; }
                    if (item.mes == 10) { nome_mes_limite = "Out"; }
                    if (item.mes == 11) { nome_mes_limite = "Nov"; }
                    if (item.mes == 12) { nome_mes_limite = "Dez"; }

                    item.Data = item.dia + "-" + nome_mes_limite + " / " + item.Hora;

                    switch (item.grupo_categoria)
                    {
                        case "Masculino":
                            item.img_ball = "ball_tennis.png";
                            break;
                        case "Feminino":
                            item.img_ball = "ball_tennis_pink.png";
                            break;
                        case "Infantil":
                            item.img_ball = "ball_tennis_beach.png";
                            break;
                        case "Senior":
                            item.img_ball = "ball_tennis_blue.png";
                            break;
                        case "Mista":
                            item.img_ball = "ball_tennis_green.png";
                            break;
                        default:
                            item.img_ball = "ball_tennis.png";
                            break;
                    }

                    try
                    {

                        item.bold_jog_1 = item.vd1 ? FontAttributes.Bold : FontAttributes.None;
                        item.bold_jog_2 = item.vd2 ? FontAttributes.Bold : FontAttributes.None;

                        if (item.vd1 && !item.vd2) { item.color_1 = "#3c62aa"; item.color_2 = "#9ce700"; }
                        if (item.vd1) { item.color_1 = "#9ce700"; item.color_2 = "#3c62aa"; } else { item.color_2 = "#9ce700"; item.color_1 = "#3c62aa"; }


                        if (item.Resultado != "WO")
                        {
                            if (item.Resultado != "W.O")
                            {
                                // Set the FontAttributes properties based on the conditions
                                if (item.set1_j1 != "-")
                                {
                                    item.bs_1_j1 = int.Parse(item.set1_j1) > int.Parse(item.set1_j2) ? FontAttributes.Bold : FontAttributes.None;
                                    item.bs_1_j2 = int.Parse(item.set1_j1) <= int.Parse(item.set1_j2) ? FontAttributes.Bold : FontAttributes.None;
                                }

                                if (item.set2_j1 != "-")
                                {
                                    item.bs_2_j1 = int.Parse(item.set2_j1) > int.Parse(item.set2_j2) ? FontAttributes.Bold : FontAttributes.None;
                                    item.bs_2_j2 = int.Parse(item.set2_j1) <= int.Parse(item.set2_j2) ? FontAttributes.Bold : FontAttributes.None;
                                }

                                if (item.set3_j1 != "-")
                                {
                                    item.bs_3_j1 = int.Parse(item.set3_j1) > int.Parse(item.set3_j2) ? FontAttributes.Bold : FontAttributes.None;
                                    item.bs_3_j2 = int.Parse(item.set3_j1) <= int.Parse(item.set3_j2) ? FontAttributes.Bold : FontAttributes.None;
                                }
                            }


                        }
                    }
                    catch (Exception)
                    { }


                }



                return jogos;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Clube>> ObterClubes(int id_master, string busca)
        {
            try
            {
                if (busca == "" || busca == null)
                {
                    busca = "vazio";
                }
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/{1}/listar_clubes", id_master, busca);
                string response = await client.GetStringAsync(Url);
                List<Clube> clubes = JsonConvert.DeserializeObject<List<Clube>>(response);

                return clubes;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static int qtd_chamada;

        public static async Task<List<Clube>> ObterClubes_qtd(int id_master, string busca)
        {
            try
            {
                if (qtd_chamada == 0)
                {
                    qtd_chamada = 30;
                }
                else
                {
                    qtd_chamada += 30;
                }

                if (busca == "" || busca == null)
                {
                    busca = "vazio";
                }
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/{1}/{2}/listar_clubes_qtd", id_master, busca, qtd_chamada);
                string response = await client.GetStringAsync(Url);
                List<Clube> clubes = JsonConvert.DeserializeObject<List<Clube>>(response);

                return clubes;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public static async Task<List<Inscricao>> ObterLista_Creditos(int id_jogador)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/obterCreditoSaldo", id_jogador);
                string response = await client.GetStringAsync(Url);
                List<Inscricao> creditos = JsonConvert.DeserializeObject<List<Inscricao>>(response);

                return creditos;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Inscricao>> ObterInscricoes_Jogador(int id_jogador, int id_master)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/{1}/listar_inscricoes", id_jogador, id_master);
                string response = await client.GetStringAsync(Url);
                List<Inscricao> inscricoes = JsonConvert.DeserializeObject<List<Inscricao>>(response);

                foreach (var item in inscricoes)
                {
                    if (item.tipo_simples)
                    {
                        item.path_pagamento = "shopping_bag_black";
                        item.pagamento_string = "Pagamentos Realizados";
                        item.color = "#000000";

                        if (!item.status_pagamento)
                        {
                            item.path_pagamento = "shopping_bag_black_orange";
                            item.pagamento_string = "Pagamentos Pendentes";
                            item.color = "#f56e02";
                        }
                    }
                    else
                    {
                        item.path_pagamento = "shopping_bag_black";
                        item.pagamento_string = "Pagamentos Realizados";
                        item.color = "#000000";

                        bool is_dupla = false;
                        if (id_jogador != item.id_jogador)
                        {
                            is_dupla = true;
                        }

                        if (is_dupla)
                        {
                            if (!item.status_pagamento_dupla)
                            {
                                item.path_pagamento = "shopping_bag_black_orange";
                                item.pagamento_string = "Pagamentos Pendentes";
                                item.color = "#f56e02";
                            }
                        }
                        else
                        {
                            if (!item.status_pagamento)
                            {
                                item.path_pagamento = "shopping_bag_black_orange";
                                item.pagamento_string = "Pagamentos Pendentes";
                                item.color = "#f56e02";
                            }
                        }



                    }


                }


                return inscricoes;
            }
            catch (Exception)
            {
                throw;
            }

        }



        public static async Task<List<Inscricao>> ObterInscricoes_Jogador_etapa(int id_jogador, int id_master, int id_etapa)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/{1}/Checkout_Inscricao", id_jogador, id_etapa);
                string response = await client.GetStringAsync(Url);
                List<Inscricao> inscricoes = JsonConvert.DeserializeObject<List<Inscricao>>(response);


                return inscricoes;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public static async Task<int> DeletarInscricao(int id_inscricao, Formulario_Inscricao dados)
        {
            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + string.Format("api/Temporadas/{0}/deletar_inscricao", id_inscricao);
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;


            }
            catch (Exception)
            {
                throw;
            }

        }



        public static async Task<int> Lembrar_Senha(Formulario_Inscricao dados)
        {
            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + string.Format("api/Login/lembrar_senha", dados.email);
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<RetornoEnvio> Lembrar_Senha_CPF(Formulario_Inscricao dados)
        {
            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + string.Format("api/Login/lembrar_senha_cpf");
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();
                RetornoEnvio result = JsonConvert.DeserializeObject<RetornoEnvio>(resultString);

                return result;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<int> Confirmar_Conta(Login dados)
        {
            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + string.Format("api/Login/confirmar_conta", dados.email);
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                throw;
            }

        }



        public static async Task<List<Inscricao>> ObterInscricoes_Creditos(int id_jogador, int id_etapa)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/{1}/Inscricao_Creditos", id_jogador, id_etapa);
                string response = await client.GetStringAsync(Url);
                List<Inscricao> creditos = JsonConvert.DeserializeObject<List<Inscricao>>(response);

                return creditos;
            }
            catch (Exception)
            {
                throw;
            }

        }



        public static async Task<List<Jogador>> ObterJogadoresEtapa(int id_etapa, int id_cat)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/{1}/jogadores", id_etapa, id_cat);
                string response = await client.GetStringAsync(Url);
                List<Jogador> jogadores = JsonConvert.DeserializeObject<List<Jogador>>(response);

                foreach (var item in jogadores)
                {
                    if (!item.tipo_simples)
                    {
                        item.Nome_Jogador_simples = item.nome_dupla_gb;
                        item.Nome_Clube = item.nome_clubes_dupla.Replace(" - ", "/");
                    }

                    if (item.lista_espera)
                    {
                        item.color_inscricao = "#ff8583";
                        item.situacao_inscricao = "lista espera";
                        item.color_font = "#ffffff";
                    }
                    else
                    {
                        item.color_inscricao = "#ffffff";
                        item.situacao_inscricao = "";
                        item.color_font = "#000000";
                    }
                }


                return jogadores;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public static async Task<List<Categoria>> ObterChaves_Torneio_Chamada(int id_etapa)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/chaves_torneio", id_etapa);
                string response = await client.GetStringAsync(Url);
                List<Categoria> categorias = JsonConvert.DeserializeObject<List<Categoria>>(response);

                foreach (var item in categorias)
                {
                    switch (item.grupo_categoria)
                    {
                        case "Masculino":
                            item.img_ball = "ball_tennis.png";
                            break;
                        case "Feminino":
                            item.img_ball = "ball_tennis_pink.png";
                            break;
                        case "Infantil":
                            item.img_ball = "ball_tennis_beach.png";
                            break;
                        case "Senior":
                            item.img_ball = "ball_tennis_blue.png";
                            break;
                        case "Mista":
                            item.img_ball = "ball_tennis_green.png";
                            break;
                        default:
                            item.img_ball = "ball_tennis.png";
                            break;
                    }
                }

                return categorias;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Categoria>> ObterCategoriaEtapa(int id_etapa)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/categorias_etapa", id_etapa);
                string response = await client.GetStringAsync(Url);
                List<Categoria> categorias = JsonConvert.DeserializeObject<List<Categoria>>(response);

                foreach (var item in categorias)
                {
                    switch (item.grupo_categoria)
                    {
                        case "Masculino":
                            item.img_ball = "ball_tennis.png";
                            break;
                        case "Feminino":
                            item.img_ball = "ball_tennis_pink.png";
                            break;
                        case "Infantil":
                            item.img_ball = "ball_tennis_beach.png";
                            break;
                        case "Senior":
                            item.img_ball = "ball_tennis_blue.png";
                            break;
                        case "Mista":
                            item.img_ball = "ball_tennis_green.png";
                            break;
                        default:
                            item.img_ball = "ball_tennis.png";
                            break;
                    }
                }

                return categorias;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Categoria>> ObterCategoriaEtapaBuild(int id_etapa, bool build)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/{1}/categorias_etapa_build", id_etapa, build);
                string response = await client.GetStringAsync(Url);
                List<Categoria> categorias = JsonConvert.DeserializeObject<List<Categoria>>(response);

                foreach (var item in categorias)
                {
                    switch (item.grupo_categoria)
                    {
                        case "Masculino":
                            item.img_ball = "ball_tennis.png";
                            break;
                        case "Feminino":
                            item.img_ball = "ball_tennis_pink.png";
                            break;
                        case "Infantil":
                            item.img_ball = "ball_tennis_beach.png";
                            break;
                        case "Senior":
                            item.img_ball = "ball_tennis_blue.png";
                            break;
                        case "Mista":
                            item.img_ball = "ball_tennis_green.png";
                            break;
                        default:
                            item.img_ball = "ball_tennis.png";
                            break;
                    }
                }

                return categorias;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public static async Task<Etapa> ObterInfoEtapa(int id_etapa)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/etapaInfo", id_etapa);
                string response = await client.GetStringAsync(Url);
                Etapa etapa = JsonConvert.DeserializeObject<Etapa>(response);

                return etapa;
            }
            catch (Exception ex)
            {
                Log.Info("ObterInfoEtapa", "error:" + ex.StackTrace);
                throw;
            }

        }

        public static async Task<Pagamento> ObterCheckoutPagamento(int id_jogador, int id_etapa, int total_pgo, string lista_idInscricoes, int id_propietario)
        {
            Etapa dados = new Etapa();

            dados.id_etapa = id_etapa;
            dados.id_jogador = id_jogador;
            dados.total_pgo = total_pgo;
            dados.lista_idInscricoes = lista_idInscricoes;
            dados.id_master = id_propietario;

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Temporadas/checkout_pagamento/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                Pagamento result = JsonConvert.DeserializeObject<Pagamento>(resultString);

                return result;


            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<Quadra> ObterCheckoutLocacao(int id_jogador, string total_pgo, string lista_pagamentos, int id_master)
        {
            Quadra dados = new Quadra
            {
                id_jogador = id_jogador,
                total_pgo = total_pgo,
                lista_pagamentos = lista_pagamentos,
                id_master = id_master
            };

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Locacao/gerarQrCode/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                Quadra result = JsonConvert.DeserializeObject<Quadra>(resultString);

                return result;


            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<Quadra> ObterCheckoutLocacaoRateio(int id_jogador, string total_pgo, string lista_pagamentos, int id_master,
            string lista_rateio, bool rateio, decimal valor_rateio,
            decimal valor_original, bool pgo_integral)
        {
            Quadra dados = new Quadra
            {
                id_jogador = id_jogador,
                total_pgo = total_pgo,
                lista_pagamentos = lista_pagamentos,
                id_master = id_master,

                rateio = rateio,
                valor_rateio = valor_rateio.ToString(),
                valor_original = valor_original,
                pgo_integral = pgo_integral,
                lista_rateio = lista_rateio,

            };
                
            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Locacao/gerarQrCodeRateio/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                Quadra result = JsonConvert.DeserializeObject<Quadra>(resultString);

                return result;


            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<Jogador> ObterDadosLogin(string Login_Usuario, string Password, int id_propietario)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("/api/Login/{0}/{1}/{2}/login_usuario", Login_Usuario, Password, id_propietario);
                string response = await client.GetStringAsync(Url);
                Jogador jogador = JsonConvert.DeserializeObject<Jogador>(response);

                return jogador;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<Jogador> ObterDadosLogin_teste(string Login_Usuario, string Password, int id_propietario)
        {
            try
            {
                Login form = new Login
                {
                    email = Login_Usuario,
                    senha = Password,
                    id_propietario = id_propietario
                };

                //HttpClient client = new HttpClient();
                //string Url = url + string.Format("/api/Login/{0}/{1}/{2}/login_usuario_teste", Login_Usuario, Password, id_propietario);
                //string response = await client.GetStringAsync(Url);
                //Jogador jogador = JsonConvert.DeserializeObject<Jogador>(response);


                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(form);
                string Url = url + "api/Login/login_caracteres";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();
                Jogador jogador = JsonConvert.DeserializeObject<Jogador>(resultString);




                return jogador;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<bool> BuscaCPF_BD(string cpf, int id_master)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("/api/Login/{0}/{1}/busca_cpf", cpf, id_master);
                string response = await client.GetStringAsync(Url);
                bool cpf_exist = JsonConvert.DeserializeObject<bool>(response);

                return cpf_exist;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<bool> BuscaEmail_BD(string email, int id_master)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("/api/Login/{0}/{1}/busca_email", email, id_master);
                string response = await client.GetStringAsync(Url);
                bool cpf_exist = JsonConvert.DeserializeObject<bool>(response);

                return cpf_exist;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<Jogador> ObterDadosJogador(int id_jogador)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/dados_jogador", id_jogador);
                string response = await client.GetStringAsync(Url);
                Jogador jogador = JsonConvert.DeserializeObject<Jogador>(response);

                return jogador;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public static async Task<Stats> ObterPerformance(int id_jogador, int id_master, int ano)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Jogadores/{0}/{1}/{2}/buscar_performance", id_jogador, id_master, ano);
                string response = await client.GetStringAsync(Url);
                Stats performance = JsonConvert.DeserializeObject<Stats>(response);

                return performance;
            }
            catch (Exception)
            {
                Stats performance = null;
                return performance;
            }

        }

        public static async Task<List<Stats>> ObterTableEtapasPerformance(int id_jogador, int id_master, int ano)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Jogadores/{0}/{1}/{2}/table_etapas", id_jogador, id_master, ano);
                string response = await client.GetStringAsync(Url);
                List<Stats> table_etapas = JsonConvert.DeserializeObject<List<Stats>>(response);

                int last = 0;
                string mp = "";
                foreach (var item in table_etapas)
                {

                    if (item.melhor_posicao != "")
                    {

                        if (int.Parse(item.melhor_posicao) < 11) { mp = "33-64"; }
                        if (int.Parse(item.melhor_posicao) > 10 && int.Parse(item.melhor_posicao) <= 50) { mp = "17-32"; }
                        if (int.Parse(item.melhor_posicao) > 50 && int.Parse(item.melhor_posicao) <= 100) { mp = "9-16"; }
                        if (int.Parse(item.melhor_posicao) > 100 && int.Parse(item.melhor_posicao) <= 175) { mp = "5-8"; }
                        if (int.Parse(item.melhor_posicao) > 175 && int.Parse(item.melhor_posicao) <= 250) { mp = "3-4"; }
                        if (int.Parse(item.melhor_posicao) == 350) { mp = "2"; }
                        if (int.Parse(item.melhor_posicao) == 500) { mp = "1"; }

                        item.mp = mp;
                        if (last > item.performance) { item.color_arrow = "#F06292"; }
                        if (last < item.performance) { item.color_arrow = "#3c62aa"; }
                        if (last == item.performance) { item.color_arrow = "#ff7a25"; }

                        last = item.performance;
                    }

                }

                return table_etapas;
            }
            catch (Exception)
            {
                List<Stats> performance = null;
                return performance;
            }

        }


        public static async Task<List<Master>> ObterCircuitos(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/proprietarios_master", id);
                string response = await client.GetStringAsync(Url);
                List<Master> master = JsonConvert.DeserializeObject<List<Master>>(response);

                return master;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public static async Task<List<Salvos>> ObterSalvosEtapas(int id_jogador)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/salvos_etapas", id_jogador);
                string response = await client.GetStringAsync(Url);
                List<Salvos> etapas_salvas = JsonConvert.DeserializeObject<List<Salvos>>(response);

                return etapas_salvas;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Salvos>> ObterSalvosCircuitos(int id_jogador)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/salvos_circuitos", id_jogador);
                string response = await client.GetStringAsync(Url);
                List<Salvos> circuitos_salvas = JsonConvert.DeserializeObject<List<Salvos>>(response);

                return circuitos_salvas;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static string Tratar_data(string data)
        {
            string[] spl = data.Split('/');
            string dia_str = "";
            string mes_str = "";
            try
            {
                int dia = int.Parse(spl[0].Trim().ToString());
                if (dia.ToString().Length == 1) dia_str = "0" + dia; else dia_str = dia.ToString();
                int mes = int.Parse(spl[1].Trim().ToString());
                if (mes.ToString().Length == 1) mes_str = "0" + mes; else mes_str = mes.ToString();
                data = dia_str + "-" + mes_str + "-" + spl[2].Substring(0, 4).Trim().ToString();
                return data;
            }
            catch (Exception)
            {
                spl = data.Split('/');
                int dia = int.Parse(spl[0].Trim().ToString());
                if (dia.ToString().Length == 1) dia_str = "0" + dia; else dia_str = dia.ToString();
                int mes = int.Parse(spl[1].Trim().ToString());
                if (mes.ToString().Length == 1) mes_str = "0" + mes; else mes_str = mes.ToString();
                data = dia_str + "-" + mes_str + "-" + spl[2].Substring(0, 4).Trim().ToString();
                return data;
            }

        }

        public static async Task<List<Etapa>> ObterTorneiosRecentes_Geral(int qtd, string busca, int tipo, string d_inicio, string d_fim, string pub)
        {
            try
            {
                if (busca == "" || busca == null) { busca = "vazio"; }
                //caso sejam identicas, zerar chamada
                if (d_inicio == d_fim) { d_inicio = null; d_fim = null; }
                if (d_inicio == "" || d_inicio == null) { d_inicio = "vazio"; } else { d_inicio = Tratar_data(d_inicio); }
                if (d_fim == "" || d_fim == null) { d_fim = "vazio"; } else { d_fim = Tratar_data(d_fim); }
                if (string.IsNullOrEmpty(pub)) { pub = "vazio"; }

                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/{1}/{2}/{3}/{4}/{5}/proximos_eventos_geral", qtd, busca, tipo, d_inicio, d_fim, pub);
                string response = await client.GetStringAsync(Url);
                List<Etapa> etapa_recente = JsonConvert.DeserializeObject<List<Etapa>>(response);

                return etapa_recente;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Etapa>> ObterTorneiosCircuito(int qtd, string busca, int id_grupo, string d_inicio, string d_fim, string pub)
        {
            try
            {
                if (busca == "" || busca == null) { busca = "vazio"; }
                //caso sejam identicas, zerar chamada
                if (d_inicio == d_fim) { d_inicio = null; d_fim = null; }
                if (d_inicio == "" || d_inicio == null) { d_inicio = "vazio"; } else { d_inicio = Tratar_data(d_inicio); }
                if (d_fim == "" || d_fim == null) { d_fim = "vazio"; } else { d_fim = Tratar_data(d_fim); }
                if (string.IsNullOrEmpty(pub)) { pub = "vazio"; }

                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/{1}/{2}/{3}/{4}/{5}/obterTorneiosPorCircuito", qtd, busca, id_grupo, d_inicio, d_fim, pub);
                string response = await client.GetStringAsync(Url);
                List<Etapa> etapa_recente = JsonConvert.DeserializeObject<List<Etapa>>(response);

                return etapa_recente;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<int> AdicionarInscricao(Formulario_Inscricao inscricao)
        {

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(inscricao);
                string Url = url + "api/Torneios/inscricao";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<int>(resultString);

                //200: sucesso
                //1: bloqueio de letra
                //2: redundante
                //8 inscrição sem parceiro
                //3: corum total
                //4: córum parcial

                return result;

            }
            catch (Exception)
            {
                return 550;
            }

        }

        public static async Task<int> EditarFavorito(Etapa dados)
        {

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Torneios/editarFavoritoEtapa/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                return 550;
            }

        }

        public static async Task<int> EnviarEmail(FAQ dados)
        {

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/FAQ/enviarEmail/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                return 550;
            }

        }

        public static async Task<int> EditarCircuitoFavorito(Salvos dados)
        {

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Torneios/editarFavoritoCircuito/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                return 550;
            }

        }

        public static async Task<int> LikePost(Feed dados)
        {

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Feed/likepost/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                return 500;
            }

        }

        public static async Task<int> DeleteComentario(Feed dados)
        {

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Feed/deletar_comentarios/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                return 550;
            }

        }


        public static async Task<int> DeletarPost(Feed dados)
        {

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Feed/deletarpost/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                return 550;
            }

        }

        public static async Task<int> SendComentario(Feed dados)
        {

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Feed/addcomentario/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                return 550;
            }

        }

        public static async Task<int> EditarJogador(Jogador dados)
        {

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Torneios/editarJogador/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                return 550;
            }

        }

        public static async Task<int> Confirmar_Codigo_post(int id_jogador, int codigo)
        {

            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Login/{0}/{1}/confirmar_codigo_post", id_jogador, codigo);
                string response = await client.GetStringAsync(Url);
                int result = JsonConvert.DeserializeObject<int>(response);

                return result;

            }
            catch (Exception)
            {
                return 550;
            }

        }


        public static async Task<int> RegistrarJogador(Usuario dados)
        {

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Login/registrar_jogador/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);
                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static async Task<int> EditarClube(Jogador dados)
        {

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Torneios/editarClubeJogador/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                return 550;
            }

        }

        public static async Task<int> BaixaCredito(Etapa dados)
        {

            try
            {
                HttpClient client = new HttpClient();
                string json = JsonConvert.SerializeObject(dados);
                string Url = url + "api/Torneios/BaixaCredito/";
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(Url, content);

                var resultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<int>(resultString);

                return result;

            }
            catch (Exception)
            {
                return 550;
            }

        }

        public static async Task<List<Chaves>> ObterHistorico_Jogos(int qtd, int id_jogador, int ano)
        {

            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Temporadas/{0}/{1}/{2}/historico_jogos", qtd, id_jogador, ano);
                string response = await client.GetStringAsync(Url);
                List<Chaves> lista = JsonConvert.DeserializeObject<List<Chaves>>(response);

                foreach (var item in lista)
                {

                    if (!item.tipo_simples)
                    {
                        item.Nome_1 = item.nome_dupla_gb_j1;
                        item.Nome_2 = item.nome_dupla_gb_j2;
                    }

                    var nome_mes_limite = "";

                    if (item.mes == 1) { nome_mes_limite = "Jan"; }
                    if (item.mes == 2) { nome_mes_limite = "Fev"; }
                    if (item.mes == 3) { nome_mes_limite = "Mar"; }
                    if (item.mes == 4) { nome_mes_limite = "Abr"; }
                    if (item.mes == 5) { nome_mes_limite = "Mai"; }
                    if (item.mes == 6) { nome_mes_limite = "Jun"; }
                    if (item.mes == 7) { nome_mes_limite = "Jul"; }
                    if (item.mes == 8) { nome_mes_limite = "Ago"; }
                    if (item.mes == 9) { nome_mes_limite = "Set"; }
                    if (item.mes == 10) { nome_mes_limite = "Out"; }
                    if (item.mes == 11) { nome_mes_limite = "Nov"; }
                    if (item.mes == 12) { nome_mes_limite = "Dez"; }

                    item.Data = item.dia + "-" + nome_mes_limite + " / " + item.Horario;



                    item.bold_jog_1 = item.vd_1 ? FontAttributes.Bold : FontAttributes.None;
                    item.bold_jog_2 = item.vd_2 ? FontAttributes.Bold : FontAttributes.None;

                    if (!item.vd_1 && !item.vd_2) { item.color_1 = "#3c62aa"; item.color_2 = "#9ce700"; }
                    if (item.vd_1) { item.color_1 = "#9ce700"; item.color_2 = "#3c62aa"; } else { item.color_2 = "#9ce700"; item.color_1 = "#3c62aa"; }

                    // Set the FontAttributes properties based on the conditions
                    if (item.set1_j1 != "-")
                    {
                        item.bs_1_j1 = int.Parse(item.set1_j1) > int.Parse(item.set1_j2) ? FontAttributes.Bold : FontAttributes.None;
                        item.bs_1_j2 = int.Parse(item.set1_j1) <= int.Parse(item.set1_j2) ? FontAttributes.Bold : FontAttributes.None;
                    }

                    if (item.set2_j1 != "-")
                    {
                        item.bs_2_j1 = int.Parse(item.set2_j1) > int.Parse(item.set2_j2) ? FontAttributes.Bold : FontAttributes.None;
                        item.bs_2_j2 = int.Parse(item.set2_j1) <= int.Parse(item.set2_j2) ? FontAttributes.Bold : FontAttributes.None;
                    }

                    if (item.set3_j1 != "-")
                    {
                        item.bs_3_j1 = int.Parse(item.set3_j1) > int.Parse(item.set3_j2) ? FontAttributes.Bold : FontAttributes.None;
                        item.bs_3_j2 = int.Parse(item.set3_j1) <= int.Parse(item.set3_j2) ? FontAttributes.Bold : FontAttributes.None;
                    }



                }

                return lista;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public static async Task<List<Ranking>> ObterEtapasPontuadas_Jogador(int id_jogador, int ano)
        {

            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Temporadas/{0}/{1}/etapas_pontuadas", id_jogador, ano);
                string response = await client.GetStringAsync(Url);
                List<Ranking> lista = JsonConvert.DeserializeObject<List<Ranking>>(response);

                foreach (var item in lista)
                {
                    switch (item.grupo_categoria)
                    {
                        case "Masculino":
                            item.img_path = "ball_tennis.png";
                            break;
                        case "Feminino":
                            item.img_path = "ball_tennis_pink.png";
                            break;
                        case "Infantil":
                            item.img_path = "ball_tennis_beach.png";
                            break;
                        case "Senior":
                            item.img_path = "ball_tennis_blue.png";
                            break;
                        case "Mista":
                            item.img_path = "ball_tennis_green.png";
                            break;
                        default:
                            item.img_path = "ball_tennis.png";
                            break;
                    }
                }


                return lista;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Ranking>> ObterEtapasPontuadas(int id_jogador, int ano)
        {

            int id_master = 1;

            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/{1}/{2}/etapas_pontuadas", id_jogador, ano, id_master);
                string response = await client.GetStringAsync(Url);
                List<Ranking> lista = JsonConvert.DeserializeObject<List<Ranking>>(response);

                return lista;
            }
            catch (Exception)
            {
                throw;
            }

        }



        //public static IEnumerable<Categoria> GetCategoriaRanking(int id_master, int id_grupo)
        //{
        //    var app = new Aplicacao_Etapa();
        //    var list = app.Listar_Categorias_Ranking(1, id_master, id_grupo);
        //    return list;
        //}


        //public static IEnumerable<Categoria> GetCategoriaRanking_dupla(int id_master, int id_grupo)
        //{
        //    var app = new Aplicacao_Etapa();
        //    var list = app.Listar_Categorias_Ranking(0, id_master, id_grupo);
        //    return list;
        //}



        //tipo_etapa se beach ou se tenis
        public static async Task<List<Categoria>> ObterEtapasCategoriasRanking(int id_master, int tipo_simples, int id_grupo)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Temporadas/{0}/{1}/{2}/categorias_ranking", id_master, id_grupo, tipo_simples);
                string response = await client.GetStringAsync(Url);
                List<Categoria> lista = JsonConvert.DeserializeObject<List<Categoria>>(response);

                return lista;
            }
            catch (Exception)
            {
                throw;
            }

        }



        public static async Task<List<Ranking>> ObterEtapasPontosClubes(int id_temporada, int id_ranking, int id_master)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Temporadas/{0}/{1}/{2}/ranking_clubes", id_temporada, id_ranking, id_master);
                string response = await client.GetStringAsync(Url);
                List<Ranking> lista = JsonConvert.DeserializeObject<List<Ranking>>(response);


                return lista;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Ranking>> ObterEtapasPontosCategoria(int id_temporada, int id_ranking, int id_categoria)
        {
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Temporadas/{0}/{1}/{2}/ranking_jogadores_usuario", id_temporada, id_ranking, id_categoria);
                string response = await client.GetStringAsync(Url);
                List<Ranking> lista = JsonConvert.DeserializeObject<List<Ranking>>(response);

                foreach (var item in lista)
                {
                    switch (item.grupo_categoria)
                    {
                        case "Feminino":
                            switch (item.contagem)
                            {
                                case 1:
                                    item.color_back = "#8a0093";
                                    item.color_font = "#ffffff";
                                    break;
                                case 2:
                                    item.color_back = "#de00a2";
                                    item.color_font = "#ffffff";
                                    break;
                                case 3:
                                    item.color_back = "#ef63c9";
                                    item.color_font = "#ffffff";
                                    break;
                                case 4:
                                    item.color_back = "#e8a1d5";
                                    item.color_font = "#ffffff";
                                    break;
                                case 5:
                                    item.color_back = "#dfcdda";
                                    item.color_font = "#ffffff";
                                    break;
                                case 6:
                                    item.color_back = "#e4e0e3";
                                    item.color_font = "#000";
                                    break;
                                case 7:
                                    item.color_back = "#f5f5f5";
                                    item.color_font = "#000";
                                    break;
                                default:
                                    item.color_back = "#ffffff";
                                    item.color_font = "#000";
                                    break;
                            }
                            break;
                        case "Masculino":
                            switch (item.contagem)
                            {
                                case 1:
                                    item.color_back = "#006793";
                                    item.color_font = "#ffffff";
                                    break;
                                case 2:
                                    item.color_back = "#009bde";
                                    item.color_font = "#ffffff";
                                    break;
                                case 3:
                                    item.color_back = "#30c1ff";
                                    item.color_font = "#ffffff";
                                    break;
                                case 4:
                                    item.color_back = "#7bc0dd";
                                    item.color_font = "#ffffff";
                                    break;
                                case 5:
                                    item.color_back = "#afcdd9";
                                    item.color_font = "#ffffff";
                                    break;
                                case 6:
                                    item.color_back = "#dbe2e5";
                                    item.color_font = "#000";
                                    break;
                                case 7:
                                    item.color_back = "#f5f5f5";
                                    item.color_font = "#000";
                                    break;
                                default:
                                    item.color_back = "#ffffff";
                                    item.color_font = "#000";
                                    break;
                            }
                            break;
                        case "Infantil":
                            switch (item.contagem)
                            {
                                case 1:
                                    item.color_back = "#d15a0f";
                                    item.color_font = "#ffffff";
                                    break;
                                case 2:
                                    item.color_back = "#e78e37";
                                    item.color_font = "#ffffff";
                                    break;
                                case 3:
                                    item.color_back = "#edaf73";
                                    item.color_font = "#ffffff";
                                    break;
                                case 4:
                                    item.color_back = "#e3c9b0";
                                    item.color_font = "#000";
                                    break;
                                case 5:
                                    item.color_back = "#c5beb8";
                                    item.color_font = "#000";
                                    break;
                                case 6:
                                    item.color_back = "#eae6e2";
                                    item.color_font = "#000";
                                    break;
                                case 7:
                                    item.color_back = "#f5f5f5";
                                    item.color_font = "#000";
                                    break;
                                default:
                                    item.color_back = "#ffffff";
                                    item.color_font = "#000";
                                    break;
                            }
                            break;
                        case "Senior":
                            switch (item.contagem)
                            {
                                case 1:
                                    item.color_back = "#02ac7a";
                                    item.color_font = "#ffffff";
                                    break;
                                case 2:
                                    item.color_back = "#07e6a5";
                                    item.color_font = "#ffffff";
                                    break;
                                case 3:
                                    item.color_back = "#73ecc9";
                                    item.color_font = "#ffffff";
                                    break;
                                case 4:
                                    item.color_back = "#b7dfd3";
                                    item.color_font = "#000";
                                    break;
                                case 5:
                                    item.color_back = "#d2e7e1";
                                    item.color_font = "#000";
                                    break;
                                case 6:
                                    item.color_back = "#d7dfdd";
                                    item.color_font = "#000";
                                    break;
                                case 7:
                                    item.color_back = "#f5f5f5";
                                    item.color_font = "#000";
                                    break;
                                default:
                                    item.color_back = "#ffffff";
                                    item.color_font = "#000";
                                    break;
                            }
                            break;

                    }
                }

                return lista;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public static async Task<List<Ranking>> ObterPontuacaoParcial(int id_jogador, int id_categoria, int ano)
        {

            int id_master = 1;

            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/{1}/{2}/{3}/pontuacao_parcial", id_jogador, id_categoria, id_master, ano);
                string response = await client.GetStringAsync(Url);
                List<Ranking> lista = JsonConvert.DeserializeObject<List<Ranking>>(response);

                return lista;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Temporada>> ObterCircuitos_Modalidade(int tipo_etapa, int id_jogador, string busca)
        {
            if (string.IsNullOrEmpty(busca)) { busca = "vazio"; }
            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/{1}/{2}/circuitos_modalidade", id_jogador, tipo_etapa, busca);
                string response = await client.GetStringAsync(Url);
                List<Temporada> lista = JsonConvert.DeserializeObject<List<Temporada>>(response);

                return lista;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static async Task<List<Temporada>> ObterCircuitos_Menu(int id_master)
        {

            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/{0}/circuitos", id_master);
                string response = await client.GetStringAsync(Url);
                List<Temporada> lista = JsonConvert.DeserializeObject<List<Temporada>>(response);

                return lista;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public static async Task<List<Temporada>> ObterCircuitos_Menu_all()
        {

            try
            {
                HttpClient client = new HttpClient();
                string Url = url + string.Format("api/Torneios/circuitos_all", 0);
                string response = await client.GetStringAsync(Url);
                List<Temporada> lista = JsonConvert.DeserializeObject<List<Temporada>>(response);

                return lista;
            }
            catch (Exception)
            {
                throw;
            }

        }







    }
}

