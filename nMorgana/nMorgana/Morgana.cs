using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;


namespace nMorgana
{
	using Aimtec;

	using Aimtec.SDK;
	using Aimtec.SDK.Damage;
	using Aimtec.SDK.Events;
	using Aimtec.SDK.Prediction;
	using Aimtec.SDK.Prediction.Health;
	using Aimtec.SDK.Prediction.Skillshots;

	using Aimtec.SDK.Menu;
	using Aimtec.SDK.Menu.Components;
	using Aimtec.SDK.Orbwalking;
	using Aimtec.SDK.TargetSelector;
	using Aimtec.SDK.Util;
	using Aimtec.SDK.Util.Cache;
	using Aimtec.SDK.Extensions;

	using Spell = Aimtec.SDK.Spell;

	internal class Morgana
	{
		public static Menu Menu = new Menu("nMorgana", "nMorgana", true);
		public static Orbwalker Orbwalker = new Orbwalker();
		public static Obj_AI_Hero MyPlayer => ObjectManager.GetLocalPlayer();
		public static HealthPrediction HealthPrediction = new HealthPrediction();

		public static Spell Q;
		public static Spell W;
		public static Spell R;



		public Morgana()
		{
			Q = new Spell(SpellSlot.Q, 1175);
			W = new Spell(SpellSlot.W, 900);
			R = new Spell(SpellSlot.R, 625);

			Q.SetSkillshot(0.5f, 70, 1200, true, SkillshotType.Line);


			Orbwalker.Attach(Menu);

			var ComboMenu = new Menu("combo", "Combo");
			{
				ComboMenu.Add(new MenuBool("useq", "Use Q"));
				ComboMenu.Add(new MenuBool("usew", "Use W"));
				ComboMenu.Add(new MenuBool("user", "Use R"));
				ComboMenu.Add(new MenuSlider("minr", "Min.Enemy For R", 3,1, 5));
			}
			Menu.Add(ComboMenu);

			var KSMenu = new Menu("ks", "KillSteal");
			{
				KSMenu.Add(new MenuBool("ksq", "KS With Q",false));
				KSMenu.Add(new MenuBool("ksr", "KS With R", false));
				KSMenu.Add(new MenuSlider("minmana", "Min. Mana For Ks",150,100,1000));
			}
			Menu.Add(KSMenu);

			var DrawMenu = new Menu("draw", "Drawings");
			{
				DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
				DrawMenu.Add(new MenuBool("drawq", "Draw W Range"));
				DrawMenu.Add(new MenuBool("drawq", "Draw R Range"));
			}
			Menu.Add(DrawMenu);
			Menu.Attach();

			Game.OnUpdate += Game_OnUpdate;
			Render.OnPresent += Render_OnPresent;
		}
		private void Game_OnUpdate()
		{	
			if (MyPlayer.IsDead)
				return;

			if (Orbwalker.Mode == OrbwalkingMode.Combo)
				Combo();

			KS();
		}

		private void Combo()
		{
			if (Menu["combo"]["user"].Enabled && R.Ready)
			{
				var besttarget = TargetSelector.GetTarget(R.Range);

				if (besttarget.IsValidTarget(R.Range) && besttarget.CountEnemyHeroesInRange(R.Range) >= Menu["combo"]["minr"].As<MenuSlider>().Value)
				{
					R.Cast();
				}
			}


			if (Menu["combo"]["useq"].Enabled && Q.Ready)
			{
				var besttarget = TargetSelector.GetTarget(Q.Range);
				
				if (besttarget.IsValidTarget(Q.Range))
				{
					var predi = Q.GetPrediction(besttarget);
					if(predi.HitChance >= HitChance.High)
					{
						Q.Cast(besttarget);
					}
				}

					Q.Cast(besttarget);

			}


			if (Menu["combo"]["usew"].Enabled && W.Ready)
			{
				var besttarget = TargetSelector.GetTarget(W.Range);

				if (besttarget.IsValidTarget(W.Range))
					W.Cast(besttarget);

			}

		}

		private void KS()
		{
			if(Menu["ks"]["ksq"].Enabled && Q.Ready && MyPlayer.Mana >= Menu["ks"]["minmana"].As<MenuSlider>().Value)
			{
				
				foreach(Obj_AI_Hero enemy in GameObjects.EnemyHeroes)
				{
				var ksc = 	GameObjects.EnemyHeroes.FirstOrDefault(x => MyPlayer.GetSpellDamage(enemy, SpellSlot.Q) > enemy.Health + enemy.MagicalShield + 10 && enemy.IsValidTarget(Q.Range));
					if (ksc == null)
					{
						return;
					}
					var predi = Q.GetPrediction(ksc);
					if (predi.HitChance >= HitChance.High)
					{
						Q.Cast(ksc);
					}
					return;
				}
				
			}
			if (Menu["ks"]["ksr"].Enabled && R.Ready && MyPlayer.Mana >= Menu["ks"]["minmana"].As<MenuSlider>().Value)
			{
				foreach (Obj_AI_Hero enemy in GameObjects.EnemyHeroes)
				{
					var ksc = GameObjects.EnemyHeroes.FirstOrDefault(x => MyPlayer.GetSpellDamage(enemy, SpellSlot.R) > enemy.Health + enemy.MagicalShield + 10 && enemy.IsValidTarget(R.Range));
					if (ksc == null)
					{
						return;
					}
					R.Cast();
					return;
				}
			}
		}

		private void Render_OnPresent()
		{
			if (Menu["draw"]["drawq"].Enabled)
			{
				Render.Circle(MyPlayer.Position, Q.Range, 30, Color.Purple);
			}
			if (Menu["draw"]["draww"].Enabled)
			{
				Render.Circle(MyPlayer.Position, W.Range, 30, Color.Purple);
			}
			if (Menu["draw"]["drawr"].Enabled)
			{
				Render.Circle(MyPlayer.Position, R.Range, 30, Color.Red);
			}
		}
		}
		   }







		






	

