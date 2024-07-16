using ff14bot.Managers;
using ff14bot;
using Magitek.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magitek.Extensions;

namespace Magitek.Logic.Pictomancer
{
    internal static class Pvp
    {
        public static async Task<bool> PaintRGB()
        {
            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            var spell = Spells.PaintRGBPvp.Masked();

            if (!spell.CanCast())
                return false;

            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            return await spell.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> PaintWB()
        {
            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            var spell = Spells.PaintWBPvp.Masked();

            if (!spell.CanCast())
                return false;

            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            return await spell.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> CreatureMotif()
        {
            var spell = Spells.CreatureMotifPvp.Masked();

            if (!spell.CanCast())
                return false;

            if (Spells.LivingMusePvp.Masked().Charges < 1 && Core.Me.HasTarget)
                return false;

            return await spell.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> LivingMuse()
        {
            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            var spell = Spells.LivingMusePvp.Masked();

            if (!spell.CanCast())
                return false;

            return await spell.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> MogoftheAges()
        {
            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            var spell = Spells.MogOfTheAgesPvp.Masked();

            if (!spell.CanCast())
                return false;

            return await spell.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> SubtractivePalette()
        {
            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            if (MovementManager.IsMoving && Core.Me.HasAura(Auras.PvpSubtractivePalette) && Spells.ReleaseSubtractivePalettePvp.CanCast())
                return await Spells.ReleaseSubtractivePalettePvp.Cast(Core.Me);

            if (!MovementManager.IsMoving && !Core.Me.HasAura(Auras.PvpSubtractivePalette) && Spells.SubtractivePalettePvp.CanCast())
                return await Spells.SubtractivePalettePvp.Cast(Core.Me);

            return false;
        }

        public static async Task<bool> AdventofChocobastion()
        {
            if (!Core.Me.CurrentTarget.ValidAttackUnit() || !Core.Me.CurrentTarget.InLineOfSight())
                return false;

            var spell = Spells.AdventofChocobastionPvp.Masked();

            if (!spell.CanCast())
                return false;

            if (Core.Me.HasAura(Auras.PvpStarstruck))
                return await spell.Cast(Core.Me.CurrentTarget);
            else
                return await spell.Cast(Core.Me);
        }
    }
}
