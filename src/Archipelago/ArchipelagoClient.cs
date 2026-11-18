using System;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;

using System.Collections.Generic;
using Newtonsoft.Json;


namespace Celeste.Mod.SJArchipelago.Archipelago;

public class APConnData
{
    public const string Game = "Celeste: Strawberry Jam";
    public string Uri;
    public string SlotName;
    public string Password;
    
    public int RoomId;
    public List<long> CheckedLocations;
    private string seed;

    private Dictionary<string, object> slotData;

    public bool NeedSlotData => slotData == null;

    public APConnData()
    {
        Uri = "localhost";
        SlotName = "Player1";
        CheckedLocations = new();
    }

    public APConnData(string uri, string slotName, string password)
    {
        Uri = uri;
        SlotName = slotName;
        Password = password;
        CheckedLocations = new();
    }


    public void SetupSession(Dictionary<string, object> roomSlotData, string roomSeed)
    {
        slotData = roomSlotData;
        seed = roomSeed;
    }


    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}

public class APClient()
{
    private static ArchipelagoSession APsession;

    public static LoginResult ConnectToAPServer(APConnData cdata)
    {
        Logger.Log(LogLevel.Info, "SJAP", $"Attempting to connect to APServer with connection data {cdata.Uri}, {cdata.SlotName}");
        
        APsession = ArchipelagoSessionFactory.CreateSession(cdata.Uri);
        LoginResult loginResult = APsession.TryConnectAndLogin(APConnData.Game, cdata.SlotName, ItemsHandlingFlags.AllItems, password: cdata.Password, requestSlotData: true);
        if (!loginResult.Successful)
        {
            return loginResult;
        }
        
        APsession.Socket.ErrorReceived += APSession_ErrorReceived;
        
        return loginResult;
    }
    
    private static 

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
}