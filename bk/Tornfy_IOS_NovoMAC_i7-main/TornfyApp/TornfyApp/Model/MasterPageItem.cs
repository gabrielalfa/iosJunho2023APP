using System;
using System.Collections.Generic;
using System.Text;

namespace TornfyApp.Model
{
    public class MasterPageItem
    {

        public string Title { get; set; }
        public string Icon { get; set; }
        public Type TargetType { get; set; }

        public string info { get; set; }

        public string IconImageSource { get; set; }

        public object[] args { get; set; } 

    }
}
