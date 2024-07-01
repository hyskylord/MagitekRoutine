using ff14bot;
using ff14bot.Managers;
using Magitek.Extensions;
using Magitek.Logic;
using Magitek.Logic.Roles;
using Magitek.Logic.Pictomancer;
using Magitek.Models.Account;
using Magitek.Models.Pictomancer;
using Magitek.Utilities;
using System.Threading.Tasks;

namespace Magitek.Rotations
{
    public static class Pictomancer
    {
        public static async Task<bool> Rest()
        {
            if (Core.Me.CurrentHealthPercent > 70 || Core.Me.ClassLevel < 4)
                return false;

            if (WorldManager.InSanctuary)
                return false;

            return false;
        }

        public static async Task<bool> PreCombatBuff()
        {
            if (Core.Me.IsCasting)
                return true;

            await Casting.CheckForSuccessfulCast();
            SpellQueueLogic.SpellQueue.Clear();

            if (WorldManager.InSanctuary)
                return false;

            if (await Palette.PrePaintPalettes()) return true;

            return false;
        }

        public static async Task<bool> Pull()
        {
            if (BotManager.Current.IsAutonomous)
            {
                if (Core.Me.HasTarget)
                    Movement.NavigateToUnitLos(Core.Me.CurrentTarget, 20 + Core.Me.CurrentTarget.CombatReach);
            }

            return await Combat();
        }
        public static async Task<bool> Heal()
        {
            if (Core.Me.IsMounted)
                return true;

            if (await Casting.TrackSpellCast()) return true;
            await Casting.CheckForSuccessfulCast();

            Casting.DoHealthChecks = false;

            if (await GambitLogic.Gambit()) return true;
            return false;

        }

        public static Task<bool> CombatBuff()
        {
            return Task.FromResult(false);
        }

        public static async Task<bool> Combat()
        {
            if (BaseSettings.Instance.ActivePvpCombatRoutine)
                return await PvP();

            if (BotManager.Current.IsAutonomous)
            {
                if (Core.Me.HasTarget)
                    Movement.NavigateToUnitLos(Core.Me.CurrentTarget, 20 + Core.Me.CurrentTarget.CombatReach);
            }

            if (!Core.Me.HasTarget || !Core.Me.CurrentTarget.ThoroughCanAttack())
                return false;

            if (await CustomOpenerLogic.Opener())
                return true;

            if (Core.Me.CurrentTarget.HasAura(Auras.MagicResistance))
                return false;

            if (Core.Me.CurrentTarget.HasAnyAura(Auras.Invincibility))
                return false;

            if (await Buff.FightLogic_TemperaCoat()) return true;
            if (await Healer.LucidDreaming(true, 80f)) return true;

            // palettes
            if (await Palette.ScenicMuse()) return true;

            if (await Palette.MogoftheAges()) return true;
            if (await Palette.HammerStamp()) return true;

            if (await Palette.CreatureMuse()) return true;
            if (await Palette.WeaponMuse()) return true;

            if (await Palette.LandscapeMotif()) return true;
            if (await Palette.CreatureMotif()) return true;
            if (await Palette.WeaponMotif()) return true;

            // attacks
            if (await AOE.HolyinWhite()) return true;
            if (await Buff.SubtractivePalette()) return true;
            if (await AOE.Paint()) return true;
            if (await SingleTarget.Paint()) return true;
            if (await SingleTarget.HolyinWhite()) return true;
            return false;
        }

        public static async Task<bool> PvP()
        {
            return false;
        }
    }
}
