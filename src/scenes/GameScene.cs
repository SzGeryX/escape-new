using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using Newtonsoft.Json;
using System.Linq;
using System.Diagnostics;

namespace escape;



public class GameScene : IScene {
    private GraphicsDeviceManager _graphics;
    public Dictionary<Vector2, int> tilemap;
    private List<Rectangle> collisionRectangles;
    private Texture2D textureAtlas;
    private Player player;
    private NPC npc;


    private ContentManager Content;
    private SceneManager SceneManager;
    private GraphicsDevice graphicsDevice;
    private Camera camera;


    private int display_tilesize; 
    private const int pixel_tilesize = 64; 
    private const int num_tiles_per_row = 25;
    private bool isPlayerDead = false;
    private bool isNpcDead = false;

    private Texture2D buttonBg;
    private Rectangle quitBtn;

    private Vector2 winPosition = new Vector2(1344, 134);

    private Dictionary<Vector2, int> loadMap(string filepath)
    {
        Dictionary<Vector2, int> result = new();

        StreamReader reader = new(filepath);

        int y = 0;
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] items = line.Split(',');

            for (int x = 0; x < items.Length; x++)
            {
                if (int.TryParse(items[x], out int value))
                {
                    if (value > -1)
                    {
                        result[new Vector2(x, y)] = value;
                    }
                }
            }

            y++;
        }

        return result;
    }

    private void LoadCollisions(string jsonFilePath)
    {
        string json = File.ReadAllText(jsonFilePath);
        var mapData = JsonConvert.DeserializeObject<MapData>(json);

        collisionRectangles = new List<Rectangle>();
        foreach (var layer in mapData.layers)
        {
            if (layer.name == "Object Layer 1")
            {
                foreach (var obj in layer.objects)
                {
                    if (obj.name == "collisions")
                    {
                        collisionRectangles.Add(new Rectangle(
                            (int)obj.x,
                            (int)obj.y,
                            (int)obj.width,
                            (int)obj.height));
                    }
                }
            }
        }
    }

    public GameScene(ContentManager contentManager, SceneManager sceneManager, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice) {
        this.Content = contentManager;
        this.SceneManager = sceneManager;
        this._graphics = graphics;
        this.graphicsDevice = graphicsDevice;
    

        camera = new(Vector2.Zero);
        tilemap = loadMap("../../../Data/prison.csv");

        int displayWidth = graphicsDevice.Viewport.Width;
        int displayHeight = graphicsDevice.Viewport.Height;

        int mapWidthInTiles = tilemap.Keys.Max(k => (int)k.X) + 1;
        int mapHeightInTiles = tilemap.Keys.Max(k => (int)k.Y) + 1;
      
        float scaleX = (float)displayWidth / (mapWidthInTiles * pixel_tilesize);
        float scaleY = (float)displayHeight / (mapHeightInTiles * pixel_tilesize);

        float scale = Math.Min(scaleX, scaleY);

        quitBtn = new(1375, 175, 10, 10);

        display_tilesize = (int)(pixel_tilesize * scale);
    }


    public void Load() {
        textureAtlas = Content.Load<Texture2D>("Characters/Tileset");

        Texture2D playerTexture = Content.Load<Texture2D>("Characters/playeranimation");
        player = new Player(playerTexture, new Rectangle(100, 220, 64, 64), new Rectangle(0, 0, 64, 64));

        Texture2D npcTexture = Content.Load<Texture2D>("Characters/guardanimation");
        npc = new NPC(npcTexture, new Vector2(100, 500), new Vector2(1090, 500), 100f);

        LoadCollisions("../../../Data/prison.json");

        buttonBg = Content.Load<Texture2D>("Characters/buttonBg");
    }

    public void Draw(SpriteBatch _spriteBatch, GraphicsDevice graphicsDevice) {
        camera.Follow(player.drect, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));

        int display_tilesize = 64;
        int num_tiles_per_row = 25;
        int pixel_tilesize = 64;

        foreach (var item in tilemap)
        {
            Rectangle drect = new(
                (int)item.Key.X * display_tilesize,
                (int)item.Key.Y * display_tilesize,
                display_tilesize,
                display_tilesize
            );

            int x = item.Value % num_tiles_per_row;
            int y = item.Value / num_tiles_per_row;

            Rectangle src = new(
                x * pixel_tilesize,
                y * pixel_tilesize,
                pixel_tilesize,
                pixel_tilesize
            );

            _spriteBatch.Draw(textureAtlas, drect, src, Color.White);
        }

        if (!isPlayerDead)
        {
            if (!isNpcDead)
            {
            
                player.Draw(_spriteBatch, Vector2.Zero);
                npc.Draw(_spriteBatch);
            }
            else
            {
                npc.Draw(_spriteBatch);
                player.Draw(_spriteBatch, Vector2.Zero);
            }
        }
        else
        {
            SceneManager.RemoveScene();
            SceneManager.AddScene(new EndScene(Content, SceneManager, _graphics, graphicsDevice));
        }



        _spriteBatch.Draw(buttonBg, quitBtn, Color.Black);

    }

    public void Update(GameTime gameTime) {
        if (!isPlayerDead)
        {
            player.Update(gameTime, collisionRectangles);
            npc.Update(gameTime);

            if (npc.IsPlayerNearby(player))
            {
                Random rnd = new Random();
                int outcome = rnd.Next(2);

                if (outcome == 0)
                {
                    isPlayerDead = true; 
                }
                else
                {
                    npc.MarkAsDead(); 
                }
            }
        }


        Vector2 playerTilePos = new Vector2(player.drect.X, player.drect.Y);
        if ((int)playerTilePos.X >= (int)winPosition.X - 4 &&
            (int)playerTilePos.X <= (int)winPosition.X + 4 &&
            (int)playerTilePos.Y >= (int)winPosition.Y - 4 &&
            (int)playerTilePos.Y <= (int)winPosition.Y + 4)
        {
            SceneManager.RemoveScene();
            SceneManager.AddScene(new WinScene(Content, SceneManager, _graphics, graphicsDevice));
        }

        

    }
}