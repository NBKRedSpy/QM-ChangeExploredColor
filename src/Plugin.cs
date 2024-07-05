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
        public static Color ExploredOutlineColor { get; private set; }

        static Plugin()
        {
            ExploredOutlineColor = Color.green;
        }

        [Hook(ModHookType.AfterBootstrap)]
        public static void Bootstrap(IModContext context)
        {
            SetConfig();
            Harmony harmony = new Harmony("nbk_redspy.QM_ChangeExploredColor");
            harmony.PatchAll();

        }

        public static void SetConfig()
        {
            string configPath = Path.Combine(Application.persistentDataPath, Assembly.GetExecutingAssembly().GetName().Name) + ".json";

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
                //Write the default file.
                try
                {
                    JSONObject obj = new JSONObject();
                    obj.Add(nameof(Plugin.ExploredOutlineColor), new JSONObject().WriteColor(Color.green));
                    string text = obj.ToString();

                    File.WriteAllText(configPath, text);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error writing configuration file {configPath}");
                    Debug.LogException(ex);
                }
            }

        }

    }


}
