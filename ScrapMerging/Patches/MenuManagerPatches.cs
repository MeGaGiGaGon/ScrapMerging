using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using BepInEx.Configuration;
using System.Linq;
using Unity.Netcode;

namespace ScrapMerging.Patches
{
    [HarmonyPatch(typeof(MenuManager))]
    public class MenuManagerPatches
    {
        public static string[] defaultFalseList = {
            "DiyFlashbang",
            "GiftBox",
            "StopSign",
            "YieldSign",
        };

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void GenerateConfig(MenuManager __instance) {
            SharedComponents.ConfigFile.Reload();
            foreach (
                GrabbableObject grabbableObject 
                in Resources.FindObjectsOfTypeAll(typeof(GrabbableObject))
                .Cast<GrabbableObject>()
                .Where(x => x.itemProperties.isScrap)
            ) {
                SharedComponents.ConfigFile.Bind(
                    "Enables", 
                    grabbableObject.name, 
                    !defaultFalseList.Contains(grabbableObject.name), 
                    $"If true, {grabbableObject.name}s will be merged."
                );
            }
        }
    }
}
