using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nMorgana
{
	using Aimtec;
	using Aimtec.SDK;
	using Aimtec.SDK.Damage;
	using Aimtec.SDK.Events;
	using Aimtec.SDK.Extensions;
	using Aimtec.SDK.Menu;
	using Aimtec.SDK.Orbwalking;
	using Aimtec.SDK.Prediction;
	using Aimtec.SDK.TargetSelector;
	using Aimtec.SDK.Util;
	
	public enum nHitChance
	{
		Immobile = 8,
		Dashing = 7,
		VeryHigh = 6,
		High = 5,
		Medium = 4,
		Low = 3,
		Impossible = 2,
		OutOfRange= 1,
		Collision = 0
	
	}

	public enum nSkillshotType
	{
		SkillshotLine,
		SkillshotCircle,
		SkillshotCone
	}

	public enum nCollisionableObjects
	{
		Minions,
		Heroes,
		Yasuowall,
		Walls,
		Allies
	}

	public class nPredictionInput
	{
		#region	Fields

		public bool Aoe = false;
		public bool Collision = false;

		public nCollisionableObjects[] CollisionableObjects =
		{
			nCollisionableObjects.Minions,nCollisionableObjects.Yasuowall
		};

		public float Delay;
		public float Radius = 1f;
		public float Range = float.MaxValue;
		public float Speed = float.MaxValue;

		public nSkillshotType Type = nSkillshotType.SkillshotLine;

		public Obj_AI_Base unit = ObjectManager.GetLocalPlayer();

		public bool UseBoundingRadius = true;

		private Vector3 _from;
		private Vector3 _rangeCheckFrom;

		#endregion
	}
}
