using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Celeste.Mod.CollabUtils2.Triggers;
using Celeste.Mod.SJArchipelago.Archipelago;
using MonoMod.RuntimeDetour;
using Newtonsoft.Json;


namespace Celeste.Mod.SJArchipelago.MapMods;

public class MapEntranceMod
{
    private static Dictionary<string, Dictionary<string, object>> MapData;
    private static Hook getHeartGemHook;
    private static Dictionary<string, int> LobbySIDtoIntID = new()
    {
        {"StrawberryJam2021/0-Lobbies/1-Beginner", 1},
        {"StrawberryJam2021/0-Lobbies/2-Intermediate", 2},
        {"StrawberryJam2021/0-Lobbies/3-Advanced", 3},
        {"StrawberryJam2021/0-Lobbies/4-Expert", 4},
        {"StrawberryJam2021/0-Lobbies/5-Grandmaster", 5}
    };
    private static Dictionary<string, int> HeartsideSIDtoIntID = new()
    {
        {"StrawberryJam2021/1-Beginner/ZZ-HeartSide", 1},
        {"StrawberryJam2021/2-Intermediate/ZZ-HeartSide", 2},
        {"StrawberryJam2021/3-Advanced/ZZ-HeartSide", 3},
        {"StrawberryJam2021/4-Expert/ZZ-HeartSide", 4},
        {"StrawberryJam2021/5-Grandmaster/ZZ-HeartSide", 5}
    };
    

    public static void Load()
    {
        MapData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>
            (File.ReadAllText("Mods/SJArchipelago/src/MapMods/SJArchipelago_Tables - MapIDs.json"));
            
        using (new DetourConfigContext(new DetourConfig("SJAP_MapRando")).Use())
        {
            On.Celeste.Trigger.ctor += ModTriggerCtor;
        }
        // hooking the HeartGems property to mod its getter.
        
        getHeartGemHook = new Hook(typeof(HeartGemDoor).GetProperty("HeartGems").GetMethod, ModHeartGate.OnGetCollectedHeartGems);
        On.Celeste.HeartGemDoor.ctor += ModHeartGate.OnHeartGateCtor;
    }

    public static void Unload()
    {
        On.Celeste.Trigger.ctor -= ModTriggerCtor;
        getHeartGemHook.Dispose();
        On.Celeste.HeartGemDoor.ctor -= ModHeartGate.OnHeartGateCtor;
    }

    // this could be done better but im not bothered rn because it works so therefore i dont touch
    // this handles both map locking and map swapping from chapter panel triggers
    private static void ModTriggerCtor(On.Celeste.Trigger.orig_ctor orig, Trigger self, EntityData data, Vector2 offset)
    {
        if (!APClient.Connected || self is not ChapterPanelTrigger)
        {
            orig(self, data, offset);
            return;
        }

        // some constructors kinda get cached, so we need to make sure to store the original map.
        if (data.Attr("orig_map") == "")
        {
            data.Values["orig_map"] = data.Attr("map");
        }
        var mapSid = data.Attr("orig_map");
        
        // elif chain my goat
        if (SJArchipelagoModule.SaveData.DisallowedMapSIDs.Contains(mapSid))
        {
            data.Values["map"] = "SJArchipelago/disabled";
        }
        
        else if (!SJArchipelagoModule.SaveData.UnlockedMapSIDs.Contains(mapSid))
        {
            data.Values["map"] = "SJArchipelago/locked";
        }
        else
        {
            var mapping = GetMapSwapFromSIDLocation(mapSid);
            data.Values["map"] = mapping;
        }
        orig(self, data, offset);
    }

    // i think the name of this doesnt do a good job explaining this.
    // it gets the map that is in place of the map SID that you pass in.
    private static string GetMapSwapFromSIDLocation(string sid)
    {
        if (SJArchipelagoModule.SaveData.MapSwapMappings.TryGetValue(sid, out var mapSwap))
        {
            return mapSwap;
        }
        return sid;
    }
    
    // oh mother of god
    public static void MapUnlockHandler()
    {
        var acquiredHeartsDict = new Dictionary<int, int>
            {
                { 1, SJArchipelagoModule.SaveData.BeginnerHearts },
                { 2, SJArchipelagoModule.SaveData.IntermediateHearts },
                { 3, SJArchipelagoModule.SaveData.AdvancedHearts },
                { 4, SJArchipelagoModule.SaveData.ExpertHearts },
                { 5, SJArchipelagoModule.SaveData.GrandmasterHearts }
            };
        
        foreach (var item in MapData)
        {
            if ((long)item.Value["Difficulty"] > SJArchipelagoModule.SaveData.UnlockedDifficulty)
            {
                continue;
            }

            // Reach: SJ maps with a difficulty lower than unlocked difficulty
            if (!HeartsideSIDtoIntID.TryGetValue(item.Key, out var lobbyNumber))
            {
                SJArchipelagoModule.SaveData.UnlockedMapSIDs.Add(item.Key);
                continue;
            }
            
            // Reach: SJ Heartsides with a difficulty lower than unlocked difficulty
            if (!SJArchipelagoModule.SaveData.LockedHeartsides.Contains(lobbyNumber))
            {
                SJArchipelagoModule.SaveData.UnlockedMapSIDs.Add(item.Key);
            }
            
            // Reach: SJ Heartsides with a difficulty lower than unlocked difficulty that are locked by hearts.
            if (acquiredHeartsDict[lobbyNumber] < SJArchipelagoModule.SaveData.RequiredHearts[lobbyNumber])
            {
                continue;
            }
            
            // Reach: SJ Heartsides with a difficulty lower than unlocked difficulty that are locked by hearts that we have enough hearts for.
            SJArchipelagoModule.SaveData.UnlockedMapSIDs.Add(item.Key);
        }
    }

    private static class ModHeartGate
    {
        internal static int OnGetCollectedHeartGems(Func<HeartGemDoor, int> orig, HeartGemDoor self)
        {
            var acquiredHeartsDict = new Dictionary<int, int>
            {
                { 1, SJArchipelagoModule.SaveData.BeginnerHearts },
                { 2, SJArchipelagoModule.SaveData.IntermediateHearts },
                { 3, SJArchipelagoModule.SaveData.AdvancedHearts },
                { 4, SJArchipelagoModule.SaveData.ExpertHearts },
                { 5, SJArchipelagoModule.SaveData.GrandmasterHearts }
            };
            var areaSid = SaveData.Instance.CurrentSession_Safe.Area.SID;
            
            if (!LobbySIDtoIntID.TryGetValue(areaSid, out var lobbyNumber))
            {
                return orig(self);
            }
            
            // Reach: SJ Lobbies
            var locked = SJArchipelagoModule.SaveData.LockedHeartsides.Contains(lobbyNumber);
            if (!locked)
            {
                return self.Requires;
            }
            
            // Reach: SJ Lobbies with heartside that is locked.
            var acquiredHearts = acquiredHeartsDict[lobbyNumber];
            return acquiredHearts;
            
        }

        internal static void OnHeartGateCtor(On.Celeste.HeartGemDoor.orig_ctor orig, HeartGemDoor self, EntityData data, Vector2 offset)
        {
            var areaSid = SaveData.Instance.CurrentSession_Safe.Area.SID;

            if (!LobbySIDtoIntID.TryGetValue(areaSid, out var lobbyNumber))
            {
                orig(self, data, offset);
                return;
            }

            if (SJArchipelagoModule.SaveData.RequiredHearts.TryGetValue(lobbyNumber, out var requiredHearts))
            {
                data.Values["requires"] = requiredHearts;
            }
                
            orig(self, data, offset);
        }
    }
    
}