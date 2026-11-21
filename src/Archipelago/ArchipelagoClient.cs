using System;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;

using System.Collections.Generic;
using Newtonsoft.Json;

using Celeste.Mod.SJArchipelago.MapMods;


namespace Celeste.Mod.SJArchipelago.Archipelago;

public class APSaveData
{
    public const string Game = "Slay the Spire II";
    public string Uri;
    public string SlotName;
    public string Password;

    public string RoomId;
    public List<long> CheckedLocations;

    public Dictionary<string, object> SlotData;

    public APSaveData()
    {
        Uri = "localhost:38281";
        SlotName = "ruby_spire";
        Password = "";
        CheckedLocations = new();
    }

    public APSaveData(string uri, string slotName, string password)
    {
        Uri = uri;
        SlotName = slotName;
        Password = password;
        CheckedLocations = new();
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

public class APClient()
{
    private static ArchipelagoSession APSession;
    private static APSaveData SaveData;

    public static LoginResult ConnectToAPServer(APSaveData cdata)
    {
        Logger.Log(LogLevel.Info, "SJAP", $"Attempting to connect to APServer with connection data {cdata.Uri}, {cdata.SlotName}");
        
        SaveData = cdata;
        APSession = ArchipelagoSessionFactory.CreateSession(cdata.Uri);
        LoginResult loginResult = APSession.TryConnectAndLogin(APSaveData.Game, cdata.SlotName, ItemsHandlingFlags.AllItems, password: cdata.Password, requestSlotData: true);
        
        if (!loginResult.Successful)
        {
            return loginResult;
        }

        APSession.Socket.ErrorReceived += APSession_ErrorReceived;
        SetupSession((LoginSuccessful)loginResult, APSession.RoomState.Seed);
        return loginResult;
    }

    private static void SetupSession(LoginSuccessful loginResult, string roomId)
    {
        SaveData.SetupSessionData(loginResult.SlotData, roomId);
        APSession.Items.ItemReceived += OnItemReceivedFromAP;
        APSession.MessageLog.OnMessageReceived += OnMessageReceivedFromAP;
    }

    private static void OnItemReceivedFromAP(IReceivedItemsHelper receivedItemsHelper)
    {
        var itemId = receivedItemsHelper.PeekItem().ItemId;
        ApplyItem(itemId);
        receivedItemsHelper.DequeueItem();
    }

    private static void OnMessageReceivedFromAP(LogMessage msg)
    {
        // TODO: implement
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

    private static void ApplyItem(long itemID)
    {
        switch (itemID)
        {
            case 500: MapEntranceMod.MapUnlockHandler(); break;
        }
    }
}