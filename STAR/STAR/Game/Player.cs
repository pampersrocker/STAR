using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Star.Game.Level;
using Star.GameManagement;

namespace Star.Game
{
	public enum PPI
	{
		Hand2,
		Body,
		Head,
		Foot1,
		Foot2,
		Hand1,
	}

    public enum CollisionType
    { 
        StandsOnIt,
        WalksAgainstIt,
        JumpsAgainstIt,
        JumpsAgainstTop,
		NoCollision
    }

    public class Player
    {
        #region Variables

        public const float MAX_SPEED = 130f;

        const float SIZE_MULTIPLIKATOR = 1.2f;
        const float HEAD_SIZE = 20.0F* SIZE_MULTIPLIKATOR;
        const float BODY_HEIGHT = 25.5f * SIZE_MULTIPLIKATOR;
        const float BODY_WIDTH = 15.5f * SIZE_MULTIPLIKATOR;
        const float FOOT_WIDTH = 10.0f * SIZE_MULTIPLIKATOR;
        const float FOOT_HEIGHT =( FOOT_WIDTH / 2.0f);
        const float HAND_HEIGHT = 5.0f * SIZE_MULTIPLIKATOR;
        const float HAND_WIDTH = HAND_HEIGHT ;
        const int AMOUNT_PIECES = 6;
        const float JUMP_CONSTANT = 100;

        float jump_fallof = 2;

		List<CollisionType> collission;

		public List<CollisionType> Collission
		{
			get { return collission; }
		}
        Vector2[] animation_foot;
        bool is_jumping = false;
        double walk_animation_time = 0;
        Vector2 pos;
        Vector2[] positions;
        Vector2 cur_speed;
        Rectangle bounding_box;
        Rectangle[] boxes;
        Texture2D[] tex;
        Vector2 physics = new Vector2();
        Vector2 jump = new Vector2();
        Vector2 prev_pos;
        Vector2 difference;
        float body_size_time = 0;
        float body_size = 1.0f;
        float hand_walk_animation_time = 0;
        float hand_animation_jump_Y = 0;
        float hand_animation_jump_X = 0;
        Vector2[] hand_animation_walk;
        Rectangle right;
        Rectangle left;
        Rectangle top;
        Rectangle bottom;
        Effect alphablur;
        bool jump_button_released = true;
        int i;

        #endregion

        #region Properties

        public Vector2 speed
        {
            get { return cur_speed; }
            set { cur_speed = value; }
        }

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public Vector2 getDifference
        {
            get { return difference; }
        }

        public Vector2 Gravity
        {
            get { return physics; }
        }

        public Vector2 Jump
        {
            get { return jump; }
        }

        public Rectangle BoundingBox
        {
            get { return bounding_box; }
        }

        #endregion

        #region Constructor

        public Player(Vector2 new_pos,ContentManager Content)
        {
            collission = new List<CollisionType>();
            pos = new_pos;
            prev_pos = new_pos;
            difference = Vector2.Zero;
            
            tex = new Texture2D[AMOUNT_PIECES];
            boxes = new Rectangle[AMOUNT_PIECES];
            positions = new Vector2[AMOUNT_PIECES];
            animation_foot = new Vector2[2];
            hand_animation_walk = new Vector2[2];
            //PPI = Player Piece Indicator
            tex[(int)PPI.Body] = Content.Load<Texture2D>("Img/Player/Body");
            tex[(int)PPI.Head] = Content.Load<Texture2D>("Img/Player/Head");
            tex[(int)PPI.Foot1] = Content.Load<Texture2D>("Img/Player/Foot");
            tex[(int)PPI.Foot2] = Content.Load<Texture2D>("Img/Player/Foot");
            tex[(int)PPI.Hand1] = Content.Load<Texture2D>("Img/Player/Hand");
            tex[(int)PPI.Hand2] = Content.Load<Texture2D>("Img/Player/Hand");
            alphablur = Content.Load<Effect>("Stuff/Effects/AlphaBlur");

            boxes[(int)PPI.Body].X = (int)new_pos.X - (int)(BODY_WIDTH / 2);
            boxes[(int)PPI.Body].Y = (int)new_pos.Y;
            boxes[(int)PPI.Body].Width = (int)BODY_WIDTH;
            boxes[(int)PPI.Body].Height = (int)BODY_HEIGHT;
            boxes[(int)PPI.Head].Width = (int)HEAD_SIZE;
            boxes[(int)PPI.Head].Height = (int)HEAD_SIZE;
            boxes[(int)PPI.Foot1].Width = (int)FOOT_WIDTH;
            boxes[(int)PPI.Foot1].Height = (int)FOOT_HEIGHT;
            boxes[(int)PPI.Foot2].Width = (int)FOOT_WIDTH;
            boxes[(int)PPI.Foot2].Height = (int)FOOT_HEIGHT;
            boxes[(int)PPI.Hand1].Width = (int)HAND_WIDTH;
            boxes[(int)PPI.Hand2].Width = (int)HAND_WIDTH;
            boxes[(int)PPI.Hand1].Height = (int)HAND_HEIGHT;
            boxes[(int)PPI.Hand2].Height = (int)HAND_HEIGHT;
            bounding_box = new Rectangle((int)pos.X, (int)pos.Y, 30, 45);
        }

        #endregion

        #region Animation

        Vector2[] walk_animation(GameTime gametime)
        {
            Vector2[] walk_animation = new Vector2[2];

            walk_animation_time += speed.X / 250.0f;

            if (walk_animation_time>2*MathHelper.Pi)
            {
                walk_animation_time = 0;
            }
            else if (walk_animation_time < 0)
            {
                walk_animation_time = 2 * MathHelper.Pi;
            }
            
            walk_animation[0] = new Vector2(
                0.75f* SIZE_MULTIPLIKATOR*(float)Math.Cos(2*(double)walk_animation_time),
                0.75f * SIZE_MULTIPLIKATOR * (float)Math.Sin(2 * (double)walk_animation_time));
            walk_animation[1] = new Vector2(
                0.75f * SIZE_MULTIPLIKATOR * -(float)Math.Cos(2 * (double)walk_animation_time),
                0.75f * SIZE_MULTIPLIKATOR *  -(float)Math.Sin(2 * (double)walk_animation_time));
            animation_foot = walk_animation;
            return walk_animation;
        }

        public void breath_animation(GameTime gametime)
        {
            body_size_time += 5 * (float)gametime.ElapsedGameTime.TotalSeconds;
            if (body_size_time > 2 * MathHelper.Pi)
            {
                body_size_time = 0;
            }
            body_size = (float)Math.Sin(body_size_time);
        }

        private void update_hand_animation(GameTime gametime)
        {
            if(!collission.Contains(CollisionType.StandsOnIt))
            {
                if (speed.X > 20)
                {
                    hand_animation_jump_X += (20 - hand_animation_jump_X)* MathHelper.Clamp(speed.X,.0f,5.0f) * (float)gametime.ElapsedGameTime.TotalSeconds;
                }
                else if (speed.X < -20)
                {
                    hand_animation_jump_X += (-20 - hand_animation_jump_X) * MathHelper.Clamp(-speed.X, .0f, 5.0f) * (float)gametime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    hand_animation_jump_X += -hand_animation_jump_X* 10 * (float)gametime.ElapsedGameTime.TotalSeconds;
                }
                if (hand_animation_jump_Y > -10)
                {
                    hand_animation_jump_Y -= 100 * (float)gametime.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                if (hand_animation_jump_Y < 0)
                {
                    hand_animation_jump_Y += 150 * (float)gametime.ElapsedGameTime.TotalSeconds;
                }
                hand_animation_jump_X -= hand_animation_jump_X * 10 * (float)gametime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void update_hand_walk_animation(GameTime gametime)
        {
            hand_walk_animation_time += speed.X / 500.0f;
            if (hand_walk_animation_time > 2 * MathHelper.Pi)
            {
                hand_walk_animation_time = 0;
            }
            else if (hand_walk_animation_time < 0)
            {
                hand_walk_animation_time = 2 * MathHelper.Pi;
            }
            hand_animation_walk[0].X = (2* SIZE_MULTIPLIKATOR * (float)Math.Sin((double)hand_walk_animation_time));
            hand_animation_walk[1].X = (2* SIZE_MULTIPLIKATOR * (float)Math.Sin((double)hand_walk_animation_time - MathHelper.Pi));
            //hand_animation.X = (SIZE_MULTIPLIKATOR * (float)Math.Sin((double)hand_walk_animation_time));
            float sin_hand1 = ((float)Math.Sin((double)hand_walk_animation_time - (MathHelper.Pi /2)));
            float sin_hand2 = ((float)Math.Sin((double)hand_walk_animation_time - (MathHelper.Pi / 2) - MathHelper.Pi));
            if (sin_hand1 < 0)
            {
                sin_hand1 = 0;
            }
            if (sin_hand2 < 0)
            {
                sin_hand2 = 0;
            }
            hand_animation_walk[0].Y = (SIZE_MULTIPLIKATOR * sin_hand1 * 5);
            hand_animation_walk[1].Y = (SIZE_MULTIPLIKATOR * sin_hand2 * 5);
        }

        #endregion

        #region Collision
        private List<CollisionType> collision(Tile tile,ref Vector2 speed,ref Vector2 temp_pos,Input.RunDirection rundirection)
        {
            //collission.Clear();
            right = new Rectangle(bounding_box.Right-5, bounding_box.Top+10, 5, bounding_box.Height-35);
            left = new Rectangle(bounding_box.Left, bounding_box.Top+10, 5, bounding_box.Height-35);
            top = new Rectangle(bounding_box.Left+6, bounding_box.Top-3, bounding_box.Width-12,3);
            bottom = new Rectangle(bounding_box.Left+6, bounding_box.Bottom-6, bounding_box.Width-12, 3);
            List<CollisionType> intersects = new List<CollisionType>();
            Vector2 distance_vector = new Vector2(tile.get_rect.Center.X,
                tile.get_rect.Center.Y) - temp_pos;
            switch (tile.TileColission_Type)
            {
                case (TileCollision.Impassable):
                    impassable_collision(tile, ref speed, ref temp_pos, ref intersects);
                    break;
                case (TileCollision.Platform):
                    platform_collision(tile,ref speed,ref temp_pos,ref intersects);
                    break;
            }
            return intersects;
        }

        private void platform_collision(Tile tile,ref Vector2 speed,ref Vector2 temp_pos,ref List<CollisionType> intersects)
        {
            Rectangle tile_rect = tile.get_rect;
            tile_rect.Y += 7;
            if (bottom.Intersects(tile_rect) && speed.Y > 0 &&
                !left.Intersects(tile_rect) &&
                !right.Intersects(tile_rect) &&
                !top.Intersects(tile_rect))
            {
                intersects.Add(CollisionType.StandsOnIt);
                temp_pos.Y -= bottom.Bottom - tile_rect.Top;
                speed.Y = 0;
            }
        }

        private void impassable_collision(Tile tile,ref Vector2 speed,
            ref Vector2 temp_pos,ref List<CollisionType> intersects)
        {
            if (bottom.Intersects(tile.get_rect) &&
                    speed.Y > 0)
            {
                intersects.Add(CollisionType.StandsOnIt);
                //int temp = (bounding_box.Bottom - tile.get_rect.Top);
                speed.Y = 0;
                temp_pos.Y -= bounding_box.Bottom - tile.get_rect.Top;
				//pos.Y -= bottom.Bottom - tile.get_rect.Top;
                //move_down = false;
            }
            else if (right.Intersects(tile.get_rect) &&
                speed.X >= 0)
            {
                if (is_jumping == true)
                {

                    intersects.Add(CollisionType.JumpsAgainstIt);
                }
                else
                {
                    intersects.Add(CollisionType.WalksAgainstIt);
                }
                speed.X = 0;
                temp_pos.X -= right.Right - tile.get_rect.Left;
                //move_right = false;
            }
            else if (left.Intersects(tile.get_rect) &&
                speed.X <= 0)
            {
                if (is_jumping == true)
                {
                    intersects.Add(CollisionType.JumpsAgainstIt);
                }
                else
                {
                    intersects.Add(CollisionType.WalksAgainstIt);
                }
                speed.X = 0;
                temp_pos.X += tile.get_rect.Right - left.Left;
                //move_left = false;
            }
            else if (top.Intersects(tile.get_rect) &&
                speed.Y < 0)
            {
                intersects.Add(CollisionType.JumpsAgainstTop);
                speed.Y = 0;
                jump.Y = -physics.Y;
                temp_pos.Y += tile.get_rect.Bottom - bounding_box.Top;
                //physics.Y = 0;
                //move_up = false;
            }
        }

        #endregion

        #region Draw

        public void draw_player(ref SpriteBatch spritebatch,Matrix translation,Input.RunDirection rundirection,GraphicsDevice device)
        {
			//3_1
            //spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, translation);
			spritebatch.Begin(SpriteSortMode.Immediate,
				BlendState.AlphaBlend,
				null, null, null, null,
				translation);
			//device.SamplerStates[0].MinFilter = TextureFilter.Linear;
            //device.SamplerStates[0].MagFilter = TextureFilter.Linear;
            //device.SamplerStates[0].MipFilter = TextureFilter.None;
            //device.SamplerStates[0].MaxAnisotropy = 16;
            //device.
            
            //this.graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings; 
            //alphablur.Begin();
            
            //alphablur.CurrentTechnique.Passes[0].Begin();   
            
            for (i = 0; i < AMOUNT_PIECES; i++)
            {
                if (rundirection == Star.Input.RunDirection.Left)
                {
                    spritebatch.Draw(tex[i], boxes[i], null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    spritebatch.Draw(tex[i], boxes[i], Color.White);
                }

                
            }
            //alphablur.CurrentTechnique.Passes[0].End();
            //alphablur.End();

           // spritebatch.Draw(temp, bounding_box, Color.White);

            //draw_collision_Boxes(spritebatch);
            //spritebatch.Draw(tex[(int)Player_Piece_Indicator.Body], bounding_box, Color.Red);
            spritebatch.End();
        }

        private void draw_collision_Boxes(SpriteBatch spritebatch)
        { 
			//spritebatch.Draw(temp, left, Color.White);
			//spritebatch.Draw(temp, top, Color.White);
			//spritebatch.Draw(temp, bottom, Color.White);
			//spritebatch.Draw(temp, right, Color.White);
        }

        #endregion

        #region Update

        public List<CollisionType> Update(GameTime gametime,Vector2 new_aim,
            Quadtree quadtree,List<Input.InputKeys> inputkeys,List<Input.InputKeys> oldinputkeys,
            Input.RunDirection rundirection,float run_factor,Vector2 applyingForce)
        {
            List<CollisionType> cur_collission = new List<CollisionType>();
            List<CollisionType> return_collission = new List<CollisionType>() ;
            prev_pos = pos;
            new_aim.Y = pos.Y;
            speed = new_aim - pos;
			speed += apply_physics(gametime);
            breath_animation(gametime);
            update_hand_animation(gametime);
            speed += apply_jump(gametime, inputkeys,oldinputkeys,run_factor);
			speed += applyingForce;
            run_factor = MathHelper.Clamp(run_factor, 1.0f, 1.5f);

            Vector2 temp_pos = pos;
            update_positions(gametime, temp_pos);
            //collission = Collission_Type.NoCollission;
            if (quadtree.GetCollision(bounding_box) != null)
            {
                List<Tile> collisionTiles = quadtree.GetCollision(bounding_box);
                foreach (Tile tile in collisionTiles)
                {
                    cur_collission = collision(tile, ref cur_speed, ref temp_pos, rundirection);
                    
                    foreach(CollisionType coltype in cur_collission)
                    {
                        return_collission.Add(coltype);
                    }
                    update_positions(gametime, temp_pos);
                }
            }
            collission = return_collission;
			if (collission.Contains(CollisionType.StandsOnIt) || is_jumping)
				physics.Y = 0;
            cur_speed.X = MathHelper.Clamp(speed.X, -MAX_SPEED, MAX_SPEED);
            cur_speed.Y = MathHelper.Clamp(speed.Y, - MAX_SPEED, MAX_SPEED);
			pos.X += speed.X * (float)gametime.ElapsedGameTime.TotalSeconds * 5;
            pos.Y += speed.Y * (float)gametime.ElapsedGameTime.TotalSeconds * 5;
            
            if (collission.Count != 0)
            {
                walk_animation(gametime);
                update_hand_walk_animation(gametime);
            }
            difference = pos - prev_pos;
            return return_collission;
        }

        Vector2 apply_physics(GameTime gametime)
        {
			physics.Y += GameParameters.Gravity* (float)gametime.ElapsedGameTime.TotalSeconds;
			physics.Y = MathHelper.Clamp(physics.Y, -GameParameters.Gravity/5, GameParameters.Gravity/5);
            return physics;
        }

        Vector2 apply_jump(GameTime gametime, List<Input.InputKeys> inputkeys, List<Input.InputKeys> oldinputkeys, float run_factor)
        {
			if (collission.Contains(CollisionType.StandsOnIt) && is_jumping)
			{
				is_jumping = false;
				jump.Y = 0;
			}

            if (inputkeys.Contains(Star.Input.InputKeys.Jump) &&
            (collission.Contains(CollisionType.StandsOnIt)) &&
            (oldinputkeys.Contains(Star.Input.InputKeys.Jump)) &&
            jump_button_released == true &&
            is_jumping == false)
            {
                is_jumping = true;
                jump.Y = -JUMP_CONSTANT * MathHelper.Clamp((run_factor / 2.25f) * 1.2f, 1.0f, 1.2f);
                jump_button_released = false;
            }
            if (!(inputkeys.Contains(Input.InputKeys.Jump)))
            {
                jump_button_released = true;
            }
            if (is_jumping == true)
            {
                if (jump_button_released == false)
                {
                    jump_fallof = 1.2f;
                }
                else
                {
                    jump_fallof = 2.5f;
                }
                jump.Y += jump_fallof * JUMP_CONSTANT * (float)gametime.ElapsedGameTime.TotalSeconds;
            }

            if (jump.Y > -50)
            {
                is_jumping = false;
                jump.Y = 0;
            }

            if ((collission.Contains(CollisionType.JumpsAgainstTop) || speed.Y == 0) && is_jumping == true)
            {
                //is_jumping = false;
                jump.Y = -physics.Y;
            }
				

            return jump;
        }

        void update_positions(GameTime gametime, Vector2 new_pos)
        {

            positions[(int)PPI.Body].X = (float)Math.Ceiling((new_pos.X - BODY_WIDTH / 2) + (body_size * 1.0f));
            positions[(int)PPI.Body].Y = (float)Math.Ceiling(new_pos.Y);
            positions[(int)PPI.Head].X = (float)Math.Ceiling(new_pos.X - HEAD_SIZE / 2);
            positions[(int)PPI.Head].Y = (float)Math.Ceiling(new_pos.Y - HEAD_SIZE);
            positions[(int)PPI.Foot1].X = (float)Math.Ceiling(new_pos.X + -FOOT_WIDTH / 2 + animation_foot[0].X * 5);
			positions[(int)PPI.Foot1].Y = (float)Math.Ceiling(new_pos.Y + BODY_HEIGHT - FOOT_HEIGHT / 2 + animation_foot[0].Y * 5);//- hand_animation_jump_Y / 2);
            positions[(int)PPI.Foot2].X = (float)Math.Ceiling(new_pos.X + -FOOT_WIDTH / 2 + animation_foot[1].X * 5);
			positions[(int)PPI.Foot2].Y = (float)Math.Ceiling(new_pos.Y + BODY_HEIGHT - FOOT_HEIGHT / 2 + animation_foot[1].Y * 5);//- hand_animation_jump_Y / 2);

			positions[(int)PPI.Hand1].X = (float)Math.Ceiling(new_pos.X - HAND_WIDTH / 2 + hand_animation_walk[0].X * 5 +hand_animation_jump_X);
            positions[(int)PPI.Hand1].Y = (float)Math.Ceiling(new_pos.Y + hand_animation_walk[0].Y + hand_animation_jump_Y);
            positions[(int)PPI.Hand2].X = (float)Math.Ceiling(new_pos.X - HAND_WIDTH / 2 + hand_animation_walk[1].X * 5  +hand_animation_jump_X);
            positions[(int)PPI.Hand2].Y = (float)Math.Ceiling(new_pos.Y + hand_animation_walk[1].Y + hand_animation_jump_Y);
            for (i = 0; i < AMOUNT_PIECES; i++)
            {
                boxes[i].Location = new Point(
                    (int)positions[i].X,
                    (int)positions[i].Y);
            }
            boxes[(int)PPI.Body].Width = (int)(BODY_WIDTH + -body_size * 1.5f);
            //Updating Bounding Box
            Rectangle temp_box;
            int height;
            height = (int)HEAD_SIZE + (int)BODY_WIDTH;
            if (boxes[(int)PPI.Foot1].Y > boxes[(int)PPI.Foot2].Y)
            {
                height += boxes[(int)PPI.Foot1].Y - (int)new_pos.Y - (int)BODY_HEIGHT + 2 * (int)FOOT_HEIGHT;
            }
            else
            {
                height += boxes[(int)PPI.Foot2].Y - (int)new_pos.Y - (int)BODY_HEIGHT + 2 * (int)FOOT_HEIGHT;
            }
            temp_box = new Rectangle(boxes[(int)PPI.Head].X,
                boxes[(int)PPI.Head].Y,
                (int)HEAD_SIZE, height);

            bounding_box = temp_box;
        }

        #endregion
    }
}
