using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.PiManager.Model.ViewModels.API
{
    public class DownMenuViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public List<DownMenuViewModel> downMenu { get; set; }
    }
}
