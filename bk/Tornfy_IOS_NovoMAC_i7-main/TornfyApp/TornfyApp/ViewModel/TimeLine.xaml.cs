﻿using Acr.UserDialogs.Extended;


            //if (TestarInternet()) CarregarFeed(); else Navigation.PushAsync(new NoInternet());

            carroucel_feed.RefreshCommand = new Command(() =>



        //private int offset = 0;
        private int qtd = 6;


            // Inicialize Lista_torneios se for nulo
            if (Lista_torneios == null)

            // Armazene os itens recém-buscados em uma lista separada
            List<Feed> novosItens = await API_Service.ObterEtapas_Feed(id_jogador, qtd);

                // Inicialize IframeSource com uma string vazia
                item.IframeSource = string.Empty;
                    // Exibir vídeo em um WebView
                    item.IsLabelVisible = false;

                    // Remova espaços e quebras de linha antes e depois do iframe
                    string iframe = item.url_video.Trim();

                    // Extraia a URL do atributo "src" usando expressões regulares
                    Match match = Regex.Match(decodedIframe.Replace("\"\"", "\""), @"<iframe.*?src=""(.*?)"".*?>", RegexOptions.IgnoreCase);

                    // Injete a URL extraída na propriedade "IframeSource" do item
                    item.IframeSource = src;
                    // Exibir texto normal em um Label
                    item.IsLabelVisible = true;

                // Adicione o item à lista existente em vez de substituir a lista inteira
                Lista_torneios.Add(item);

            // Atualize a fonte de dados do carrossel com a lista atualizada
            carroucel_feed.ItemsSource = null; // Remova a referência antiga
            carroucel_feed.ItemsSource = Lista_torneios; // Atribua a lista atualizada

            IsRefreshing = false;



        //public async void CarregarFeed()
        //{
        //    UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
        //    Lista_torneios = await API_Service.ObterEtapas_Feed(id_jogador, qtd);


        //    foreach (var item in Lista_torneios)
        //    {
        //        switch (item.aspecto)
        //        {
        //            case "9:11":
        //                item.altura = 340;
        //                break;
        //            case "1:1":
        //                item.altura = 340;
        //                break;
        //            case "3:2":
        //                item.altura = 220;
        //                break;
        //            default:
        //                break;
        //        }

        //        // Inicialize IframeSource com uma string vazia
        //        item.IframeSource = string.Empty;

        //        if (item.tipo_postagem == 2)
        //        {
        //            // Exibir vídeo em um WebView
        //            item.IsLabelVisible = false;
        //            item.IsWebViewVisible = true;

        //            // Remova espaços e quebras de linha antes e depois do iframe
        //            string iframe = item.url_video.Trim();

        //            byte[] decodedIframeBytes = Convert.FromBase64String(iframe);
        //            string decodedIframe = Encoding.UTF8.GetString(decodedIframeBytes);

        //            //// Carregue o conteúdo do iframe no WebView
        //            //var htmlSource = decodedIframe;
        //            //item.IframeSource = htmlSource.Replace("\"\"", "\"");

        //            //item.IframeSource = "https://www.youtube.com/embed/URS6UkBk-Zo";


        //            // Extraia a URL do atributo "src" usando expressões regulares
        //            Match match = Regex.Match(decodedIframe.Replace("\"\"", "\""), @"<iframe.*?src=""(.*?)"".*?>", RegexOptions.IgnoreCase);
        //            string src = match.Groups[1].Value;

        //            // Injete a URL extraída na propriedade "IframeSource" do item
        //            item.IframeSource = src;
        //        }
        //        else
        //        {
        //            // Exibir texto normal em um Label
        //            item.IsLabelVisible = true;
        //            item.IsWebViewVisible = false;
        //        }

        //    }

        //    this.Lista_torneios.AddRange(Lista_torneios);
        //    carroucel_feed.ItemsSource = Lista_torneios.ToList();
        //    UserDialogs.Instance.HideLoading();
        //}


        FormattedString Linkify(string text)



        private async void carroucel_days_SelectionChanged(object sender, SelectionChangedEventArgs e)
                            //Em implementação
                            UserDialogs.Instance.ShowLoading(title: "Buscando Estatísticas");
                            await Navigation.PushAsync(new MasterPage(config, id_jogador, 4));
                        case 14:
                            await Navigation.PushAsync(new AgendaAula(config, id_jogador));
                            break;
                //like
                case 200:
                //deslike
                case 300:
                //não logado
                case 100:
                // Você pode adicionar mais opções se necessário


                titulo = string.Empty;
                // Você pode adicionar mais opções se necessário

                switch (action)