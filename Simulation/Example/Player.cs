using Game.Interfaces;
using SFML.Graphics;
using SFML.System;

namespace Game.Example
{
    public class Player : IPlayer
    {
        public IGameWorld GameWorld { get; set; }

        public Player(IGameWorld gameWorld)
        {
            this.GameWorld = gameWorld;

            this.GameWorld.DynamicObjects.Add(new CircleShape(100)
            {
                Origin = new Vector2f(100, 100),
                FillColor = Color.Red
            });
        }

        public void Draw(RenderWindow window)
        {
            GameWorld.DrawDynamicObjects(window);
        }

        public void Initalize()
        {
            GameWorld.Initalize();
        }

        public void Update(float deltaT)
        {
            GameWorld.Update(deltaT);
        }
    }
}