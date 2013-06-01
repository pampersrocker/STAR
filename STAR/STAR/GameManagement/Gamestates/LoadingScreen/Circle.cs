using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Star.GameManagement.Gamestates.LoadingScreen
{
	class Circle
	{
		Texture2D tex;
		float speed;
		float angle;
		Vector2 pos;
		Vector2 origin;
		Rectangle rect;
		bool left;
		float time;
		float elapsedTime;
		Random rand;
		Color color;

		public Color Color
		{
			get { return color; }
			set { color = value; }
		}

		public Circle()
		{
			rand = new Random((int)DateTime.Now.Ticks);
			origin = new Vector2();
			pos = new Vector2();
			rect = new Rectangle();
			color = Color.White;
		}

		public Circle(Vector2 startpos)
			: this()
		{
			pos = startpos;
		}
	

		public Circle(Vector2 startpos,Texture2D tex)
			: this(startpos)
		{
			this.tex = tex;
			origin = new Vector2(tex.Width / 2, tex.Height / 2);
		}

		public Circle(Vector2 startpos, Texture2D tex, int width, int height)
			: this(new Vector2(startpos.X - width / 2, startpos.Y - height / 2), tex)
		{
			rect = new Rectangle((int)startpos.X - width/2, (int)startpos.Y - height/2, width, height);
		}

		public void Update(GameTime gameTime)
		{
			elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (elapsedTime > time)
			{
				elapsedTime = 0;
				time = (float)rand.NextDouble() * 2f + 0.2f;
				speed = (float)rand.NextDouble() * 2 * MathHelper.Pi;
				if (rand.Next(100) > 50)
					left = !left;
				if (!left)
					speed = -speed;
			}

			angle += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		public void Draw(SpriteBatch spriteBatch,Matrix matrix)
		{
			if (tex != null)
			{
				//3_1
				spriteBatch.Begin(
					SpriteSortMode.Immediate,
					BlendState.AlphaBlend,
					null,
					null,
					null,
					null,
					matrix);
				//spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, matrix);
				spriteBatch.Draw(tex, rect, null, color, angle, origin, SpriteEffects.None, 0);
				spriteBatch.End();
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Draw(spriteBatch, Matrix.CreateTranslation(Vector3.Zero));
		}
	}
}
