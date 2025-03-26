using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
namespace escape;

public class WinScene : IScene
{
    private GraphicsDeviceManager _graphics;
    private SceneManager SceneManager;
    private ContentManager Content;
    private GraphicsDevice graphicsDevice;

    private Texture2D buttonBg;
    private Rectangle quitBtn;
    private Rectangle restartBtn;
    private SpriteFont font;
    private const string quitBtnString = "Quit";
    private const string deathMsg = "You WON!";
    private const string restartBtnString = "Restart";

    public WinScene(ContentManager contentManager, SceneManager sceneManager, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice)
    {
        this.Content = contentManager;
        this.SceneManager = sceneManager;
        this._graphics = graphics;
        this.graphicsDevice = graphicsDevice;

        int btnH = 64;
        int btnW = 256;
        int margin = 5;

        int width = graphicsDevice.Viewport.Width;
        int height = graphicsDevice.Viewport.Height;

        restartBtn = new(width / 2 - btnW / 2, height / 2 - btnH / 2 + btnH * 2, btnW, btnH);
        quitBtn = new(width / 2 - btnW / 2, height / 2 - btnH / 2 + btnH * 3 + margin, btnW, btnH);
    }

    public void Load()
    {
        buttonBg = Content.Load<Texture2D>("Characters/buttonBg");
        font = Content.Load<SpriteFont>("Font/File");
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        graphicsDevice.Clear(Color.CornflowerBlue);

        Vector2 quitBtnStringOrigin = font.MeasureString(quitBtnString) / 2;
        Vector2 restartBtnStringOrigin = font.MeasureString(restartBtnString) / 2;
        Vector2 deathMsgOrigin = font.MeasureString(deathMsg) / 2;

        spriteBatch.Draw(buttonBg, quitBtn, Color.White);
        spriteBatch.Draw(buttonBg, restartBtn, Color.White);

        spriteBatch.DrawString(font, quitBtnString, quitBtn.Center.ToVector2(), Color.Black,
        0, quitBtnStringOrigin, 1.0f, SpriteEffects.None, 0.5f);

        spriteBatch.DrawString(font, restartBtnString, restartBtn.Center.ToVector2(), Color.Black,
        0, restartBtnStringOrigin, 1.0f, SpriteEffects.None, 0.5f);

        spriteBatch.DrawString(font, deathMsg, new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2), Color.Red,
        0, deathMsgOrigin, 1.5f, SpriteEffects.None, 1f);
    }

    public void Update(GameTime gameTime)
    {
        MouseState mouse = Mouse.GetState();
        if (quitBtn.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
        {
            Environment.Exit(0);
        }
        if (restartBtn.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
        {
            SceneManager.RemoveScene();
            SceneManager.AddScene(new MenuScene(Content, SceneManager, _graphics, graphicsDevice));
        }
    }
}
