using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Monocle;
using MonoMod.RuntimeDetour;
using Celeste.Mod.HonlyHelper;
using Celeste.Mod.SJArchipelago.Items;
using On.Celeste.Mod.Core;

namespace Celeste.Mod.SJArchipelago.Locations;

public class LocationBehavior
{
    private const string Separator = "=^.^=";
    
    private static Hook _cathook;

    public static void Load()
    {
        On.Celeste.Strawberry.OnCollect += ModStrawberry.OnCollect;
        _cathook = new Hook(typeof(PettableCat).GetMethod("OnPetting", BindingFlags.Instance | BindingFlags.NonPublic), ModCat.OnPetting);
    }
    
    public static void Unload()
    {
        On.Celeste.Strawberry.OnCollect -= ModStrawberry.OnCollect;
        _cathook.Dispose();
    }
    
    public static class ModCat
    {
        public static void OnPetting(Action<PettableCat, Player> orig, PettableCat self, Player player)
        {
            orig(self, player);
            string areaSid = SaveData.Instance.CurrentSession_Safe.Area.SID;
            string entityId = self.SourceId.Key;
            string fullId = areaSid + Separator + entityId;
        }
    }

    public static class ModStrawberry
    {
        public static void OnCollect(On.Celeste.Strawberry.orig_OnCollect orig, Strawberry self)
        {
            orig(self);
            string areaSid = SaveData.Instance.CurrentSession_Safe.Area.SID;
            string entityId = self.ID.Key;
            string fullId = areaSid + Separator + entityId;
            string humanReadableName = LocationHandler.LocationsToReadableDict[fullId];
            // send this off to some mysterious network manager.
        }
    }
}