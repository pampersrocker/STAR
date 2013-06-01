using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;
using Star.Game.Debug;
using System.Diagnostics;
using XNAParticleSystem;

namespace Star.Game.Level.InteractiveObjects
{
	public enum JumpPadValues
	{ 
		RectangleX,
		RectangleY,
		RectangleWidth,
		RectangleHeight,
		SpeedX,
		SpeedY,
		Fallof
	}

	public class JumpPad : InteractiveObject
	{
		bool forceApplying;
		Texture2D blank;
		Texture2D jumpPadTex;
		ExtendedRectangle[] drawRects;
		Vector2 force;
		Vector2 appliedForce;
		float[] values;
		float[] drawRectsTime;
		byte[] drawRectsAlpha;
		float elapsedTime;
		float timeThreshold;
		ParticleSystem particles;

		public Vector2 Force
		{
			get { return force; }
			set { force = value; }
		}
		readonly int MaxLengthDrawRects = 100;
		readonly float MaxForceLength = 5000;
		readonly float AnimationTimeThreshold = 1;
		readonly float speed = 100 * 1;


		public override string Type
		{
			get { return InteractiveObjectsIDs.JumpPad.ToString(); }
		}

		public override Vector2 GetPlayerInfluence()
		{
			if (forceApplying)
				return appliedForce;
			else
				return base.GetPlayerInfluence();
		}

		public override Vector2 Pos
		{
			set
			{
				values[(int)JumpPadValues.RectangleX] = value.X - values[(int)JumpPadValues.RectangleWidth] / 2;
				values[(int)JumpPadValues.RectangleY] = value.Y - values[(int)JumpPadValues.RectangleHeight] / 2;
			}
			get
			{
				return new Vector2(
					values[(int)JumpPadValues.RectangleX] +
					values[(int)JumpPadValues.RectangleWidth] / 2,
					values[(int)JumpPadValues.RectangleY] +
					values[(int)JumpPadValues.RectangleHeight] / 2);
			}
		}

		public override void HandlePlayerCollision(List<CollisionType> collision)
		{
			if (collision.Count >= 1 && forceApplying && elapsedTime > 0.3f)
				forceApplying = false;
		}

		protected override void InitializeObject(string data)
		{
			string[] temp = data.Split(',');
			values = new float[Enum.GetValues(typeof(JumpPadValues)).Length];
			for(int i = 0; i< temp.Length;i++)
			{
				try
				{
					values[i] = float.Parse(temp[i], CultureInfo.CreateSpecificCulture("en-us"));
				}
				catch (Exception e)
				{
					DebugManager.AddItem(e.Message, this.ToString(), new StackTrace(e), System.Drawing.Color.Red);
					FileManager.WriteInErrorLog(this, "Failed to load Data of "+ Type +": \n" + e.Message, e.GetType());
				}
			}
			timeThreshold = values[(int)JumpPadValues.Fallof];
			blank = Content.Load<Texture2D>("Stuff\\Blank");
			jumpPadTex = Content.Load<Texture2D>(GameParameters.CurrentGraphXPackPath + "Level/InteractiveObjects/JumpPad");
			drawRects = new ExtendedRectangle[5];
			drawRectsAlpha = new byte[5];
			drawRectsTime = new float[5];
			ActionRectangle = new Rectangle(
				(int)values[(int)JumpPadValues.RectangleX], 
				(int)values[(int)JumpPadValues.RectangleY], 
				(int)values[(int)JumpPadValues.RectangleWidth], 
				(int)values[(int)JumpPadValues.RectangleHeight]);

			force = new Vector2(values[(int)JumpPadValues.SpeedX],values[(int)JumpPadValues.SpeedY]);
			float rotation = force.GetRotation();
			for (int i = 0; i < drawRects.Length; i++)
			{
				drawRectsTime[i] = (float)i / drawRects.Length * AnimationTimeThreshold;
				Vector2 offset = (force / MaxForceLength) * (((float)i / drawRects.Length) * MaxLengthDrawRects);
				drawRects[i] = ExtendedRectangle.Transform(new Rectangle(
					(int)values[(int)JumpPadValues.RectangleX], (int)values[(int)JumpPadValues.RectangleY],
					32,
					2),
					new Vector2(blank.Width,blank.Height),
					new Vector2((int)values[(int)JumpPadValues.RectangleWidth] / 2,
					(int)values[(int)JumpPadValues.RectangleHeight] / 2),
					offset,
					force.GetRotation() - (float)Math.PI/2);

			}
			particles = new ParticleSystem();
			particles.Initialize(false, SpawnType.Fontaine,
				50,
				100,
				3,
				Color.White,
				new Vector2(values[(int)JumpPadValues.RectangleX] , values[(int)JumpPadValues.RectangleY]),
				new Vector2(values[(int)JumpPadValues.RectangleX] + values[(int)JumpPadValues.RectangleWidth], values[(int)JumpPadValues.RectangleY] + values[(int)JumpPadValues.RectangleHeight]),
				 SpawnDirections.Angle, force /MaxForceLength * 1000, blank, GravityType.OverallForce, Vector2.Zero, 1, XNAParticleSystem.CollisionType.None);
			Options.InitObjectHolder.particleManagers[EGameState.Game].Add(particles);
			particles.Reset();
			particles.UseMinSpeed = true;
			particles.MinSpeed = force / MaxForceLength * 500;
			particles.SpawnAngle = 0.0f;
			particles.AirFriction = 0.95f;
			//particles.UseCuda(true);
		}

		protected override void UpdateObject(GameTime gameTime, Vector2 playerPos, Rectangle playerRect)
		{
			if (ActionRectangle.Intersects(playerRect) && !forceApplying)
			{
				forceApplying = true;
				appliedForce = force;
				elapsedTime = 0;
			}
			if (forceApplying)
			{
				elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
				//appliedForce *= fallof;
				if (elapsedTime >= timeThreshold)
					forceApplying = false;
			}
			for (int i = 0; i < drawRects.Length; i++)
			{
				drawRectsTime[i] += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (drawRectsTime[i] >= AnimationTimeThreshold)
					drawRectsTime[i] = 0;
				drawRectsAlpha[i] = (byte)(255 - ((drawRectsTime[i] / AnimationTimeThreshold) * 255));
				Vector2 force1 = new Vector2(force.X,force.Y);
				force1.Normalize();
				Vector2 offset = (force1/3) * (((float)drawRectsTime[i] / AnimationTimeThreshold) * speed);
				
				drawRects[i] = ExtendedRectangle.Transform(new Rectangle(
					(int)values[(int)JumpPadValues.RectangleX], (int)values[(int)JumpPadValues.RectangleY],
					32,
					3),
					new Vector2(blank.Width, blank.Height),
					new Vector2((int)values[(int)JumpPadValues.RectangleWidth] / 2,
					(int)values[(int)JumpPadValues.RectangleHeight] / 2),
					offset+ new Vector2(32,0).Rotate(force.GetRotation(),true),
					force.GetRotation() - (float)Math.PI / 2);
			}
			//particles.Update(gameTime);
			particles.MaxSpeed = force / MaxForceLength * 1000;
			particles.UseMinSpeed = true;
			particles.MinSpeed = particles.MaxSpeed / 2;
			particles.SpawnPos = new Vector2(values[(int)JumpPadValues.RectangleX], values[(int)JumpPadValues.RectangleY]);
			particles.Spawnend = new Vector2(values[(int)JumpPadValues.RectangleX] + values[(int)JumpPadValues.RectangleWidth], values[(int)JumpPadValues.RectangleY] + values[(int)JumpPadValues.RectangleHeight]);
			
		}

		protected override void DrawObject(SpriteBatch spriteBatch, Matrix matrix)
		{
			
			ExtendedRectangle rect = ExtendedRectangle.Transform(new Rectangle((int)values[(int)JumpPadValues.RectangleX], (int)values[(int)JumpPadValues.RectangleY], 32, 32), new Vector2(jumpPadTex.Width, jumpPadTex.Height), new Vector2(16, 16), Vector2.Zero, force.GetRotation());
			particles.Draw(spriteBatch, matrix);
			//3_1
			
			//spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, matrix);
			spriteBatch.Begin(SpriteSortMode.Immediate,
				BlendState.NonPremultiplied,
				null,
				null,
				null,
				null,
				matrix);
			for (int i = 0; i < drawRects.Length;i++)
			{
				spriteBatch.Draw(blank, drawRects[i].TransformedRectangle, null, new Color(255,255,255,drawRectsAlpha[i]), drawRects[i].Rotation, drawRects[i].DrawOrigin, SpriteEffects.None, 0);
			}
			spriteBatch.Draw(jumpPadTex, rect.TransformedRectangle, null, Color.White, rect.Rotation, rect.DrawOrigin, SpriteEffects.None, 0);
			
			spriteBatch.End();
			
		}

		public override void Dispose()
		{
			particles.Dispose();
			blank.Dispose();
			base.Dispose();
		}

		public override string GetDataString()
		{
			CultureInfo info = CultureInfo.CreateSpecificCulture("en-us");
			string data = "";
			data += Type + "," +
				values[(int)JumpPadValues.RectangleX].ToString(info) + "," +
				values[(int)JumpPadValues.RectangleY].ToString(info) + "," +
				values[(int)JumpPadValues.RectangleWidth].ToString(info) + "," +
				values[(int)JumpPadValues.RectangleHeight].ToString(info) + "," +
				force.X.ToString(info) + "," +
				force.Y.ToString(info) + "," +
				timeThreshold.ToString(info);
			return data;
		}

		public override bool Enabled
		{
			get { return forceApplying; }
		}
	}
}
