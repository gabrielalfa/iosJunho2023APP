using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TornfyApp.Model
{
    public class Feed : INotifyPropertyChanged
    {
        public bool IsLabelVisible { get; set; }
        public bool IsWebViewVisible { get; set; }
        public string IframeSource { get; set; }

        public string conteudo_resumido { get; set; }
        public string path_jogador { get; set; }
        public int qtd_comentarios { get; set; }

        private int _likes;
        public string aspecto { get; set; }
        public int id { get; set; }
        public string uploadPath { get; set; }
        public int tipo_postagem { get; set; }
        public int id_jogador { get; set; }
        public string data { get; set; }
        public string conteudo { get; set; }
        public int altura { get; set; }
        //public string img_like { get; set; }
        //public int likes { get; set; }
        public string nome_jogador { get; set; }
        public string timeDisplay { get; set; }
        public int count { get; set; }
        public bool self { get; set; }

        public int qtd_image { get; set; }
        public string url_video { get; set; }

        public string comentario { get; set; }

        private string _img_like;
        public string img_like
        {
            get { return _img_like; }
            set
            {
                _img_like = value;
                OnPropertyChanged(nameof(img_like));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        public int likes
        {
            get { return _likes; }
            set
            {
                if (_likes != value)
                {
                    _likes = value;
                    OnPropertyChanged(nameof(likes));
                }
            }
        }





        //Agora, quando você clicar na imagem ou no label, o número de likes será atualizado adequadamente.
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }


}

