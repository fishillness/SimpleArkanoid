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
    static Texture strongBlockTexture;
    static Texture crackedBlockTexture;
    static Texture stickTexture;

    static Sprite stick;
    static Sprite[] blocks;

    static Ball ball;

    static int blocksNumber = 100;
    static bool isWin = false;

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

        isWin = false;
        blocksNumber = 100;
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
        strongBlockTexture = new Texture("StrongBlock.png");
        crackedBlockTexture = new Texture("CrackedBlock.png");
        stickTexture = new Texture("Stick.png");

        Font font = new Font("comic.ttf");
        Text win = new Text("Ты выиграл!", font);
        win.Position = new Vector2f(300, 250);


        ball = new Ball(ballTexture);
        stick = new Sprite(stickTexture);
        blocks = new Sprite[100];


        Random rnd = new Random();

        for (int i = 0; i < blocks.Length; i++)
        {
            int randomNumber = rnd.Next(0, 20);

            if (randomNumber == 13)
                blocks[i] = new Sprite(strongBlockTexture);
            else
                blocks[i] = new Sprite(blockTexture);
        }

        SetStartPosition();

        while (window.IsOpen == true)
        {
            window.Clear();

            window.DispatchEvents();

            if (isWin == false)
            {
                if (Mouse.IsButtonPressed(Mouse.Button.Left) == true)
                {
                    ball.Start(7, new Vector2f(0, -1));
                }
                ball.Move(new Vector2i(0, 0), new Vector2i(800, 600));

                ball.CheckCollision(stick, "Stick");
                for (int i = 0; i < blocks.Length; i++)
                {
                    if (ball.CheckCollision(blocks[i], "Block") == true)
                    {
                        if (blocks[i].Texture == strongBlockTexture)
                            blocks[i].Texture = crackedBlockTexture;
                        else
                        {
                            blocks[i].Position = new Vector2f(1000, 1000);
                            blocksNumber--;
                        }
                        break;
                    }
                }


                stick.Position = new Vector2f(Mouse.GetPosition(window).X - stick.Texture.Size.X * 0.5f, stick.Position.Y);

                window.Draw(ball.sprite);
                window.Draw(stick);
                for (int i = 0; i < blocks.Length; i++)
                {
                    window.Draw(blocks[i]);
                }
            }
            

            if (blocksNumber == 0)
            {
                window.Clear();
                window.Draw(win);
                isWin = true;
            }

            if (isWin == true && Keyboard.IsKeyPressed(Keyboard.Key.R) == true)
            {
                SetStartPosition();
            }

                window.Display();
        }

    }

    private static void Window_Closed(object sender, EventArgs e)
    {
        window.Close();
    }
}
