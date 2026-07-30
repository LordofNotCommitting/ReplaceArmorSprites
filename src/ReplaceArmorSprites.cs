
using HarmonyLib;
using MGSC;
using ReplaceArmorSprites;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace replacearmorsprites
{
    [HarmonyPatch(typeof(Creature3dView), nameof(Creature3dView.RefreshItemsRender))]

    public static class ReplaceArmorSprites
    {
        public static bool bool_load_tsv_status = false;
        private static string tsvFilePath;
        public static Dictionary<string, string> ReplacementMap = new Dictionary<string, string>();



        public static bool Prefix(bool forceBareHands, Creature3dView __instance)
        {
            if (!bool_load_tsv_status) {

                string ModAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                string AllModsConfigFolder = Path.Combine(Application.persistentDataPath, "../Quasimorph_ModConfigs/");
                string ModPersistenceFolder = Path.Combine(AllModsConfigFolder, ModAssemblyName);

                //tsvFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ArmorSpriteConversionList.tsv");
                tsvFilePath = Path.Combine(ModPersistenceFolder, Plugin.tsv_filename);

                ReplacementMap = LoadArmorReplacementTSV(tsvFilePath);
                bool_load_tsv_status = true;
            }

            //if pc
            if (__instance._inventory.CanHaveVest)
            {
                //then do the replacing
                __instance.ClearEquippedItems();
                BasePickupItem first_before_mod = __instance._inventory.ArmorSlot.First;
                BasePickupItem first = null;
                if (first_before_mod != null && !string.IsNullOrEmpty(first_before_mod.Id))
                {
                    // Fast dictionary lookup instead of looping through all keys
                    if (ReplacementMap.TryGetValue(first_before_mod.Id, out string toId))
                    {
                        //Plugin.Logger.Log($"Proccing replacement for {first_before_mod.Id} -> {toId}");

                        // Construct the new pickup item instance
                        first = new PickupItem(toId, 1, 1);
                    }
                }

                // Fallback if no replacement rule was mapped
                if (first == null)
                {
                    first = first_before_mod;
                }

                if (first != null)
                {
                    ArmorDescriptor armorDescriptor = null;

                    // 1. Try fetching descriptor directly from the record (Primary method for synthesized items)
                    BasePickupItemRecord record = Data.Items.GetRecord(first.Id, false) as BasePickupItemRecord;
                    if (record != null && record.ItemDesc != null)
                    {
                        armorDescriptor = record.ItemDesc as ArmorDescriptor;
                    }

                    // 2. Fallback to view lookup (for items already initialized by the game engine)
                    if (armorDescriptor == null)
                    {
                        armorDescriptor = first.View<ItemContentDescriptor>() as ArmorDescriptor;
                    }

                    //Plugin.Logger.Log($" Final check for {first.Id}: Descriptor found? {armorDescriptor != null}");

                    if (armorDescriptor != null)
                    {
                        __instance.EquipItem(armorDescriptor.Parts, first.Id);
                    }
                    else
                    {
                        Plugin.Logger.LogWarning($"Could not locate ArmorDescriptor for item '{first.Id}'.");
                    }
                }


                BasePickupItem first2_before_mod = __instance._inventory.LeggingsSlot.First;
                BasePickupItem first2 = null;
                if (first2_before_mod != null && !string.IsNullOrEmpty(first2_before_mod.Id))
                {
                    // Fast dictionary lookup instead of looping through all keys
                    if (ReplacementMap.TryGetValue(first2_before_mod.Id, out string toId))
                    {
                        //Plugin.Logger.Log($"Proccing replacement for {first2_before_mod.Id} -> {toId}");

                        // Construct the new pickup item instance
                        first2 = new PickupItem(toId, 1, 1);
                    }
                }

                // Fallback if no replacement rule was mapped
                if (first2 == null)
                {
                    first2 = first2_before_mod;
                }

                if (first2 != null)
                {
                    LeggingsDescriptor leggingsDescriptor = null;

                    // 1. Try fetching descriptor directly from the record (Primary method for synthesized items)
                    BasePickupItemRecord record = Data.Items.GetRecord(first2.Id, false) as BasePickupItemRecord;
                    if (record != null && record.ItemDesc != null)
                    {
                        leggingsDescriptor = record.ItemDesc as LeggingsDescriptor;
                    }

                    // 2. Fallback to view lookup (for items already initialized by the game engine)
                    if (leggingsDescriptor == null)
                    {
                        leggingsDescriptor = first2.View<ItemContentDescriptor>() as LeggingsDescriptor;
                    }

                    //Plugin.Logger.Log($" Final check for {first2.Id}: Descriptor found? {LeggingsDescriptor != null}");

                    if (leggingsDescriptor != null)
                    {
                        __instance.EquipItem(leggingsDescriptor.Parts, first2.Id);
                    }
                    else
                    {
                        Plugin.Logger.LogWarning($"Could not locate LeggingsDescriptor for item '{first2.Id}'.");
                    }
                }

                BasePickupItem first3_before_mod = __instance._inventory.HelmetSlot.First;
                BasePickupItem first3 = null;
                bool flag = false;
                if (first3_before_mod != null && !string.IsNullOrEmpty(first3_before_mod.Id))
                {
                    // Fast dictionary lookup instead of looping through all keys
                    if (ReplacementMap.TryGetValue(first3_before_mod.Id, out string toId))
                    {
                        //Plugin.Logger.Log($"Proccing replacement for {first3_before_mod.Id} -> {toId}");

                        // Construct the new pickup item instance
                        first3 = new PickupItem(toId, 1, 1);
                    }
                }

                // Fallback if no replacement rule was mapped
                if (first3 == null)
                {
                    first3 = first3_before_mod;
                }

                if (first3 != null)
                {
                    HelmetDescriptor helmetDescriptor = null;

                    // 1. Fetch record from database
                    BasePickupItemRecord record = Data.Items.GetRecord(first3.Id, false) as BasePickupItemRecord;

                    if (record != null)
                    {
                        // Get Descriptor
                        helmetDescriptor = record.ItemDesc as HelmetDescriptor;

                        // Get HelmetRecord directly from the database record or via Record method
                        HelmetRecord helmetRecord = record as HelmetRecord;

                        // If HelmetRecord is stored inside record or fetched via Record<T>()
                        if (helmetRecord == null)
                        {
                            helmetRecord = first3.Record<HelmetRecord>();
                        }

                        // Set flag safely
                        if (helmetRecord != null)
                        {
                            flag = helmetRecord.HideHair;
                        }
                    }

                    // 2. Fallback to view lookup if record.ItemDesc was null
                    if (helmetDescriptor == null)
                    {
                        helmetDescriptor = first3.View<ItemContentDescriptor>() as HelmetDescriptor;
                    }

                    if (helmetDescriptor != null)
                    {
                        __instance.EquipItem(helmetDescriptor.Parts, first3.Id);
                    }
                    else
                    {
                        Plugin.Logger.LogWarning($"Could not locate HelmetDescriptor for item '{first3.Id}'.");
                    }
                }

                if (!flag && !string.IsNullOrEmpty(__instance._hairType))
                {
                    __instance.EquipHair(__instance._hairType, __instance._hairColor);
                }


                BasePickupItem first4_before_mod = __instance._inventory.BootsSlot.First;
                BasePickupItem first4 = null;
                if (first4_before_mod != null && !string.IsNullOrEmpty(first4_before_mod.Id))
                {
                    // Fast dictionary lookup instead of looping through all keys
                    if (ReplacementMap.TryGetValue(first4_before_mod.Id, out string toId))
                    {
                        //Plugin.Logger.Log($"Proccing replacement for {first4_before_mod.Id} -> {toId}");

                        // Construct the new pickup item instance
                        first4 = new PickupItem(toId, 1, 1);
                    }
                }

                // Fallback if no replacement rule was mapped
                if (first4 == null)
                {
                    first4 = first4_before_mod;
                }

                if (first4 != null)
                {
                    BootsDescriptor bootsDescriptor = null;

                    // 1. Try fetching descriptor directly from the record (Primary method for synthesized items)
                    BasePickupItemRecord record = Data.Items.GetRecord(first4.Id, false) as BasePickupItemRecord;
                    if (record != null && record.ItemDesc != null)
                    {
                        bootsDescriptor = record.ItemDesc as BootsDescriptor;
                    }

                    // 2. Fallback to view lookup (for items already initialized by the game engine)
                    if (bootsDescriptor == null)
                    {
                        bootsDescriptor = first4.View<ItemContentDescriptor>() as BootsDescriptor;
                    }

                    //Plugin.Logger.Log($" Final check for {first4.Id}: Descriptor found? {LeggingsDescriptor != null}");

                    if (bootsDescriptor != null)
                    {
                        __instance.EquipItem(bootsDescriptor.Parts, first4.Id);
                    }
                    else
                    {
                        Plugin.Logger.LogWarning($"Could not locate HelmetDescriptor for item '{first4.Id}'.");
                    }
                }


                if (first4 != null)
                {
                    BootsDescriptor bootsDescriptor = first4.View<ItemContentDescriptor>() as BootsDescriptor;
                    if (bootsDescriptor != null)
                    {
                        __instance.EquipItem(bootsDescriptor.Parts, first4.Id);
                    }
                }
                BasePickupItem currentWeapon = __instance._inventory.CurrentWeapon;
                if (currentWeapon != null)
                {
                    WeaponDescriptor weaponDescriptor = currentWeapon.View<ItemContentDescriptor>() as WeaponDescriptor;
                    if (weaponDescriptor != null && !forceBareHands)
                    {
                        __instance.RefreshWeapon(weaponDescriptor, currentWeapon.Id, currentWeapon.IsImplicit);
                    }
                }
                __instance.RefreshWeapon(null, null, false);
                BaseCreatureVisualState baseCreatureVisualState;
                if (__instance._currentVisualState != CreatureVisualState.None && __instance._visualStates.TryGetValue(__instance._currentVisualState, out baseCreatureVisualState))
                {
                    foreach (KeyValuePair<CreatureVisualState, BaseCreatureVisualState> keyValuePair in __instance._visualStates)
                    {
                        if (__instance._currentVisualState != CreatureVisualState.Frozen || keyValuePair.Key != CreatureVisualState.Frozen)
                        {
                            keyValuePair.Value.Hide();
                        }
                    }
                    baseCreatureVisualState.Show();
                }
                return false;
            }
            else
            {
                return true;
            }
        }


        private static Dictionary<string, string> LoadArmorReplacementTSV(string path)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();

            if (!File.Exists(path))
            {
                Plugin.Logger.Log($"TSV file not found at: {path}");
                return map;
            }
            else {
                Plugin.Logger.Log($"TSV file found at: {path}");
            }

                // Handles \r\n, \n, and \r automatically across OS environments
                string[] lines = File.ReadAllLines(path);

            //Plugin.Logger.Log($"raw:" + lines);
            // Skip line 0 (Header: FromArmorID    ToArmorID)
            for (int i = 1; i < lines.Length; i++)
            {

                //Plugin.Logger.Log($"replacement processing:"+ lines[i]);
                // Trims outer whitespace and residual line-ending chars like \r
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                // Split on TAB character
                string[] columns = line.Split('\t');
                if (columns.Length >= 2)
                {
                    string fromId = columns[0].Trim().Trim('"');
                    string toId = columns[1].Trim().Trim('"');

                    //Plugin.Logger.Log($"fromto:" + fromId + toId);

                    if (!string.IsNullOrEmpty(fromId) && !string.IsNullOrEmpty(toId))
                    {
                        map[fromId] = toId;
                    }
                }
            }

            return map;
        }
    }

    
}
