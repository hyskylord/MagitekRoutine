using ff14bot;
using Magitek.Models.WhiteMage;
using Magitek.Utilities;
using Magitek.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magitek.Models.Account;
using ff14bot.Objects;
using Auras = Magitek.Utilities.Auras;

namespace Magitek.Logic.WhiteMage
{
    internal class HealFightLogic
    {
        public static async Task<bool> Aoe()
        {
            if (!Globals.InParty)
                return false;

            if (!FightLogic.ZoneHasFightLogic())
                return false;

            if (!FightLogic.EnemyIsCastingBigAoe() && !FightLogic.EnemyIsCastingAoe())
                return false;

            if (WhiteMageSettings.Instance.FightLogicDivineCaress
                && Spells.DivineCaress.IsKnownAndReady()
                && Spells.DivineCaress.CanCast())
            {
                if (BaseSettings.Instance.DebugFightLogic)
                    Logger.WriteInfo($"[AOE Response] Cast Divine Caress");
                return await FightLogic.DoAndBuffer(Spells.DivineCaress.Cast(Core.Me));
            }

            if (WhiteMageSettings.Instance.FightLogicTemperance
                && Spells.Temperance.IsKnownAndReady()
                && Spells.Temperance.CanCast())
            {
                if (BaseSettings.Instance.DebugFightLogic)
                    Logger.WriteInfo($"[AOE Response] Cast Temperance");
                return await FightLogic.DoAndBuffer(Spells.Temperance.Cast(Core.Me));
            }

            if (WhiteMageSettings.Instance.FightLogicLiturgyoftheBell
                && Spells.LiturgyOfTheBell.IsKnownAndReady()
                && Spells.LiturgyOfTheBell.CanCast())
            {
                if (BaseSettings.Instance.DebugFightLogic)
                    Logger.WriteInfo($"[AOE Response] Cast Liturgy of the Bell");

                Character target = Core.Me;

                if (WhiteMageSettings.Instance.LiturgyOfTheBellCenterParty)
                {
                    var targets = Group.CastableAlliesWithin30.OrderBy(r =>
                        Group.CastableAlliesWithin30.Sum(ot => r.Distance(ot.Location))
                    ).ThenBy(t => Core.Me.Distance(t.Location));

                    target = targets.FirstOrDefault(Core.Me);
                }

                return await FightLogic.DoAndBuffer(Spells.LiturgyOfTheBell.Cast(target));
            }

            if (WhiteMageSettings.Instance.FightLogicAsylum
                && Spells.Asylum.IsKnownAndReady()
                && Spells.Asylum.CanCast())
            {
                if (BaseSettings.Instance.DebugFightLogic)
                    Logger.WriteInfo($"[AOE Response] Cast Asylum");

                Character target = Core.Me;

                if (WhiteMageSettings.Instance.AsylumCenterParty)
                {
                    var targets = Group.CastableAlliesWithin30.OrderBy(r =>
                        Group.CastableAlliesWithin30.Sum(ot => r.Distance(ot.Location))
                    ).ThenBy(t => Core.Me.Distance(t.Location));

                    target = targets.FirstOrDefault(Core.Me);
                }

                return await FightLogic.DoAndBuffer(Spells.Asylum.Cast(target));
            }

            if (WhiteMageSettings.Instance.FightLogicPlenaryIndulgence
                && Spells.PlenaryIndulgence.IsKnownAndReady()
                && Spells.PlenaryIndulgence.CanCast())
            {
                if (BaseSettings.Instance.DebugFightLogic)
                    Logger.WriteInfo($"[AOE Response] Cast Plenary Indulgence");

                return await FightLogic.DoAndBuffer(Spells.PlenaryIndulgence.Cast(Core.Me));
            }

            if (WhiteMageSettings.Instance.FightLogicMedica2
                && Spells.Medica2.IsKnownAndReady()
                && Spells.Medica2.CanCast())
            {
                if (BaseSettings.Instance.DebugFightLogic)
                    Logger.WriteInfo($"[AOE Response] Cast Medica 2");

                return await FightLogic.DoAndBuffer(Spells.Medica2.Cast(Core.Me));
            }

            return false;
        }

        public static async Task<bool> Tankbuster()
        {
            if (!Globals.InParty)
                return false;

            if (!FightLogic.ZoneHasFightLogic())
                return false;

            var target = FightLogic.EnemyIsCastingTankBuster();

            if (target == null)
            {
                target = FightLogic.EnemyIsCastingSharedTankBuster();

                if (target == null)
                    return false;
            }

            if (WhiteMageSettings.Instance.FightLogicDivineBenison
                && Spells.DivineBenison.IsKnownAndReady()
                && !target.HasAura(Auras.DivineBenison)
                && Spells.DivineBenison.CanCast(target))
            {
                if (BaseSettings.Instance.DebugFightLogic)
                    Logger.WriteInfo($"[TankBuster Response] Cast Divine Benison on {target.Name}");
                return await FightLogic.DoAndBuffer(Spells.DivineBenison.HealAura(target, Auras.DivineBenison));
            }

            if (WhiteMageSettings.Instance.FightLogicAquaveil
                && Spells.Aquaveil.IsKnownAndReady()
                && !target.HasAura(Auras.Aquaveil)
                && Spells.Aquaveil.CanCast(target))
            {
                if (BaseSettings.Instance.DebugFightLogic)
                    Logger.WriteInfo($"[TankBuster Response] Cast Aquaveil on {target.Name}");
                return await FightLogic.DoAndBuffer(Spells.Aquaveil.HealAura(target, Auras.Aquaveil));
            }

            return false;
        }
    }
}
