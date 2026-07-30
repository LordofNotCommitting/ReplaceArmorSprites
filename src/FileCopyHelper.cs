using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public static class FileCopyHelper
{
    public static string GetAssemblyDirectory()
    {
        // Method 1: Try CodeBase (handles Uri paths like file:///C:/...)
        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
        if (!string.IsNullOrEmpty(codeBase))
        {
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        // Method 2: Try standard Location
        string location = Assembly.GetExecutingAssembly().Location;
        if (!string.IsNullOrEmpty(location))
        {
            return Path.GetDirectoryName(location);
        }

        // Method 3: Fall back to Unity's managed DLL directory (GameName_Data/Managed)
        return Application.dataPath;
    }

    public static void CopyDefaultTsvIfMissing()
    {
        try
        {
            string assemblyDir = GetAssemblyDirectory();
            if (string.IsNullOrEmpty(assemblyDir))
            {
                Debug.LogError("[ReplaceArmorSprites] Failed to determine assembly directory.");
                return;
            }

            string ModAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            string AllModsConfigFolder = Path.Combine(Application.persistentDataPath, "../Quasimorph_ModConfigs/");
            string ModPersistenceFolder = Path.Combine(AllModsConfigFolder, ModAssemblyName);

            string defaultTsvPath = Path.Combine(assemblyDir, "default_ArmorSpriteConversionList.tsv");
            string targetTsvPath = Path.Combine(ModPersistenceFolder, "ArmorSpriteConversionList.tsv");

            Debug.Log($"[ReplaceArmorSprites] Source: {defaultTsvPath}");
            Debug.Log($"[ReplaceArmorSprites] Target: {targetTsvPath}");

            if (!File.Exists(targetTsvPath))
            {
                if (File.Exists(defaultTsvPath))
                {
                    File.Copy(defaultTsvPath, targetTsvPath);
                    Debug.Log($"[ReplaceArmorSprites] Successfully copied default TSV to: {targetTsvPath}");
                }
                else
                {
                    Debug.LogWarning($"[ReplaceArmorSprites] Source file does not exist at: {defaultTsvPath}");
                }
            }
            else
            {
                Debug.Log($"[ReplaceArmorSprites] Target file already exists at: {targetTsvPath}");
            }

            //also copy example

            defaultTsvPath = Path.Combine(assemblyDir, "example_ArmorSpriteConversionList.tsv");
            targetTsvPath = Path.Combine(ModPersistenceFolder, "example_ArmorSpriteConversionList.tsv");

            if (!File.Exists(targetTsvPath))
            {
                if (File.Exists(defaultTsvPath))
                {
                    File.Copy(defaultTsvPath, targetTsvPath);
                    Debug.Log($"[ReplaceArmorSprites] Successfully copied default TSV to: {targetTsvPath}");
                }
                else
                {
                    Debug.LogWarning($"[ReplaceArmorSprites] Source file does not exist at: {defaultTsvPath}");
                }
            }
            else
            {
                Debug.Log($"[ReplaceArmorSprites] Target file already exists at: {targetTsvPath}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[ArmorReplacer] Exception during file copy: {ex}");
        }
    }
}