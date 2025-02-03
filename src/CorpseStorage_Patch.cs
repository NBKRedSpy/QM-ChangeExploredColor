//using HarmonyLib;
//using MGSC;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace QM_ChangeExploredColor
//{

    //TODO:  ---- This appears to have changed to some sort of shader or material or something.
    //  I used to be an outline

//    [HarmonyPatch(typeof(CorpseStorage), nameof(CorpseStorage.Highlight))]
//    public static class CorpseStorage_Patch
//    {
//        public static void Postfix(CorpseStorage __instance, bool val)
//        {
//            //keep for empty items.
//            if (__instance._inventory != null && __instance._inventory.Empty) return;

//            if(__instance.WasExamined && __instance._creatureView != null)
//            {
//                __instance._creatureView.Highlight(val, __instance.MapObstacle.normalColor, Plugin.ExploredOutlineColor);
//            }
//        }
//    }
//}
