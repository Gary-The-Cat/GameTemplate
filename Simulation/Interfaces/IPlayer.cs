using SFML.Graphics;

namespace Game.Interfaces
{
    public interface IPlayer
    {
        void Initalize();

        void Update(float deltaT);

        void Draw(RenderWindow window);

        IGameWorld GameWorld { get; set; }
    }
}
