using HarmonyLib;
using ModConfigMenu;
using ModConfigMenu.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QM_ChangeExploredColor
{
    internal class McmConfiguration
    {
        /// <summary>
        /// Attempts to configure the MCM, but logs an error and continues if it fails.
        /// </summary>
        public bool TryConfigure()
        {
            try
            {
                Configure();
                return true;
            }
            catch (FileNotFoundException)
            {
                Debug.Log($"[{Plugin.ModAssemblyName}] Bypassing MCM. The 'Mod Configuration Menu' mod is not loaded. ");
            }
            catch (Exception ex)
            {
                Debug.Log($"[{Plugin.ModAssemblyName}] Bypassing MCM. ");
                Debug.LogException(ex);
            }

            return false;

        }
        public void Configure()
        {
            ModConfigMenuAPI.RegisterModConfig("Change Explored Color", new List<ConfigValue>()
            {
                new ConfigValue("Color", Plugin.ExploredOutlineColor,"General",Color.green,"The outline color to use for explored containers","Outline Color")
            }, OnSave);
        }

      
        private bool OnSave(Dictionary<string, object> currentConfig, out string feedbackMessage)
        {
            feedbackMessage = "";

            try
            {
                Plugin.ExploredOutlineColor = (Color)currentConfig["Color"];

                Plugin.WriteConfig(Plugin.ExploredOutlineColor);

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error saving the configuration");
                Debug.LogException(ex);
                return false;
            }
        }
    }
}
