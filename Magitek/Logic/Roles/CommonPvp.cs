using Buddy.Coroutines;
using ff14bot;
using Magitek.Extensions;
using Magitek.Models.Roles;
using Magitek.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auras = Magitek.Utilities.Auras;

namespace Magitek.Logic.Roles
{
    internal class CommonPvp
    {

        public static async Task<bool> Guard<T>(T settings) where T : JobSettings
        {
            if (!settings.Pvp_UseGuard)
                return false;

            if (!Spells.Guard.CanCast())
                return false;

            if (Core.Me.HasAura(Auras.Guard))
                return false;

            if (Core.Me.CurrentHealthPercent > settings.Pvp_GuardHealthPercent)
                return false;

            if (!await Spells.Guard.CastAura(Core.Me, Auras.Guard))
                return false;

            return await Coroutine.Wait(1500, () => Core.Me.HasAura(Auras.Guard, true));
        }

        public static bool GuardCheck<T>(T settings) where T : JobSettings
        {
            return (settings.Pvp_GuardCheck && Core.Me.CurrentTarget.HasAura(Auras.Guard))
                || (settings.Pvp_InvulnCheck && Core.Me.CurrentTarget.HasAnyAura(new uint[] {Auras.PvpHallowedGround, Auras.PvpUndeadRedemption}));
        }

        public static async Task<bool> Purify<T>(T settings) where T : JobSettings
        {
            if (!settings.Pvp_UsePurify)
                return false;

            if (!Spells.Purify.CanCast())
                return false;

            if (Core.Me.HasAura(Auras.Guard))
                return false;

            if (!Core.Me.HasAura("Stun") && !Core.Me.HasAura("Heavy") && !Core.Me.HasAura("Bind") && !Core.Me.HasAura("Silence") && !Core.Me.HasAura("Half-asleep") && !Core.Me.HasAura("Sleep") && !Core.Me.HasAura("Deep Freeze"))
                return false;

            return await Spells.Purify.Cast(Core.Me);
        }


        public static async Task<bool> Recuperate<T>(T settings) where T : JobSettings
        {
            if (!settings.Pvp_UseRecuperate)
                return false;

            if (!Spells.Recuperate.CanCast())
                return false;

            if (Core.Me.HasAura(Auras.Guard))
                return false;

            if (Core.Me.CurrentHealthPercent > settings.Pvp_RecuperateHealthPercent)
                return false;

            return await Spells.Recuperate.Cast(Core.Me);
        }
    }
}
