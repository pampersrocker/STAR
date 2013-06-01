using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Game.Level;

namespace Star.Graphics.Effects.PostProcessEffects
{
    public class Wave : GraphicEffect
    {
        float disortion;
        Vector2 centerCoord;
        float radius;
        float size;


        float height_to_width;
        int screenwidth = 800;
        int screenheight = 600;

        float distortionchange = 0;
        float radiuschange = 0;
        float sizechange = 0;
        float maxradius;

        #region Poperties

        public Vector2 CenterCoord
        {
            get { return centerCoord; }
            set 
            {
                centerCoord = new Vector2(
                    value.X / screenwidth,
                    value.Y / screenheight);
                SetValues();
            }
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public float Disortion
        {
            get { return disortion; }
            set { disortion = value; }
        }

        public float DistortionChange
        {
            get { return distortionchange; }
            set { distortionchange = value; }
        }

        public float RadiusChange
        {
            get { return radiuschange; }
            set { radiuschange = value; }
        }

        public float SizeChange
        {
            get { return sizechange; }
            set { sizechange = value; }
        }

        public float MaxRadius
        {
            get { return maxradius; }
            set { maxradius = value; }
        }

        #endregion

        protected override AvailableEffects InitializeEffect(GraphicsDevice device, Star.GameManagement.Options options)
        {
            height_to_width = (float)options.ScreenHeight / (float)options.ScreenWidth;
            screenwidth = options.ScreenWidth;
            screenheight = options.ScreenHeight;
            return AvailableEffects.Wave;
        }

        protected override void SetEffectParameters()
        {
			//disortion = -0.5f;
			//centerCoord = new Vector2();
			//radius = 0;
			//size = 0.1f;
			//Alpha = 1;
        }

        private void SetValues()
        {
            effect.Parameters["distortion"].SetValue(disortion);
            effect.Parameters["centerCoord"].SetValue(centerCoord);
            effect.Parameters["radius"].SetValue(radius);
            effect.Parameters["size"].SetValue(size);
            effect.Parameters["height_to_width"].SetValue(height_to_width);
        }

        protected override void UpdateEffect(GameTime gameTime, Vector2 pos)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            disortion += distortionchange * elapsed;
            //radius += radiuschange * elapsed;+
            radius += (maxradius - radius) * 2 * elapsed;
            size += sizechange * elapsed;
            radius = MathHelper.Clamp(radius, 0, maxradius);
            if (radius >= maxradius || disortion <= 0)
            {
               Enabled = false;
            }
            SetValues();
        }

        //protected override void DrawEffect(Texture2D tex, SpriteBatch spriteBatch, GraphicsDevice device)
        //{
        //    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, effect);
        //    spriteBatch.Draw(tex, Vector2.Zero, Color.White);
        //    spriteBatch.End();
        //}

        protected override void ResetEffect()
        {
            SetEffectParameters();
            SetValues();
        }


    }
}
