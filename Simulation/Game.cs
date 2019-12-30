using Game.Screens;
using Game.Screens;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace Game
{
    public class Game
    {
        ScreenManager screenManager;
        Clock clock;
        RenderWindow window;
        
        public Game()
        {
            // Create the main window
            window = new RenderWindow(new VideoMode(Configuration.Width, Configuration.Height), "Map Maker");
            window.SetFramerateLimit(60);

            // Handle window events
            window.Closed += OnClose;
            window.Resized += OnResize;

            window.SetMouseCursorVisible(true);

            screenManager = new ScreenManager(window);

            var gameScreen = new GameScreen(window, Configuration.SinglePlayer);

            screenManager.AddScreen(gameScreen);

            clock = new Clock();
        }

        public void Run()
        {
            while (window.IsOpen)
            {
                float deltaT = Configuration.IsDebugFrameTime 
                    ? Configuration.DebugFrameTime 
                    : clock.Restart().AsMicroseconds() / 1000000f;

                // Clear the previous frame
                window.Clear(Configuration.Background);
                
                // Process events
                window.DispatchEvents();

                // Update all the screens
                screenManager.Update(deltaT);

                // Draw all the screens
                screenManager.Draw(deltaT);

                // Update the window
                window.Display();
            }
        }


        private void OnResize(object sender, SizeEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            screenManager.OnResize(window.Size.X, window.Size.Y);
        }

        private static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }
    }
}
