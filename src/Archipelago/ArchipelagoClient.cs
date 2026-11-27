using System;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;

using System.Collections.Generic;
using System.Threading.Tasks;
using Celeste.Mod.SJArchipelago.Items;
using Newtonsoft.Json;

using Celeste.Mod.SJArchipelago.MapMods;


namespace Celeste.Mod.SJArchipelago.Archipelago;

public class APConnData
{
    public const string Game = "Slay the Spire II";
    public string Uri;
    public string SlotName;
    public string Password;

    public string RoomId;

    public Dictionary<string, object> SlotData;
    
    public APConnData(string uri, string slotName, string password)
    {
        Uri = uri;
        SlotName = slotName;
        Password = password;
    }

    public APConnData()
    {
        Uri = "localhost:38281";
        SlotName = "ruby_spire";
        Password = "";
    }
    
    public static APConnData FromJson(string json)
    {
        return JsonConvert.DeserializeObject<APConnData>(json);
    }

    public void SetupSessionData(Dictionary<string, object> roomSlotData, string roomId)
    {
        SlotData = roomSlotData;
        RoomId = roomId;
    }
    
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}


public static class APClient
{
    public static bool Connected { get; set; } = false;
    
    private static ArchipelagoSession APSession;
    private static APConnData connData;

    public static LoginResult ConnectToAPServer(APConnData cdata)
    {
        Logger.Log(LogLevel.Debug, "SJAP", $"Attempting to connect to APServer with connection data {cdata.Uri}, {cdata.SlotName}.");
        
        connData = cdata;
        APSession = ArchipelagoSessionFactory.CreateSession(cdata.Uri);
        LoginResult loginResult = APSession.TryConnectAndLogin(APConnData.Game, cdata.SlotName, ItemsHandlingFlags.AllItems, password: cdata.Password, requestSlotData: true);
        
        if (!loginResult.Successful)
        {
            return loginResult;
        }
        Logger.Log(LogLevel.Info, "SJAP", $"Connected to APServer with connection data {cdata.Uri}, {cdata.SlotName}.");
        
        Connected = true;
        APSession.Socket.ErrorReceived += APSession_ErrorReceived;
        APSession.Socket.SocketClosed += OnDisconnect;
        SetupSession((LoginSuccessful)loginResult, APSession.RoomState.Seed);
        return loginResult;
    }

    private static void SetupSession(LoginSuccessful loginResult, string roomId)
    {
        connData.SetupSessionData(loginResult.SlotData, roomId);
        SJArchipelagoModule.SaveData.ConnectionData = connData.ToString();
        
        ResyncItems();
        ResyncLocations();
        
        APSession.Items.ItemReceived += OnItemReceivedFromAP;
        APSession.MessageLog.OnMessageReceived += OnMessageReceivedFromAP;
    }

    public static void Disconnect()
    {
        Task.Run(APSession.Socket.DisconnectAsync);
        Connected = false;
    }

    private static void OnDisconnect(string reason)
    {
        Connected = false;
        Logger.Log(LogLevel.Info, "SJAP", $"Disconnected from Archipelago Server with reason {reason}.");
    }

    private static void OnItemReceivedFromAP(IReceivedItemsHelper receivedItemsHelper)
    {
        while (receivedItemsHelper.PeekItem() != null)
        {
            var itemId = receivedItemsHelper.PeekItem().ItemId;
            ApplyItem(itemId);
            receivedItemsHelper.DequeueItem();
        }
    }

    private static void OnMessageReceivedFromAP(LogMessage msg)
    {
        ArchipelagoChat.Chat(msg.ToString());
    }

    private static HashSet<string> SocketWarningsAlreadyShown = new();
    
    private static void APSession_ErrorReceived(Exception e, string msg)
    {
        if (SocketWarningsAlreadyShown.Contains(msg))
        {
            return;
        }
        SocketWarningsAlreadyShown.Add(msg);
        Logger.Log(
            LogLevel.Warn, 
            "SJAP", 
            $"Receive error from APSession: '{msg}'\n" +
            $"{e.StackTrace}");
    }
    
    

    private static void ResyncLocations()
    {
        // the server will always have less completed locations than the save data
        // probably
        // unless the user connects to a save file that has less progress than the server (different room seeds)
        // i plan to handle this in other ways, but right now, lets just ignore this glaring fucking problem
        var locations = APSession.Locations.AllLocationsChecked;
        HashSet<long> savedLocations = new HashSet<long>(SJArchipelagoModule.SaveData.CheckedLocations);
        foreach (var location in locations)
        {
            savedLocations.Remove(location);
        }

        foreach (var missedLocation in savedLocations)
        {
            CompleteLocation([missedLocation]);
        }
    }

    private static void ResyncItems()
    {
        var items = APSession.Items.AllItemsReceived;
        
        // since this is a full resync done on connection, we overwrite the items save data.
        SJArchipelagoModule.SaveData.ProgressiveDifficulties = 0;
        SJArchipelagoModule.SaveData.Strawberries = 0;
        SJArchipelagoModule.SaveData.UnlockedInteractables.Clear();
        foreach (var item in items)
        {
            var itemId = item.ItemId;
            ApplyItem(itemId);
        }
    }
    
    public static void CompleteLocation(long[] apWorldID)
    {
        APSession.Locations.CompleteLocationChecks(apWorldID);
    } 

    private static void ApplyItem(long itemID)
    {
        if (itemID >= ItemHandler.IDOffsetNonProgression)
        {
            return;
        }

        if (itemID >= ItemHandler.IDOffsetNonInteractables)
        {
            switch (itemID)
            {
                case 1000:
                    SJArchipelagoModule.SaveData.ProgressiveDifficulties++;
                    MapEntranceMod.MapUnlockHandler();
                    return;
                case 1001:
                    SJArchipelagoModule.SaveData.BeginnerHearts++;
                    MapEntranceMod.HeartsideUnlockHandler();
                    return;
                case 1002:
                    SJArchipelagoModule.SaveData.IntermediateHearts++;
                    MapEntranceMod.HeartsideUnlockHandler();
                    return;
                case 1003:
                    SJArchipelagoModule.SaveData.AdvancedHearts++;
                    MapEntranceMod.HeartsideUnlockHandler();
                    return;
                case 1004:
                    SJArchipelagoModule.SaveData.ExpertHearts++;
                    MapEntranceMod.HeartsideUnlockHandler();
                    return;
                case 1005:
                    SJArchipelagoModule.SaveData.GrandmasterHearts++;
                    MapEntranceMod.HeartsideUnlockHandler();
                    return;
            }
        }

        SJArchipelagoModule.SaveData.UnlockedInteractables.Add(itemID);
    }
}