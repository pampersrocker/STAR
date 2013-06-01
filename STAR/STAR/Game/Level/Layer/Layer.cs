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
using Star.Graphics;
using Star.GameManagement;

namespace Star.Game.Level
{


	public abstract class Layer : IGraphicsChange,ILevelContent
	{
		public enum Data_Layer
		{
			NumLayerObjects,

		}

		LayerObject[] layerobjects;
		ContentManager content;
		//protected Colorize colorizeEffect;
		protected ColorizeLUT colorizeLUT;
		LayerFXData fxData;
		bool initialized = false;
		bool enabled     = true;
		string layer     = "";



		public string LayerName
		{
			get { return layer; }
			set { layer = value; }
		}

		public LayerObject[] LayerObjects
		{
			get { return layerobjects; }
			set { layerobjects = value; }
		}

		public ContentManager Content
		{
			get { return content; }
		}

		public bool Initalized
		{
			get { return initialized; }
		}

		/// <summary>
		/// Activates or Deactivates the Layer Update(...) and Draw(...) Method.
		/// Enabled can only be changed when the Layer is initialized.
		/// </summary>
		public bool Enabled
		{
			get { return enabled; }
			set
			{
				if (initialized)
				{
					enabled = value;
				}
			}
		}

		public LayerFXData FXData
		{
			get { return fxData; }
			set 
			{ 
				fxData = value;
				setColorizeEffectParameters();
			}
		}

		private void setColorizeEffectParameters()
		{
			//colorizeEffect.LayerFXData = fxData;
			colorizeLUT.FxData = fxData;
		}

		#region ILayer Member

		protected void Initialize(IServiceProvider ServiceProvider,int numberOfLayerObjects,Star.GameManagement.Options options,LevelVariables levelvariables,GraphicsDevice graphicsDevice)
		{
			content = new ContentManager(ServiceProvider);
			content.RootDirectory = "Data";
			//colorizeEffect = new Colorize();
			//colorizeEffect.Initialize(ServiceProvider, graphicsDevice, options);
			colorizeLUT = new ColorizeLUT();
			colorizeLUT.Initialize(ServiceProvider, graphicsDevice, options);
			colorizeLUT.Enabled = true;
			//fxData = LayerFXData.Default;
			//fxData.SaturationEnabled = true;
			//fxData.HueEnabled = true;
			//fxData.Saturation = 0f;
			GraphicsManager.AddItem(this);
			//colorizeEffect.Enabled = true;

			//setColorizeEffectParameters();
			layerobjects = new LayerObject[numberOfLayerObjects];
			InitializeGraphX(options,levelvariables);
			FinalizeInitialize();
		}

		/// <summary>
		/// Initializes the LayerObjects
		/// Is called by Initialize
		/// </summary>
		/// <param name="options">Optionen for the Resolution</param>
		protected abstract void InitializeGraphX(Star.GameManagement.Options options,LevelVariables levelvariables);

		private void FinalizeInitialize()
		{
			initialized = true;
		}

		public void Update(GameTime gametime, Vector2 player_difference, Star.GameManagement.Options options,Vector2 relativePosition)
		{
			if (enabled && initialized)
			{
				colorizeLUT.Update(gametime, Vector2.Zero);
				//colorizeEffect.Update(gametime, Vector2.Zero);
				UpdateLayer(gametime, player_difference, options,relativePosition);
			}
		}

		protected abstract void UpdateLayer(GameTime gametime, Vector2 player_difference,Star.GameManagement.Options options,Vector2 playerRelativePositioninLevel);

		public void Draw(SpriteBatch spritebatch, Matrix matrix)
		{
			if (enabled && initialized)
			{
				//3_1
				//spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState,matrix);
				colorizeLUT.ApplyParameters();
				spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, 
					null, null, null, colorizeLUT.Effect
					, matrix);
				//colorizeEffect.Begin();
				DrawLayer(spritebatch, matrix);
				//colorizeEffect.End();
				spritebatch.End();
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Draw(spriteBatch, Matrix.CreateTranslation(Vector3.Zero));
		}

		protected abstract void DrawLayer(SpriteBatch spritebatch, Matrix matrix);

		#endregion

		#region IDisposable Member

		public void Dispose()
		{
			content.Dispose();
			foreach (LayerObject lo in layerobjects)
			{
				if (lo != null)
					lo.Dispose();
			}
			colorizeLUT.Dispose();
			initialized = false;
			UnloadGraphicsChanged();
		}

		#endregion

		#region IGraphicsChange Member

		public abstract void GraphicsChanged(GraphicsDevice device, Star.GameManagement.Options options);

		public void UnloadGraphicsChanged()
		{
			GraphicsManager.RemoveItem(this);
		}

		#endregion

		#region ILevelContent Member

		public bool Initialized
		{
			get { return initialized; }
		}

		#endregion

		#region ILevelContent Member

		public void Initialize(Options options, LevelVariables levelVariables)
		{
			Initialize(
				options.InitObjectHolder.serviceProvider, 
				options.InitObjectHolder.dataHolder.GetData<int>(Data_Layer.NumLayerObjects.GetKey()), 
				options, 
				levelVariables, 
				options.InitObjectHolder.graphics);
		}

		#endregion
	}
}
