using System;
using System.Collections.Generic;
using System.Linq;
using Mazes.Core;
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
    
    private HashSet<(int X, int Y)> _path = [];

    public Renderer()
    {
        _graphics = new GraphicsDeviceManager(this);

        PuzzleManager.Path = "Data/Puzzles.json";
    }

    protected override void LoadContent()
    {
        _maze = PuzzleManager.Instance.GetPuzzle(0);

        var result = new Solver(_maze).Solve();

        _path = result.Path.ToHashSet();
        
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
        Array.Fill(_data, Color.Black);

        for (var mazeY = 0; mazeY < _maze.Height; mazeY++)
        {
            for (var mazeX = 0; mazeX < _maze.Width; mazeX++)
            {
                var position = (mazeX, mazeY);

                if (_maze[mazeX, mazeY])
                {
                    DrawTile(mazeX, mazeY, new Color(64, 64, 64), 0);

                    continue;
                }

                if (position == (_maze.Start.X, _maze.Start.Y))
                {
                    DrawTile(mazeX, mazeY, Color.Lime, 2);

                    continue;
                }

                if (position == (_maze.End.X, _maze.End.Y))
                {
                    DrawTile(mazeX, mazeY, Color.Fuchsia, 2);

                    continue;
                }

                if (_path.Contains(position))
                {
                    DrawTile(mazeX, mazeY, Color.Green, borderSize: 2);
                }
            }
        }
    }

    private void DrawTile(int mazeX, int mazeY, Color color, int borderSize)
    {
        var pixelWidth = _maze.Width * Constants.TileSize;

        var startX = mazeX * Constants.TileSize + borderSize;
        
        var startY = mazeY * Constants.TileSize + borderSize;

        var endX = (mazeX + 1) * Constants.TileSize - borderSize;
        
        var endY = (mazeY + 1) * Constants.TileSize - borderSize;

        for (var y = startY; y < endY; y++)
        {
            for (var x = startX; x < endX; x++)
            {
                _data[x + y * pixelWidth] = color;
            }
        }
    }
}