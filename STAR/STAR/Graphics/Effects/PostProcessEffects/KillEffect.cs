using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Game.Level;
using Star.GameManagement;
using Star.Game.GameEvents;

namespace Star.Graphics.Effects.PostProcessEffects
{
    class KillEffect : GraphicEffect
    {
        float ymotion;
        protected override AvailableEffects InitializeEffect(Microsoft.Xna.Framework.Graphics.GraphicsDevice device, Star.GameManagement.Options options)
        {
            return AvailableEffects.KillEffect;
        }

        protected override void UpdateEffect(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Vector2 pos)
        {
            ymotion += 0.5f *(float)gameTime.ElapsedGameTime.TotalSeconds;
            SetEffectParameters();
        }

        protected override void DrawEffect(Microsoft.Xna.Framework.Graphics.Texture2D tex, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Graphics.GraphicsDevice device)
        {
            //3_1
			//spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            //effect.Begin();
            //foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            //{
            //    pass.Begin();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, effect);    
			spriteBatch.Draw(tex, Vector2.Zero, Color.White);
            //    pass.End();
            //}
            //effect.End();
            spriteBatch.End();
        }

        protected override void ResetEffect()
        {
        }

        protected override void SetEffectParameters()
        {
            effect.Parameters["ymotion"].SetValue(ymotion);
        }
    }
}
