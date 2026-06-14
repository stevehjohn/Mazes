using Mazes.Core.Infrastructure;
using Mazes.Core.Models;
using Mazes.FrontEnd.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mazes.FrontEnd.Display;

public class Renderer : Game
{
    // ReSharper disable once NotAccessedField.Local
    private readonly GraphicsDeviceManager _graphics;

    private Texture2D _texture;

    private Color[] _data;

    private SpriteBatch _spriteBatch;

    private Maze _maze;

    public Renderer()
    {
        _graphics = new GraphicsDeviceManager(this);

        PuzzleManager.Path = "Data/Puzzles.json";
    }

    protected override void LoadContent()
    {
        _maze = PuzzleManager.Instance.GetPuzzle(0);

        var pixelWidth = _maze.Width * Constants.TileSize;
        
        var pixelHeight = _maze.Height * Constants.TileSize;

        _graphics.PreferredBackBufferWidth = pixelWidth;
        
        _graphics.PreferredBackBufferHeight = pixelHeight;
        
        _graphics.ApplyChanges();

        _data = new Color[pixelWidth * pixelHeight];

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _texture = new Texture2D(GraphicsDevice, pixelWidth, pixelHeight);

        base.LoadContent();
    }

    protected override void Draw(GameTime gameTime)
    {
        DrawIntoData();

        GraphicsDevice.Clear(Color.Black);

        _texture.SetData(_data);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_texture, Vector2.Zero, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawIntoData()
    {
        var pixelWidth = _maze.Width * Constants.TileSize;
        
        var pixelHeight = _maze.Height * Constants.TileSize;

        for (var y = 0; y < pixelHeight; y++)
        {
            for (var x = 0; x < pixelWidth; x++)
            {
                var mazeX = x / Constants.TileSize;
                
                var mazeY = y / Constants.TileSize;

                _data[x + y * pixelWidth] = _maze[mazeX, mazeY] ? Color.Black : new Color(64, 64, 64);
            }
        }
    }
}