using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Celeste.Mod.CollabUtils2.Triggers;
using MonoMod.RuntimeDetour;
using Newtonsoft.Json;


namespace Celeste.Mod.SJArchipelago.MapMods;

public class MapEntranceMod
{
    private static Dictionary<string, Dictionary<string, string>> MapData;
    public static void Load()
    {
        using (new DetourConfigContext(new DetourConfig("SJAP_MapRando")).Use())
        {
            On.Celeste.Trigger.ctor += ModTriggerCtor;
        }
    }

    public static void Unload()
    {
        On.Celeste.Trigger.ctor -= ModTriggerCtor;
    }

    private static void ModTriggerCtor(On.Celeste.Trigger.orig_ctor orig, Trigger self, EntityData data, Vector2 offset)
    {
        if (self is not ChapterPanelTrigger)
        {
            orig(self, data, offset);
            return;
        }

        if (!SJArchipelagoModule.SaveData.UnlockedMapSIDs.Contains(data.Attr("map")))
        {
            data.Values["map"] = "SJArchipelago/locked";
        }
        orig(self, data, offset);
    }

    // i think the name of this doesnt do a good job explaining this but wtvr
    // it gets the map that is in place of the map SID that you pass in.
    private static string GetMapSwapFromLocation(string SID)
    {
        if (SJArchipelagoModule.SaveData.MapSwapMappings.TryGetValue(SID, out var mapSwap))
        {
            return mapSwap;
        }
        return SID;
    }

    public static void MapUnlockHandler()
    {
        SJArchipelagoModule.SaveData.UnlockedDifficulty += 1;
        
        MapData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText("Mods/SJArchipelago/src/MapMods/MapIDs.json"));
        foreach (KeyValuePair<string, Dictionary<string, string>> item in MapData)
        {
            if (Convert.ToInt32(item.Value["Difficulty"]) <= SJArchipelagoModule.SaveData.UnlockedDifficulty)
            {
                SJArchipelagoModule.SaveData.UnlockedMapSIDs.Add(item.Key);
            }
        }
    }
}