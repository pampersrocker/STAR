using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Star.Graphics.Effects;

namespace Star.Game.Enemy
{
    public enum Anims
    {
        Stand,
        Walk,
        Jump,
        Cheer,
        Die,
    }

    public class Animation
    {
        Keyframe[] keyframes;
        int currentframe = 0;

        public Animation()
        {
            keyframes = new Keyframe[1];
            keyframes[0] = new Keyframe();
        }

        public void LoadAnimation(string data,string[] rectangles)
        {
            string[] frames = data.Split('#');
            keyframes = new Keyframe[frames.Length];
            for (int i = 0; i < keyframes.Length; i++)
            {
                keyframes[i] = new Keyframe();
                keyframes[i].LoadFrame(rectangles, frames[i],i);
            }
        }

        public Keyframe CurrentFrame
        {
            get { return keyframes[currentframe]; }
            set { keyframes[currentframe] = value; }
        }

        public int CurrentFrameValue
        {
            get { return currentframe; }
            set
            {
                currentframe = (int)MathHelper.Clamp(value, 0, keyframes.Length - 1);
            }
        }

        public Keyframe[] Frames
        {
            get { return keyframes; }
            set { keyframes = value; }
        }

		public static Rectangle DefaultCollision
		{
			get { return new Rectangle(0, 0, 100, 100); }
		}


        public void NextFrame()
        {
            currentframe++;
            if (currentframe >= keyframes.Length)
            {
                currentframe = 0;
            }

        }

		public void LastFrame()
		{
			currentframe--;
			if (currentframe < 0)
				currentframe = keyframes.Length - 1;
		}

        public void AddKeyframe(Keyframe newframe)
        {
            Keyframe[] newframes = new Keyframe[keyframes.Length + 1];
            for (int i = 0; i < keyframes.Length; i++)
            {
                newframes[i] = keyframes[i];
            }
            newframes[keyframes.Length] = newframe;

            keyframes = newframes;
        }

        public void RemoveKeyframe(int number)
        {
            Keyframe[] oldframes = (Keyframe[])keyframes.Clone();
            Keyframe[] newframes = new Keyframe[oldframes.Length-1];
            //oldframes.CopyTo(newframes, 0);
            for (int i = 0; i < newframes.Length; i++)
            {
                newframes[i] = oldframes[i].Copy();
            }
            for (int i = number; i < newframes.Length; i++)
            { 
                newframes[i] = oldframes[i+1].Copy();
            }

            for (int i = 0; i < newframes.Length; i++)
            {
                newframes[i].KeyFrameNumber = i;
            }

            keyframes = newframes;


        }

        public void Scale(float scale)
        {
            foreach (Keyframe frame in keyframes)
            {
                frame.Scale(scale);
            }
        }

        public string GetDataString()
        {
            string data ="";
            foreach (Keyframe frame in keyframes)
            {
                
                data += frame.GetDataString();
                if (frame.KeyFrameNumber != keyframes.Length - 1)
                {
                    data += "\n#";
                }
            }
			
            return data;
        }

		public void Draw(SpriteBatch spriteBatch, Matrix matrix, Vector2 pos, Dictionary<string, Texture2D> textures, Star.Game.Enemy.Enemy.StandardDirection rundirection, Star.Game.Enemy.Enemy.StandardDirection standarddirection, RenderTarget2D target, RenderTarget2D resolvedTex)
        {
				
			
            Keyframe drawframe = keyframes[currentframe];
            FrameRectangle rect;
            Rectangle translatedrect;
			SpriteEffects effect = SpriteEffects.None;
			if (rundirection != standarddirection)
				effect = SpriteEffects.FlipHorizontally;
			if (effect == SpriteEffects.FlipHorizontally)
			{
				#region 1stTry
				resolvedTex = (RenderTarget2D)spriteBatch.GraphicsDevice.GetRenderTargets()[0].RenderTarget;
				//spriteBatch.GraphicsDevice.ResolveBackBuffer(resolvedTex);
				spriteBatch.GraphicsDevice.SetRenderTarget(target);
				//spriteBatch.GraphicsDevice.SetRenderTarget(0, target);
				spriteBatch.GraphicsDevice.Clear(Color.Transparent);
				//spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.SaveState);
				spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
				foreach (string name in drawframe.GetRectangles.Keys)
				{
					rect = drawframe.GetRectangles[name];

					translatedrect = RectangleFunctions.TranslateRectangle(rect.Rect, new Vector2(target.Width / 2, target.Height / 2) + rect.Origin);
					spriteBatch.Draw(
						textures[name],
						translatedrect,
						null,
						rect.Color,
						rect.Rotation,
						rect.Origin,
						SpriteEffects.None,
						rect.DrawPosition);
				}
				spriteBatch.End();
				spriteBatch.GraphicsDevice.SetRenderTarget(resolvedTex);
				//spriteBatch.GraphicsDevice.ResolveBackBuffer(resolvedEnemy);
				//spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
				//spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
				//spriteBatch.Draw(resolvedTex, Vector2.Zero, Color.White);
				//spriteBatch.End();
				//spriteBatch.GraphicsDevice.ResolveBackBuffer(resolvedTex);
				//spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, matrix);
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,null,null,null,null,matrix);
				spriteBatch.Draw(target, new Rectangle((int)pos.X - target.Width / 2, (int)pos.Y - target.Height / 2, target.Width, target.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
				
				spriteBatch.End();

				#endregion

				#region 2nd try
				/*
				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.SaveState, matrix);
				//spriteBatch.Begin();
				foreach (string name in drawframe.GetRectangles.Keys)
				{
					rect = drawframe.GetRectangles[name];
					Vector2 DrawOrigin = new Vector2(textures[name].Width, rect.Origin.Y) - new Vector2(rect.Origin.X, 0);

					translatedrect = RectangleFunctions.TranslateRectangle(rect.Rect, pos + DrawOrigin);
					int deltaX = translatedrect.Right - (int)pos.X;
					translatedrect.X = (int)pos.X + deltaX/3 ;
					spriteBatch.Draw(
					    textures[name],
					    translatedrect,
					    null,
					    rect.Color,
						rect.Rotation*-1,
					    DrawOrigin,
					    effect,
					    rect.DrawPosition);


				}
				spriteBatch.End();
				*/
				#endregion
			}
			else
			{
				//spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.SaveState, matrix);
				spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend,null,null,null,null,matrix);
				//spriteBatch.Begin();
				foreach (string name in drawframe.GetRectangles.Keys)
				{
					rect = drawframe.GetRectangles[name];

					translatedrect = RectangleFunctions.TranslateRectangle(rect.Rect, pos + rect.Origin );
						spriteBatch.Draw(
							textures[name],
							translatedrect,
							null,
							rect.Color,
							rect.Rotation,
							rect.Origin,
							effect,
							rect.DrawPosition);
				}
				spriteBatch.End();
			}
			
        }
    }
}
