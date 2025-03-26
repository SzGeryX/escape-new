using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace escape;

public class Sprite {
    public Texture2D texture;
    public Rectangle drect, srect;

    public Sprite(Texture2D texture, Rectangle drect, Rectangle srect) {
        this.texture = texture;
        this.drect = drect;
        this.srect = srect;
    }

    public virtual void Update(GameTime gameTime) {

    }

    public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset) {

        
    }
}