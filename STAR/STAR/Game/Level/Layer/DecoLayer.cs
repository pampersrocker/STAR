using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Input;
using Star.Game;
using Star.Graphics.Effects.PostProcessEffects;
using Star.GameManagement;

namespace Star.Game.Level
{
	public enum DecorationsLayer
	{
		Rear,
		Front
	}

	public class DecoLayer : Layer
	{
		
		List<LayerObject> layerObjects;

		protected override void InitializeGraphX(Star.GameManagement.Options options, LevelVariables levelvariables)
		{
			layerObjects = new List<LayerObject>();
			if(!string.IsNullOrEmpty(LayerName.Trim()))
			{
				try
				{
					DecorationsLayer decorationLayerType = (DecorationsLayer)Enum.Parse(typeof(DecorationsLayer), LayerName);
					if (decorationLayerType == DecorationsLayer.Rear && !string.IsNullOrEmpty(levelvariables.Dictionary[LV.RearDecorationsLayerData]))
						ParseData(levelvariables.Dictionary[LV.RearDecorationsLayerData], levelvariables);
					else if (decorationLayerType == DecorationsLayer.Front && !string.IsNullOrEmpty(levelvariables.Dictionary[LV.FrontDecorationsLayerData]))
						ParseData(levelvariables.Dictionary[LV.FrontDecorationsLayerData], levelvariables);
				}
				catch (Exception)
				{
					LayerObjects = new LayerObject[1];
					LayerObjects[0] = new LayerObject(Content.Load<Texture2D>("Stuff/Blank"), new Rectangle());
					Debug.DebugManager.AddItem("Failed to Parse Decorations Layer " + LayerName, this.ToString(), new System.Diagnostics.StackTrace(),System.Drawing.Color.Red);
					throw;
				}


			}
		}

		private void ParseData(string data,LevelVariables levelvariables)
		{ 
			string[] dataLines = SplitData(data);
			for (int i = 0; i < dataLines.Length; i++)
			{
				if (!string.IsNullOrEmpty(dataLines[i]))
				{
					Vector2 texSize = new Vector2(1);
					ExtendedRectangle extRect;
					Texture2D tex;
					string texName = "";
					try
					{
						texName = dataLines[i].Split(',')[9].Trim();
						tex = Content.Load<Texture2D>(
							GameConstants.GraphXPacksPath +
							levelvariables.Dictionary[LV.GraphXPack] +
							GameConstants.DECORATIONS_PATH +
							texName);
						texSize = new Vector2(tex.Width, tex.Height);
						tex.Name = texName;
					}
					catch (Exception)
					{
						Debug.DebugManager.AddItem(
							"Failed to Load DecorationTexture: " + texName,
							this.ToString(),
							new System.Diagnostics.StackTrace(), System.Drawing.Color.Yellow);
						tex = Content.Load<Texture2D>("Stuff/Blank");
						texSize = new Vector2(tex.Width, tex.Height);
					}
					extRect = ExtendedRectangle.CreateFromString(dataLines[i], texSize);
					layerObjects.Add(new LayerObject(tex, extRect));
				}

			}

			
		}

		private string[] SplitData(string data)
		{
			string[] dataLines = data.Split('#');
			return dataLines;
		}

		protected override void UpdateLayer(GameTime gametime, Vector2 player_difference, Star.GameManagement.Options options, Vector2 relativePosition)
		{
			//This layer has currently nothing to update...
		}

		protected override void DrawLayer(SpriteBatch spritebatch, Matrix matrix)
		{
			foreach (LayerObject layerObject in layerObjects)
			{
				spritebatch.Draw(
					layerObject.Texture, 
					layerObject.ExtendedRectangle.TransformedRectangle, 
					null, 
					Color.White, 
					layerObject.ExtendedRectangle.Rotation, 
					layerObject.ExtendedRectangle.Origin, 
					SpriteEffects.None, 
					0);
			}
		}

		public void AddItem(LayerObject item)
		{
			layerObjects.Add(item);
		}

		public void RemoveItem(LayerObject layerObject)
		{
			layerObjects.Remove(layerObject);
		}

		public void RemoveItem(int index)
		{
			if (index >= layerObjects.Count)
				layerObjects.RemoveAt(layerObjects.Count - 1);
			else
				layerObjects.RemoveAt(index);
		}

		public string GetItemDataString()
		{
			string data = "";
			foreach (LayerObject obj in layerObjects)
			{
				data += obj.ExtendedRectangle.GetDataString()+ "," + obj.Texture.Name + "#\n";
			}
			return data;
		}

		public override void GraphicsChanged(GraphicsDevice device, Options options)
		{
			
		}
	}
}
