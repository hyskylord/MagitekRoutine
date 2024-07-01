using ff14bot;
using ff14bot.Managers;
using Magitek.Extensions;
using Magitek.Logic;
using Magitek.Logic.Viper;
using Magitek.Logic.Roles;
using Magitek.Models.Account;
using Magitek.Models.Viper;
using Magitek.Utilities;
using Magitek.Utilities.CombatMessages;
using ViperRoutine = Magitek.Utilities.Routines.Viper;
using System.Threading.Tasks;
using ff14bot.Enums;

namespace Magitek.Rotations
{
    public static class Viper
    {
        public static Task<bool> Rest()
        {
            var needRest = Core.Me.CurrentHealthPercent < ViperSettings.Instance.RestHealthPercent;
            return Task.FromResult(needRest);
        }

        public static async Task<bool> PreCombatBuff()
        {
            if (Core.Me.IsCasting)
                return true;

            if (await Casting.TrackSpellCast())
                return true;

            await Casting.CheckForSuccessfulCast();

            //Openers.OpenerCheck();

            if (WorldManager.InSanctuary)
                return false;

            if (Core.Me.HasTarget && Core.Me.CurrentTarget.CanAttack)
                return false;


            if (Globals.OnPvpMap)
                return false;


            return false;

        }

        public static async Task<bool> Pull()
        {

            if (BotManager.Current.IsAutonomous)
            {
                if (Core.Me.HasTarget)
                {
                    Movement.NavigateToUnitLos(Core.Me.CurrentTarget, Core.Me.CurrentTarget.CombatReach);
                }
            }

            if (await Casting.TrackSpellCast())
                return true;

            await Casting.CheckForSuccessfulCast();

            return await Combat();
        }

        public static async Task<bool> Heal()
        {
            return await GambitLogic.Gambit();
        }

        public static Task<bool> CombatBuff()
        {
            return Task.FromResult(false);
        }

        public static async Task<bool> Combat()
        {
            if (BaseSettings.Instance.ActivePvpCombatRoutine)
                return await PvP();

            if (Core.Me.IsCasting)
                return true;

            if (await Casting.TrackSpellCast())
                return true;

            await Casting.CheckForSuccessfulCast();

            ViperRoutine.RefreshVars();

            if (await CustomOpenerLogic.Opener()) return true;


            if (BotManager.Current.IsAutonomous)
            {
                if (Core.Me.HasTarget)
                {
                    Movement.NavigateToUnitLos(Core.Me.CurrentTarget, 2 + Core.Me.CurrentTarget.CombatReach);
                }
            }

            if (SingleTarget.ForceLimitBreak()) return true;


                if (ViperRoutine.GlobalCooldown.CanWeave())
                {
                    if (await PhysicalDps.Interrupt(ViperSettings.Instance)) return true;
                    if (await PhysicalDps.SecondWind(ViperSettings.Instance)) return true;
                    if (await PhysicalDps.Bloodbath(ViperSettings.Instance)) return true;

                    //if (await Utility.TrueNorth()) return true;
                    if (await SingleTarget.Slither()) return true;
                }

                if (await SingleTarget.FourthGeneration()) return true;
                if (await SingleTarget.ThirdGeneration()) return true;
                if (await SingleTarget.SecondGeneration()) return true;
                if (await SingleTarget.FirstGeneration()) return true;

                if (await SingleTarget.Reawaken()) return true;
                if (await Cooldown.SerpentIre()) return true;
                if (await SingleTarget.UncoiledFury()) return true;


                if (await Cooldown.TwinThreshCombo()) return true;
                if (await AoE.HunterOrSwiftskinDen()) return true;
                if (await AoE.PitOfDread()) return true;

                if (await Cooldown.TwinBiteCombo()) return true;
                if (await SingleTarget.HunterOrSwiftskinCoil()) return true;
                if (await SingleTarget.Dreadwinder()) return true;

                if (await Cooldown.LastLash()) return true;
                if (await AoE.JaggedOrBloodiedMaw()) return true;
                if (await AoE.HunterOrSwiftSkinBite()) return true;
                if (await AoE.SteelDreadMaw()) return true;

                if (await Cooldown.DeathRattle()) return true;
                if (await SingleTarget.FankstingOrFlankbane()) return true;
                if (await SingleTarget.HunterOrSwiftSkinSting()) return true;
                return await SingleTarget.SteelOrDreadFangs();
     
            
        }

        public static async Task<bool> PvP()
        {
            if (!BaseSettings.Instance.ActivePvpCombatRoutine)
                return await Combat();

            return false;
        }

        public static void RegisterCombatMessages()
        {
            //Highest priority: Don't show anything if we're not in combat
            CombatMessageManager.RegisterMessageStrategy(
                new CombatMessageStrategy(100,
                                          "",
                                          () => !Core.Me.InCombat || !Core.Me.HasTarget)
                );

            //Second priority: Don't show anything if positional requirements are Nulled
            CombatMessageManager.RegisterMessageStrategy(
                new CombatMessageStrategy(200,
                                          "",
                                          () => ViperSettings.Instance.HidePositionalMessage || Core.Me.HasAura(Auras.TrueNorth) || ViperSettings.Instance.EnemyIsOmni || Core.Me.HasAura(Auras.Enshrouded))
                );

        }
    }
}


