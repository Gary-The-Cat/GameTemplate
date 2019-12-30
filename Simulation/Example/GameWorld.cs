using Game.Interfaces;
using SFML.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Game.Example
{
    public class GameWorld : IGameWorld
    {
        public List<Drawable> StaticObjects { get; set; }
            = new List<Drawable>();

        public List<Drawable> DynamicObjects { get; set; }
            = new List<Drawable>();

        public void DrawDynamicObjects(RenderWindow window)
        {
            foreach (var dynamicObject in DynamicObjects)
            {
                window.Draw(dynamicObject);
            }
        }

        public void DrawStaticObjects(RenderWindow window)
        {
            foreach (var staticObject in StaticObjects)
            {
                window.Draw(staticObject);
            }
        }

        public void Initalize()
        {

        }

        public void Update(float deltaT)
        {

        }

        public IGameWorld Clone()
        {
            return new GameWorld()
            {
                StaticObjects = this.StaticObjects.ToList(),
                DynamicObjects = this.DynamicObjects.ToList()
            };
        }
    }
}
