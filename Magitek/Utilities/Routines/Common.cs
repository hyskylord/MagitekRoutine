using ff14bot;
using Magitek.Extensions;
using Magitek.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magitek.Utilities.Routines
{
    internal class Common
    {
        public static bool CheckTTDIsEnemyDyingSoon<T>(T settings) where T : JobSettings
        {
            return settings.UseTTD
                && Combat.CurrentTargetCombatTimeLeft < settings.SaveIfEnemyDyingWithin
                && !Core.Me.CurrentTarget.IsBoss();
        }
    }
}
