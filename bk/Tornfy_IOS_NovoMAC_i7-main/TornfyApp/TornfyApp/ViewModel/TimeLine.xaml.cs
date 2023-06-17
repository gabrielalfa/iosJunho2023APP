using Acr.UserDialogs.Extended;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Drawing;using System.IO;using System.Linq;using System.Net;using System.Text.RegularExpressions;using TornfyApp.API;using TornfyApp.Model;using Xamarin.Essentials;using Xamarin.Forms;using Xamarin.Forms.Xaml;using Image = Xamarin.Forms.Image;using SkiaSharp;using System.Text;using Xamarin.Forms.PlatformConfiguration;using Rg.Plugins.Popup.Extensions;using System.Collections;using System.Windows.Input;namespace TornfyApp.ViewModel{    [XamlCompilation(XamlCompilationOptions.Compile)]    public partial class TimeLine : ContentPage    {        public Config_Home config;        public List<Etapa> Lista_torneios_proximos;        public List<Ranking> Lista_Etapas_Pontuadas;        public List<Chaves> Lista_jogos_recentes;        public string color = "#3897f0";        public string path = "";        public string circuito = "";        public int master = 0;        public int id_jogador;        public Jogador info_jogador;        public bool message_bool = true;        public int tipo_etapa_sel;        public int id_post;        public string mensagem = "";        public string titulo = "";        public TimeLine(Config_Home config, int _id_jogador)        {            InitializeComponent();            this.id_jogador = _id_jogador;            this.config = config;            carroucel_days.ItemsSource = GetBudgets_ranking();


            //if (TestarInternet()) CarregarFeed(); else Navigation.PushAsync(new NoInternet());

            carroucel_feed.RefreshCommand = new Command(() =>            {                if (TestarInternet()) CarregarFeed(); else Navigation.PushAsync(new NoInternet());                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => carroucel_feed.IsRefreshing = false);            });            RefreshCommand = new Command(() => CarregarFeed());        }        public static bool TestarInternet()        {            bool coneccao = true;            if (Connectivity.NetworkAccess != NetworkAccess.Internet)            {                coneccao = false;            }            return coneccao;        }        protected override void OnAppearing()        {            base.OnAppearing();            CarregarFeed();        }        private void carroucel_feed_ItemAppearing(object sender, ItemVisibilityEventArgs e)        {            var itens = this.Lista_torneios as IList;            if (API_Service.offset <= itens.Count)            {                if (itens != null && e.Item == itens[itens.Count - 1])                {                    CarregarFeed();                }            }        }        private bool _isRefreshing;        public bool IsRefreshing        {            get => _isRefreshing;            set            {                _isRefreshing = value;                OnPropertyChanged(nameof(IsRefreshing));            }        }        public ICommand RefreshCommand { get; }



        //private int offset = 0;
        private int qtd = 6;        public List<Feed> Lista_torneios;        public async void CarregarFeed()        {            IsRefreshing = true;


            // Inicialize Lista_torneios se for nulo
            if (Lista_torneios == null)            {                Lista_torneios = new List<Feed>();            }

            // Armazene os itens recém-buscados em uma lista separada
            List<Feed> novosItens = await API_Service.ObterEtapas_Feed(id_jogador, qtd);            foreach (var item in novosItens)            {                switch (item.aspecto)                {                    case "9:11":                        item.altura = 340;                        break;                    case "1:1":                        item.altura = 340;                        break;                    case "3:2":                        item.altura = 220;                        break;                    default:                        break;                }

                // Inicialize IframeSource com uma string vazia
                item.IframeSource = string.Empty;                if (item.tipo_postagem == 2)                {
                    // Exibir vídeo em um WebView
                    item.IsLabelVisible = false;                    item.IsWebViewVisible = true;

                    // Remova espaços e quebras de linha antes e depois do iframe
                    string iframe = item.url_video.Trim();                    byte[] decodedIframeBytes = Convert.FromBase64String(iframe);                    string decodedIframe = Encoding.UTF8.GetString(decodedIframeBytes);

                    // Extraia a URL do atributo "src" usando expressões regulares
                    Match match = Regex.Match(decodedIframe.Replace("\"\"", "\""), @"<iframe.*?src=""(.*?)"".*?>", RegexOptions.IgnoreCase);                    string src = match.Groups[1].Value;

                    // Injete a URL extraída na propriedade "IframeSource" do item
                    item.IframeSource = src;                }                else                {
                    // Exibir texto normal em um Label
                    item.IsLabelVisible = true;                    item.IsWebViewVisible = false;                }

                // Adicione o item à lista existente em vez de substituir a lista inteira
                Lista_torneios.Add(item);            }

            // Atualize a fonte de dados do carrossel com a lista atualizada
            carroucel_feed.ItemsSource = null; // Remova a referência antiga
            carroucel_feed.ItemsSource = Lista_torneios; // Atribua a lista atualizada

            IsRefreshing = false;        }



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


        FormattedString Linkify(string text)        {            string urlPattern = @"(?:(\b(?:https?|ftp|file):\/\/)|(?<!\b\w))(?:www\.)[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|]";            string input = text.Length > 200 ? text.Substring(0, 200) : text;            MatchCollection matches = Regex.Matches(input, urlPattern, RegexOptions.IgnoreCase);            FormattedString formattedText = new FormattedString();            int currentIndex = 0;            foreach (Match match in matches)            {                if (match.Index > currentIndex)                {                    formattedText.Spans.Add(new Span { Text = input.Substring(currentIndex, match.Index - currentIndex) });                }                string url = match.Groups[1].Success ? match.Value : "http://" + match.Value;                formattedText.Spans.Add(new Span                {                    Text = match.Value,                    TextColor = System.Drawing.Color.Blue,                    TextDecorations = TextDecorations.Underline,                    FontAttributes = FontAttributes.Bold                });                currentIndex = match.Index + match.Length;            }            if (currentIndex < input.Length)            {                formattedText.Spans.Add(new Span { Text = input.Substring(currentIndex) });            }            if (text.Length > 200)            {                formattedText.Spans.Add(new Span                {                    Text = " Leia mais",                    TextColor = System.Drawing.Color.Blue,                    TextDecorations = TextDecorations.Underline,                    FontAttributes = FontAttributes.Bold                });            }            return formattedText;        }        private ObservableCollection<Budget_Ranking> GetBudgets_ranking()        {            return new ObservableCollection<Budget_Ranking>            {                new Budget_Ranking { Name = "Feed",  Image = "feed1", menu = 12 },                new Budget_Ranking { Name = "Pagamentos",  Image = "qrcode", menu = 6 },                new Budget_Ranking { Name = "Stats",  Image = "stats", menu = 1 },                new Budget_Ranking { Name = "Locações",  Image = "menu_calendario", menu = 2 },                new Budget_Ranking { Name = "Aulas",  Image = "menu_padel", menu = 14 },                new Budget_Ranking { Name = "Historico",  Image = "historico", menu = 3 },                new Budget_Ranking { Name = "Salvos",  Image = "favorito_preto", menu = 4 },                new Budget_Ranking { Name = "Dados",  Image = "menu_user", menu = 5 },                new Budget_Ranking { Name = "Meu Ranking",  Image = "trofeu_home", menu = 7 },                new Budget_Ranking { Name = "Créditos",  Image = "menu_info", menu = 8 },                new Budget_Ranking { Name = "Ajuda",  Image = "menu_faq", menu = 9 },            };        }        private void ToolbarItem_Clicked(object sender, EventArgs e)        {            Navigation.PushAsync(new Dados_Jogador(config, id_jogador, 1));        }        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)        {            await Navigation.PushAsync(new CriarPost(id_jogador, 1));        }



        private async void carroucel_days_SelectionChanged(object sender, SelectionChangedEventArgs e)        {            if (TestarInternet())            {                if (e.CurrentSelection.Count() != 0)                {                    Budget_Ranking item = e.CurrentSelection[0] as Budget_Ranking;                    if (item != null) { ((CollectionView)sender).SelectedItem = null; }                    switch (item.menu)                    {                        case 1:
                            //Em implementação
                            UserDialogs.Instance.ShowLoading(title: "Buscando Estatísticas");                            await Navigation.PushAsync(new Estatisticas(id_jogador, 1));                            UserDialogs.Instance.HideLoading();                            break;                        case 2:                            UserDialogs.Instance.ShowLoading(title: "Buscando Horarários");                            await Navigation.PushAsync(new Complexos(config, id_jogador));                            UserDialogs.Instance.HideLoading();                            break;                        case 3:                            UserDialogs.Instance.ShowLoading(title: "Buscando Jogos");                            await Navigation.PushAsync(new Meu_Historico(config, id_jogador));                            UserDialogs.Instance.HideLoading();                            break;                        case 4:                            UserDialogs.Instance.ShowLoading(title: "Buscando Salvos");                            await Navigation.PushAsync(new Favoritos(config, id_jogador));                            UserDialogs.Instance.HideLoading();                            break;                        case 5:                            UserDialogs.Instance.ShowLoading(title: "Buscando Dados");                            await Navigation.PushAsync(new Dados_Jogador(config, id_jogador, 1));                            UserDialogs.Instance.HideLoading();                            break;                        case 6:                            UserDialogs.Instance.ShowLoading(title: "Buscando Inscrições");                            await Navigation.PushAsync(new Minhas_Inscricoes(config, id_jogador));                            UserDialogs.Instance.HideLoading();                            break;                        case 7:                            UserDialogs.Instance.ShowLoading(title: "Buscando Meus Rankings");                            await Navigation.PushAsync(new Etapas_Pontuadas(config, id_jogador));                            UserDialogs.Instance.HideLoading();                            break;                        case 8:                            UserDialogs.Instance.ShowLoading(title: "Buscando Meus Créditos");                            await Navigation.PushAsync(new Creditos(id_jogador, 1));                            UserDialogs.Instance.HideLoading();                            break;                        case 9:                            await Navigation.PushAsync(new CentralAjuda());                            break;                        case 12:
                            await Navigation.PushAsync(new MasterPage(config, id_jogador, 4));                            break;
                        case 14:
                            await Navigation.PushAsync(new AgendaAula(config, id_jogador));
                            break;                        default:                            break;                    }                }            }        }        private void carroucel_feed_ItemTapped(object sender, ItemTappedEventArgs e)        {            ((ListView)sender).SelectedItem = null;        }        private void carroucel_feed_ItemSelected(object sender, SelectedItemChangedEventArgs e)        {            ((ListView)sender).SelectedItem = null;        }        private void LongPressGestureRecognizer_LongPressed(object sender, EventArgs e)        {            if (sender is Label label && !string.IsNullOrEmpty(label.Text))            {                Clipboard.SetTextAsync(label.Text);            }        }        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)        {            if (sender is Label label && label.BindingContext is Feed feedItem)            {                Navigation.PushAsync(new Conteudo(feedItem.conteudo, 1, feedItem.id, id_jogador));            }            if (sender is Image image && image.BindingContext is Feed feedItem2)            {                Navigation.PushAsync(new Conteudo(feedItem2.conteudo, 1, feedItem2.id, id_jogador));            }        }        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)        {            if (sender is Image image && image.BindingContext is Feed feedItem)            {                Navigation.PushAsync(new Conteudo(feedItem.uploadPath, 2, feedItem.id, id_jogador));            }        }        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)        {            int id_post = 0;            bool self = false;            Feed currentFeed = null;            if (sender is Image image && image.BindingContext is Feed feedItem)            {                id_post = feedItem.id;                self = feedItem.self;                currentFeed = feedItem;            }            if (sender is Label label && label.BindingContext is Feed feedItem_2)            {                id_post = feedItem_2.id;                self = feedItem_2.self;                currentFeed = feedItem_2;                var parent = label.Parent;                if (parent is StackLayout stackLayout)                {                    var img = stackLayout.Children.FirstOrDefault(x => x is Image) as Image;                    if (img != null && img.BindingContext is Feed)                    {                        currentFeed = img.BindingContext as Feed;                    }                }            }            if (currentFeed == null)            {                return;            }            Feed dados = new Feed();            dados.id_jogador = id_jogador;            dados.id = id_post;            int result = await API_Service.LikePost(dados);            switch (result)            {
                //like
                case 200:                    currentFeed.img_like = "heart.png";                    currentFeed.likes += 1;                    break;
                //deslike
                case 300:                    currentFeed.img_like = "broken_heart.png";                    currentFeed.likes -= 1;                    break;
                //não logado
                case 100:                    currentFeed.img_like = "broken_heart.png";                    break;                default:                    break;            }        }        private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)        {            int id_jogador_sel = 0;            if (sender is Image image && image.BindingContext is Feed feedItem)            {                id_jogador_sel = feedItem.id_jogador;                id_post = feedItem.id;            }            Feed feed = new Feed { id = id_post, id_jogador = id_jogador };            if (this.id_jogador == 1 || this.id_jogador == 163)            {                var action = await DisplayActionSheet("Opções", "Cancelar", null, "editar post", "deletar post", "deletar e banir", "banir usuário");
                // Você pode adicionar mais opções se necessário


                titulo = string.Empty;                mensagem = string.Empty;                switch (action)                {                    case "editar post":                        await Navigation.PushAsync(new CriarPost(id_post, 2));                        return;                    case "deletar post":                        feed.tipo_postagem = 1;                        break;                    case "deletar e banir":                        feed.tipo_postagem = 2;                        break;                    case "banir usuário":                        feed.tipo_postagem = 3;                        break;                    default:                        return;                }                var result = await API_Service.DeletarPost(feed);                if (result == 200)                {                    CarregarFeed();                    titulo = "Post deletado!";                    mensagem = "Post foi deletado da plataforma!";                    ShowPopupMessage(titulo, mensagem);                }            }            else if (this.id_jogador == id_jogador_sel)            {                var action = await DisplayActionSheet("Opções", "Cancelar", null, "editar post", "deletar post2");
                // Você pode adicionar mais opções se necessário

                switch (action)                {                    case "editar post":                        await Navigation.PushAsync(new CriarPost(id_post, 2));                        break;                    case "deletar post":                        feed.tipo_postagem = 1;                        break;                }                var result = await API_Service.DeletarPost(feed);                if (result == 200)                {                    CarregarFeed();                    titulo = "Post deletado!";                    mensagem = "Post foi deletado da plataforma!";                    ShowPopupMessage(titulo, mensagem);                }            }            else            {                var action = await DisplayActionSheet("Opções", "Cancelar", null, "não há opções");                titulo = "Ação não permitida!";                mensagem = "Esta ação não é permitida para este perfil!";                ShowPopupMessage(titulo, mensagem);            }        }        async void ShowPopupMessage(string titulo, string mensagem)        {            var pop = new Mensagem_Simples(titulo, mensagem, "#3c62aa", "Fechar Mensagem");            await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);        }        private async void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)        {            string postContent = string.Empty;            string imageUrl = string.Empty;            if (sender is Image image && image.BindingContext is Feed feedItem)            {                postContent = feedItem.conteudo;                imageUrl = feedItem.uploadPath;            }            else if (sender is Label label && label.BindingContext is Feed feedItem_2)            {                postContent = feedItem_2.conteudo;                imageUrl = feedItem_2.uploadPath;            }            if (!string.IsNullOrEmpty(imageUrl))            {                var webClient = new WebClient();                var imageBytes = await webClient.DownloadDataTaskAsync(imageUrl);                var tempImageFileName = Path.Combine(FileSystem.CacheDirectory, "shared_image.jpg");                File.WriteAllBytes(tempImageFileName, imageBytes);                var shareImage = new ShareFileRequest                {                    Title = "Compartilhar imagem",                    File = new ShareFile(tempImageFileName, "image/jpeg")                };                await Share.RequestAsync(shareImage);            }            else if (!string.IsNullOrEmpty(postContent))            {                var shareText = new ShareTextRequest                {                    Title = "Compartilhar texto",                    Text = postContent                };                await Share.RequestAsync(shareText);            }        }          }}