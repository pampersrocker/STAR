using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Game.Debug;
using Star.GameManagement;
using System.Collections.Generic;

namespace Star.Game.Level.InteractiveObjects
{
	public abstract class InteractiveObject : IDisposable
	{
		#region Fields
		bool initialized;
		GraphicsDevice graphicsDevice;
		Options options;
		ContentManager content;
		Rectangle actionRectangle;
		protected Vector2 pos;
		#endregion

		#region Attributes

		protected Rectangle ActionRectangle
		{
			get { return actionRectangle; }
			set { actionRectangle = value; }
		}

		public GraphicsDevice GraphicsDevice
		{
			get { return graphicsDevice; }
		}

		public Options Options
		{
			get { return options; }
		}

		public ContentManager Content
		{
			get { return content; }
		}

		public bool Initialized
		{
			get { return initialized; }
		}

		public virtual Vector2 Pos
		{
            set { }
            get { return Vector2.Zero; }
		}

		#endregion

		#region PublicMembers

		public void Initialize(IServiceProvider serviceProvider, Options options, GraphicsDevice device,string data)
		{
			try
			{
				content = new ContentManager(serviceProvider,"Data");
				graphicsDevice = device;
				this.options = options;
				InitializeObject(data);
				initialized = true;
			}
			catch (Exception e)
			{
				DebugManager.AddItem(e.Message, this.ToString(), new StackTrace(e), System.Drawing.Color.Red);
				FileManager.WriteInErrorLog(this, "Failed to initialize InteractiveObject: \n" + e.Message, e.GetType());
				Trace.Fail(e.GetType().ToString(), e.Message + "\n" + e.StackTrace);
			}
		}

		public void Update(GameTime gameTime,Vector2 playerPos,Rectangle playerRect)
		{
			try
			{
				UpdateObject(gameTime, playerPos, playerRect);
			}
			catch (Exception e)
			{
				DebugManager.AddItem(e.Message, this.ToString(), new StackTrace(e), System.Drawing.Color.Red);
				FileManager.WriteInErrorLog(this, "Failed to update InteractiveObject: \n" + e.Message, e.GetType());
				Trace.Fail(e.GetType().ToString(), e.Message + "\n" + e.StackTrace);
			}
		
		}

		public void Draw(SpriteBatch spriteBatch, Matrix matrix)
		{
			try
			{
				DrawObject(spriteBatch, matrix);
			}
			catch (Exception e)
			{
				DebugManager.AddItem(e.Message, this.ToString(), new StackTrace(e), System.Drawing.Color.Red);
				FileManager.WriteInErrorLog(this, "Failed to draw InteractiveObject ("+GetType().ToString()+"): \n" + e.Message, e.GetType());
				Trace.Fail(e.GetType().ToString(), e.Message + "\n" + e.StackTrace);
			}
		}

		public virtual Vector2 GetPlayerInfluence()
		{
			return Vector2.Zero;
		}

		public virtual Rectangle[] GetCollisionRectangles
		{
			get { return new Rectangle[0]; }
		}

		public virtual void HandlePlayerCollision(List<CollisionType> collision)
		{ 
		
		}

		#endregion

		#region AbstractMembers

		public abstract string Type { get; }

		protected abstract void InitializeObject(string data);

		protected abstract void UpdateObject(GameTime gameTime, Vector2 playerPos, Rectangle playerRect);

		protected abstract void DrawObject(SpriteBatch spriteBatch, Matrix matrix);

		public abstract string GetDataString();

        public abstract bool Enabled
        {
            get;
        }

		#endregion

		#region IDisposable Member

		public virtual void Dispose()
		{
			content.Dispose();
			initialized = false;
		}

		#endregion
	}
}
