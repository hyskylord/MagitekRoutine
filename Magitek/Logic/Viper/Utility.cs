using ff14bot;
using ff14bot.Managers;
using Magitek.Extensions;
using Magitek.Models.Viper;
using Magitek.Utilities;
using System.Threading.Tasks;

namespace Magitek.Logic.Viper
{
    internal static class Utility
    {

        public static async Task<bool> TrueNorth()
        {
            if (ViperSettings.Instance.EnemyIsOmni || !ViperSettings.Instance.UseTrueNorth) return false;

            if (Core.Me.HasAura(Auras.TrueNorth))
                return false;

            if (Core.Me.CurrentTarget.IsBehind)
                return false;

            return await Spells.TrueNorth.CastAura(Core.Me, Auras.TrueNorth);
 
        }

    }
}