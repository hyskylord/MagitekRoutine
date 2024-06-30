using ff14bot;
using ff14bot.Managers;
using Magitek.Extensions;
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

    }
}