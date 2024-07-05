using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QM_ChangeExploredColor
{

    [HarmonyPatch(typeof(MapObstacle), nameof(MapObstacle.Highlight))]
    public static class MapObstacle_Patch
    {

        //public static bool Prepare()
        //{

        //    //Protection for a pretty heavy handed patch.
        //    //TODO:  Find a better way since the IL is changing with the exact same code.
        //    // Maybe a transpile.
        //    bool hashMatches = Plugin.FunctionHashMatches(typeof(MapObstacle), nameof(MapObstacle.Highlight),
        //        "3C390ABCE6049197B3849147B3B5F7B6CBCE7EB739B8A4D80B98D3DC5DB132B5");

        //    if(!hashMatches)
        //    {
        //        Debug.LogError("Function changed.  Aborting MapObstacle.Highlight patch");
        //    }

        //    return hashMatches;


        //}
        public static bool Prefix(MapObstacle __instance, bool val, bool isDestroyable)
        {

            //NOTE:  I wasn't able to get the outline to change with a postfix.
            //  Tried just setting the sprite outline and enabled, but it would not change.
            //  Not sure why.
            //  Not a fan of copying an entire function.

            if (__instance.ObstacleHealth != null && __instance.ObstacleHealth.Health <= 0 && !__instance.ObstacleHealth.KeepRemains)
            {
                return false;
            }
            if (__instance.ObstacleHealth != null && __instance.ObstacleHealth.Health <= 0 && __instance.ObstacleHealth.KeepRemains && __instance.Elevator == null)
            {
                val = false;
            }
            Color color = ((__instance.Store != null && __instance.Store.storage.Empty) ? __instance.highlightedEmptyStorageColor : __instance.highlightedColor);
            Color color2 = ((__instance.Store != null && __instance.Store.storage.Empty) ? __instance.highlightedEmptyStorageBorderColor : __instance.highlightedBorderColor);


            //--------- Start Mod Change

            if (__instance.Store != null && __instance.Store.storage.WasExamined)
            {
                color2 = Plugin.ExploredOutlineColor;
            }

            //--------- End Mod Change

            if (__instance.spriteOutline != null)
            {
                __instance.spriteOutline.color = ((!val) ? __instance.normalColor : (isDestroyable ? __instance.highlightedDestroyableBorderColor : color2));
                __instance.spriteOutline.enabled = val;
            }
            if (__instance.spRenderer != null)
            {
                __instance.spRenderer.color = ((!val) ? __instance.normalColor : (isDestroyable ? __instance.highlightedDestroyableColor : color));
            }
            foreach (MapObstacleComponent comp in __instance._comps)
            {
                comp.Highlight(val);
            }

            return false;
        }
    }
}
