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
        static string pathUrl;

        public GRInstallCustomAction()
        {
            pathUrl = (string)Registry.LocalMachine.CreateSubKey(environmentKey).GetValue("SPOTIFY-OPEN", "", RegistryValueOptions.DoNotExpandEnvironmentNames);
            var UltimaLetra = pathUrl.Substring(pathUrl.Length - 1, 1);
            if (UltimaLetra == "\\" || UltimaLetra == "/")
                pathUrl = pathUrl.Substring(0, pathUrl.Length - 1);
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);

            string environmentVar = Environment.GetEnvironmentVariable("PATH");


            //get non-expanded PATH environment variable            
            string oldPath = (string)Registry.LocalMachine.CreateSubKey(environmentKey).GetValue("Path", "", RegistryValueOptions.DoNotExpandEnvironmentNames);


            var index = oldPath.IndexOf(pathUrl);
            if (index < 0)
            {
                //set the path as an an expandable string
                Registry.LocalMachine.CreateSubKey(environmentKey).SetValue("Path", oldPath + ";" + pathUrl, RegistryValueKind.ExpandString);
            }

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
            string oldPath = (string)Registry.LocalMachine.CreateSubKey(environmentKey).GetValue("Path", "", RegistryValueOptions.DoNotExpandEnvironmentNames);

            string removeString = pathUrl + ";";
            var index = oldPath.IndexOf(removeString);
            if (index < 0)
            {
                removeString = pathUrl;
                index = oldPath.IndexOf(removeString);
            }

            if (index > -1)
            {
                oldPath = oldPath.Remove(index, pathUrl.Length);
                //set the path as an an expandable string
                Registry.LocalMachine.CreateSubKey(environmentKey).SetValue("Path", oldPath, RegistryValueKind.ExpandString);
            }
        }
    }
}
