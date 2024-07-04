using ff14bot;
using ff14bot.Managers;
using Magitek.Extensions;
using Magitek.Models.Machinist;
using Magitek.Models.Viper;
using Magitek.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Magitek.Logic.Viper
{
    internal static class Cooldown
    {

        public static async Task<bool> DeathRattle()
        {
            //Add level check so it doesn't hang here
            if (Core.Me.ClassLevel < Spells.DeathRattle.LevelAcquired)
                return false;

            if (!ViperSettings.Instance.UseDeathRattle)
                return false;

            return await Spells.DeathRattle.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> LastLash()
        {
            //Add level check so it doesn't hang here
            if (Core.Me.ClassLevel < Spells.LastLash.LevelAcquired)
                return false;

            if (!ViperSettings.Instance.UseLastLash)
                return false;

            return await Spells.LastLash.Cast(Core.Me);
        }

        public static async Task<bool> TwinBiteCombo()
        {
            if (Core.Me.ClassLevel < 75)
                return false;

            if (Core.Me.HasAura(Auras.HunterVenom, true))
                return await Spells.TwinfangBite.Cast(Core.Me.CurrentTarget);

            if (Core.Me.HasAura(Auras.SwiftskinVenom, true))
                return await Spells.TwinbloodBite.Cast(Core.Me.CurrentTarget);

            return false;
        }

        public static async Task<bool> TwinThreshCombo()
        {
            if (Core.Me.ClassLevel < 75)
                return false;

            if (Core.Me.HasAura(Auras.FellhunterVenom, true))
                return await Spells.TwinfangThresh.Cast(Core.Me);

            if (Core.Me.HasAura(Auras.FellskinVenom, true))
                return await Spells.TwinbloodThresh.Cast(Core.Me);

            return false;
        }

        public static async Task<bool> SerpentIre()
        {
            if (Core.Me.ClassLevel < 86)
                return false;

            if (!ViperSettings.Instance.UseSerpentIre)
                return false;

            if (!Core.Me.HasAura(Auras.HunterInstinct, true))
                return false;

            if (!Core.Me.HasAura(Auras.Swiftscaled, true))
                return false;

            return await Spells.SerpentIre.Cast(Core.Me);

        }

        public static async Task<bool> FirstLegacy()
        {
            if (Core.Me.ClassLevel < 100)
                return false;

            if (!Spells.FirstLegacy.CanCast())
                return false;

            return await Spells.FirstLegacy.Cast(Core.Me.CurrentTarget);

        }

        public static async Task<bool> SecondLegacy()
        {
            if (Core.Me.ClassLevel < 100)
                return false;

            if (!Spells.SecondLegacy.CanCast())
                return false;

            return await Spells.SecondLegacy.Cast(Core.Me.CurrentTarget);

        }

        public static async Task<bool> ThirdLegacy()
        {
            if (Core.Me.ClassLevel < 100)
                return false;

            if (!Spells.ThirdLegacy.CanCast())
                return false;

            return await Spells.ThirdLegacy.Cast(Core.Me.CurrentTarget);

        }

        public static async Task<bool> FourthLegacy()
        {
            if (Core.Me.ClassLevel < 100)
                return false;

            if (!Spells.FourthLegacy.CanCast())
                return false;

            return await Spells.FourthLegacy.Cast(Core.Me.CurrentTarget);

        }

    }
}