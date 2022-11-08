using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;

class Ball
{
    public Sprite sprite;
    private float speed;
    private Vector2f direction;

    public Ball(Texture texture)
    {
        sprite = new Sprite(texture);
    }

    public void Start(float speed, Vector2f direction)
    {
        if (this.speed != 0) return;
        
        this.speed = speed;
        this.direction = direction;
    }

    public void Move(Vector2i boundsPos, Vector2i boundsSize)
    {
        sprite.Position += direction * speed;

        if (sprite.Position.X > boundsSize.X - sprite.Texture.Size.X || sprite.Position.X < boundsPos.X)
        {
            direction.X *= -1;
        }
        if (sprite.Position.Y < boundsPos.Y)
        {
            direction.Y *= -1;
        }
    }

    public bool CheckCollision(Sprite sprite, string tag)
    {
        if (this.sprite.GetGlobalBounds().Intersects(sprite.GetGlobalBounds()) == true)
        {
            if (tag == "Stick")
            {
                direction.Y *= -1;
                float f = ((this.sprite.Position.X + this.sprite.Texture.Size.X * 0.5f) - (sprite.Position.X + sprite.Texture.Size.X * 0.5f)) / sprite.Texture.Size.X * 0.5f;
                direction.X = f * 2;
            }
            if (tag == "Block")
            {
                direction.Y *= -1;
            }

            return true;
        }
        return false;
    }
}
