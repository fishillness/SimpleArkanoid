using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;



using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static RenderWindow window;
    static Texture ballTexture;
    static Texture blockTexture;
    static Texture stickTexture;

    static Sprite stick;
    static Sprite[] blocks;

    public static void SetStartPosition()
    {
        int index = 0;
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                blocks[index].Position = new Vector2f(x * (blocks[index].TextureRect.Width + 15) + 75,
                    y * (blocks[index].TextureRect.Height + 15) + 50);
                index++;
            }
        }


        stick.Position = new Vector2f(400, 500);

    }

    static void Main(string[] args)
    {
        window = new RenderWindow(new VideoMode(800, 600), "Arkanoid");
        window.SetFramerateLimit(60);
        window.Closed += Window_Closed;

        // Load Textures
        ballTexture = new Texture("Ball.png");
        blockTexture = new Texture("Block.png");
        stickTexture = new Texture("Stick.png");

        stick = new Sprite(stickTexture);
        blocks = new Sprite[100];
        for (int i = 0; i < blocks.Length; i++) blocks[i] = new Sprite(blockTexture);

        SetStartPosition();

        while (window.IsOpen == true)
        {
            window.Clear();

            window.DispatchEvents();

            //Stick
            stick.Position = new Vector2f(Mouse.GetPosition(window).X - stick.Texture.Size.X * 0.5f, stick.Position.Y);


            window.Draw(stick);
            for (int i = 0; i < blocks.Length; i++)
            {
                window.Draw(blocks[i]);
            }


            window.Display();
        }

    }

    private static void Window_Closed(object sender, EventArgs e)
    {
        window.Close();
    }
}
