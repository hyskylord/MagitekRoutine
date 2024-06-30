using ff14bot;
using ff14bot.Managers;
using Magitek.Enumerations;
using Magitek.Extensions;
using Magitek.Gambits.Conditions;
using Magitek.Logic.Roles;
using Magitek.Models.Reaper;
using Magitek.Models.Samurai;
using Magitek.Models.Viper;
using Magitek.Utilities;
using System.Linq;
using System.Threading.Tasks;

namespace Magitek.Logic.Viper
{
    internal static class SingleTarget
    {

        public static async Task<bool> SteelOrDreadFangs()
        {
            if (Core.Me.ClassLevel >= 10 && Spells.DreadFangs.CanCast() && !Core.Me.CurrentTarget.HasAura(Auras.NoxiousGnash, true, 4000))
                return await Spells.DreadFangs.Cast(Core.Me.CurrentTarget);
            else
                return await Spells.SteelFangs.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> HunterOrSwiftSkinSting()
        {
            if (Core.Me.ClassLevel < 5)
                return false;

            if (Core.Me.ClassLevel < 20)
                return await Spells.HunterSting.Cast(Core.Me.CurrentTarget);

            if (Core.Me.HasAura(Auras.HindstungVenom, true) || Core.Me.HasAura(Auras.HindsbaneVenom, true))
                return await Spells.SwiftskinSting.Cast(Core.Me.CurrentTarget);

            if (Core.Me.HasAura(Auras.FlankstungVenom, true) || Core.Me.HasAura(Auras.FlanksbaneVenom, true))
                return await Spells.HunterSting.Cast(Core.Me.CurrentTarget);

            return await Spells.HunterSting.Cast(Core.Me.CurrentTarget);

        }

        public static async Task<bool> FankstingOrFlankbane()
        {

            if (Core.Me.ClassLevel < 30)
                return false;

            if ( Core.Me.HasAura(Auras.FlankstungVenom, true))
                return await Spells.FankstingStrike.Cast(Core.Me.CurrentTarget);

            if (Core.Me.HasAura(Auras.FlanksbaneVenom, true))
                return await Spells.FanksbaneFang.Cast(Core.Me.CurrentTarget);

            if (Core.Me.HasAura(Auras.HindstungVenom, true))
                return await Spells.HindstingStrike.Cast(Core.Me.CurrentTarget);

            if (Core.Me.HasAura(Auras.HindsbaneVenom, true))
                return await Spells.HindsbaneFang.Cast(Core.Me.CurrentTarget);

            else
                return await Spells.FankstingStrike.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> Dreadwinder()
        {
            if (Core.Me.ClassLevel < 65)
                return false;
            
            if (!ViperSettings.Instance.UseDreadwinder)
                return false;

            return await Spells.Dreadwinder.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> HunterOrSwiftskinCoil()
        {
            if (Core.Me.ClassLevel < 65)
                return false;

            if (Spells.HunterCoil.CanCast())
                return await Spells.HunterCoil.Cast(Core.Me.CurrentTarget);
            else
                return await Spells.SwiftskinCoil.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> Slither()
        {
            if (Core.Me.ClassLevel < 15)
                return false;

            if (MovementManager.IsMoving)
                return false;

            if (!ViperSettings.Instance.UseSlither)
                return false;

            return await Spells.Slither.Cast(Core.Me.CurrentTarget);
        }

        /**********************************************************************************************
        *                              Limit Break
        * ********************************************************************************************/
        public static bool ForceLimitBreak()
        {
            if (!Core.Me.HasTarget)
                return false;

            return PhysicalDps.ForceLimitBreak(Spells.Braver, Spells.Bladedance, Spells.TheEnd, Spells.Slice);
        }

    }
}
