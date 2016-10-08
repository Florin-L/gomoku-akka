using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gomoku.Common;
using GomokuClient.ViewModels;

namespace GomokuClient.ViewModels
{
    public class PlayerViewModel : BindableBase
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public PlayerType Type { get; set; }

        public PlayerViewModel()
        {

        }
    }
}
