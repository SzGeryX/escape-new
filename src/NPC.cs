using escape;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public class NPC
{
    private Texture2D texture;
    private Vector2 position;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private Vector2 direction;
    private float speed;
    private bool movingForward;
    private Rectangle collisionBox;
    private Rectangle drawRectangle;
    private bool flipped;

    private int currentFrame;
    private float animationTimer;
    private float animationSpeed = 0.15f; 
    private int totalFrames = 6;
    private int frameWidth = 32; 
    private int frameHeight = 32; 
    
    private bool isPlayerDead = false;

    public NPC(Texture2D texture, Vector2 startPosition, Vector2 endPosition, float speed)
    {
        this.texture = texture;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.position = startPosition;
        this.speed = speed;
        this.movingForward = true;
        this.direction = Vector2.Normalize(endPosition - startPosition);


        drawRectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);

        UpdateCollisionBox();
    }

    private void UpdateCollisionBox()
    {
        collisionBox = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
    }

    public void Update(GameTime gameTime)
    {

        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        position += direction * speed * delta;
        UpdateCollisionBox();

        animationTimer += delta;
        if (animationTimer > animationSpeed)
        {
            currentFrame = (currentFrame + 1) % totalFrames;
            animationTimer = 0f;
        }

        if (Vector2.Distance(position, startPosition) < 1f)
        {
            movingForward = true;
            direction = Vector2.Normalize(endPosition - startPosition);
            flipped = false;
        }
        else if (Vector2.Distance(position, endPosition) < 1f)
        {
            movingForward = false;
            direction = Vector2.Normalize(startPosition - endPosition);
            flipped = true;
        }

        int scaleFactor = 2;
        drawRectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth * scaleFactor, frameHeight * scaleFactor);
    }

    public void Draw(SpriteBatch spriteBatch)
    {

        Rectangle sourceRectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
        
        spriteBatch.Draw(texture, drawRectangle, sourceRectangle, Color.White, 0f, Vector2.Zero, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

    }

    public bool IsPlayerNearby(Player player)
    {
        return collisionBox.Intersects(player.drect);
    }
}
