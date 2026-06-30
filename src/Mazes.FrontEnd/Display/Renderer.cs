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
    private readonly GraphicsDeviceManager _graphics;
    
    private Texture2D _texture;
    
    private Color[] _data;
    
    private SpriteBatch _spriteBatch;
    
    private Maze _maze;
    
    private HashSet<(int X, int Y)> _path = [];

    public Renderer()
    {
        _graphics = new GraphicsDeviceManager(this);
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
        
        var sw = Stopwatch.StartNew();
        
        var result = new Solver(_maze).Solve();
        
        sw.Stop();
        
        WriteLine(@$"Solved in {sw.Elapsed:ss\.fff}.");
        
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
        
        for (var y = 0; y < _maze.Height; y++)
        for (var x = 0; x < _maze.Width; x++)
        {
            var p = (x, y);
            
            if (_maze[x, y])
            {
                DrawTile(x, y, Constants.WallColour, 0);
                
                continue;
            }

            if (_path.Contains(p)) DrawPathDot(x, y, Constants.PathColour);
            
            if (p == (_maze.Start.X, _maze.Start.Y))
            {
                DrawPathDot(x, y, Constants.StartColour);
                
                continue;
            }

            if (p == (_maze.End.X, _maze.End.Y))
            {
                DrawPathDot(x, y, Constants.FinishColour);
            }
        }

        DrawSolutionLine();
    }

    private void DrawSolutionLine()
    {
        var ordered = _path.OrderBy(_ => 0).ToList();
        
        for (var i = 0; i < ordered.Count - 1; i++)
        {
            DrawLine(ordered[i], ordered[i + 1], Constants.PathColour);
        }
    }

    private void DrawLine((int X, int Y) from, (int X, int Y) to, Color colour)
    {
        var x1 = GetCentre(from.X);
        
        var y1 = GetCentre(from.Y);
        
        var x2 = GetCentre(to.X);
        
        var y2 = GetCentre(to.Y);
        
        var pixelWidth = GetPixelSize(_maze.Width) + Constants.BorderSize * 2;
        
        var thickness = Math.Max(1, Constants.LineThickness);
        
        if (x1 == x2)
        {
            for (var y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
                
            for (var dx = -thickness; dx <= thickness; dx++)
                _data[(x1 + dx) + y * pixelWidth] = colour;
        }
        else if (y1 == y2)
        {
            for (var x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
                
            for (var dy = -thickness; dy <= thickness; dy++)
                _data[x + (y1 + dy) * pixelWidth] = colour;
        }
    }

    private static int GetCentre(int mazeCoord)
    {
        var s = GetPixelStart(mazeCoord) + Constants.BorderSize;
        
        var e = GetPixelStart(mazeCoord + 1) + Constants.BorderSize;
        
        return (s + e) / 2;
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
        for (var x = startX; x < endX; x++)
            _data[x + y * pixelWidth] = color;
    }

    private void DrawPathDot(int mazeX, int mazeY, Color color)
    {
        if (IsMazeBorder(mazeX, mazeY)) return;
        
        var pixelWidth = GetPixelSize(_maze.Width) + Constants.BorderSize * 2;
        
        var startX = GetPixelStart(mazeX) + Constants.BorderSize - 1;
        
        var startY = GetPixelStart(mazeY) + Constants.BorderSize - 1;
        
        var endX = GetPixelStart(mazeX + 1) + Constants.BorderSize;
        
        var endY = GetPixelStart(mazeY + 1) + Constants.BorderSize;
        
        var centreX = (startX + endX) / 2;
        
        var centreY = (startY + endY) / 2;
        
        for (var y = centreY - Constants.LineThickness; y <= centreY + Constants.LineThickness; y++)
        for (var x = centreX - Constants.LineThickness; x <= centreX + Constants.LineThickness; x++)
        {
            var dx = x - centreX;
            
            var dy = y - centreY;
            
            if (dx * dx + dy * dy <= Constants.LineThickness * Constants.LineThickness) _data[x + y * pixelWidth] = color;
        }
    }

    private bool IsMazeBorder(int x, int y) => x == 0 || y == 0 || x == _maze.Right || y == _maze.Bottom;

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