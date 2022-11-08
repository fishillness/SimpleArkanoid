using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;



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

    static Ball ball;

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
        ball.sprite.Position = new Vector2f(375, 400);
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

        ball = new Ball(ballTexture);
        stick = new Sprite(stickTexture);
        blocks = new Sprite[100];
        for (int i = 0; i < blocks.Length; i++) blocks[i] = new Sprite(blockTexture);

        SetStartPosition();

        while (window.IsOpen == true)
        {
            window.Clear();

            window.DispatchEvents();

            if (Mouse.IsButtonPressed(Mouse.Button.Left) == true)
            {
                ball.Start(5, new Vector2f(0, -1));
            }
            ball.Move(new Vector2i(0,0), new Vector2i(800, 600));

            ball.CheckCollision(stick, "Stick");
            for (int i = 0; i < blocks.Length; i++)
            {
               if(ball.CheckCollision(blocks[i], "Block") == true)
                {
                    blocks[i].Position = new Vector2f(1000, 1000);
                    break;
                }
            }

            //Stick
            stick.Position = new Vector2f(Mouse.GetPosition(window).X - stick.Texture.Size.X * 0.5f, stick.Position.Y);
 
            window.Draw(ball.sprite);
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
