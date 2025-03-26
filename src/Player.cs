using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace escape;

public class Player : Sprite
{
    private bool direction;
    private int currentFrame;
    private float animationTimer;
    private float animationSpeed = 0.15f;
    private int totalFrames = 4;
    private int frameWidth = 32;  
    private int frameHeight = 32; 
    private bool isMoving;
    public Rectangle CollisionBox => drect;

    public Player(Texture2D texture, Rectangle drect, Rectangle srect)
        : base(texture, drect, srect)
    {


        direction = false;
        currentFrame = 0;
        animationTimer = 0f;
    }

    public void Update(GameTime gameTime, List<Rectangle> collisionRectangles)
    {
        Vector2 newPosition = new Vector2(drect.X, drect.Y);
        bool wasMoving = isMoving; 
        isMoving = false;
        float speed = 2f;

        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            newPosition.Y -= speed;
            isMoving = true;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            newPosition.Y += speed;
            isMoving = true;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            direction = true;
            newPosition.X -= speed;
            isMoving = true;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            direction = false;
            newPosition.X += speed;
            isMoving = true;
        }

        // �tk�z�skezel�s
        Rectangle newRect = new Rectangle((int)newPosition.X, (int)newPosition.Y, drect.Width, drect.Height);
        bool colliding = false;
        foreach (var rect in collisionRectangles)
        {
            if (newRect.Intersects(rect))
            {
                colliding = true;
                break;
            }
        }
        if (!colliding)
        {
            drect.X = (int)newPosition.X;
            drect.Y = (int)newPosition.Y;
        }

        if (isMoving)
        {

            animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds * (5f / speed);
            if (animationTimer >= animationSpeed)
            {
                currentFrame = (currentFrame + 1) % totalFrames;
                animationTimer = 0f;
            }
        }
        else if (wasMoving) 
        {
            currentFrame = 0;
            animationTimer = 0f;
        }
    }


    public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
       
        Rectangle sourceRectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);

        Rectangle dest = new(
            drect.X + (int)offset.X,
            drect.Y + (int)offset.Y,
            drect.Width,
            drect.Height
        );


        spriteBatch.Draw(
            texture,
            dest,
            sourceRectangle,
            Color.White,
            0.0f,
            Vector2.Zero,
            (direction ? SpriteEffects.FlipHorizontally : SpriteEffects.None),
            1.0f
        );
    }
}
