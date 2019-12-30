using Game.Example;
using Game.Interfaces;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace Game.Screens
{
    public class GameScreen : Screen
    {
        private int frame;

        private IGameWorld gameWorld;

        private List<IPlayer> players;

        public GameScreen(
            RenderWindow window,
            FloatRect configuration)
            : base(window, configuration)
        {
            frame = 0;

            players = new List<IPlayer>();

            gameWorld = new GameWorld();

            gameWorld.StaticObjects.Add(
                new CircleShape(100)
                {
                    Position = new Vector2f(Configuration.Width / 2, Configuration.Height / 2),
                    Origin = new Vector2f(100, 100),
                    FillColor = Color.White
                });

            players.Add(new Player(gameWorld.Clone()));
        }
        
        /// <summary>
        /// Update - Here we add all our logic for updating components in this screen. 
        /// This includes checking for user input, updating the position of moving objects and more!
        /// </summary>
        /// <param name="deltaT">The amount of time that has passed since the last frame was drawn.</param>
        public override void Update(float deltaT)
        {
            this.gameWorld.Update(deltaT);

            foreach (var player in players)
            {
                player.Update(deltaT);
            }
        }
        
        /// <summary>
        /// Draw - Here we don't update any of the components, only draw them in their current state to the screen.
        /// </summary>
        /// <param name="deltaT">The amount of time that has passed since the last frame was drawn.</param>
        public override void Draw(float deltaT)
        {
            gameWorld.DrawStaticObjects(window);

            foreach (var player in players)
            {
                player.Draw(window);
            }

            frame++;
        }
    }
}