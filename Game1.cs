using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Linq;

namespace escape;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private SceneManager SceneManager;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        this.SceneManager = new();
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = true;
        
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        this.SceneManager.AddScene(new MenuScene(Content, SceneManager, _graphics, GraphicsDevice));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        SceneManager.GetCurrentScene().Update(gameTime);

        base.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        SceneManager.GetCurrentScene().Draw(_spriteBatch, GraphicsDevice);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
