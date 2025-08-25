// Ignore Spelling: Plugin

using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.CodeDom;
using Unity;
using System.Runtime.Serialization.Json;
using SimpleJSON;

namespace QM_ChangeExploredColor
{
    public class Plugin
    {

        public static string ModAssemblyName { get; private set; }

        /// <summary>
        /// The full path to the config file.  Stored in the mod's persistence folder.
        /// </summary>
        public static string ConfigPath { get; private set; }

        /// <summary>
        /// This mod's persistence folder.
        /// </summary>
        public static string ModsPersistenceFolder { get; private set; }

        /// <summary>
        /// The Quasimorph_Mods folder that is parallel to the game's folder.
        /// This is a workaround for Quasimorph syncing and overwriting all files in the 
        /// Game's App Data folder.
        /// </summary>
        private static string AllModsConfigFolder { get; set; }

        private static McmConfiguration McmConfiguration { get; set; }


        public static Color ExploredOutlineColor { get; set; }

        public const byte ExploredOutlineShaderId = 4;

        static Plugin()
        {
            ModAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            AllModsConfigFolder = Path.Combine(Application.persistentDataPath, "../Quasimorph_ModConfigs/");
            ModsPersistenceFolder = Path.Combine(AllModsConfigFolder, ModAssemblyName);
            ConfigPath = Path.Combine(ModsPersistenceFolder, ModAssemblyName + ".json");
            ExploredOutlineColor = Color.green;

            McmConfiguration = new McmConfiguration();
        }

        [Hook(ModHookType.BeforeBootstrap)]
        public static void Bootstrap(IModContext context)
        {
            Directory.CreateDirectory(AllModsConfigFolder);

            Directory.CreateDirectory(ModsPersistenceFolder);

            UpgradeModDirectory();

            SetConfig();

            Plugin.McmConfiguration.TryConfigure();  //Log and continue if there is a MCM issue.

            Harmony harmony = new Harmony("nbk_redspy.QM_ChangeExploredColor");
            harmony.PatchAll();

        }

        public static void SetConfig()
        {
            string configPath = ConfigPath;

            Color defaultColor = Color.green;
            ExploredOutlineColor = defaultColor;

            if (File.Exists(configPath))
            {
                try
                {
                    JSONNode node = JSON.Parse(File.ReadAllText(configPath));
                    ExploredOutlineColor = node[nameof(Plugin.ExploredOutlineColor)].ReadColor(defaultColor);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error deserializing configuration file {configPath}");
                    Debug.LogException(ex);
                }
            }
            else

            {
                WriteConfig(defaultColor);
            }

        }

        public static void WriteConfig(Color color)
        {
            //Write the default file.
            try
            {
                JSONObject obj = new JSONObject();
                obj.Add(nameof(Plugin.ExploredOutlineColor), new JSONObject().WriteColor(color));
                string text = obj.ToString();

                File.WriteAllText(ConfigPath, text);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error writing configuration file {ConfigPath}");
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// Moves the config files from the legacy directory to the new directory.
        /// </summary>
        private static void UpgradeModDirectory()
        {
            try
            {
                string oldConfigFile = Path.Combine(Application.persistentDataPath, Assembly.GetExecutingAssembly().GetName().Name) + ".json";


                if (!File.Exists(oldConfigFile)) return;

                Debug.LogWarning($"Moving config from '{oldConfigFile}' to '{ModsPersistenceFolder}'");
                File.Move(oldConfigFile, ConfigPath);
            }
            catch (Exception ex)
            {
                Debug.Log($"Unable to move the config files.  Exception: {ex.ToString()}");
            }
        }


    }


}
