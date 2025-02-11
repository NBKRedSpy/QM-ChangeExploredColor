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
    [HarmonyPatch(typeof(Creature3dView), nameof(Creature3dView.HighlightAsCorpse))]
    public static class Creature3dView_HighlightAsCorpse__Patch
    {
        public static bool Prefix(Creature3dView __instance)
        {
            if (!CorpseStorage_Patch.UseExploredColor) return true;

            CorpseStorage_Patch.UseExploredColor = false;

            //--- This is a copy of the original function, just with the new color property id.
            float outlineColorId = __instance._currentOutlineId = Plugin.ExploredOutlineShaderId;

            foreach (Material material in __instance._materials)
            {
                material.SetFloat(Creature3dView.PIXELIZER_ID_PROPERTY, outlineColorId);
            }

            return false;
        }
    }
}
