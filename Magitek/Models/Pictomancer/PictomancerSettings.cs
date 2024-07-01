using Magitek.Models.Roles;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magitek.Models.Pictomancer
{
    [AddINotifyPropertyChangedInterface]
    public class PictomancerSettings : MagicDpsSettings, IRoutineSettings
    {
        public PictomancerSettings() : base(CharacterSettingsDirectory + "/Magitek/Pictomancer/PictomancerSettings.json") { }

        public static PictomancerSettings Instance { get; set; } = new PictomancerSettings();

        #region General-Stuff
        #endregion

        #region Palette-Abilities
        [Setting]
        [DefaultValue(true)]
        public bool UseStarrySky { get; set; }

        [Setting]
        [DefaultValue(4)]
        public int StarrySkyCount { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool StarrySkyEntireParty { get; set; }
        #endregion

        #region Single-Target-Abilities
        [Setting]
        [DefaultValue(true)]
        public bool UseHolyinWhiteMoving { get; set; }
        #endregion

        #region AoE-Abilities
        [Setting]
        [DefaultValue(true)]
        public bool UseAoe { get; set; }

        [Setting]
        [DefaultValue(3)]
        public int AoeEnemies { get; set; }
        #endregion

        #region Utility-Abilities
        [Setting]
        [DefaultValue(true)]
        public bool LucidDreaming { get; set; }
        #endregion

        #region PVP

        #endregion
    }
}
