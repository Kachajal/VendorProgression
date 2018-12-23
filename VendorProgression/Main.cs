using System;
using System.Linq;
using System.Reflection;
using System.Resources;
using Harmony12;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Shields;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums;
using Kingmaker.Items;
using UnityEngine;
using UnityModManagerNet;

namespace VendorProgression
{
    public class Main
    {
        public static bool enabled;
        public static UnityModManager.ModEntry.ModLogger logger;


        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            modEntry.OnToggle = OnToggle;

            logger = modEntry.Logger;
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;

            return true;
        }

    }


    [HarmonyPatch(typeof(UnityModManager.UI), "Update")]
    static class UnityModManager_UI_Update_Patch
    {
        static void Postfix(UnityModManager.UI __instance)
        {
            if (!Main.enabled)
                return;
            try
            {
                if (Input.GetKeyUp(KeyCode.H) && (Input.GetKey(KeyCode.LeftControl) ||
                                                  Input.GetKey(KeyCode.RightControl))
                                              && Game.Instance.Player != null)
                {
                    /*var weps = ResourcesLibrary.GetBlueprints<BlueprintItemWeapon>();
                    foreach (var wep in weps)
                    {
                        if (wep.Category == WeaponCategory.Longbow || wep.Category == WeaponCategory.Shortbow)
                        {
                            Game.Instance.Player.Inventory.Add(wep);
                        }
                    }*/


                    var arms = ResourcesLibrary.GetBlueprints<BlueprintItemShield>();
                    foreach (var armo in arms)
                    {
                        Game.Instance.Player.Inventory.Add(armo);
                    }
                }
                
                if (Input.GetKeyUp(KeyCode.G) && (Input.GetKey(KeyCode.LeftControl) ||
                                                  Input.GetKey(KeyCode.RightControl))
                                              && Game.Instance.Player != null)
                {
                    Main.logger.Log("Wearing armor guid: " + Game.Instance.Player.MainCharacter.Value.Body.Armor.Armor.Blueprint.AssetGuid);
                    Main.logger.Log("Wielding weapon guid: " + Game.Instance.Player.MainCharacter.Value.GetFirstWeapon().Blueprint.AssetGuid);
                    Main.logger.Log("Items in stash: ");
                    foreach (var ar in Game.Instance.Player.SharedStash.Items)
                    {
                        Main.logger.Log(ar.Blueprint.AssetGuid);
                    }
                }
                

            }
            catch (Exception)
            {
            }
        }
    }
}
