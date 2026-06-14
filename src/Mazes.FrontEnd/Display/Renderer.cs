using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mazes.FrontEnd.Display;

public class Renderer : Game
{
    // ReSharper disable once NotAccessedField.Local
    private GraphicsDeviceManager _graphics;

    private Texture2D _texture;

    private readonly Color[] _data = new Color[Constants.Width * Constants.TileSize * Constants.Height * Constants.TileSize];

    private SpriteBatch _spriteBatch;

    public Renderer()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = (int) (Constants.Width * Constants.TileSize),
            PreferredBackBufferHeight = (int) (Constants.Height * Constants.TileSize)
        };
    }
    
    protected override void Draw(GameTime gameTime)
    {
        DrawIntoData();

        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.FrontToBack);

        _texture.SetData(_data);

        _spriteBatch.Draw(_texture, new Vector2(0, 0), new Rectangle(0, 0, Constants.Width * Constants.TileSize, Constants.Height * Constants.TileSize), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawIntoData()
    {
        for (var x = 0; x < Constants.Width * Constants.TileSize; x++)
        {
            for (var y = 0; y < Constants.Height * Constants.TileSize; y++)
            {
                if (_mazeSolution[x / Constants.TileSize, y / Constants.TileSize])
                {
                    if (x % Constants.TileSize > 2 && x % Constants.TileSize < Constants.TileSize - 3
                                                   && y % Constants.TileSize > 2 && y % Constants.TileSize < Constants.TileSize - 3)
                    {
                        _data[x + y * Constants.Width * Constants.TileSize] = Color.FromNonPremultiplied(0, 192, 0, 255);

                        continue;
                    }

                    if (x % Constants.TileSize > 1 && x % Constants.TileSize < Constants.TileSize - 2
                                                   && y % Constants.TileSize > 1 && y % Constants.TileSize < Constants.TileSize - 2)
                    {
                        _data[x + y * Constants.Width * Constants.TileSize] = Color.FromNonPremultiplied(0, 96, 0, 255);

                        continue;
                    }
                }

                if (_mazeVisited[x / Constants.TileSize, y / Constants.TileSize])
                {
                    if (x % Constants.TileSize > 2 && x % Constants.TileSize < Constants.TileSize - 3
                                                   && y % Constants.TileSize > 2 && y % Constants.TileSize < Constants.TileSize - 3)
                    {
                        _data[x + y * Constants.Width * Constants.TileSize] = Color.FromNonPremultiplied(171, 107, 0, 255);

                        continue;
                    }

                    if (x % Constants.TileSize > 1 && x % Constants.TileSize < Constants.TileSize - 2
                                                   && y % Constants.TileSize > 1 && y % Constants.TileSize < Constants.TileSize - 2)
                    {
                        _data[x + y * Constants.Width * Constants.TileSize] = Color.FromNonPremultiplied(107, 43, 0, 255);

                        continue;
                    }
                }

                if (! _maze[x / Constants.TileSize, y / Constants.TileSize])
                {
                    _data[x + y * Constants.Width * Constants.TileSize] = Color.FromNonPremultiplied(64, 64, 64, 255);
                }
                else
                {
                    _data[x + y * Constants.Width * Constants.TileSize] = Color.Black;
                }
            }
        }
    }}