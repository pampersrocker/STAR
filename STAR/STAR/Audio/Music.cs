using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Star.Game;
using Star.GameManagement;
using Star.Components;



namespace Star.Audio
{
	public class Music
	{
		bool transition;
		Options options;
		AudioEngine audioEngine;
		WaveBank waveBank;
		SoundBank soundBank;
		Cue currentMusic,oldMusic;
		AudioCategory menuCategory;
		AudioCategory graphXPackCategory;
		AudioCategory oldCategory, currentCategory;
		float oldVolume, currentVolume;
		Dictionary<string, string> playList;
		Dictionary<string, string> graphXPackPlaylist,currentPlaylist;
		Rectangle popUp;
		DrawableText popUpText;
		int currentMusicindex = 0;

		bool popUpActive;
		bool popUpFloatIn;
		float popUpStillStandelpasedTime=0;
		float popUpStillStandThreshold= 5f;
		float popUpSpeed = 200;
		Texture2D popUpBG;

		ContentManager content;

		public void Initialize(IServiceProvider serviceProvider,Options options)
		{
			
			popUpText = new DrawableText();
			content = new ContentManager(serviceProvider, "Data");
			popUpText.font = content.Load<SpriteFont>("Stuff\\Arial");
			popUpBG = content.Load<Texture2D>("Stuff\\popUpBG");
			popUpText.Text = "";
			popUpText.color = Color.White;
			this.options = options;
			options.MusicVolumeChanged += new MusicVolumeChangedEventHandler(options_MusicVolumeChanged);
			options.ResolutionChanged += new ResolutionChangedEventHandler(options_ResolutionChanged);
			audioEngine = new AudioEngine(GameConstants.AUDIO_PATH + "Music.xgs");
			waveBank = new WaveBank(audioEngine, GameConstants.AUDIO_PATH + "Music.xwb");
			soundBank = new SoundBank(audioEngine, GameConstants.AUDIO_PATH + "Music.xsb");
			menuCategory = audioEngine.GetCategory("Music");
			graphXPackCategory = menuCategory;
			menuCategory.SetVolume(options.MusicVolumeFloat);
			playList = FileManager.GetFileDictString(GameConstants.MENU_PLAYLIST);
			currentPlaylist = playList;
			oldVolume = options.MusicVolumeFloat;
			currentVolume = options.MusicVolumeFloat;
			currentCategory = menuCategory;
			CreatePopUpRectangle(options.Resolution);
			
			
		}

		void options_ResolutionChanged(Options options, Resolution resolution)
		{
			CreatePopUpRectangle(resolution);
		}

		private void CreatePopUpRectangle(Resolution resolution)
		{
			popUp = new Rectangle((resolution.ScreenWidth - (int)(resolution.ScreenWidth * 0.3)), -(int)(resolution.ScreenHeight * 0.1), (int)(resolution.ScreenWidth * 0.3), (int)(resolution.ScreenHeight * 0.1));
		}

		void options_MusicVolumeChanged(Options options, float vol)
		{
			//oldVolume = vol;
			currentVolume = vol;
			currentCategory.SetVolume(currentVolume);
		}

		private void AlignPopUpText()
		{
			popUpText.pos = popUp.Location.ToVector2() + new Vector2(popUp.Width * 0.1f, popUp.Height * 0.1f);
			popUpText.Text = popUpText.Text.Replace("#", "\n");
		}

		private void StartPopUp()
		{
			popUpActive = true;
			popUpFloatIn = true;
			CreatePopUpRectangle(options.Resolution);
			AlignPopUpText();
			popUpStillStandelpasedTime = 0;
		}

		public void Update(GameTime gameTime)
		{
			if (currentMusic == null)
			{
				currentMusic = soundBank.GetCue(GetRandomTrack(currentPlaylist));
				//Fake 3D Sound for Surround Effect ;)
				currentMusic.Apply3D(new AudioListener(), new AudioEmitter());
				currentMusic.Play();
				popUpText.Text = currentPlaylist[currentMusic.Name];
				StartPopUp();
			}
			if (currentMusic.IsStopped)
			{
				currentMusicindex++;
				if (currentMusicindex >= playList.Keys.Count)
					currentMusicindex = 0;
				currentMusic = soundBank.GetCue(GetRandomTrack(currentPlaylist));
				currentMusic.Apply3D(new AudioListener(), new AudioEmitter());
				currentMusic.Play();
				popUpText.Text = currentPlaylist[currentMusic.Name];
				StartPopUp();
			}
			if (transition)
			{
				oldVolume -= gameTime.GetElapsedTotalSecondsFloat();
				oldVolume = MathHelper.Clamp(oldVolume, 0, options.MusicVolumeFloat);
				currentVolume += gameTime.GetElapsedTotalSecondsFloat();
				if (currentVolume >= options.MusicVolumeFloat)
				{
					transition = false;
					currentVolume = MathHelper.Clamp(currentVolume, 0, options.MusicVolumeFloat);
					oldCategory.Stop(AudioStopOptions.Immediate);
					oldMusic.Stop( AudioStopOptions.Immediate);
				}
				oldCategory.SetVolume(oldVolume);
				currentCategory.SetVolume(currentVolume);
			}
			if (popUpActive)
			{
				if (popUpFloatIn)
				{
					popUp.Y += (int)(popUpSpeed * gameTime.GetElapsedTotalSecondsFloat());
					AlignPopUpText();
					if (popUp.Y >= 0)
					{
						popUpFloatIn = false;
					}
				}
				else
				{
					popUpStillStandelpasedTime += gameTime.GetElapsedTotalSecondsFloat();
					if (popUpStillStandelpasedTime >= popUpStillStandThreshold)
					{
						popUp.Y-= (int)(popUpSpeed * gameTime.GetElapsedTotalSecondsFloat());
						if (popUp.X <= -(int)(options.Resolution.ScreenHeight * 0.1))
							popUpActive = false;
						AlignPopUpText();
					}
				}

			}

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (popUpActive)
			{
				spriteBatch.Begin();
				spriteBatch.Draw(popUpBG, popUp, Color.White);
				spriteBatch.DrawString(popUpText.font, popUpText.Text, popUpText.pos, popUpText.color, 0, Vector2.Zero, options.ScaleFactor, SpriteEffects.None, 0);
				spriteBatch.End();
			}
		}

		public void StartCategoryTransition(bool toInGame)
		{
			transition = true;
			if (toInGame)
			{
				oldCategory = menuCategory;
				currentCategory = graphXPackCategory;
				currentCategory.Stop(AudioStopOptions.Immediate);
				oldMusic = currentMusic;
				currentPlaylist = graphXPackPlaylist;
				currentMusic = soundBank.GetCue(GetRandomTrack(currentPlaylist));
				popUpText.Text = currentPlaylist[currentMusic.Name];
				StartPopUp();
			}
			else
			{
				oldCategory = graphXPackCategory;
				currentCategory = menuCategory;
				currentCategory.Stop(AudioStopOptions.Immediate);
				oldMusic = currentMusic;
				currentPlaylist = playList;
				currentMusic = soundBank.GetCue(GetRandomTrack(currentPlaylist));
				popUpText.Text = currentPlaylist[currentMusic.Name];
				StartPopUp();
			}
			currentVolume = 0;
			currentCategory.SetVolume(currentVolume);
			currentMusic.Apply3D(new AudioListener(), new AudioEmitter());
			currentMusic.Play();
		}

		public void GraphXPackChanged(string graphXPack)
		{
			graphXPackCategory = audioEngine.GetCategory(graphXPack);
			graphXPackPlaylist = FileManager.GetFileDictString("Data\\" + GameConstants.GraphXPacksPath + graphXPack + ".songs");
		}

		private string GetRandomTrack(Dictionary<string, string> playList)
		{
			Random rand = new Random((int)DateTime.Now.Ticks);
			return playList.Keys.ToArray()[
				rand.Next(
				0, 
				playList.Keys.Count)];
		}
	}
}
