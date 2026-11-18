using Microsoft.Xna.Framework;
using Celeste.Mod.CollabUtils2.Triggers;



namespace Celeste.Mod.SJArchipelago.Misc;

public class MapRando
{
    public static void Load()
    {
        On.Celeste.Trigger.ctor += ModTriggerCtor;
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
        }
        // data.Values["map"] = "StrawberryJam2021/2-Intermediate/thebreadstick1";
        orig(self, data, offset);
    }
}