using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magitek.Models.Roles
{
    [AddINotifyPropertyChangedInterface]
    public abstract class JobSettings : JsonSettings
    {
        protected JobSettings(string path) : base(path) { }

        #region pvp
        [Setting]
        [DefaultValue(true)]
        public bool Pvp_UseRecuperate { get; set; }

        [Setting]
        [DefaultValue(70.0f)]
        public float Pvp_RecuperateHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Pvp_UsePurify { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Pvp_UseGuard { get; set; }

        [Setting]
        [DefaultValue(40.0f)]
        public float Pvp_GuardHealthPercent { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Pvp_GuardCheck { get; set; }

        [Setting]
        [DefaultValue(true)]
        public bool Pvp_InvulnCheck { get; set; }
        #endregion
    }
}
