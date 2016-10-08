using System;

namespace GomokuClient.ViewModels
{
    /// <summary>
    /// The board view model.
    /// </summary>
    public class BoardViewModel : BindableBase
    {
        /// <summary>
        /// The size of the board (19 by default).
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BoardViewModel()
        {
            Size = 19;
        }
    }
}
