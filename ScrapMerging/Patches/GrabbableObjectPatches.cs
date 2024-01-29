using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ScrapMerging.Patches
{
    [HarmonyPatch(typeof(GrabbableObject))]
    public class GrabbableObjectPatches
    {
        [HarmonyPatch("OnHitGround")]
        [HarmonyPostfix]
        public static void MergeScrap(GrabbableObject __instance) {
            SharedComponents.ConfigFile.Reload();

            if ( __instance is null 
                 || !__instance.itemProperties.isScrap
                 || !__instance.isInShipRoom 
                 || !SharedComponents.ConfigFile.TryGetEntry("Enables", __instance.name.Replace("(Clone)", ""), out ConfigEntry<bool> entry)
                 || !entry.Value ) {
                return;
            }

            GrabbableObject[] mergeCandidates = UnityEngine.Object.FindObjectsByType<GrabbableObject>(FindObjectsSortMode.None)
                .Where(x => x != null
                         && x.isInShipRoom 
                         && x.itemProperties.isScrap 
                         && x.name == __instance.name)
                .ToArray();
            if ( mergeCandidates.Length < 2 ) { return; }

            GrabbableObject[] mergableObjects = mergeCandidates.Where(
                x => Vector3.Distance(x.transform.position, __instance.transform.position) < SharedComponents.MergeDistance.Value
            ).ToArray();
            if (mergableObjects.Length < 2) { return; }

            __instance.SetScrapValue(mergableObjects.Select(x => x.scrapValue).Sum());
            mergableObjects.Where(x => x.GetInstanceID() != __instance.GetInstanceID()).Do(x => UnityEngine.Object.Destroy(x.gameObject));
        }
    }
}
