using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
namespace escape;

public class MenuScene : IScene
{
    private GraphicsDeviceManager _graphics;
    private SceneManager SceneManager;
    private ContentManager Content;
    private GraphicsDevice graphicsDevice;

    private Texture2D buttonBg;
    private Rectangle quitBtn;
    private Rectangle startBtn;
    private SpriteFont font;

    private const string quitBtnString = "Quit";
    private const string startBtnString = "Start";


    public MenuScene( ContentManager contentManager, SceneManager sceneManager, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice) {
        this.Content = contentManager;
        this.SceneManager = sceneManager;
        this._graphics = graphics;
        this.graphicsDevice = graphicsDevice;

        int btnH = 64; 
        int btnW = 256;

        int width = graphicsDevice.Viewport.Width;
        int height = graphicsDevice.Viewport.Height;

        int margin = 10;


        startBtn = new(width / 2 - btnW - margin /2, height / 2 - btnH / 2, btnW, btnH);
        quitBtn = new(width / 2 + margin / 2, height / 2 - btnH / 2, btnW, btnH);
    }

    public void Load() {
        buttonBg = Content.Load<Texture2D>("Characters/buttonBg");
        font = Content.Load<SpriteFont>("Font/File");
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
        graphicsDevice.Clear(Color.CornflowerBlue);

        Vector2 quitBtnStringOrigin = font.MeasureString(quitBtnString) / 2;
        Vector2 startBtnStringOrigin = font.MeasureString(startBtnString) / 2;

        spriteBatch.Draw(buttonBg, quitBtn, Color.White);
        spriteBatch.Draw(buttonBg, startBtn, Color.White);

        spriteBatch.DrawString(font, startBtnString, startBtn.Center.ToVector2(), Color.Black,
        0, quitBtnStringOrigin, 1.0f, SpriteEffects.None, 0.5f);

        spriteBatch.DrawString(font, quitBtnString, quitBtn.Center.ToVector2(), Color.Black,
        0, quitBtnStringOrigin, 1.0f, SpriteEffects.None, 0.5f);
    }
    public void Update(GameTime gameTime) {
        MouseState mouse = Mouse.GetState();
        if(quitBtn.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
        {
            Environment.Exit(0);
        }

        if(startBtn.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
        {
            SceneManager.RemoveScene();
            SceneManager.AddScene(new GameScene(Content, SceneManager, _graphics, graphicsDevice));
        }
    }

};