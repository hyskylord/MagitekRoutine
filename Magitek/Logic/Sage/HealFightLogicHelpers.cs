using ff14bot;
using Magitek.Extensions;
using Magitek.Logic.Sage;
using Magitek.Models.Account;
using Magitek.Models.Sage;
using Magitek.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using static ff14bot.Managers.ActionResourceManager.Sage;
using Auras = Magitek.Utilities.Auras;

internal static class HealFightLogicHelpers
{
    public static async Task<bool> Aoe()
    {
        if (!Globals.InParty)
            return false;

        if (!FightLogic.ZoneHasFightLogic())
            return false;

        if (!FightLogic.EnemyIsCastingBigAoe() && !FightLogic.EnemyIsCastingAoe())
            return false;

        var useAoEBuffs = Heal.UseAoEHealingBuff(Group.CastableAlliesWithin15);

        if (SageSettings.Instance.FightLogic_Kerachole
            && Spells.Kerachole.IsKnownAndReady()
            && Addersgall >= 1
            && useAoEBuffs)
        {
            var targets = Group.CastableAlliesWithin15.Where(r => !r.HasAura(Auras.Kerachole) && !r.HasAura(Auras.Taurochole));
            var tankCheck = !SageSettings.Instance.FightLogic_RespectOnlyTank
                || !SageSettings.Instance.KeracholeOnlyWithTank
                || targets.Any(r => r.IsTank(SageSettings.Instance.KeracholeOnlyWithMainTank));

            if (targets.Count() >= Heal.AoeNeedHealing &&
                tankCheck)
            {
                if (BaseSettings.Instance.DebugFightLogic)
                    Logger.WriteInfo($"[AOE Response] Cast Kerachole");
                return await FightLogic.DoAndBuffer(Spells.Kerachole.CastAura(Core.Me, Auras.Kerachole));
            }
        }

        if (SageSettings.Instance.FightLogic_Panhaima
            && Spells.Panhaima.IsKnownAndReady()
            && useAoEBuffs)
        {
            //Radius is now 30y
            var targets = Group.CastableAlliesWithin30.Where(r => !r.HasAura(Auras.Panhaimatinon));
            var tankCheck = !SageSettings.Instance.FightLogic_RespectOnlyTank
                || !SageSettings.Instance.PanhaimaOnlyWithTank
                || targets.Any(r => r.IsTank(SageSettings.Instance.PanhaimaOnlyWithMainTank));

            if (targets.Count() >= Heal.AoeNeedHealing
                && tankCheck)
            {
                if (BaseSettings.Instance.DebugFightLogic)
                    Logger.WriteInfo($"[AOE Response] Cast Panhaima");
                return await FightLogic.DoAndBuffer(Spells.Panhaima.CastAura(Core.Me, Auras.Panhaimatinon));
            }
        }

        if (SageSettings.Instance.FightLogic_Holos
            && Spells.Holos.IsKnownAndReady()
            && useAoEBuffs)
        {
            //Radius is now 30y
            var targets = Group.CastableAlliesWithin30.Where(r => !r.HasAura(Auras.Holos));
            var tankCheck = !SageSettings.Instance.FightLogic_RespectOnlyTank
                || !SageSettings.Instance.HolosTankOnly
                || targets.Any(r => r.IsTank(SageSettings.Instance.HolosMainTankOnly));

            if (targets.Count() >= Heal.AoeNeedHealing
                && tankCheck)
            {
                if (BaseSettings.Instance.DebugFightLogic)
                    Logger.WriteInfo($"[AOE Response] Cast Holos");
                return await FightLogic.DoAndBuffer(Spells.Holos.CastAura(Core.Me, Auras.Holos));
            }
        }

        if (SageSettings.Instance.FightLogic_EukrasianPrognosis
            && Core.Me.ClassLevel >= Spells.Eukrasia.LevelAcquired
            && Heal.IsEukrasiaReady())
        {
            var targets = Group.CastableAlliesWithin15.Where(r => !r.HasAura(Auras.EukrasianDiagnosis)
                                                            && !r.HasAura(Auras.EukrasianPrognosis)
                                                            && !r.HasAura(Auras.Galvanize));
            var tankCheck = !SageSettings.Instance.FightLogic_RespectOnlyTank
                || targets.Any(r => r.IsTank());

            if (targets.Count() >= Heal.AoeNeedHealing
                && tankCheck)
            {
                if (BaseSettings.Instance.DebugFightLogic)
                    Logger.WriteInfo($"[AOE Response] Cast Eukrasian Prognosis");
                if (await Heal.UseEukrasia(Spells.EukrasianPrognosis.Id))
                    return await FightLogic.DoAndBuffer(Spells.EukrasianPrognosis.HealAura(Core.Me, Auras.EukrasianPrognosis));
            }
        }

        if (SageSettings.Instance.FightLogic_Philosophia
           && Spells.Philosophia.IsKnownAndReady())
            return await FightLogic.DoAndBuffer(Spells.Philosophia.HealAura(Core.Me, Auras.Philosophia));

        return false;
    }
}