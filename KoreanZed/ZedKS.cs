﻿namespace KoreanZed
{
    using LeagueSharp;
    using LeagueSharp.Common;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SharpDX;

    class ZedKS
    {
        private readonly ZedSpell q;

        private readonly ZedSpell w;

        private readonly ZedSpell e;

        private readonly Orbwalking.Orbwalker zedOrbwalker;

        private readonly Obj_AI_Hero player;

        private readonly ZedShadows zedShadows;

        public ZedKS(ZedSpells spells, Orbwalking.Orbwalker orbwalker, ZedShadows zedShadows)
        {
            q = spells.Q;
            w = spells.W;
            e = spells.E;

            player = ObjectManager.Player;

            zedOrbwalker = orbwalker;
            this.zedShadows = zedShadows;

            Game.OnUpdate += Game_OnUpdate;
        }

        private void Game_OnUpdate(EventArgs args)
        {
            if (q.IsReady() && player.Mana > q.ManaCost)
            {
                foreach (Obj_AI_Hero objAiHero in player.GetEnemiesInRange(q.Range).Where(hero => !hero.IsDead && !hero.IsZombie && q.IsKillable(hero)))
                {
                    PredictionOutput predictionOutput = q.GetPrediction(objAiHero);

                    if ((predictionOutput.Hitchance >= HitChance.High) &&
                        ((!q.GetCollision(player.Position.To2D(), new List<Vector2> { predictionOutput.CastPosition.To2D() }).Any())
                        || q.GetDamage(objAiHero) / 2 > objAiHero.Health))
                    {
                        q.Cast(predictionOutput.CastPosition);
                    }
                }
            }

            if (e.IsReady() && player.Mana > e.ManaCost)
            {
                if (player.GetEnemiesInRange(e.Range).Any(hero => !hero.IsDead && !hero.IsZombie && e.IsKillable(hero)))
                {
                    e.Cast();
                }
            }

            if (zedOrbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo || !zedShadows.CanCast)
            {
                return;
            }

            List<Obj_AI_Hero> heroList = ObjectManager.Player.GetEnemiesInRange(2000F);

            if (heroList.Count() == 1)
            {
                Obj_AI_Hero target = heroList.FirstOrDefault();

                if (target != null && w.IsReady() && player.Distance(target) < w.Range + Orbwalking.GetRealAutoAttackRange(target))
                {
                    if ((player.GetAutoAttackDamage(target) > target.Health && player.Mana > w.ManaCost)
                        || (e.IsReady() && e.GetDamage(target) + player.GetAutoAttackDamage(target) > target.Health
                        && player.Mana > w.ManaCost + e.ManaCost))
                    {
                        zedShadows.Cast(target);
                    }
                }
            }

        }
    }
}
