using ff14bot;
using ff14bot.Managers;
using Magitek.Extensions;
using Magitek.Models.Pictomancer;
using Magitek.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magitek.Logic.Pictomancer
{
    internal static class SingleTarget
    {
        public static async Task<bool> Paint()
        {
            if (Core.Me.HasAura(Auras.SubtractivePalette))
            {
                if (Core.Me.HasAura(Auras.Aetherhues2))
                    if (Spells.ThunderinMagenta.IsKnownAndReady())
                        return await Spells.ThunderinMagenta.Cast(Core.Me.CurrentTarget);
                if (Core.Me.HasAura(Auras.Aetherhues))
                    if (Spells.StoneinYellow.IsKnownAndReady())
                        return await Spells.StoneinYellow.Cast(Core.Me.CurrentTarget);
                if (Spells.BlizzardinCyan.IsKnownAndReady())
                    return await Spells.BlizzardinCyan.Cast(Core.Me.CurrentTarget);
            }
            else
            {
                if (Core.Me.HasAura(Auras.Aetherhues2))
                    if (Spells.WaterinBlue.IsKnownAndReady())
                        return await Spells.WaterinBlue.Cast(Core.Me.CurrentTarget);
                if (Core.Me.HasAura(Auras.Aetherhues))
                    if (Spells.AeroinGreen.IsKnownAndReady())
                        return await Spells.AeroinGreen.Cast(Core.Me.CurrentTarget);
                if (Spells.FireinRed.IsKnownAndReady())
                    return await Spells.FireinRed.Cast(Core.Me.CurrentTarget);
            }

            return false;
        }

        public static async Task<bool> HolyinWhite()
        {
            if (!PictomancerSettings.Instance.UseHolyinWhiteMoving)
                return false;

            if (!Spells.HolyinWhite.IsKnownAndReady())
                return false;

            if (!Spells.HolyinWhite.CanCast(Core.Me.CurrentTarget))
                return false;

            // check # of white paint when ActionResourceManager.Pictomancer is available
            // if capped on white paint, cast HolyinWhite
            // if only have 1 white paint, save it for black paint
            if (!MovementManager.IsMoving)
                return false;

            return await Spells.HolyinWhite.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> CometinBlack()
        {
            if (!Spells.CometinBlack.IsKnownAndReady())
                return false;

            if (!Spells.CometinBlack.CanCast(Core.Me.CurrentTarget))
                return false;

            // Save comet for Hyperphantasia/StarrySky only when single target

            return await Spells.CometinBlack.Cast(Core.Me.CurrentTarget);
        }
    }
}
