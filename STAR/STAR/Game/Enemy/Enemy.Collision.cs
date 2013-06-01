using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Star.Game.Level;
using Star.GameManagement;

namespace Star.Game.Enemy
{
	public partial class Enemy
	{
		Rectangle orgcollisionrect;

		public Rectangle Collisionrect
		{
			get { return orgcollisionrect; }
			set 
			{ 
				orgcollisionrect = value;
				enemyvariables[EnemyVariables.BoundingBox] = orgcollisionrect.X + "," + orgcollisionrect.Y + "," + orgcollisionrect.Width + "," + orgcollisionrect.Height;
			}
		}

		private void Collision(Quadtree quadtree,float elapsed,int numThread)
		{
			if (usesKI)
				AICollision(quadtree, elapsed, numThread);
			else
				switch (collision)
				{
					case EnemyCollision.Normal:
						NormalCollision(quadtree, elapsed, numThread);
						break;
					case EnemyCollision.NoSuicide:
						NormalCollision(quadtree, elapsed, numThread);
						break;
					default:
						NormalCollision(quadtree, elapsed, numThread);
						break;
				}
		}

		private void AICollision(Quadtree quadtree, float elapsed, int numThread)
		{
			canJump = false;
			Vector2 newpos = new Vector2();
			newpos = pos + speed * elapsed;
			Rectangle collisionrect = new Rectangle(
				orgcollisionrect.X + (int)pos.X + orgcollisionrect.Width/2,
				orgcollisionrect.Y + (int)pos.Y+ orgcollisionrect.Height / 2,
				orgcollisionrect.Width - orgcollisionrect.Width / 2,
				orgcollisionrect.Height - orgcollisionrect.Height / 2);
			Rectangle newcollisionrect = new Rectangle(
				orgcollisionrect.X + (int)newpos.X,
				orgcollisionrect.Y + (int)newpos.Y,
				orgcollisionrect.Width,
				orgcollisionrect.Height);
			List<Tile> leveltiles;
			leveltiles = quadtree.GetEnemyCollision(newcollisionrect, numThread);
			foreach (Tile tile in leveltiles)
			{
				if (tile.TileColission_Type != TileCollision.Event)
				{
					//newpos = pos + speed * elapsed;
					//BottomCollision
					if (collisionrect.Bottom <= tile.get_rect.Y &&
						newcollisionrect.Bottom >= tile.get_rect.Y)
					{
						if (newcollisionrect.Right >= tile.get_rect.X && (int)(newcollisionrect.Left) <= tile.get_rect.Right)
						{
							animations.CurrentAnimation = Anims.Walk;
							newpos.Y -= speed.Y * elapsed;
							speed.Y = 0;
							gravity = new Vector2();
							jumping = false;
							jumpSpeed = new Vector2();
							canJump = true;
							break;
						}
					}
					if (tile.TileColission_Type == TileCollision.Impassable)
					{
						//RightCollision
						if (tile.get_rect.X >= collisionrect.Right && newcollisionrect.Right > tile.get_rect.X)
						{
							if (newcollisionrect.Bottom > tile.get_rect.Y && newcollisionrect.Top < tile.get_rect.Bottom)
							{
								if (!pathfound)
									rundirection = StandardDirection.Left;
								speed.X = 0;
								//continue;
							}
						}
						//LeftCollision
						else if (collisionrect.Left > tile.get_rect.Right && newcollisionrect.Left <= tile.get_rect.Right)
						{
							if (newcollisionrect.Bottom >= tile.get_rect.Y && newcollisionrect.Top <= tile.get_rect.Bottom)
							{
								if (!pathfound)
									rundirection = StandardDirection.Right;
								speed.X = 0;
								//continue;

							}
						}
						//UpCollision
						else if (collisionrect.Y >= tile.get_rect.Bottom && newcollisionrect.Top < tile.get_rect.Bottom)
						{
							if (newcollisionrect.Right >= tile.get_rect.X && newcollisionrect.Left <= tile.get_rect.Right)
							{
								speed.Y *= -1;
								jumping = false;
								jumpSpeed = new Vector2();
								continue;
							}
						}
					}
				}



			}

		}

		private void NormalCollision(Quadtree quadtree,float elapsed,int numThread)
		{
			Vector2 newpos = new Vector2();
			newpos = pos + speed * elapsed;
			Rectangle collisionrect = new Rectangle(
				orgcollisionrect.X + (int)pos.X,
				orgcollisionrect.Y + (int)pos.Y,
				orgcollisionrect.Width,
				orgcollisionrect.Height);
			Rectangle newcollisionrect = new Rectangle(
				orgcollisionrect.X + (int)newpos.X,
				orgcollisionrect.Y + (int)newpos.Y,
				orgcollisionrect.Width,
				orgcollisionrect.Height);
			List<Tile> leveltiles;
			leveltiles = quadtree.GetEnemyCollision(newcollisionrect, numThread);
			foreach (Tile tile in leveltiles)
			{
				if (tile.TileColission_Type != TileCollision.Event)
				{
					//newpos = pos + speed * elapsed;
					//BottomCollision
					if (collisionrect.Bottom<= tile.get_rect.Y && 
						newcollisionrect.Bottom >= tile.get_rect.Y)
					{
						if (newcollisionrect.Right>= tile.get_rect.X && (int)(newcollisionrect.Left) <= tile.get_rect.Right)
						{
							animations.CurrentAnimation = Anims.Walk;
							newpos.Y -= speed.Y * elapsed;
							speed.Y = 0;
							gravity = new Vector2();
							break;
						}
					}
					if (tile.TileColission_Type == TileCollision.Impassable)
					{
						//RightCollision
						if (tile.get_rect.X >= collisionrect.Right && newcollisionrect.Right > tile.get_rect.X)
						{
							if (newcollisionrect.Bottom > tile.get_rect.Y && newcollisionrect.Top < tile.get_rect.Bottom)
							{
								rundirection = StandardDirection.Left;
								speed.X *= -1;
								//continue;
							}
						}
						//LeftCollision
						else if (collisionrect.Left> tile.get_rect.Right && newcollisionrect.Left <= tile.get_rect.Right)
						{
							if (newcollisionrect.Bottom  >= tile.get_rect.Y && newcollisionrect.Top <= tile.get_rect.Bottom)
							{
								rundirection = StandardDirection.Right;
								speed.X *= -1;
								//continue;

							}
						}
						//UpCollision
						else if (collisionrect.Y>= tile.get_rect.Bottom && newcollisionrect.Top < tile.get_rect.Bottom)
						{
							if (newcollisionrect.Right>= tile.get_rect.X && newcollisionrect.Left <= tile.get_rect.Right)
							{
								speed.Y *= -1;
								continue;
							}
						}
					}
				}



			}
			
		}

		private void UpdateRectangles(Vector2 newpos)
		{
			rect.Location = new Point((int)newpos.X + rect.Width/2,(int)newpos.Y + rect.Height/2);
			//animations.CurrentAnimationKeyframe.KillingRect
			animations.CurrentAnimationKeyframe.KillingRect.Rectangle = RectangleFunctions.ReCenterSpecialRects(animations.CurrentAnimationKeyframe.KillingRect, new Point((int)pos.X,(int)pos.Y));

			animations.CurrentAnimationKeyframe.DieRect.Rectangle = RectangleFunctions.ReCenterSpecialRects(animations.CurrentAnimationKeyframe.DieRect, new Point((int)pos.X, (int)pos.Y));
			if (rundirection != standardirection)
			{
				animations.CurrentAnimationKeyframe.KillingRect.Rectangle = new Rectangle(
					animations.CurrentAnimationKeyframe.KillingRect.Rectangle.X + 2 * ((int)pos.X - animations.CurrentAnimationKeyframe.KillingRect.Rectangle.Center.X),
					animations.CurrentAnimationKeyframe.KillingRect.Rectangle.Y,
					animations.CurrentAnimationKeyframe.KillingRect.Rectangle.Width,
					animations.CurrentAnimationKeyframe.KillingRect.Rectangle.Height);

				animations.CurrentAnimationKeyframe.DieRect.Rectangle = new Rectangle(
					animations.CurrentAnimationKeyframe.DieRect.Rectangle.X + 2 * ((int)pos.X - animations.CurrentAnimationKeyframe.DieRect.Rectangle.Center.X),
					animations.CurrentAnimationKeyframe.DieRect.Rectangle.Y,
					animations.CurrentAnimationKeyframe.DieRect.Rectangle.Width,
					animations.CurrentAnimationKeyframe.DieRect.Rectangle.Height);
				//animations.CurrentAnimationKeyframe.DieRect.Rectangle = new Rectangle((int)pos.X + Collisionrect.Width/2,(int)pos.Y +;

			}
		}
	}
}
