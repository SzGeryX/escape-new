using Microsoft.Xna.Framework;

namespace escape;
public class Camera
{
    public Vector2 position;

    public Camera(Vector2 position)
    {
        this.position = position; 
    }

    public void Follow(Rectangle target, Vector2 screenSize)
    {
        position = new Vector2(
                -target.X + (screenSize.X / 2 - target.Width / 2),
                -target.Y + (screenSize.Y / 2 - target.Height / 2)
            );
        
    }

}
