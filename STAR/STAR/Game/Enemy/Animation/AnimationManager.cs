using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.GameManagement;
using Star.Input;

namespace Star.Game.Enemy
{
    

    public class AnimationManager : ICloneable
    {
        ContentManager content;
        Animation[] animations;
        Anims currentAnimation = Anims.Walk;
        //Dictionary<string,Texture2D> textures;

        

        public Animation[] Animations
        {
            get { return animations; }
            set { animations = value; }
        }

        public Anims CurrentAnimation
        {
            get { return currentAnimation; }
            set { currentAnimation = value; }
        }

        public Keyframe[] CurrentAnimationKeyframes
        {
            get { return animations[(int)currentAnimation].Frames; }
            set { animations[(int)currentAnimation].Frames = value; }
        }

        public int CurrentAnimationKeyFrameCount
        {
            get { return animations[(int)currentAnimation].Frames.Length; }
        }

        public int CurrentAnimationKeyFrameValue
        {
            get { return animations[(int)currentAnimation].CurrentFrameValue; }
            set
            {
                animations[(int)currentAnimation].CurrentFrameValue = value;
            }
        }

        public Keyframe CurrentAnimationKeyframe
        {
            get { return animations[(int)currentAnimation].CurrentFrame; }
            set { animations[(int)currentAnimation].CurrentFrame = value; }
        }

        public AnimationManager(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Data");
            animations = new Animation[Enum.GetValues(typeof(Anims)).Length];
            //textures = new Dictionary<string, Texture2D>();
            for (int i = 0; i < animations.Length; i++)
            {
                animations[i] = new Animation();
            }
        }

        public void ToFile(string name)
        {
            string data = "";
            foreach (Anims anim in Enum.GetValues(typeof(Anims)))
            {
                if (animations[(int)anim] != null)
                {
                    data += anim.ToString() + "=\n";
                    data += animations[(int)anim].GetDataString() + ";\n";
                    //data += "C" + anim.ToString() + "=\n";
                    //data += animations[(int)anim].GetCollisionString() +";\n";
                }
            }
            FileManager.WriteFile(data,"Data/" + GameConstants.EnemiesPath + name + "/animation.anim"); 
        }

        public void Scale(float scale)
        {
            foreach (Animation anim in animations)
            {
                if (anim != null)
                {
                    anim.Scale(scale);
                }
            }
        }

        public void Initialize(Dictionary<Anims,string> anims,Dictionary<Star.Game.Enemy.Enemy.EnemyVariables,string> dict,string name)
        {
            string path = GameConstants.EnemiesPath + name + "/" ;
            string[] rectangles;
            rectangles = dict[Enemy.EnemyVariables.AnimRectangles].Split(',');

            for (int i = 0; i < rectangles.Length;i++)
            {
                rectangles[i] = rectangles[i].Trim();
            }
            //textures = new Dictionary<string, Texture2D>();
			//foreach (string rect in rectangles)
			//{
			//    try
			//    {
			//        textures.Add(rect, content.Load<Texture2D>(path + rect));
			//    }
			//    catch
			//    { 
			//        textures.Add(rect, content.Load<Texture2D>("Stuff/Error"));
			//    }
			//}

			animations = new Animation[Enum.GetValues(typeof(Anims)).Length];
            foreach (Anims anim in anims.Keys.ToArray())
            {
                animations[(int)anim] = new Animation();
                animations[(int)anim].LoadAnimation(anims[anim], rectangles);
            }
        }

		public void Initialize()
		{
			foreach (Anims anim in Enum.GetValues(typeof(Anims)))
				animations[(int)anim] = new Animation();
		}

        public void AddRectangle(string rectangle, Texture2D tex,FrameRectangle newrect)
        {
            //textures.Add(rectangle, tex);
            foreach (Animation anim in animations)
            {
                foreach (Keyframe frame in anim.Frames)
                {
                    frame.GetRectangles.Add(rectangle, newrect);
                }
            }
        }

		public void RemoveRectangle(string name)
		{
			foreach (Animation anim in animations)
			{
				foreach (Keyframe frame in anim.Frames)
				{
					frame.GetRectangles.Remove(name);
				}
			}
		}

        public void Update(GameTime gameTime)
        {
            animations[(int)currentAnimation].NextFrame();
        }

		public void Update(GameTime gameTime, bool reverse)
		{
			//if (reverse)
			//	animations[(int)currentAnimation].LastFrame();
			//else
				animations[(int)currentAnimation].NextFrame();
		}

		public void Draw(SpriteBatch spriteBatch, Matrix matrix, Vector2 EnemyPos, Star.Game.Enemy.Enemy.StandardDirection rundirection, Star.Game.Enemy.Enemy.StandardDirection standardirection, Dictionary<string, Texture2D> tex, RenderTarget2D target, RenderTarget2D resolvedTex)
        {
            animations[(int)currentAnimation].Draw(spriteBatch, matrix, EnemyPos, tex,rundirection,standardirection,target,resolvedTex);
        }


		#region ICloneable Member

		public object Clone()
		{
			AnimationManager clone = (AnimationManager)this.MemberwiseClone();
			
			return clone;
		}

		#endregion
	}
}
