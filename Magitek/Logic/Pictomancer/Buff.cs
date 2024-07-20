using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using Magitek.Extensions;
using Magitek.Models.Pictomancer;
using Magitek.Utilities;
using Magitek.Utilities.Routines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magitek.Logic.Pictomancer
{
    internal static class Buff
    {
        public static async Task<bool> Swiftcast()
        {
            if (await Spells.Swiftcast.CastAura(Core.Me, Auras.Swiftcast))
            {
                return await Coroutine.Wait(15000, () => Core.Me.HasAura(Auras.Swiftcast, true, 7000));
            }

            return false;
        }

        public static async Task<bool> SubtractivePalette()
        {
            if (!PictomancerSettings.Instance.UseSubtractivePalette)
                return false;

            if (Utilities.Routines.Pictomancer.HasBlackPaint())
                return false;

            if (Core.Me.HasAura(Auras.SubtractivePalette))
                return false;

            if (!Spells.SubtractivePalette.IsKnownAndReady())
                return false;

            if (Core.Me.HasAura(Auras.MonochromeTones))
                return false;

            return await Spells.SubtractivePalette.Cast(Core.Me);
        }

        public static async Task<bool> FightLogic_TemperaCoat()
        {
            if (!PictomancerSettings.Instance.FightLogicTemperaCoat)
                return false;

            if (!Spells.TemperaCoat.IsKnownAndReady())
                return false;

            if (!FightLogic.ZoneHasFightLogic() || !FightLogic.EnemyHasAnyAoeLogic())
                return false;

            if (FightLogic.EnemyIsCastingAoe() || FightLogic.EnemyIsCastingBigAoe())
            {
                // intentionally don't wait for the grassa aura, it either happens or it doesn't. 
                return await FightLogic.DoAndBuffer(Spells.TemperaCoat.Cast(Core.Me));             
            }
            return false;
        }

        public static async Task<bool> FightLogic_TemperaGrassa()
        {
            if (!PictomancerSettings.Instance.FightLogicTemperaCoat)
                return false;

            if (!Spells.TemperaGrassa.IsKnownAndReady())
                return false;

            if (!Core.Me.HasAura(Auras.TempuraCoat))
                return false;

            if (!Globals.InParty)
                return false;
               
            // intentionally don't wait for the grassa aura, it either happens or it doesn't. 
            return await Spells.TemperaGrassa.Cast(Core.Me);
        }

        public static async Task<bool> FightLogic_Addle()
        {
            if (!PictomancerSettings.Instance.FightLogicAddle)
                return false;

            if (!Spells.Addle.IsKnownAndReady())
                return false;

            if (!FightLogic.ZoneHasFightLogic())
                return false;

            if (FightLogic.EnemyIsCastingAoe() || FightLogic.EnemyIsCastingBigAoe() || FightLogic.EnemyIsCastingTankBuster() != null)
            {
                return await FightLogic.DoAndBuffer(Spells.Addle.Cast(Core.Me.CurrentTarget));
            }

            return false;
        }
    }
}
