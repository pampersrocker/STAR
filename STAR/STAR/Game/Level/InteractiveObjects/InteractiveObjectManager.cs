using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Star.GameManagement;
using Star.Game.Debug;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Star.GameManagement.Gamestates;
using System.Globalization;

namespace Star.Game.Level.InteractiveObjects
{
	public class InteractiveObjectManager : IDisposable
	{
		List<InteractiveObject> interactiveObjects;
        Options options;
        Vector2 cameraPos;

		public InteractiveObjectManager()
		{
			interactiveObjects = new List<InteractiveObject>();
		}

		public void Initialize(IServiceProvider serviceProvider, LevelVariables variables, GraphicsDevice graphicsDevice,Options options)
		{
            this.options = options;
			string[] data = variables.Dictionary[LV.InteractiveObjects].Split(':');
			foreach (string iObject in data)
			{
				if (!string.IsNullOrEmpty(iObject.Trim()))
				{
					string[] temp = iObject.Split(',');
					try
					{
						InteractiveObjectsIDs id = (InteractiveObjectsIDs)Enum.Parse(typeof(InteractiveObjectsIDs), temp[0]);

						string objectData = "";
						for (int i = 1; i < temp.Length - 1; i++)
						{
							objectData += temp[i] + ",";
						}
						objectData += temp[temp.Length - 1];
						switch (id)
						{
							case InteractiveObjectsIDs.JumpPad:
								JumpPad pad = new JumpPad();
								pad.Initialize(serviceProvider, options, graphicsDevice, objectData);
								interactiveObjects.Add(pad);
								break;
						}
					}
					catch (Exception e)
					{
						DebugManager.AddItem("Failed to load InteractiveObject: " + iObject + "\n" + e.Message, this.ToString(), new StackTrace(e), System.Drawing.Color.Red);
						FileManager.WriteInErrorLog(this.ToString(), "Failed to load InteractiveObject: " + iObject + "\n" + e.Message, e.GetType());
					}
				}
			}
		}

		public void Update(GameTime gameTime, SGame game)
		{
			if (game != null)
			{
                cameraPos = game.Camera.Position;
				foreach (InteractiveObject iObject in interactiveObjects)
				{
                    if ((iObject.Pos + cameraPos).Length() <= new Vector2(options.ScreenWidth / 2, options.ScreenHeight / 2).Length() || iObject.Enabled)
                    {
                        iObject.Update(gameTime, game.Player.Pos, game.Player.BoundingBox);
                        iObject.HandlePlayerCollision(game.Player.Collission);
                    }
				}
			}
			else
			{
				foreach (InteractiveObject iObject in interactiveObjects)
				{

					iObject.Update(gameTime, Vector2.Zero, Rectangle.Empty);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch, Matrix matrix)
		{
			foreach (InteractiveObject iObject in interactiveObjects)
			{
                if ((iObject.Pos + cameraPos).Length() <= new Vector2(options.ScreenWidth / 2, options.ScreenHeight / 2).Length() || iObject.Enabled)
                {
                    iObject.Draw(spriteBatch, matrix);
                }
			}
		}

		public void AddObject(InteractiveObject iObject)
		{
			interactiveObjects.Add(iObject);
		}

		public void AddObject(IServiceProvider serviceProvider, GraphicsDevice graphicsDevice, Options options, string iObjectName, float[] iObjectParameters,Vector2 position)
		{
			InteractiveObject iObject;
			CultureInfo info = CultureInfo.CreateSpecificCulture("en-us");
			switch (iObjectName)
			{
				case "JumpPad":
					Vector2 force = new Vector2(iObjectParameters[1], 0).Rotate(iObjectParameters[0], true);
					iObject = new JumpPad();
					iObject.Initialize(serviceProvider, options, graphicsDevice,
						(int)position.X+ "," + (int)position.Y + ",32,32," +
						force.X.ToString(info) + "," +
						force.Y.ToString(info) + "," +
						iObjectParameters[2].ToString(info));
					interactiveObjects.Add(iObject);
					break;
			}
		}

		public Vector2 GetPlayerInfluences()
		{
			Vector2 influences= Vector2.Zero;
			foreach (InteractiveObject iObject in interactiveObjects)
				influences += iObject.GetPlayerInfluence();
			return influences;
		}

		public string GetDataString()
		{
			string data = "";
			foreach (InteractiveObject iObject in interactiveObjects)
				data += iObject.GetDataString() + ":";
			if (data.Length > 0)
			{
				char[] dataChar = data.ToCharArray();
				dataChar[dataChar.Length - 1] = ' ';
				data = new string(dataChar);
			}
			return data;
		}

		#region IDisposable Member

		public void Dispose()
		{
			foreach (InteractiveObject iObject in interactiveObjects)
				iObject.Dispose();
            interactiveObjects.Clear();
		}

		#endregion
	}
}
