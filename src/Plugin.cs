// Ignore Spelling: Plugin

using BepInEx.Configuration;
using BepInEx;
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

namespace QM_ChangeExploredColor
{
    [BepInPlugin("nbk_redspy.QM_ChangeExploredColor", "QM_ChangeExploredColor", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Color ExploredOutlineColor { get; private set; }
        public static BepInEx.Logging.ManualLogSource Log { get; set; }

        public Plugin()
        {
            ExploredOutlineColor = Config.Bind("General", nameof(Plugin.ExploredOutlineColor), Color.green, "The outline color for items that have already been explored").Value;
        }

        private void Awake()
        {
            Log = Logger;
            Harmony harmony = new Harmony("nbk_redspy.QM_ChangeExploredColor");
            harmony.PatchAll();





        }

        public static bool FunctionHashMatches(Type type, string functionName, string targetHash)
        {
            MethodInfo method = AccessTools.Method(type, functionName);

            if(method == null)
            {
                Log.LogError($"Type: {type.Name} Function: {functionName} could not be found");
                return false;
            }

            byte[] ilData = method.GetMethodBody().GetILAsByteArray();

            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(ilData);
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            string computedHash = hash.ToString().ToUpper();

            bool matches = targetHash == computedHash; 

            if( !matches)
            {
                Log.LogInfo($"Hash does not match.  Type: {type.Name} {functionName} actual hash: {computedHash} target: {targetHash}");
            }

            return matches;
        }

    }


}
