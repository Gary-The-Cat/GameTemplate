using GameAI.GeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Interfaces
{
    public interface IGame
    {
        IGameWorld GameWorld { get; set; }

        List<IPlayer> Players { get; set; }

        GeneticAlgorithm AiTrainer { get; set; }

        Action LevelComplete { get; set; }
    }
}
