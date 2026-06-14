using Mazes.Core.Infrastructure;
using Mazes.Core.Models;
using Mazes.FrontEnd.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mazes.FrontEnd.Display;

public class Renderer : Game
{
    // ReSharper disable once NotAccessedField.Local
    private GraphicsDeviceManager _graphics;

    private Texture2D _texture;

    private Color[] _data;

    private SpriteBatch _spriteBatch;

    private Maze _maze;

    public Renderer()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 20 * Constants.TileSize,
            PreferredBackBufferHeight = 20 * Constants.TileSize
        };

        PuzzleManager.Path = "Data/Puzzles.json";
    }

    protected override void LoadContent()
    {
        _maze = PuzzleManager.Instance.GetPuzzle(0);

        _data = new Color[_maze.Width * Constants.TileSize * _maze.Height * Constants.TileSize];

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _texture = new Texture2D(GraphicsDevice, _maze.Width * Constants.TileSize, _maze.Height * Constants.TileSize);

        base.LoadContent();
    }

    protected override void Draw(GameTime gameTime)
    {
        DrawIntoData();

        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.FrontToBack);

        _texture.SetData(_data);

        _spriteBatch.Draw(_texture, new Vector2(0, 0), new Rectangle(0, 0, _maze.Width * Constants.TileSize, _maze.Height * Constants.TileSize), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawIntoData()
    {
        for (var x = 0; x < _maze.Width * Constants.TileSize; x++)
        {
            for (var y = 0; y < _maze.Height * Constants.TileSize; y++)
            {
                // if (_mazeSolution[x / Constants.TileSize, y / Constants.TileSize])
                // {
                //     if (x % Constants.TileSize > 2 && x % Constants.TileSize < Constants.TileSize - 3
                //                                    && y % Constants.TileSize > 2 && y % Constants.TileSize < Constants.TileSize - 3)
                //     {
                //         _data[x + y * _maze.Width * Constants.TileSize] = Color.FromNonPremultiplied(0, 192, 0, 255);
                //
                //         continue;
                //     }
                //
                //     if (x % Constants.TileSize > 1 && x % Constants.TileSize < Constants.TileSize - 2
                //                                    && y % Constants.TileSize > 1 && y % Constants.TileSize < Constants.TileSize - 2)
                //     {
                //         _data[x + y * _maze.Width * Constants.TileSize] = Color.FromNonPremultiplied(0, 96, 0, 255);
                //
                //         continue;
                //     }
                // }

                // if (_mazeVisited[x / Constants.TileSize, y / Constants.TileSize])
                // {
                //     if (x % Constants.TileSize > 2 && x % Constants.TileSize < Constants.TileSize - 3
                //                                    && y % Constants.TileSize > 2 && y % Constants.TileSize < Constants.TileSize - 3)
                //     {
                //         _data[x + y * Constants.Width * Constants.TileSize] = Color.FromNonPremultiplied(171, 107, 0, 255);
                //
                //         continue;
                //     }
                //
                //     if (x % Constants.TileSize > 1 && x % Constants.TileSize < Constants.TileSize - 2
                //                                    && y % Constants.TileSize > 1 && y % Constants.TileSize < Constants.TileSize - 2)
                //     {
                //         _data[x + y * Constants.Width * Constants.TileSize] = Color.FromNonPremultiplied(107, 43, 0, 255);
                //
                //         continue;
                //     }
                // }

                if (! _maze[x / Constants.TileSize, y / Constants.TileSize])
                {
                    _data[x + y * _maze.Width * Constants.TileSize] = Color.FromNonPremultiplied(64, 64, 64, 255);
                }
                else
                {
                    _data[x + y * _maze.Width * Constants.TileSize] = Color.Black;
                }
            }
        }
    }
}