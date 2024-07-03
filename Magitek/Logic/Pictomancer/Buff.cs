using Buddy.Coroutines;
using ff14bot;
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
            if (Core.Me.HasAura(Auras.SubtractivePalette))
                return false;

            if (Spells.SubtractivePalette.IsKnownAndReady())
                return await Spells.SubtractivePalette.Cast(Core.Me);

            return false;
        }

        public static async Task<bool> FightLogic_TemperaCoat()
        {
            if (!Spells.TemperaCoat.IsKnownAndReady())
                return false;

            if (!FightLogic.ZoneHasFightLogic() || !FightLogic.EnemyHasAnyAoeLogic())
                return false;

            if (FightLogic.EnemyIsCastingAoe() || FightLogic.EnemyIsCastingBigAoe())
            {
                if (Spells.TemperaGrassa.IsKnownAndReady())
                {
                    await Spells.TemperaCoat.CastAura(Core.Me, Auras.TempuraCoat);
                    if (await Coroutine.Wait(2500, () => Core.Me.HasAura(Auras.TempuraCoat, true)))
                        return await FightLogic.DoAndBuffer(Spells.TemperaGrassa.CastAura(Core.Me, Auras.TempuraGrassa));
                    else
                        return false;
                } else
                {
                    return await FightLogic.DoAndBuffer(Spells.TemperaCoat.CastAura(Core.Me, Auras.TempuraCoat));
                }                
            }
            return false;
        }
    }
}
