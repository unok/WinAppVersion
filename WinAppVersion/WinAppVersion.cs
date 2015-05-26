using System.Diagnostics;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Web;

namespace WinAppVersion
{
    public class WinAppVersion
    {
        public string ExecFilePath { set; get; }
        public string ExecFileName { set; get; }
        public string CurrentPath { set; get; }
        public string VersionSettingFilePath { set; get; }
        public string DefaultVersion { set; get; }
        public string CurrentVersion { set; get; }
        public string VersionFileNamePath { set; get;  }
        public UserSetting UserSettingData { set; get; }
        public string Command { set; get; }
        public string[] ProgramArguments { set; get; }
        public Dictionary<string, string> Environments { set; get; }

        public WinAppVersion(string[] arg)
        {
            ExecFilePath = Process.GetCurrentProcess().MainModule.FileName;
            ExecFileName = Path.GetFileNameWithoutExtension(ExecFilePath);
            VersionSettingFilePath = GetVersionSettingFilePath();
            UserSettingData = GetUserSettingData();
            DefaultVersion = UserSettingData.DefaultVersion;
            CurrentPath = Environment.CurrentDirectory;
            VersionFileNamePath = GetVersionFileNamePath();
            CurrentVersion = GetCurrentVersion();
            ProgramArguments = arg;
            Validate();
        }

        public void Execute()
        {
            try
            {
                using (var process = new Process())
                {
                    process.StartInfo.CreateNoWindow = false;
                    process.StartInfo.FileName = Command;
                    if (ProgramArguments.Length > 0)
                    {
                        process.StartInfo.Arguments = ProgramArgument.EncodeCommandLineValues(ProgramArguments);
                    }
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    foreach (string key in Environments.Keys)
                    {
                        var env_string = Environments[key];
                        if (process.StartInfo.EnvironmentVariables.ContainsKey(key))
                        {
                            var replace = "%" + key + "%";
                            env_string = Regex.Replace(env_string, replace, process.StartInfo.EnvironmentVariables[key], RegexOptions.IgnoreCase);
                            process.StartInfo.EnvironmentVariables.Remove(key);
                        }
                        process.StartInfo.EnvironmentVariables.Add(key, env_string);
                    }
                    process.Start();

                    process.WaitForExit();
                    Environment.Exit(process.ExitCode);
                }
            } catch (Exception e) {
                throw new WinAppVersionException(e.Message, e);
            }
        }

        private UserSetting GetUserSettingData()
        {
            var settings = new DataContractJsonSerializerSettings();
            settings.UseSimpleDictionaryFormat = true;
            var dcjs = new DataContractJsonSerializer(typeof(UserSetting), settings);
            string sr = new StreamReader(VersionSettingFilePath, System.Text.Encoding.UTF8).ReadToEnd();
            MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(sr));
            try
            {
                return (UserSetting)dcjs.ReadObject(ms);
            } catch (InvalidDataContractException e) {
                throw new WinAppVersionException(e.Message, e);
            }
        }

        private string GetVersionSettingFilePath()
        {
            return Path.GetDirectoryName(ExecFilePath) + Path.DirectorySeparatorChar + ExecFileName + ".json";
        }

        private string GetVersionFileNamePath()
        {
            return CurrentPath + Path.DirectorySeparatorChar + "." + ExecFileName + "-version";
        }

        private string GetCurrentVersion()
        {
            try {
                if (!File.Exists(VersionFileNamePath))
                {
                    return "";
                }
                var sr = new StreamReader(VersionFileNamePath);
                var version_string = sr.ReadLine();
                return version_string;
            }
            catch (InvalidDataContractException e)
            {
                throw new WinAppVersionException(e.Message, e);
            }
        }

        private Boolean Validate()
        {
            // TODO: 実装待ち
            if (!string.IsNullOrWhiteSpace(CurrentVersion))
            {
                if (UserSettingData.VersionCommands.ContainsKey(CurrentVersion))
                {
                    Command = UserSettingData.VersionCommands[CurrentVersion].Command;
                    if (UserSettingData.VersionCommands[CurrentVersion].Setting.Keys.Count > 0)
                    {
                        Environments = UserSettingData.VersionCommands[CurrentVersion].Setting;
                    }
                }
                else
                {
                    throw new WinAppVersionException("version command not found: " + CurrentVersion);
                }
            }
            else if (!string.IsNullOrWhiteSpace(DefaultVersion))
            {
                if (UserSettingData.VersionCommands.ContainsKey(DefaultVersion))
                {
                    Command = UserSettingData.VersionCommands[DefaultVersion].Command;
                    if (UserSettingData.VersionCommands[DefaultVersion].Setting.Keys.Count > 0)
                    {
                        Environments = UserSettingData.VersionCommands[DefaultVersion].Setting;
                    }
                }
                else
                {
                    throw new WinAppVersionException("version command not found: " + DefaultVersion);
                }
            }
            else
            {
                throw new WinAppVersionException("default version not found");
            }
            return true;
        }
    }
}
