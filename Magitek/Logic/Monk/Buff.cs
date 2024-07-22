using ff14bot;
using ff14bot.Managers;
using Magitek.Extensions;
using Magitek.Logic.Roles;
using Magitek.Models.Monk;
using MonkRoutine = Magitek.Utilities.Routines.Monk;
using Magitek.Utilities;
using System.Linq;
using System.Threading.Tasks;

namespace Magitek.Logic.Monk
{
    internal static class Buff
    {
        public static async Task<bool> Meditate()
        {
            if (Core.Me.ClassLevel < 54)
                return false;

            if (!MonkSettings.Instance.UseAutoMeditate)
                return false;

            if(!Core.Me.IsAlive)
                return false;

            if (!Core.Me.InCombat && ActionResourceManager.Monk.ChakraCount < 5)
                return await Spells.Meditation.Cast(Core.Me);

            if (!Core.Me.HasTarget && ActionResourceManager.Monk.ChakraCount < 5)
                return await Spells.Meditation.Cast(Core.Me);

            return false;
        }

        public static async Task<bool> PerfectBalance()
        {
            if (Core.Me.ClassLevel < Spells.PerfectBalance.LevelAcquired)
                return false;

            if (!MonkSettings.Instance.UsePerfectBalance)
                return false;

            if (MonkRoutine.AoeEnemies5Yards >= MonkSettings.Instance.AoeEnemies)
            {
                if (Casting.LastSpell != Spells.ArmOfTheDestroyer)
                    return false;
            }
            else
            {
                if (Casting.LastSpell != Spells.Bootshine)
                    return false;
            }

            return await Spells.PerfectBalance.Cast(Core.Me);
        }

        public static async Task<bool> RiddleOfEarth()
        {
            if (Core.Me.ClassLevel < Spells.RiddleofEarth.LevelAcquired)
                return false;

            if (!MonkSettings.Instance.UseRiddleOfEarth)
                return false;

            return await Spells.RiddleofEarth.Cast(Core.Me);

        }

        public static async Task<bool> RiddleOfFire()
        {
            if (Core.Me.ClassLevel < Spells.RiddleofFire.LevelAcquired)
                return false;

            if (!MonkSettings.Instance.UseRiddleOfFire)
                return false;

            if (Spells.PerfectBalance.IsKnownAndReady() && !Core.Me.HasAura(Auras.PerfectBalance,true))
                return false;

            return await Spells.RiddleofFire.Cast(Core.Me);
        }

        public static async Task<bool> RiddleOfWind()
        {
            if (Core.Me.ClassLevel < Spells.RiddleofWind.LevelAcquired)
                return false;

            if (!MonkSettings.Instance.UseRiddleOfWind)
                return false;

            if (Spells.PerfectBalance.IsKnownAndReady() && !Core.Me.HasAura(Auras.PerfectBalance, true))
                return false;

            return await Spells.RiddleofWind.Cast(Core.Me);
        }

        public static async Task<bool> Brotherhood()
        {
            if (Core.Me.ClassLevel < Spells.Brotherhood.LevelAcquired)
                return false;

            if (!MonkSettings.Instance.UseBrotherhood)
                return false;

            if (Spells.PerfectBalance.IsKnownAndReady() && !Core.Me.HasAura(Auras.PerfectBalance, true))
                return false;

            return await Spells.Brotherhood.Cast(Core.Me);
        }

        public static async Task<bool> Mantra()
        {
            if (Core.Me.ClassLevel < Spells.Mantra.LevelAcquired)
                return false;

            if (CustomOpenerLogic.InOpener)
                return false;

            if (!MonkSettings.Instance.UseMantra)
                return false;

            if (!Globals.PartyInCombat)
                return false;

            if (!ActionManager.CanCast(Spells.Mantra.Id, Core.Me))
                return false;

            if (Group.CastableAlliesWithin30.Count(r => r.CurrentHealthPercent <= MonkSettings.Instance.MantraHealthPercent) < MonkSettings.Instance.MantraAllies)
                return false;

            return await Spells.Mantra.Cast(Core.Me);
        }

        public static async Task<bool> EarthReply()
        {
            if (Core.Me.ClassLevel < Spells.EarthReply.LevelAcquired)
                return false;

            if (CustomOpenerLogic.InOpener)
                return false;

            if (!MonkSettings.Instance.UseEarthReply)
                return false;

            if(!Core.Me.HasAura(Auras.EarthRumination,true))
                return false;

            if (!ActionManager.CanCast(Spells.EarthReply.Id, Core.Me))
                return false;

            if (Group.CastableAlliesWithin30.Count(r => r.CurrentHealthPercent <= MonkSettings.Instance.EarthReplyHealthPercent) < MonkSettings.Instance.EarthReplyAllies)
                return false;

            return await Spells.EarthReply.Cast(Core.Me);
        }


        public static async Task<bool> FormShiftIC()
        {
            if (Core.Me.ClassLevel < Spells.FormShift.LevelAcquired)
                return await Spells.Bootshine.Cast(Core.Me.CurrentTarget);

            if (!Spells.FormShift.CanCast())
                return false;

            if (Core.Me.HasAura(Auras.PerfectBalance))
                return false;

            if (Core.Me.HasAura(Auras.FormlessFist))
                return false;

            if (Core.Me.HasAura(Auras.OpoOpoForm))
                return false;

            if (Core.Me.HasAura(Auras.RaptorForm))
                return false;

            if (Core.Me.HasAura(Auras.CoeurlForm))
                return false;

            return await Spells.FormShift.Cast(Core.Me);
        }

        public static async Task<bool> UsePotion()
        {
            if (Spells.Brotherhood.IsKnown() && !Spells.Brotherhood.IsReady(11000))
                return false;

            return await PhysicalDps.UsePotion(MonkSettings.Instance);
        }
    }
}
