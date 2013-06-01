using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Input;
using Star.Game;
using Star.Game.Level;

namespace Star.Game.Level
{
    public interface ILayer : IDisposable
    {
        void Initialize(IServiceProvider ServiceProvider, int numberOfLayerObjects,Star.GameManagement.Options options,LevelVariables levelvariables);
        void Update(GameTime gametime, Vector2 player_difference,Star.GameManagement.Options options);
        void Draw(SpriteBatch spritebatch, Matrix matrix);
    }
}
