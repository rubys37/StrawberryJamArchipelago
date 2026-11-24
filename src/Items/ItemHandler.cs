using System.Collections.Generic;

namespace Celeste.Mod.SJArchipelago.Items;



public static class ItemHandler
{
    public const int IDOffsetModdedItems = 100;
    public const int IDOffsetNonInteractables = 1000;
    public const int IDOffsetNonProgression = 2000;
    public enum Item {
        // 0-9999 is for progression items (interactables)
        // vanilla (1-31)
        IntroCrushers,
        Springs,
        TrafficBlocks,
        DashCrystals,
        DoubleDashCrystals,
        CassetteBlocks,
        DreamBlocks,
        StrawberrySeeds,
        Coins,
        SinkingPlatforms,
        MovingPlatforms,
        BlueClouds,
        PinkClouds,
        GreenBubbles,
        RedBubbles,
        MoveBlocks,
        SwapBlocks,
        DashSwitch,
        Seekers,
        TheoCrystals,
        Feathers,
        Kevins,
        Bumpers,
        BadelineOrbs,
        CoreBlocks,
        Iceballs,
        CoreSwitches,
        Pufferfish,
        Jellyfish,
        PowerBoxes,
        Birds,
        
        // modded entities (100-200)
        // introduced in beginner
        DreamDashCrystals,
        CerealBlocks,
        DashTrafficBlocks,
        BlueSprings,
        BlueBubbles,
        CassetteZippers,
        SingleJumpCrystals,
        TripleJumpCrystals,
        GravityTriggers,
        BlueTimeCrystals,
        DashCrystalShards,
        TinyStrawberries,
        Roses,
        Pipes,
        TeleportFields,
        // introduced in intermediate
        Ziplines,
        DashToggleBlocks,
        CrystalBombs,
        DashSprings,
        // introduced in advanced
        GrayTimeCrystals,
        GrayBubbles,
        PlatformJellyfish,
        Skateboard,
        MomentumSprings,
        PurpleBubbles,
        Umbrella,
        PowerSwitchCrates,
        WormholeBubbles,
        FakeHearts,
        Portals,
        PinkBubbles,
        GravitySprings,
        // introduced in expert
        Batteries,
        CoreBubbles,
        BowlPuffers,
        GrabToggleGates,
        GlitchWalls,
        PropellerBoxes,
        JellyCrystals,
        CloudCrystals,
        RewindCrystals,
        SkyLanterns,
        BlueStopwatches,
        GrayStopwatches,
        GreenStopwatches,
        FloatingBubbles,
        DashBoostFields,
        FlowerSprings,
        FlowerHyperBoosts,
        // introduced in gm (
        BackgroundToggleSwitches,
        
        // progressive items that unlock things start at id 1000
        ProgressiveDifficulty,
        Strawberry,
        
        // filler items start at 2000
        Meow,
        Mrrrp,
    };

    public static long GetAPWorldIDFromEnum(Item item)
    {
        var apWorldID = (long)item;
        
        // DreamDashCrystals are the first modded entity in the list, and modded entities start with ID 100.
        if (item >= Item.DreamDashCrystals)
        {
            apWorldID += (IDOffsetModdedItems - (long)Item.DreamDashCrystals);
        }

        return apWorldID;
    }

    public static bool HasInteractable(Item item)
    {
        // this method should be called specifically for interactables (id<1000). we can return false otherwise.
        var apWorldID = GetAPWorldIDFromEnum(item);
        if (apWorldID >= IDOffsetNonInteractables)
        {
            return false;
        }
        
        return SJArchipelagoModule.SaveData.UnlockedInteractables.Contains(apWorldID);
    }
}

