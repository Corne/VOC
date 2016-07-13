﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.WPF.Game.Board;
using VOC.Client.WPF.Main;

namespace VOC.Client.WPF.Game
{
    public class GameViewModel : IContentViewModel
    {
        public BoardViewModel Board { get; } = new BoardViewModel();


        public Task OnClose()
        {
            return Task.FromResult(0);
        }

        public Task OnNavigate()
        {
            return Task.FromResult(0);
        }

    }
}
