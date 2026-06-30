using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mazes.Core;
using Mazes.Core.Infrastructure;
using Mazes.Core.Models;
using Mazes.FrontEnd.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Console;

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
        Window.Title = "Mazes";

        IsMouseVisible = true;

        var client = new PuzzleClient();

        WriteLine("Loading maze...");

        var response = client.GetPuzzle(Difficulty.Small, DateOnly.FromDateTime(DateTime.Now.Date));

        if (response == null)
        {
            WriteLine("No puzzle found.");

            return;
        }

        _maze = response.Value.Maze;

        var stopwatch = Stopwatch.StartNew();

        var result = new Solver(_maze).Solve();

        stopwatch.Stop();

        WriteLine(@$"Solved in {stopwatch.Elapsed:ss\.fff}.");

        _path = result.Path.ToHashSet();

        var pixelWidth = GetPixelSize(_maze.Width) + Constants.BorderSize * 2;

        var pixelHeight = GetPixelSize(_maze.Height) + Constants.BorderSize * 2;

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
        Array.Fill(_data, Constants.BackgroundColour);

        for (var mazeY = 0; mazeY < _maze.Height; mazeY++)
        {
            for (var mazeX = 0; mazeX < _maze.Width; mazeX++)
            {
                var position = (mazeX, mazeY);

                if (_maze[mazeX, mazeY])
                {
                    DrawTile(mazeX, mazeY, Constants.WallColour, 0);
                }

                if (position == (_maze.Start.X, _maze.Start.Y))
                {
                    DrawPathDot(mazeX, mazeY, Constants.StartColour);

                    continue;
                }

                if (position == (_maze.End.X, _maze.End.Y))
                {
                    DrawPathDot(mazeX, mazeY, Constants.FinishColour);

                    continue;
                }

                if (_path.Contains(position))
                {
                    DrawPathDot(mazeX, mazeY, Constants.PathColour);
                }
            }
        }
    }

    private void DrawTile(int mazeX, int mazeY, Color color, int borderSize)
    {
        var pixelWidth = GetPixelSize(_maze.Width) + Constants.BorderSize * 2;

        var startX = GetPixelStart(mazeX) + Constants.BorderSize;

        var startY = GetPixelStart(mazeY) + Constants.BorderSize;

        var endX = GetPixelStart(mazeX + 1) + Constants.BorderSize;

        var endY = GetPixelStart(mazeY + 1) + Constants.BorderSize;

        var xBorder = Math.Min(borderSize, (endX - startX - 1) / 2);

        var yBorder = Math.Min(borderSize, (endY - startY - 1) / 2);

        startX += xBorder;

        startY += yBorder;

        endX -= xBorder;

        endY -= yBorder;

        for (var y = startY; y < endY; y++)
        {
            for (var x = startX; x < endX; x++)
            {
                _data[x + y * pixelWidth] = color;
            }
        }
    }

    private void DrawPathDot(int mazeX, int mazeY, Color color)
    {
        if (IsMazeBorder(mazeX, mazeY))
        {
            return;
        }

        var pixelWidth = GetPixelSize(_maze.Width) + Constants.BorderSize * 2;

        var startX = GetPixelStart(mazeX) + Constants.BorderSize;

        var startY = GetPixelStart(mazeY) + Constants.BorderSize;

        var endX = GetPixelStart(mazeX + 1) + Constants.BorderSize;

        var endY = GetPixelStart(mazeY + 1) + Constants.BorderSize;

        var centreX = (startX + endX) / 2;

        var centreY = (startY + endY) / 2;

        const int radius = 2;

        for (var y = centreY - radius; y <= centreY + radius; y++)
        {
            for (var x = centreX - radius; x <= centreX + radius; x++)
            {
                var dx = x - centreX;

                var dy = y - centreY;

                if (dx * dx + dy * dy <= radius * radius)
                {
                    _data[x + y * pixelWidth] = color;
                }
            }
        }
    }

    private bool IsMazeBorder(int mazeX, int mazeY)
    {
        return mazeX == 0 ||
               mazeY == 0 ||
               mazeX == _maze.Right ||
               mazeY == _maze.Bottom;
    }

    private static int GetPixelSize(int mazeSize)
    {
        var pathCount = mazeSize / 2;

        var wallCount = mazeSize - pathCount;

        return pathCount * Constants.PathSize + wallCount * Constants.WallSize;
    }

    private static int GetPixelStart(int mazeCoordinate)
    {
        var pathCount = mazeCoordinate / 2;

        var wallCount = mazeCoordinate - pathCount;

        return pathCount * Constants.PathSize + wallCount * Constants.WallSize;
    }
}