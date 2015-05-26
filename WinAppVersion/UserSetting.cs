using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Web;


namespace WinAppVersion
{
    [DataContract]
    public class UserSetting
    {
        [DataMember(Name = "default_version")]
        public string DefaultVersion;
        [DataMember(Name = "console_flag")]
        public Boolean ConsoleFlag = false;
        [DataMember(Name = "version_commands")]
        public Dictionary<string, ProgramProperty> VersionCommands = new Dictionary<string, ProgramProperty>();

        [DataContract]
        public class ProgramProperty
        {
            [DataMember(Name = "setting")]
            public Dictionary<string, string> Setting = new Dictionary<string, string>();
            [DataMember(Name = "command")]
            public string Command;
        }
    }
}
