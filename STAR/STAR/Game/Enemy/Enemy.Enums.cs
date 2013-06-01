using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Star.Game.Enemy
{
	public partial class Enemy
	{
		public enum CollisionRects
		{ 
			BoundingBox,
			KillingRect,
			DieRect
		}

		public enum EnemyVariables
		{
			AnimRectangles,
			BoundingBox,
			MaxSpeed,
			MovementType,
			EnemyCollision,
			StandardDirection,
			PlayerTracking
		}

		public enum StandardDirection
		{ 
			Left,
			Right,
		}

		public enum EnemyCollision
		{ 
			NoCollision,
			Normal,
			NoSuicide,
		}

		public enum EnemyMovement
		{ 
			Normal,
			Flying
		}

		public enum PlayerTracking
		{ 
			Tracking,
			NotTracking
		}

		public enum MovementType
		{
			Normal,
			ExponentialToPlayer
		}
	}
}
