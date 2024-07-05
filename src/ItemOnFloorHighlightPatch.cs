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
    [HarmonyPatch(typeof(ItemOnFloor), nameof(ItemOnFloor.Highlight))]
    public static class ItemOnFloorHighlightPatch
    {

        public static bool Prefix(ItemOnFloor __instance, ItemStorage ___Storage, bool val) 
        {

            if (___Storage.Empty)
            {
                val = false;
            }

            __instance._spriteRenderer.color = (val ? __instance._highlightedColor : __instance._normalColor);
            __instance._spriteOutline.color = (___Storage.WasExamined ? 
                Plugin.ExploredOutlineColor :
                val ? __instance._highlightedBorderColor : Color.clear);
            __instance._spriteOutline.enabled = val;

            return false;


        }
    }
}
