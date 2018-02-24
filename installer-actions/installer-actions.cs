using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Install;
using System.ComponentModel;
using System.Collections;
using Microsoft.Win32;

namespace installer_actions
{
    [RunInstaller(true)]
    public partial class GRInstallCustomAction : Installer
    {
        static string environmentKey = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
        static string InstallationPath = Registry.LocalMachine.CreateSubKey(environmentKey).GetValue("SPOTIFY-OPEN", "", RegistryValueOptions.DoNotExpandEnvironmentNames).ToString();

        public GRInstallCustomAction()
        {
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }


        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);

            /* using SetEnv.exe so don't need to restart the computer */
            var CommandAddVariable = $"cd /D \"{InstallationPath}\" & SetEnv -a PATH %\"{InstallationPath}";
            Cmd.RunCommandHidden(CommandAddVariable);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            //get non-expanded PATH environment variable            
            var oldPath = Registry.LocalMachine.CreateSubKey(environmentKey).GetValue("Path", "", RegistryValueOptions.DoNotExpandEnvironmentNames).ToString();

            var removeString = InstallationPath + ";";
            var index = oldPath.IndexOf(removeString);
            if (index < 0)
            {
                removeString = InstallationPath;
                index = oldPath.IndexOf(removeString);
            }

            if (index > -1)
            {
                oldPath = oldPath.Remove(index, InstallationPath.Length);
                //set the path as an an expandable string
                Registry.LocalMachine.CreateSubKey(environmentKey).SetValue("Path", oldPath, RegistryValueKind.ExpandString);
            }
        }
    }
}
