using System;
using EightTracksPlayer.Utils;

namespace EightTracksTests.Stubs
{
    public class SettingsStub : Settings
    {
        public SettingsStub()
            :base("regPath", "ext")
        {
                
        }

        private string obj = "50";
        public override object this[string key]
        {
            get { return obj.ToString(); }
            set { obj = value.ToString(); }
        }
    }
}