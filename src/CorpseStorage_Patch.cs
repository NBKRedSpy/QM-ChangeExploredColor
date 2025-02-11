using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QM_ChangeExploredColor
{

    [HarmonyPatch(typeof(CorpseStorage), nameof(CorpseStorage.Highlight))]
    public static class CorpseStorage_Patch
    {
        /// <summary>
        /// If true, will use the "explored" color
        /// </summary>
        public static bool UseExploredColor = false;

        /// <summary>
        /// Signal if the Create3d highlighting needs to use the custom color.
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="val"></param>
        public static void Prefix(CorpseStorage __instance, bool val)
        {
            if (!val || !__instance.WasExamined || (__instance.CreatureData?.Inventory?.Empty ?? true)) return;

            UseExploredColor = true;
        }
    }
}
