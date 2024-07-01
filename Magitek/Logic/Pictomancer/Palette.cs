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
            if (DutyManager.InInstance && !Globals.InActiveDuty)
                return false;

            if (Spells.WingMotif.IsKnownAndReady() && Spells.WingMotif.CanCast())
                return await Spells.WingMotif.Cast(Core.Me);

            if (Spells.PomMotif.IsKnownAndReady() && Spells.PomMotif.CanCast())
                return await Spells.PomMotif.Cast(Core.Me);

            if (Spells.HammerMotif.IsKnownAndReady() && Spells.HammerMotif.CanCast())
                return await Spells.HammerMotif.Cast(Core.Me);

            if (Spells.StarrySkyMotif.IsKnownAndReady() && Spells.StarrySkyMotif.CanCast())
                return await Spells.StarrySkyMotif.Cast(Core.Me);

            return false;
        }

        public static async Task<bool> CreatureMotif()
        {
            if (Spells.LivingMuse.Charges < 0.90)
                return false;

            if (Spells.WingMotif.IsKnownAndReady() && Spells.WingMotif.CanCast()) {                                    
                if (Core.Me.HasAura(Auras.StarryMuse))
                    if (Spells.Swiftcast.IsKnownAndReady())
                        await Roles.Healer.Swiftcast();
                    else
                        return false;

                return await Spells.WingMotif.Cast(Core.Me);
            }

            if (Core.Me.HasAura(Auras.StarryMuse))
                return false;

            if (Spells.PomMotif.IsKnownAndReady() && Spells.PomMotif.CanCast())
                return await Spells.PomMotif.Cast(Core.Me);

            return false;
        }

        public static async Task<bool> CreatureMuse()
        {
            if (Spells.WingedMuse.IsKnown() && Spells.WingedMuse.CanCast(Core.Me.CurrentTarget))
                return await Spells.WingedMuse.Cast(Core.Me.CurrentTarget);

            if (Spells.PomMuse.IsKnown() && Spells.PomMuse.CanCast(Core.Me.CurrentTarget))
                return await Spells.PomMuse.Cast(Core.Me.CurrentTarget);

            return false;
        }

        public static async Task<bool> MogoftheAges()
        {
            if (Spells.MogoftheAges.IsKnownAndReady() && Spells.MogoftheAges.CanCast())
                return await Spells.MogoftheAges.Cast(Core.Me.CurrentTarget);

            return false;
        }

        public static async Task<bool> WeaponMotif()
        {
            if (Spells.SteelMuse.Charges < 0.90)
                return false;

            if (Core.Me.HasAura(Auras.StarryMuse))
                return false;

            if (Spells.HammerMotif.IsKnownAndReady() && Spells.HammerMotif.CanCast())
                return await Spells.HammerMotif.Cast(Core.Me);

            return false;
        }

        public static async Task<bool> WeaponMuse()
        {
            if (Spells.StrikingMuse.IsKnown() && Spells.StrikingMuse.CanCast(Core.Me.CurrentTarget))
                return await Spells.StrikingMuse.Cast(Core.Me.CurrentTarget);

            return false;
        }

        public static async Task<bool> HammerStamp()
        {
            // subtractive does more damage than hammer stamp
            if (Core.Me.HasAura(Auras.SubtractivePalette) && Spells.BlizzardinCyan.CanCast(Core.Me.CurrentTarget))
                return false;

            // hyperphantasia does more damage than hammer stamp
            if (Core.Me.HasAura(Auras.Hyperphantasia))
                return false;

            if (Spells.HammerStamp.IsKnown() && Spells.HammerStamp.CanCast(Core.Me.CurrentTarget))
                return await Spells.HammerStamp.Cast(Core.Me.CurrentTarget);

            return false;
        }

        public static async Task<bool> LandscapeMotif()
        {
            if (!PictomancerSettings.Instance.UseStarrySky)
                return false;

            if (Spells.ScenicMuse.Charges < 0.90)
                return false;

            if (Spells.StarrySkyMotif.IsKnownAndReady() && Spells.StarrySkyMotif.CanCast())
                return await Spells.StarrySkyMotif.Cast(Core.Me);

            return false;
        }

        public static async Task<bool> ScenicMuse()
        {
            if (!PictomancerSettings.Instance.UseStarrySky)
                return false;

            if (Spells.StarryMuse.IsKnown() && Spells.StarryMuse.CanCast(Core.Me)){
                if (Globals.InParty)
                {
                    var couldStar = Group.CastableAlliesWithin30.Count(r => !r.HasAura(Auras.StarryMuse));
                    var starNeededCount = PictomancerSettings.Instance.StarrySkyCount;

                    if (PictomancerSettings.Instance.StarrySkyEntireParty)
                        starNeededCount = Group.CastableParty.Count();

                    if (couldStar >= starNeededCount)
                        return await Spells.StarryMuse.Cast(Core.Me);
                    else
                        return false;
                }

                return await Spells.StarryMuse.Cast(Core.Me);
            }

            return false;
        }
    }
}
