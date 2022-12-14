using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

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
    static bool isLose = false;
    static int missesNumber = 0;
    static int attempts = 3;
    static int level = 1;

    public static void SetStartPositionFirstLevel()
    {
        SetTexturesForBlocks();

        int index = 0;
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                blocks[index].Position = new Vector2f(x * (blocks[index].TextureRect.Width + 15) + 75,
                    y * (blocks[index].TextureRect.Height + 15) + 50);
                if (y > 5)
                    blocks[index].Position = new Vector2f(1000, 1000);
                index++;
            }
        }

        level = 1;
        isWin = false;
        isLose = false;
        blocksNumber = 60;
        missesNumber = 0;
        stick.Position = new Vector2f(400, 500);
        ball.sprite.Position = new Vector2f(375, 400);
    }

    public static void SetStartPositionSecondLevel()
    {
        SetTexturesForBlocks();

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

        level = 2;
        isWin = false;
        isLose = false;
        blocksNumber = 100;                                                                         
        missesNumber = 0;
        stick.Position = new Vector2f(400, 500);
        ball.sprite.Position = new Vector2f(375, 400);
    }

    public static void SetTexturesForBlocks()
    {
        Random rnd = new Random();

        for (int i = 0; i < blocks.Length; i++)
        {
            int randomNumber = rnd.Next(0, 10);

            if (randomNumber == 9)
                blocks[i] = new Sprite(strongBlockTexture);
            else
                blocks[i] = new Sprite(blockTexture);
        }
    }

    static void Main(string[] args)
    {
        window = new RenderWindow(new VideoMode(800, 600), "Arkanoid");
        window.SetFramerateLimit(60);
        window.Closed += Window_Closed;

        ballTexture = new Texture("Ball.png");
        blockTexture = new Texture("Block.png");
        strongBlockTexture = new Texture("StrongBlock.png");
        crackedBlockTexture = new Texture("CrackedBlock.png");
        stickTexture = new Texture("Stick.png");

        uint headerSize = 30;
        uint additionSize = 20;
        uint infoSize = 12;
        int firstLinePosY = 250;
        int secondLinePosY = 300;
        int thirdLinePosY = 350;
        int infoLinePosY = 550;
        int windowHalfWidth = Convert.ToInt32(window.Size.X) / 2;

        Font font = new Font("comic.ttf");
        Text winFirstLevel = new Text("Ты прошел первый уровень!", font, headerSize);
        winFirstLevel.Position = new Vector2f(windowHalfWidth - winFirstLevel.DisplayedString.Length * winFirstLevel.CharacterSize / 4,
            firstLinePosY);
        Text winSecondLevel = new Text("Ты прошел второй уровень!", font, headerSize);
        winSecondLevel.Position = new Vector2f(windowHalfWidth - winSecondLevel.DisplayedString.Length * winSecondLevel.CharacterSize / 4,
            firstLinePosY);
        Text lose = new Text("Ты проиграл!", font, headerSize);
        lose.Position = new Vector2f(windowHalfWidth - lose.DisplayedString.Length * lose.CharacterSize / 4,
            firstLinePosY);
        Text nextLevel = new Text("Нажмите \"N\" для перехода на следующий уровень", font, additionSize);
        nextLevel.Position = new Vector2f(windowHalfWidth - nextLevel.DisplayedString.Length * nextLevel.CharacterSize / 4,
            secondLinePosY);        
        Text returnFirstLevel = new Text("Нажмите \"T\" для возвращения на 1 уровень", font, additionSize);
        returnFirstLevel.Position = new Vector2f(windowHalfWidth - returnFirstLevel.DisplayedString.Length * returnFirstLevel.CharacterSize / 4,
            secondLinePosY);        
        Text clickRforRestart = new Text("Нажми \"R\" для перезапуска", font, additionSize);
        clickRforRestart.Position = new Vector2f(windowHalfWidth - clickRforRestart.DisplayedString.Length * clickRforRestart.CharacterSize / 4,
            thirdLinePosY);         Text remainingAttempts = new Text($"Оставшиеся попытки: {attempts - missesNumber}.", font, infoSize);
        remainingAttempts.Position = new Vector2f(75, infoLinePosY);


        ball = new Ball(ballTexture);
        stick = new Sprite(stickTexture);
        blocks = new Sprite[100];

        SetTexturesForBlocks();
        SetStartPositionFirstLevel();

        while (window.IsOpen == true)
        {
            window.Clear();

            window.DispatchEvents();

            if (isWin == false && isLose == false)
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

                //проверка на промах
                if (ball.CheckMiss() == true)
                {
                    missesNumber++;
                }

                stick.Position = new Vector2f(Mouse.GetPosition(window).X - stick.Texture.Size.X * 0.5f, stick.Position.Y);

                window.Draw(ball.sprite);
                window.Draw(stick);
                for (int i = 0; i < blocks.Length; i++)
                {
                    window.Draw(blocks[i]);
                }
                remainingAttempts = new Text($"Оставшиеся попытки: {attempts - missesNumber}.", font, infoSize);
                remainingAttempts.Position = new Vector2f(75, infoLinePosY);
                window.Draw(remainingAttempts);
            }
            //проверка на выигрыш
            if (blocksNumber == 0)
            {
                window.Clear();
                if (level == 1)
                {
                    window.Draw(winFirstLevel);
                    window.Draw(nextLevel);
                }
                if (level == 2)
                {
                    window.Draw(winSecondLevel);
                    window.Draw(returnFirstLevel);
                }
                window.Draw(clickRforRestart);
                isWin = true;
            }
            //проверка на проигрыш
            if (missesNumber == attempts)
            {
                window.Clear();
                window.Draw(lose);
                window.Draw(clickRforRestart);
                isLose = true;
            }

            if ((isWin == true || isLose == true) && Keyboard.IsKeyPressed(Keyboard.Key.R) == true)
            {
                if (level == 1)
                    SetStartPositionFirstLevel();
                if (level == 2)
                    SetStartPositionSecondLevel();
            }
            if (isWin == true && level == 1 && Keyboard.IsKeyPressed(Keyboard.Key.N) == true)
            {
                SetStartPositionSecondLevel();
            }
            if (isWin == true && level == 2 && Keyboard.IsKeyPressed(Keyboard.Key.T) == true)
            {
                SetStartPositionFirstLevel();
            }

            window.Display();
        }

    }

    private static void Window_Closed(object sender, EventArgs e)
    {
        window.Close();
    }
}
