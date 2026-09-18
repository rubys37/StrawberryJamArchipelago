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

public class LocationBehavior : LoadableItemMod
{
    
    private Hook _cathook;

    public override void Load()
    {
        On.Celeste.Strawberry.OnCollect += ModStrawberry.OnCollect;
        _cathook = new Hook(typeof(PettableCat).GetMethod("OnPetting", BindingFlags.Instance | BindingFlags.NonPublic), ModCat.OnPetting);
    }
    
    public override void Unload()
    {
        On.Celeste.Strawberry.OnCollect -= ModStrawberry.OnCollect;
        _cathook.Dispose();
    }
    
    public static class ModCat
    {
        public static void OnPetting(Action<PettableCat> orig, PettableCat self)
        {
            orig(self);
        }
    }

    public static class ModStrawberry
    {
        public static void OnCollect(On.Celeste.Strawberry.orig_OnCollect orig, Strawberry self)
        {
            orig(self);
        }
    }
}