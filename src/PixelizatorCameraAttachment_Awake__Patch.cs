using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM_ChangeExploredColor
{
    [HarmonyPatch(typeof(PixelizatorCameraAttachment), nameof(PixelizatorCameraAttachment.Awake))]
    public static class PixelizatorCameraAttachment_Awake__Patch
    {
        public static void Postfix(PixelizatorCameraAttachment __instance)
        {
            __instance.SetOutlinesColor(Plugin.ExploredOutlineShaderId, Plugin.ExploredOutlineColor);
            __instance.RefreshGlobalProperties();
        }
    }
}
