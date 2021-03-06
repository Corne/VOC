﻿using System;
using VOC.Core.Boards;
using VOC.Core.Games.Turns;
using VOC.Core.Players;

namespace VOC.Core.Games.Commands
{
    public class BuildRoadCommand : IPlayerCommand
    {
        private readonly IBoard board;
        private readonly IEdge edge;
        public BuildRoadCommand(IPlayer player, IBoard board, IEdge edge)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (board == null)
                throw new ArgumentNullException(nameof(board));
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));
            Player = player;
            this.board = board;
            this.edge = edge;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.BuildRoad; } }

        public void Execute()
        {
            if (!Player.HasResources(Road.BUILD_RESOURCES))
                throw new InvalidOperationException("Player has not enough resources to be able to build a road!");

            board.BuildRoad(edge, Player);
            Player.TakeResources(Road.BUILD_RESOURCES);
        }
    }
}
