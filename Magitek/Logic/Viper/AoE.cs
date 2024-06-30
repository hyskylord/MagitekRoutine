using ff14bot;
using ff14bot.Managers;
using Magitek.Enumerations;
using Magitek.Extensions;
using Magitek.Models.Viper;
using ViperRoutine = Magitek.Utilities.Routines.Viper;
using Magitek.Utilities;
using System.Linq;
using System.Threading.Tasks;

namespace Magitek.Logic.Viper
{
    internal static class AoE
    {
        public static async Task<bool> SteelDreadMaw()
        {
            if (Core.Me.ClassLevel < 25)
                return false;

            if (!ViperSettings.Instance.UseAoe)
                return false;

            if (ViperRoutine.EnemiesAroundPlayer5Yards < ViperSettings.Instance.AoeEnemies)
                return false;

            if (Core.Me.ClassLevel >=35 && Spells.DreadMaw.CanCast() && !Core.Me.CurrentTarget.HasAura(Auras.NoxiousGnash, true))
                return await Spells.DreadMaw.Cast(Core.Me);
            else
                return await Spells.SteelMaw.Cast(Core.Me);
        }

        public static async Task<bool> HunterOrSwiftSkinBite()
        {
            if (Core.Me.ClassLevel < 40)
                return false;

            if (Core.Me.ClassLevel < 45)
                return await Spells.HunterBite.Cast(Core.Me.CurrentTarget);

            if (Core.Me.HasAura(Auras.HindstungVenom, true) || Core.Me.HasAura(Auras.GrimskinVenom, true))
                return await Spells.SwiftskinBite.Cast(Core.Me.CurrentTarget);

            if (Core.Me.HasAura(Auras.FlankstungVenom, true) || Core.Me.HasAura(Auras.GrimhunterVenom, true))
                return await Spells.HunterBite.Cast(Core.Me.CurrentTarget);

            return await Spells.HunterBite.Cast(Core.Me.CurrentTarget);

        }

        public static async Task<bool> JaggedOrBloodiedMaw()
        {
            if (Core.Me.ClassLevel < 50)
                return false;

            if (Core.Me.HasAura(Auras.GrimhunterVenom, true))
                return await Spells.JaggedMaw.Cast(Core.Me.CurrentTarget);

            if (Core.Me.HasAura(Auras.GrimskinVenom, true))
                return await Spells.BloodiedMaw.Cast(Core.Me.CurrentTarget);

            else
                return await Spells.JaggedMaw.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> PitOfDread()
        {
            if (Core.Me.ClassLevel < 70)
                return false;

            if (!ViperSettings.Instance.UseAoe)
                return false;

            if (ViperRoutine.EnemiesAroundPlayer5Yards < ViperSettings.Instance.AoeEnemies)
                return false;

            if (!ViperSettings.Instance.UsePitOfDread)
                return false;

            return await Spells.PitOfDread.Cast(Core.Me);
        }

        public static async Task<bool> HunterOrSwiftskinDen()
        {
            if (Core.Me.ClassLevel < 70)
                return false;

            if (Spells.HunterDen.CanCast())
                return await Spells.HunterDen.Cast(Core.Me);
            else
                return await Spells.SwiftskinDen.Cast(Core.Me);
        }
    }
}