using System;
using System.Collections.Generic;
using System.IO;
using Archipelago.MultiClient.Net.Exceptions;
using Newtonsoft.Json;
using Celeste.Mod.SJArchipelago.Archipelago;

namespace Celeste.Mod.SJArchipelago.Locations;

internal class LocationHandler
{ 
    public static readonly Dictionary<string, long> LocationToAPWorldID = 
        JsonConvert.DeserializeObject<Dictionary<string, long>>(File.ReadAllText("Mods/SJArchipelago/src/Locations/SJArchipelago_Tables - SID_to_APworldID.json"));


    public static bool TrySendLocation(string entitySid)
    {
        if (!LocationToAPWorldID.TryGetValue(entitySid, out var apWorldID))
        {
            return false;
        }

        if (!TryAddLocationToSaveData(apWorldID))
        {
            return false;
        }

        try
        {
            SendLocation(apWorldID);
        }
        catch (ArchipelagoSocketClosedException)
        {
            Logger.Log(LogLevel.Warn, "SJAP", $"Cannot send location {entitySid}: connection not established. This location will be sent on next login.");
        }

        return true;
    }
    
    public static void SendLocation(long apWorldID)
    {
        try
        {
            APClient.CompleteLocations([apWorldID]);
        }
        catch (ArchipelagoSocketClosedException)
        {
            // connection is down. do something idk
            // TODO - do something idk
        }
    }

    public static bool TryAddLocationToSaveData(long apWorldID)
    {
        var added = SJArchipelagoModule.SaveData.UnlockedLocations.Add(apWorldID);
        return added;
    }
}