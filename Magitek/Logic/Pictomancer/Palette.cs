using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using Magitek.Extensions;
using Magitek.Models.Pictomancer;
using Magitek.Models.Reaper;
using Magitek.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeSharp;
using Auras = Magitek.Utilities.Auras;

namespace Magitek.Logic.Pictomancer
{
    internal static class Palette
    {
        // creature -> pom/wing muse -> pom of ages
        // living muse
        // weapon -> hammer stamp
        // steel muse
        // landscape -> starry muse
        // scenic muse

        public static async Task<bool> PrePaintPalettes()
        {
            // creatures
            if (Spells.CreatureMotif.IsKnownAndReady() && Spells.CreatureMotif.CanCast())
                return await Spells.CreatureMotif.Cast(Core.Me);

            // hammers
            if (Spells.WeaponMotif.IsKnownAndReady() && Spells.WeaponMotif.CanCast())
                return await Spells.WeaponMotif.Cast(Core.Me);

            // stars
            if (Spells.LandscapeMotif.IsKnownAndReady() && Spells.LandscapeMotif.CanCast())
                return await Spells.LandscapeMotif.Cast(Core.Me);

            return false;
        }

        public static async Task<bool> SwitfcastMotif()
        {
            if (!Core.Me.InCombat)
                return false;

            if (!Spells.Swiftcast.IsKnownAndReady())
                return false;

            if (Spells.StarryMuse.IsKnown())
                if (Core.Me.HasAura(Auras.StarryMuse) || Spells.Swiftcast.AdjustedCooldown <= Spells.ScenicMuse.Cooldown)
                    return await Roles.Healer.Swiftcast();
            else
                return await Roles.Healer.Swiftcast();

            return false;
        }

        public static bool MotifCanCast(SpellData motif, SpellData muse)
        {
            //if ((muse.Cooldown - muse.AdjustedCooldown).TotalMilliseconds > (motif.AdjustedCastTime.TotalMilliseconds + 500))
            //    return false;
            //if (muse.Charges < 1)
            //    return false;

            // hack since muse.Cooldown doesn't seem to be accurate, as it's doubling 
            // the Cooldown time for some reason with a single cast of the Muse.
            // The same information can be derived from Charges, so I used that instead.
            var castTime = motif.AdjustedCastTime.TotalMilliseconds;
            var precastCooldown = (castTime + 750) / muse.AdjustedCooldown.TotalMilliseconds;
            var precastThreshold = 1 - precastCooldown;
            if (muse.Charges < precastThreshold) return false;
            return true;
        }

        public static async Task<bool> CreatureMotif()
        {
            if (!MotifCanCast(Spells.CreatureMotif, Spells.LivingMuse))
                return false;

            await SwitfcastMotif();

            // PomMotif -> WingMotif -> ClawMotif -> MawMotif
            if (Spells.CreatureMotif.IsKnownAndReady() && Spells.CreatureMotif.CanCast())
                return await Spells.CreatureMotif.Cast(Core.Me);

            return false;
        }

        public static async Task<bool> CreatureMuse()
        {
            if (Spells.LivingMuse.IsKnown() && Spells.LivingMuse.CanCast(Core.Me.CurrentTarget))
                return await Spells.LivingMuse.Cast(Core.Me.CurrentTarget);

            //if (Spells.FangedMuse.IsKnown() && Spells.FangedMuse.CanCast(Core.Me.CurrentTarget))
            //    return await Spells.FangedMuse.Cast(Core.Me.CurrentTarget);

            //if (Spells.ClawedMuse.IsKnown() && Spells.ClawedMuse.CanCast(Core.Me.CurrentTarget))
            //    return await Spells.ClawedMuse.Cast(Core.Me.CurrentTarget);

            //if (Spells.WingedMuse.IsKnown() && Spells.WingedMuse.CanCast(Core.Me.CurrentTarget))
            //    return await Spells.WingedMuse.Cast(Core.Me.CurrentTarget);

            //if (Spells.PomMuse.IsKnown() && Spells.PomMuse.CanCast(Core.Me.CurrentTarget))
            //    return await Spells.PomMuse.Cast(Core.Me.CurrentTarget);

            return false;
        }

        public static async Task<bool> MogoftheAges()
        {
            if (Spells.RetributionoftheMadeen.IsKnownAndReady() && Spells.RetributionoftheMadeen.CanCast())
                return await Spells.RetributionoftheMadeen.Cast(Core.Me.CurrentTarget);

            if (Spells.MogoftheAges.IsKnownAndReady() && Spells.MogoftheAges.CanCast())
                return await Spells.MogoftheAges.Cast(Core.Me.CurrentTarget);

            return false;
        }

        public static async Task<bool> WeaponMotif()
        {
            if (!MotifCanCast(Spells.WeaponMotif, Spells.SteelMuse))
                return false;

            await SwitfcastMotif();

            if (Spells.WeaponMotif.IsKnownAndReady() && Spells.WeaponMotif.CanCast())
                return await Spells.WeaponMotif.Cast(Core.Me);

            return false;
        }

        public static async Task<bool> StrikingMuse()
        {
            if (Spells.StrikingMuse.IsKnown() && Spells.StrikingMuse.CanCast(Core.Me.CurrentTarget))
                return await Spells.StrikingMuse.Cast(Core.Me.CurrentTarget);

            return false;
        }

        public static async Task<bool> HammerStamp()
        {
            //if (Spells.PolishingHammer.IsKnown() && Spells.PolishingHammer.CanCast(Core.Me.CurrentTarget))
            //    return await Spells.PolishingHammer.Cast(Core.Me.CurrentTarget);

            //if (Spells.HammerBrush.IsKnown() && Spells.HammerBrush.CanCast(Core.Me.CurrentTarget))
            //    return await Spells.HammerBrush.Cast(Core.Me.CurrentTarget);

            if (Spells.HammerStamp.IsKnown() && Spells.HammerStamp.CanCast(Core.Me.CurrentTarget))
                return await Spells.HammerStamp.Cast(Core.Me.CurrentTarget);

            return false;
        }

        public static async Task<bool> LandscapeMotif()
        {
            if (!PictomancerSettings.Instance.UseStarrySky)
                return false;

            if (!MotifCanCast(Spells.LandscapeMotif, Spells.ScenicMuse))
                return false;

            if (Spells.LandscapeMotif.IsKnownAndReady() && Spells.LandscapeMotif.CanCast())
                return await Spells.LandscapeMotif.Cast(Core.Me);

            return false;
        }

        public static async Task<bool> ScenicMuse()
        {
            if (!PictomancerSettings.Instance.UseStarrySky)
                return false;

            if (Spells.ScenicMuse.IsKnownAndReady() && Spells.ScenicMuse.CanCast(Core.Me)){
                if (Globals.InParty)
                {
                    var couldStar = Group.CastableAlliesWithin30.Count(r => !r.HasAura(Auras.StarryMuse));
                    var starNeededCount = PictomancerSettings.Instance.StarrySkyCount;

                    if (PictomancerSettings.Instance.StarrySkyEntireParty)
                        starNeededCount = Group.CastableParty.Count();

                    if (couldStar >= starNeededCount)
                        return await Spells.ScenicMuse.Cast(Core.Me);
                    else
                        return false;
                }

                return await Spells.ScenicMuse.Cast(Core.Me);
            }

            return false;
        }
        public static async Task<bool> RainbowDrip()
        {
            if (!Spells.RainbowDrip.IsKnownAndReady())
                return false;

            if (!Spells.RainbowDrip.CanCast(Core.Me.CurrentTarget))
                return false;

            if (!Core.Me.HasAura(Auras.RainbowBright))
                if (Spells.Swiftcast.IsKnownAndReady())
                    if (!await Roles.Healer.Swiftcast())
                        return false;

            return await Spells.RainbowDrip.Cast(Core.Me.CurrentTarget);
        }

        public static async Task<bool> StarPrism()
        {
            if (!Spells.StarPrism.IsKnownAndReady())
                return false;

            if (!Spells.StarPrism.CanCast(Core.Me.CurrentTarget))
                return false;

            if (!Core.Me.HasAura(Auras.Starstruck))
                return false;

            return await Spells.StarPrism.Cast(Core.Me.CurrentTarget);
        }
    }
}
