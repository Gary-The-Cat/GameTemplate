using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Interfaces
{
    public interface IGameWorld
    {
        List<Drawable> StaticObjects { get; set; }

        List<Drawable> DynamicObjects { get; set; }

        void Initalize();

        void Update(float deltaT);

        void DrawStaticObjects(RenderWindow texture);

        void DrawDynamicObjects(RenderWindow texture);

        IGameWorld Clone();
    }
}
