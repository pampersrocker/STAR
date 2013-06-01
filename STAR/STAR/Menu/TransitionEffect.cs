using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Star.Menu
{
	public enum TransitionType
	{ 
		Scale,
		Translate,
		FadeOut
	}

	class TransitionEffect
	{
		Matrix matrix;

		public Matrix Matrix
		{
			get 
			{

				matrix =
					//Matrix.CreateTranslation(new Vector3(center,0)) *
					Matrix.CreateTranslation(new Vector3(translation, 0)) *
					Matrix.CreateScale(scale);
					Matrix.CreateTranslation(new Vector3(-center, 0));
				return matrix;
			}
		}
		float scale;
		Vector2 translation;
		Vector2 center;
		TransitionType type;
		float alpha;

		public float AlphaFloat
		{
			get { return alpha; }
			//set { alpha = value; }
		}

		public Byte AlphaByte
		{
			get { return (Byte)(alpha * 255); }
		}

		public TransitionEffect(Vector2 center,TransitionType type)
		{
			
			this.scale = 1;
			if (type == TransitionType.Translate)
				this.translation = new Vector2(-500, 0);
			else
				translation = Vector2.Zero;
			this.center = center;
			this.type = type;
			alpha = 1;
		}

		public void Update(GameTime gameTime)
		{
			switch (type)
			{ 
				case TransitionType.Scale:
					Scale(gameTime);
					break;
				case TransitionType.Translate:
					Translate(gameTime);
					break;
				case TransitionType.FadeOut:
					FadeOut(gameTime);
					break;

			}
		}

		private void Translate(GameTime gameTime)
		{
			translation += (center - translation) * (float)gameTime.ElapsedGameTime.TotalSeconds *10;
			alpha -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			alpha = MathHelper.Clamp(alpha, 0, 1);
		}

		private void FadeOut(GameTime gameTime)
		{
			translation += center * (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
			alpha -= 2*(float)gameTime.ElapsedGameTime.TotalSeconds;
			alpha = MathHelper.Clamp(alpha, 0, 1);
		}

		private void Scale(GameTime gameTime)
		{
			scale += 10*(float)gameTime.ElapsedGameTime.TotalSeconds;
			alpha -= 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
			alpha = MathHelper.Clamp(alpha, 0, 1);
		}

		
	}
}
