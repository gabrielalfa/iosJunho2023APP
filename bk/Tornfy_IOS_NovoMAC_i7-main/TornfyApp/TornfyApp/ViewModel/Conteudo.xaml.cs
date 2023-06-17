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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Conteudo : ContentPage
    {

        public int id_jogador;
        public int id_post;


        public Conteudo(string conteudo, int tipo, int id_post, int id_jogador)
        {
            InitializeComponent();

            this.id_jogador = id_jogador;
            this.id_post = id_post;

            switch (tipo)
            {
                case 1:
                    label_txt.Text = conteudo;
                    stk_label.IsVisible = true;
                    painel_image.IsVisible = false;

                    GetComments(id_post);

                    break;
                case 2:
                    image_post.Source = conteudo;
                    stk_label.IsVisible = false;
                    painel_image.IsVisible = true;
                    break;
                default:
                    break;
            }

        }

        public async void GetComments(int id_post)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            List<Feed> lista_comentarios = await API_Service.ObterComentarios(id_post);
            UserDialogs.Instance.HideLoading();

            foreach (var item in lista_comentarios)
            {
                if (item.id_jogador == id_jogador)
                {
                    item.self = true;
                }
                else
                {
                    item.self = false;
                }
            }

            if (lista_comentarios.Count > 0)
            {
                commentsListView.ItemsSource = lista_comentarios.ToList();
                lbl_coment.Text = "comentários:";
            }
            else
            {
                commentsListView.ItemsSource = null;
                lbl_coment.Text = "ainda não há comentários...";
            }


        }


        //private void LongPressHandler(object sender, MR.Gestures.LongPressEventArgs e)
        //{
        //    if (sender is Label label && !string.IsNullOrEmpty(label.Text))
        //    {
        //        Clipboard.SetTextAsync(label.Text);
        //    }
        //}

        private void PinchGestureRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            double currentScale = 1;
            double startScale = 1;
            double xOffset = 0;
            double yOffset = 0;

            if (e.Status == GestureStatus.Started)
            {
                // Armazena a escala atual aplicada à imagem quando o gesto começa
                startScale = image_post.Scale;
                image_post.AnchorX = 0;
                image_post.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calcula a escala a ser aplicada com base no gesto de pinça
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);

                // Aplica a escala à imagem
                image_post.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                // Guarda a escala final quando o gesto termina
                startScale = currentScale;
            }
        }

        private async void floatingButton_Clicked(object sender, EventArgs e)
        {
            Editor commentEditor = new Editor { HeightRequest = 100 };

            // Criação da página suspensa para adicionar um comentário
            var addCommentPage = new ContentPage
            {
                Title = "Adicionar um comentário",
                Content = new StackLayout
                {
                    Padding = 20,
                    Children =
            {
                new Label { Text = "Escreva seu comentário:", FontAttributes = FontAttributes.Bold },
                commentEditor,
                new Button
                {
                    Text = "Enviar",
                    BackgroundColor = Color.FromHex("#ffd900"),
                    TextColor = Color.FromHex("#1f2225"),
                    Command = new Command(async () =>
                    {
                        int result = await SendCommentAsync(commentEditor.Text);
                        if (result == 200)
                        {
                            GetComments(id_post);
                            await Navigation.PopModalAsync();
                        }
                    })
                },
                new Button
                {
                    Text = "Cancelar",
                    BackgroundColor = Color.FromHex("#e9e9e9"),
                    TextColor = Color.FromHex("#1f2225"),
                    Command = new Command(async () => await Navigation.PopModalAsync())
                }
            }
                }
            };

            // Abrindo a página suspensa
            await Navigation.PushModalAsync(new NavigationPage(addCommentPage));
        }



        private async Task<int> SendCommentAsync(string comentario)
        {
            Feed dados = new Feed();
            dados.comentario = comentario;
            dados.id = id_post;
            dados.id_jogador = id_jogador;

            int result = await API_Service.SendComentario(dados);
            return result;
        }

        private void commentsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var image = sender as Image;
            if (image == null)
            {
                return;
            }

            Feed dado = image.BindingContext as Feed;
            if (dado == null)
            {
                return;
            }

            Feed feed = new Feed();
            feed.id = dado.id;

            int result = await API_Service.DeleteComentario(feed);
            if (result == 200)
            {
                GetComments(id_post);
            }
        }
    }
}