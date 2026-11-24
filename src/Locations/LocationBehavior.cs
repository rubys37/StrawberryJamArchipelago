using System;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Celeste.Mod.HonlyHelper;
using Celeste.Mod.VivHelper;
using VivHelper.Triggers;

namespace Celeste.Mod.SJArchipelago.Locations;

public class LocationBehavior
{
    private const string Separator = "=^.^=";
    
    private static Hook cathook;
    private static Hook gymhook;

    public static void Load()
    {
        On.Celeste.Strawberry.OnCollect += ModStrawberry.OnCollect;
        On.Celeste.Level.RegisterAreaComplete += ModLevel.OnComplete;
        cathook = new Hook(typeof(PettableCat).GetMethod("OnPetting", BindingFlags.Instance | BindingFlags.NonPublic), ModCat.OnPetting);
        gymhook = new Hook(typeof(InstantTeleportTrigger).GetMethod("Teleport", BindingFlags.Instance | BindingFlags.NonPublic), ModInstantTeleportTrigger.OnTeleport);

    }
    
    public static void Unload()
    {
        On.Celeste.Strawberry.OnCollect -= ModStrawberry.OnCollect;
        On.Celeste.Level.RegisterAreaComplete -= ModLevel.OnComplete;

        cathook.Dispose();
        gymhook.Dispose();
    }
    
    private static class ModCat
    {
        internal static void OnPetting(Action<PettableCat, Player> orig, PettableCat self, Player player)
        {
            orig(self, player);
            var areaSid = SaveData.Instance.CurrentSession_Safe.Area.SID;
            var levelId = self.SourceId.Level;
            var entityId = self.SourceData.ID;
            var fullId = areaSid + Separator + levelId + ":" + entityId;
            LocationHandler.TrySendLocation(fullId);
        }
    }

    private static class ModStrawberry
    {
        internal static void OnCollect(On.Celeste.Strawberry.orig_OnCollect orig, Strawberry self)
        {
            orig(self);
            var areaSid = SaveData.Instance.CurrentSession_Safe.Area.SID;
            var entityId = self.ID.Key;
            var fullId = areaSid + Separator + entityId;
            LocationHandler.TrySendLocation(fullId);
        }
    }

    private class ModLevel
    {
        internal static void OnComplete(On.Celeste.Level.orig_RegisterAreaComplete orig, Level self)
        {
            var sid = self.Session.Area.SID;
            var fullId = sid + Separator + "complete";
            LocationHandler.TrySendLocation(fullId);
        }
    }

    private static class ModInstantTeleportTrigger
    {
        internal static void OnTeleport(Action<InstantTeleportTrigger, Player> orig, InstantTeleportTrigger self, Player player)
        {
            orig(self, player);
            var areaSid = SaveData.Instance.CurrentSession_Safe.Area.SID;
            var levelId = self.SourceId.Level;
            var entityId = self.SourceData.ID;
            var fullId = areaSid + Separator + levelId + ":" + entityId;
            LocationHandler.TrySendLocation(fullId);
        }
    }
}