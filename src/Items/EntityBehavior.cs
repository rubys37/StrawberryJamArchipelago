using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Celeste.Mod.AdventureHelper.Entities;
using Celeste.Mod.CommunalHelper.DashStates;
using Celeste.Mod.CommunalHelper.Entities;
using Celeste.Mod.MaxHelpingHand.Entities;
using Celeste.Mod.DJMapHelper.Entities;
using ExtendedVariants.Entities.ForMappers;
using ExtendedVariants.Entities.Legacy;
using Celeste.Mod.GravityHelper.Triggers;
using vitmod;
using Celeste.Mod.StrawberryJam2021.Entities;
using Celeste.Mod.StrawberryJam2021.Triggers;
using FrostHelper.Entities.Boosters;
using Celeste.Mod.CherryHelper;
using Celeste.Mod.CommunalHelper.Entities.StrawberryJam;
using Celeste.Mod.IsaGrabBag;
using Celeste.Mod.PandorasBox;
using Celeste.Mod.CavernHelper;
using Celeste.Mod.FemtoHelper;
using Celeste.Mod.OutbackHelper;
using Celeste.Mod.VortexHelper.Entities;
using BrokemiaHelper;
using Celeste.Mod.GravityHelper.Entities;
using Celeste.Mod.Batteries;
using Celeste.Mod.BounceHelper;
using Celeste.Mod.JackalHelper.Entities;
using Celeste.Mod.TwigHelper.Entities;
using Celeste.Mod.Anonhelper;
using Celeste.Mod.FurryHelper;
using ExtendedVariants.Module;
using Celeste.Mod.Spirialis;
using Celeste.Mod.BGswitch;
using FlaglinesAndSuch;
using FrostHelper;
using Celeste.Mod.EmHelper.Entities;
using Monocle;
using MonoMod.RuntimeDetour;
using VivHelper.Entities;
using VivHelper.Entities.CurvedStuff;
// wow thats a lot of imports, im sure there wouldnt be any conflicts
using DashZipMover = Celeste.Mod.StrawberryJam2021.Entities.DashZipMover;
using ExpiringDashRefill = Celeste.Mod.StrawberryJam2021.Entities.ExpiringDashRefill;
using Entity = Monocle.Entity;
using GrabTempleGate = Celeste.Mod.StrawberryJam2021.Entities.GrabTempleGate;
using InstantTeleportTrigger = VivHelper.Triggers.InstantTeleportTrigger;
using ToggleSwapBlock = Celeste.Mod.StrawberryJam2021.Entities.ToggleSwapBlock;

namespace Celeste.Mod.SJArchipelago.Items;

internal class EntityBehavior
{
    public static List<LoadableItemMod> LoadedItemBehaviorMods { get; } =
    [
        new ModItemCollision(),
        new ModItemUpdate()
    ];
    
    public static bool HaveInteractable(ItemHandler.Item item)
    {
        SJArchipelagoModuleSettings.ReceiveSpecificItems receiveItems = SJArchipelagoModule.Settings.ReceiveItems;
        if (SJArchipelagoModule.Settings.DisableAllItems) return false;
        return item switch
        {
            ItemHandler.Item.IntroCrushers => receiveItems.ReceiveIntroCrushers,
            ItemHandler.Item.Springs => receiveItems.ReceiveSprings,
            ItemHandler.Item.TrafficBlocks => receiveItems.ReceiveTrafficBlocks,
            ItemHandler.Item.DashCrystals => receiveItems.ReceiveDashCrystals,
            ItemHandler.Item.DoubleDashCrystals => receiveItems.ReceiveDoubleDashCrystals,
            ItemHandler.Item.CassetteBlocks => receiveItems.ReceiveCassetteBlocks,
            ItemHandler.Item.DreamBlocks => receiveItems.ReceiveDreamBlocks,
            ItemHandler.Item.StrawberrySeeds => receiveItems.ReceiveStrawberrySeeds,
            ItemHandler.Item.Coins => receiveItems.ReceiveCoins,
            ItemHandler.Item.SinkingPlatforms => receiveItems.ReceiveSinkingPlatforms,
            ItemHandler.Item.MovingPlatforms => receiveItems.ReceiveMovingPlatforms,
            ItemHandler.Item.BlueClouds => receiveItems.ReceiveBlueClouds,
            ItemHandler.Item.PinkClouds => receiveItems.ReceivePinkClouds,
            ItemHandler.Item.GreenBubbles => receiveItems.ReceiveGreenBubbles,
            ItemHandler.Item.RedBubbles => receiveItems.ReceiveRedBubbles,
            ItemHandler.Item.MoveBlocks => receiveItems.ReceiveMoveBlocks,
            ItemHandler.Item.SwapBlocks => receiveItems.ReceiveSwapBlocks,
            ItemHandler.Item.DashSwitch => receiveItems.ReceiveDashSwitch,
            ItemHandler.Item.Seekers => receiveItems.ReceiveSeekers,
            ItemHandler.Item.TheoCrystals => receiveItems.ReceiveTheoCrystals,
            ItemHandler.Item.Feathers => receiveItems.ReceiveFeathers,
            ItemHandler.Item.Kevins => receiveItems.ReceiveKevins,
            ItemHandler.Item.Bumpers => receiveItems.ReceiveBumpers,
            ItemHandler.Item.BadelineOrbs => receiveItems.ReceiveBadelineOrbs,
            ItemHandler.Item.CoreBlocks => receiveItems.ReceiveCoreBlocks,
            ItemHandler.Item.Iceballs => receiveItems.ReceiveIceballs,
            ItemHandler.Item.CoreSwitches => receiveItems.ReceiveCoreSwitches,
            ItemHandler.Item.Pufferfish => receiveItems.ReceivePufferfish,
            ItemHandler.Item.Jellyfish => receiveItems.ReceiveJellyfish,
            ItemHandler.Item.PowerBoxes => receiveItems.ReceivePowerBoxes,
            ItemHandler.Item.Birds => receiveItems.ReceiveBirds,

            ItemHandler.Item.DashTrafficBlocks => receiveItems.ReceiveDashTrafficBlocks,
            ItemHandler.Item.CerealBlocks => receiveItems.ReceiveCerealBlocks,
            ItemHandler.Item.DreamDashCrystals => receiveItems.ReceiveDreamDashCrystals,
            ItemHandler.Item.BlueSprings => receiveItems.ReceiveBlueSprings,
            ItemHandler.Item.BlueBubbles => receiveItems.ReceiveBlueBubbles,
            ItemHandler.Item.CassetteZippers => receiveItems.ReceiveCassetteZippers,
            ItemHandler.Item.SingleJumpCrystals => receiveItems.ReceiveSingleJumpCrystals,
            ItemHandler.Item.TripleJumpCrystals => receiveItems.ReceiveTripleJumpCrystals,
            ItemHandler.Item.GravityTriggers => receiveItems.ReceiveGravityTriggers,
            ItemHandler.Item.BlueTimeCrystals => receiveItems.ReceiveBlueTimeCrystals,
            ItemHandler.Item.GrayTimeCrystals => receiveItems.ReceiveGrayTimeCrystals,
            ItemHandler.Item.DashCrystalShards => receiveItems.ReceiveDashCrystalShards,
            ItemHandler.Item.TinyStrawberries => receiveItems.ReceiveTinyStrawberries,
            ItemHandler.Item.Roses => receiveItems.ReceiveRoses,
            ItemHandler.Item.Pipes => receiveItems.ReceivePipes,
            ItemHandler.Item.TeleportFields => receiveItems.ReceiveTeleportFields,
            ItemHandler.Item.Ziplines => receiveItems.ReceiveZiplines,
            ItemHandler.Item.DashToggleBlocks => receiveItems.ReceiveDashToggleBlocks,
            ItemHandler.Item.CrystalBombs => receiveItems.ReceiveCrystalBombs,
            ItemHandler.Item.PurpleBubbles => receiveItems.ReceivePurpleBubbles,
            ItemHandler.Item.DashSprings => receiveItems.ReceiveDashSprings,
            ItemHandler.Item.PlatformJellyfish => receiveItems.ReceivePlatformJellyfish,
            ItemHandler.Item.GrayBubbles => receiveItems.ReceiveGrayBooster,
            ItemHandler.Item.Skateboard => receiveItems.ReceiveSkateboard,
            ItemHandler.Item.Umbrella => receiveItems.ReceiveUmbrella,
            ItemHandler.Item.FakeHearts => receiveItems.ReceiveFakeHearts,
            ItemHandler.Item.WormholeBubbles => receiveItems.ReceiveWormholeBoosters,
            ItemHandler.Item.Portals => receiveItems.ReceivePortals,
            ItemHandler.Item.MomentumSprings => receiveItems.ReceiveMomentumSprings,
            ItemHandler.Item.PinkBubbles => receiveItems.ReceivePinkBubbles,
            ItemHandler.Item.GravitySprings => receiveItems.ReceiveGravitySprings,
            ItemHandler.Item.PowerSwitchCrates => receiveItems.ReceivePowerSwitchCrates,
            ItemHandler.Item.Batteries => receiveItems.ReceiveBatteries,
            ItemHandler.Item.FloatingBubbles => receiveItems.ReceiveFloatingBubbles,
            ItemHandler.Item.DashBoostFields => receiveItems.ReceiveDashBoostFields,
            ItemHandler.Item.PropellerBoxes => receiveItems.ReceivePropellerBoxes,
            ItemHandler.Item.CoreBubbles => receiveItems.ReceiveCoreBubbles,
            ItemHandler.Item.FlowerSprings => receiveItems.ReceiveFlowerSprings,
            ItemHandler.Item.FlowerHyperBoosts => receiveItems.ReceiveFlowerHyperBoosts,
            ItemHandler.Item.CloudCrystals => receiveItems.ReceiveCloudCrystals,
            ItemHandler.Item.JellyCrystals => receiveItems.ReceiveJellyCrystals,
            ItemHandler.Item.RewindCrystals => receiveItems.ReceiveRewindCrystals,
            ItemHandler.Item.BowlPuffers => receiveItems.ReceiveBowlPuffers,
            ItemHandler.Item.GrayStopwatches => receiveItems.ReceiveGrayStopwatches,
            ItemHandler.Item.BlueStopwatches => receiveItems.ReceiveBlueStopwatches,
            ItemHandler.Item.GreenStopwatches => receiveItems.ReceiveGreenStopwatches,
            ItemHandler.Item.SkyLanterns => receiveItems.ReceiveSkyLanterns,
            ItemHandler.Item.GlitchWalls => receiveItems.ReceiveGlitchWalls,
            ItemHandler.Item.GrabToggleGates => receiveItems.ReceiveGrabToggleGates,
            ItemHandler.Item.BackgroundToggleSwitches => receiveItems.ReceiveBackgroundToggleSwitches,
            _ => true
        };
    }

    public class ModItemCollision : LoadableItemMod
    {
        public override void Load()
        {
            On.Monocle.Collide.Check_Entity_Entity += modCollide_EntityCheck;
            On.Celeste.Solid.GetPlayerRider += modSolid_GetPlayerRider;
        }

        public override void Unload()
        {
            On.Monocle.Collide.Check_Entity_Entity -= modCollide_EntityCheck;
            On.Celeste.Solid.GetPlayerRider -= modSolid_GetPlayerRider;
        }
        
        private static Type TripleBoostFlower = Type.GetType("Celeste.Mod.StrawberryJam2021.Entities.TripleBoostFlower");
        private static Type PocketUmbrellaTrigger = Type.GetType("Celeste.Mod.StrawberryJam2021.Triggers.PocketUmbrellaTrigger");
        private static Type WormholeBooster = Type.GetType("Celeste.Mod.StrawberryJam2021.Entities.WormholeBooster");
        private static Type PropellerBox = Type.GetType("Celeste.Mod.PandorasBox.PropellerBox");
        private static Type PlusOneRefill = Type.GetType("FrostHelper.PlusOneRefill");
        private static Type SkyLantern = Type.GetType("Celeste.Mod.StrawberryJam2021.Entities.SkyLantern");
        private static Type Stopwatch = Type.GetType("Celeste.Mod.Spirialis.Stopwatch");
        private static Type TimeGrabTrigger = Type.GetType("Celeste.Mod.Spirialis.TimeGrabTrigger");
            

        /* Handles every entity that should have no collision when disabled by the ap
        TODO: Add a list of every entity handled here*/
        private static bool modCollide_EntityCheck(On.Monocle.Collide.orig_Check_Entity_Entity orig, Entity a, Entity b)
        {
            if (!orig(a, b))
            {
                return false;
            }

            // this makes debugging easier i promise (its also probably better for performance)
            if (b.GetType() == typeof(SolidTiles)) return true;
            FieldInfo field;
            Type t;
            string areaSid;
            int stopType;
            // player is usually a, (if not then its theo, or jellyfish, or pufferfish or allat). ill sort out exceptions as they come along
            switch (b)
            {
                // modded
                case DreamTunnelRefill:
                    return HaveInteractable(ItemHandler.Item.DreamDashCrystals);

                case NoDashRefillSpring:
                    return HaveInteractable(ItemHandler.Item.BlueSprings);

                case BlueBooster blueBooster:
                    // Blue boosters inherit generic custom booster which is where the field we are looking for is stored.
                    t = typeof(BlueBooster);
                    field = null;
                    while (field == null && t != null)
                    {
                        field = t.GetField("Red",
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        t = t.BaseType;
                    }

                    bool red = (bool)field.GetValue(blueBooster);
                    return red switch
                    {
                        false => HaveInteractable(ItemHandler.Item.BlueBubbles),
                        true => HaveInteractable(ItemHandler.Item.PurpleBubbles)
                    };

                case JumpRefill jumpRefill:
                    // i shamelessly copypasted this reflection from stackoverflow
                    t = jumpRefill.GetType();
                    field = null;
                    while (field == null && t != null)
                    {
                        field = t.GetField("extraJumps",
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        t = t.BaseType;
                    }

                    int extraJumps;
                    if (field == null)
                    {
                        extraJumps = 1;
                    }
                    else
                    {
                        extraJumps = (int)field.GetValue(jumpRefill);
                    }

                    if (extraJumps == 1)
                    {
                        return HaveInteractable(ItemHandler.Item.SingleJumpCrystals);
                    }

                    return HaveInteractable(ItemHandler.Item.TripleJumpCrystals);

                case GravityTrigger:
                    return HaveInteractable(ItemHandler.Item.GravityTriggers);

                case TimeCrystal timeCrystal:
                    field = typeof(TimeCrystal).GetField("untilDash", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    bool untilDash = (bool)field.GetValue(timeCrystal);
                    if (untilDash) return HaveInteractable(ItemHandler.Item.BlueTimeCrystals);
                    return HaveInteractable(ItemHandler.Item.GrayTimeCrystals);

                case RefillShard:
                    return HaveInteractable(ItemHandler.Item.DashCrystalShards);

                case MultiRoomStrawberrySeed:
                    return HaveInteractable(ItemHandler.Item.TinyStrawberries);

                case FlagTouchSwitch:
                    return HaveInteractable(ItemHandler.Item.Coins);

                case ZipLine:
                    return HaveInteractable(ItemHandler.Item.Ziplines);

                case ResettingRefill resettingRefill:
                    field = typeof(ResettingRefill).GetField("extraJump", BindingFlags.Instance | BindingFlags.NonPublic);
                    bool extraJump = (bool)field.GetValue(resettingRefill);
                    field = typeof(ResettingRefill).GetField("dashes", BindingFlags.Instance | BindingFlags.NonPublic);
                    int dashes = (int)field.GetValue(resettingRefill);
                    if (extraJump) return HaveInteractable(ItemHandler.Item.SingleJumpCrystals);
                    if (dashes == 1) return HaveInteractable(ItemHandler.Item.DashCrystals);
                    if (dashes == 2) return HaveInteractable(ItemHandler.Item.DoubleDashCrystals);
                    // idk how we'd end up here but just in case
                    return true;

                case ExpiringDashRefill:
                    return HaveInteractable(ItemHandler.Item.DashCrystals);

                case CrystalBomb:
                    return HaveInteractable(ItemHandler.Item.CrystalBombs);

                case DashSpring:
                    return HaveInteractable(ItemHandler.Item.DashSprings);

                case PlatformJelly:
                    return HaveInteractable(ItemHandler.Item.PlatformJellyfish);

                case GrayBooster:
                    return HaveInteractable(ItemHandler.Item.GrayBubbles);

                case TriggerTrigger trigger:
                    if (HaveInteractable(ItemHandler.Item.Skateboard)) return true;
                    areaSid = SaveData.Instance.CurrentSession_Safe.Area.SID;
                    if (areaSid != "StrawberryJam2021/3-Advanced/mmm") return true;
                    string level = trigger.SceneAs<Level>().Session.Level;
                    if (level is "a00" or "a01") return false;
                    return true;

                case CustomFakeHeart:
                    return HaveInteractable(ItemHandler.Item.FakeHearts);

                case Portal:
                    return HaveInteractable(ItemHandler.Item.Portals);

                case MovingTouchSwitch:
                    return HaveInteractable(ItemHandler.Item.Coins);

                case SpringGreen:
                    return HaveInteractable(ItemHandler.Item.MomentumSprings);

                case PurpleBooster:
                    return HaveInteractable(ItemHandler.Item.PinkBubbles);

                case GravitySpring:
                    return HaveInteractable(ItemHandler.Item.GravitySprings);

                case RefillWall refillWall:
                    field = typeof(RefillWall).GetField("twoDashes", BindingFlags.Instance | BindingFlags.NonPublic);
                    bool twoDashes = (bool)field.GetValue(refillWall);
                    return twoDashes ? HaveInteractable(ItemHandler.Item.DoubleDashCrystals) : HaveInteractable(ItemHandler.Item.DashCrystals);

                case SwitchCrate:
                    return HaveInteractable(ItemHandler.Item.PowerSwitchCrates);

                case Battery:
                    return HaveInteractable(ItemHandler.Item.Batteries);

                case FloatingBubble:
                    return HaveInteractable(ItemHandler.Item.FloatingBubbles);

                case DashBoostField:
                    return HaveInteractable(ItemHandler.Item.DashBoostFields);

                case CoreBooster:
                    return HaveInteractable(ItemHandler.Item.CoreBubbles);

                case ShroomRefill:
                    return HaveInteractable(ItemHandler.Item.DashCrystals);

                case ExtendedVariantTrigger trigger:
                    areaSid = SaveData.Instance.CurrentSession_Safe.Area.SID;
                    if (areaSid == "StrawberryJam2021/4-Expert/DanTKO")
                    {
                        field = typeof(ExtendedVariantTrigger).GetField("variantChange", BindingFlags.Instance | BindingFlags.NonPublic);
                        ExtendedVariantsModule.Variant variant = (ExtendedVariantsModule.Variant)field.GetValue(trigger);
                        if (variant == ExtendedVariantsModule.Variant.BounceEverywhere)
                        {
                            return HaveInteractable(ItemHandler.Item.FlowerSprings);
                        }

                        if (variant is ExtendedVariantsModule.Variant.HyperdashSpeed
                            or ExtendedVariantsModule.Variant.WallBouncingSpeed)
                        {
                            return HaveInteractable(ItemHandler.Item.FlowerHyperBoosts);
                        }
                    }

                    if (areaSid == "StrawberryJam2021/4-Expert/KAERRA")
                    {
                        field = typeof(ExtendedVariantTrigger).GetField("variantChange", BindingFlags.Instance | BindingFlags.NonPublic);
                        ExtendedVariantsModule.Variant variant = (ExtendedVariantsModule.Variant)field.GetValue(trigger);

                        if (variant is ExtendedVariantsModule.Variant.Gravity)
                        {
                            return HaveInteractable(ItemHandler.Item.GravityTriggers);
                        }
                    }

                    return true;

                case CardinalBumper:
                    return HaveInteractable(ItemHandler.Item.Bumpers);

                case MonumentBooster monumentBooster:
                    field = typeof(MonumentBooster).GetField("red", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if ((bool)field.GetValue(monumentBooster))
                    {
                        return HaveInteractable(ItemHandler.Item.RedBubbles);
                    }

                    return HaveInteractable(ItemHandler.Item.GreenBubbles);

                case CloudRefill:
                    return HaveInteractable(ItemHandler.Item.CloudCrystals);

                case JellyRefill:
                    return HaveInteractable(ItemHandler.Item.JellyCrystals);

                case StopwatchRefill:
                    return HaveInteractable(ItemHandler.Item.RewindCrystals);

                case BowlPuffer:
                    return HaveInteractable(ItemHandler.Item.BowlPuffers);

                case CustomizableRefill:
                    return HaveInteractable(ItemHandler.Item.DashCrystals);

                case BGModeToggle:
                    return HaveInteractable(ItemHandler.Item.BackgroundToggleSwitches);

                // these classes are internal and i cannot reference them directly, which means a string comparison must be used
                case Object obj when obj.GetType().FullName == "Celeste.Mod.StrawberryJam2021.Entities.TripleBoostFlower":
                return HaveInteractable(ItemHandler.Item.Roses);
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.StrawberryJam2021.Triggers.PocketUmbrellaTrigger":
                if (HaveInteractable(ItemHandler.Item.Umbrella)) return true;
                field = obj.GetType().GetField("Enable", BindingFlags.Instance | BindingFlags.NonPublic);
                field.SetValue(obj, false);
                return false;
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.StrawberryJam2021.Entities.WormholeBooster":
                return HaveInteractable(ItemHandler.Item.WormholeBubbles);
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.PandorasBox.PropellerBox":
                return HaveInteractable(ItemHandler.Item.PropellerBoxes);
                
                case Object obj when obj.GetType().FullName == "FrostHelper.PlusOneRefill":
                return HaveInteractable(ItemHandler.Item.DashCrystals);
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.StrawberryJam2021.Entities.SkyLantern":
                return HaveInteractable(ItemHandler.Item.SkyLanterns);
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.Spirialis.Stopwatch":
                // TODO: this does not work. you need to disable the actual grab to toggle time functionality.
                field = obj.GetType().GetField("stopType", BindingFlags.Instance | BindingFlags.NonPublic);
                stopType = (int)field.GetValue(obj);
                switch (stopType)
                {
                    case 0:
                        return HaveInteractable(ItemHandler.Item.GrayStopwatches);
                    case 1:
                        return HaveInteractable(ItemHandler.Item.BlueStopwatches);
                    case 2:
                        return HaveInteractable(ItemHandler.Item.GreenStopwatches);
                }
                return true;
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.Spirialis.TimeGrabTrigger":
                field = obj.GetType().GetField("stopType", BindingFlags.Instance | BindingFlags.NonPublic);
                stopType = (int)field.GetValue(obj);
                switch (stopType)
                {
                    case 0:
                        return HaveInteractable(ItemHandler.Item.GrayStopwatches);
                    case 1:
                        return HaveInteractable(ItemHandler.Item.BlueStopwatches);
                    case 2:
                        return HaveInteractable(ItemHandler.Item.GreenStopwatches);
                }
                return true;
            
                //vanilla
                case Spring:
                return HaveInteractable(ItemHandler.Item.Springs);
                
                case Refill { twoDashes: false }:
                return HaveInteractable(ItemHandler.Item.DashCrystals);
                
                case Refill { twoDashes: true }:
                return HaveInteractable(ItemHandler.Item.DoubleDashCrystals);
                
                case StrawberrySeed:
                return HaveInteractable(ItemHandler.Item.StrawberrySeeds);
                
                case TouchSwitch:
                return HaveInteractable(ItemHandler.Item.Coins);
                
                case Booster { red: false }:
                return HaveInteractable(ItemHandler.Item.GreenBubbles);
                
                case Booster { red: true }:
                return HaveInteractable(ItemHandler.Item.RedBubbles);
                
                case TheoCrystal:
                return HaveInteractable(ItemHandler.Item.TheoCrystals);
                
                case FlyFeather:
                return HaveInteractable(ItemHandler.Item.Feathers);
                
                case Bumper:
                return HaveInteractable(ItemHandler.Item.Bumpers);
                
                case BadelineBoost:
                return HaveInteractable(ItemHandler.Item.BadelineOrbs);
                
                case CoreModeToggle:
                return HaveInteractable(ItemHandler.Item.CoreSwitches);
                
                case Puffer:
                return HaveInteractable(ItemHandler.Item.Pufferfish);
                
                case Glider:
                return HaveInteractable(ItemHandler.Item.Jellyfish);
                
                case FlingBird:
                return HaveInteractable(ItemHandler.Item.Birds);
                
                default:
                return true;
            }
        }
        
        static List<Type> ZipMovers =
        [
            typeof(ZipMover),
            typeof(CustomZipMover),
            typeof(CustomCurvedZipMover),
            typeof(ConnectedZipMover),
            typeof(CassetteZipMover),
            typeof(LinkedZipMover),
            typeof(LinkedZipMoverNoReturn)
        ];
        
        private static Player modSolid_GetPlayerRider(On.Celeste.Solid.orig_GetPlayerRider orig, Solid self)
        {
            var player = orig(self);
            if (player is null)
            {
                return null;
            }
            if (ZipMovers.Contains(self.GetType()) && !HaveInteractable(ItemHandler.Item.TrafficBlocks))
            {
                return null;
            }
            if (self.GetType() == typeof(GlitchWall) && !HaveInteractable(ItemHandler.Item.GlitchWalls))
            {
                return null;
            }
            return player;
        }
    }

    public class ModItemUpdate : LoadableItemMod
    {
        private static List<Hook> CustomHooks = new();

        public override void Load()
        {
            On.Celeste.IntroCrusher.Sequence += ModIntroCrusher.Sequence;
            using (new DetourConfigContext(new DetourConfig("SJAP/CassetteBlockUpdateNonCollidable").WithPriority(1))
                       .Use())
            {
                On.Celeste.CassetteBlock.Update += ModCassetteBlock.Update;
            }

            using (new DetourConfigContext(new DetourConfig("SJAP/DisableCassetteBlockShiftSize").WithPriority(1))
                       .Use())
            {
                On.Celeste.CassetteBlock.ShiftSize += ModCassetteBlock.ShiftSize;
            }

            On.Celeste.Player.DreamDashCheck += ModDreamBlock.Update;
            On.Celeste.SinkingPlatform.Update += ModSinkingPlatform.Update;
            On.Celeste.MovingPlatform.Update += ModMovingPlatform.Update;
            On.Celeste.Cloud.Update += ModCloud.Update;
            On.Celeste.MoveBlock.MoveCheck += ModMoveBlock.MoveCheck;
            On.Celeste.SwapBlock.OnDash += ModSwapBlock.OnDash;
            using (new DetourConfigContext(new DetourConfig("SJAP/DisableDashSwitches").WithPriority(1)).Use())
            {
                On.Celeste.DashSwitch.OnDashed += ModDashSwitch.OnDashed;
            }

            On.Celeste.Seeker.Awake += ModSeeker.Awake;
            On.Celeste.CrushBlock.CanActivate += ModCrushBlock.CanActivate;
            On.Celeste.BounceBlock.Update += ModCoreBlock.Update;
            On.Celeste.FireBall.Update += ModFireball.Update;
            On.Celeste.Puffer.Explode += ModPuffer.Explode;
            On.Celeste.LightningBreakerBox.Break += ModPowerBox.Break;
            CustomLoad();
        }

        public static void CustomLoad()
        {
            CustomHooks.Add(new Hook( typeof(DashZipMover).GetMethod("Sequence", BindingFlags.Instance | BindingFlags.NonPublic), ModDashTrafficBlock.Sequence));
            CustomHooks.Add(new Hook( typeof(LoopBlock).GetMethod("OnDashed", BindingFlags.Instance | BindingFlags.NonPublic), ModCerealBlock.OnDashed));
            CustomHooks.Add(new Hook( typeof(LoopBlock).GetMethod("Update", BindingFlags.Instance | BindingFlags.Public), ModCerealBlock.Update));
            CustomHooks.Add(new Hook( typeof(CassetteSwapBlock).GetMethod("OnDash", BindingFlags.Instance | BindingFlags.NonPublic), ModCassetteSwapBlock.OnDash));
            CustomHooks.Add(new Hook( typeof(ToggleSwapBlock).GetMethod("OnPlayerDashed", BindingFlags.Instance | BindingFlags.NonPublic), ModSJToggleSwapBlock.OnPlayerDashed));
            CustomHooks.Add(new Hook( typeof(FrostHelper.ToggleSwapBlock).GetMethod("OnDash", BindingFlags.Instance | BindingFlags.NonPublic), ModFHToggleSwapBlock.OnDash));
            CustomHooks.Add(new Hook( typeof(ConnectedMoveBlock).GetMethod("MoveCheck", BindingFlags.Instance | BindingFlags.NonPublic), ModConnectedMoveBlock.MoveCheck));
            CustomHooks.Add(new Hook( typeof(VitMoveBlock).GetMethod("MoveCheck", BindingFlags.Instance | BindingFlags.NonPublic), ModVitMoveBlock.MoveCheck));
            CustomHooks.Add(new Hook( typeof(NonReturnCrushBlock).GetMethod("OnDashed", BindingFlags.Instance | BindingFlags.Public), ModNonReturnKevin.OnDashed));
            CustomHooks.Add(new Hook( typeof(UninterruptedNRCB).GetMethod("OnDashed", BindingFlags.Instance | BindingFlags.Public), ModUnInterruptableNonReturnKevin.OnDashed));
            CustomHooks.Add(new Hook( typeof(MarioClearPipeHelper).GetMethod("CanTransportEntity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static), ModClearPipeHelper.CanTransportEntity));
            CustomHooks.Add(new Hook( typeof(InstantTeleportTrigger).GetMethod("TeleportMaster", BindingFlags.Instance | BindingFlags.NonPublic), ModInstantTeleport.TeleportMaster));
            CustomHooks.Add(new Hook( typeof(StationBlock).GetMethod("OnDashed", BindingFlags.Instance | BindingFlags.NonPublic), ModDashToggleBlock.OnDashed));
            CustomHooks.Add(new Hook( typeof(PlatformJelly).GetMethod("Update", BindingFlags.Instance | BindingFlags.Public), ModPlatformJellyfish.Update));
            CustomHooks.Add(new Hook(typeof(DreamMoveBlock).GetMethod("MoveCheck", BindingFlags.Instance | BindingFlags.NonPublic), ModDreamMoveBlock.MoveCheck));
            CustomHooks.Add(new Hook(typeof(BounceMoveBlock).GetMethod("MoveVCheck", BindingFlags.Instance | BindingFlags.NonPublic), ModBounceMoveBlock.MoveCheck));
            CustomHooks.Add(new Hook(typeof(BounceMoveBlock).GetMethod("MoveHCheck", BindingFlags.Instance | BindingFlags.NonPublic), ModBounceMoveBlock.MoveCheck));
            CustomHooks.Add(new Hook(typeof(BowlPuffer).GetMethod("Explode", BindingFlags.Instance | BindingFlags.NonPublic), ModBowlPuffer.Explode));
            CustomHooks.Add(new Hook(typeof(GrabTempleGate).GetMethod("Open", BindingFlags.Instance | BindingFlags.Public), ModGrabSwitchGate.OpenClose));
            CustomHooks.Add(new Hook(typeof(GrabTempleGate).GetMethod("Close", BindingFlags.Instance | BindingFlags.Public), ModGrabSwitchGate.OpenClose));
        }

        public override void Unload()
        {
            On.Celeste.IntroCrusher.Sequence -= ModIntroCrusher.Sequence;
            On.Celeste.CassetteBlock.Update -= ModCassetteBlock.Update;
            On.Celeste.Player.DreamDashCheck -= ModDreamBlock.Update;
            On.Celeste.SinkingPlatform.Update -= ModSinkingPlatform.Update;
            On.Celeste.MovingPlatform.Update -= ModMovingPlatform.Update;
            On.Celeste.Cloud.Update -= ModCloud.Update;
            On.Celeste.MoveBlock.MoveCheck -= ModMoveBlock.MoveCheck;
            On.Celeste.SwapBlock.OnDash -= ModSwapBlock.OnDash;
            On.Celeste.DashSwitch.OnDashed -= ModDashSwitch.OnDashed;
            On.Celeste.Seeker.Awake -= ModSeeker.Awake;
            On.Celeste.CrushBlock.CanActivate -= ModCrushBlock.CanActivate;
            On.Celeste.BounceBlock.Update -= ModCoreBlock.Update;
            On.Celeste.FireBall.Update -= ModFireball.Update;
            On.Celeste.Puffer.Explode -= ModPuffer.Explode;
            On.Celeste.LightningBreakerBox.Break -= ModPowerBox.Break;
            CustomUnload();
        }

        public static void CustomUnload()
        {
            foreach (Hook hook in CustomHooks)
            {
                hook.Dispose();
            }
        }

        private static class ModIntroCrusher
        {
            internal static IEnumerator Sequence(On.Celeste.IntroCrusher.orig_Sequence orig, IntroCrusher self)
            {
                if (HaveInteractable(ItemHandler.Item.IntroCrushers))
                {
                    yield return new SwapImmediately(orig(self));
                }

                yield return null;
            }
        }

        private static class ModCassetteBlock
        {
            internal static void Update(On.Celeste.CassetteBlock.orig_Update orig, CassetteBlock self)
            {
                orig(self);
                if (HaveInteractable(ItemHandler.Item.CassetteBlocks)) return;
                self.Collidable = false;
                self.DisableStaticMovers();
                self.SetActivatedSilently(false);
            }

            internal static void ShiftSize(On.Celeste.CassetteBlock.orig_ShiftSize orig, CassetteBlock self, int shift)
            {
                if (HaveInteractable(ItemHandler.Item.CassetteBlocks))
                {
                    orig(self, shift);
                }
            }
        }

        private static class ModDreamBlock
        {
            internal static bool Update(On.Celeste.Player.orig_DreamDashCheck orig, Player self, Vector2 v)
            {
                return HaveInteractable(ItemHandler.Item.DreamBlocks) && orig(self, v);
            }
        }

        private static class ModSinkingPlatform
        {
            internal static void Update(On.Celeste.SinkingPlatform.orig_Update orig, SinkingPlatform self)
            {
                if (!HaveInteractable(ItemHandler.Item.SinkingPlatforms))
                {
                    return;
                }

                orig(self);
            }
        }

        private static class ModMovingPlatform
        {
            internal static void Update(On.Celeste.MovingPlatform.orig_Update orig, MovingPlatform self)
            {
                if (!HaveInteractable(ItemHandler.Item.MovingPlatforms))
                {
                    self.Collidable = false;
                    return;
                }

                orig(self);
            }
        }

        private static class ModCloud
        {
            internal static void Update(On.Celeste.Cloud.orig_Update orig, Cloud self)
            {
                if ((!self.fragile && !HaveInteractable(ItemHandler.Item.BlueClouds)) ||
                    (self.fragile && !HaveInteractable(ItemHandler.Item.PinkClouds)))
                {
                    self.sprite.Visible = false;
                    self.Collidable = false;
                    return;
                }

                orig(self);
            }
        }

        private static class ModMoveBlock
        {
            internal static bool MoveCheck(On.Celeste.MoveBlock.orig_MoveCheck orig, MoveBlock self, Vector2 v)
            {
                // we cant destroy the one in prologue because otherwise there will be zero sphere 1 checks, so we check for that first.
                var sid = SaveData.Instance.CurrentSession.Area.SID;
                if (sid == "StrawberryJam2021/0-Lobbies/0-Prologue")
                {
                    return false;
                }
                return !HaveInteractable(ItemHandler.Item.MoveBlocks) || orig(self, v);
            }
        }

        private static class ModSwapBlock
        {
            internal static void OnDash(On.Celeste.SwapBlock.orig_OnDash orig, SwapBlock self, Vector2 v)
            {
                if (HaveInteractable(ItemHandler.Item.SwapBlocks))
                {
                    orig(self, v);
                }
            }
        }

        private static class ModDashSwitch
        {
            internal static DashCollisionResults OnDashed(On.Celeste.DashSwitch.orig_OnDashed orig, DashSwitch self,
                Player player, Vector2 v)
            {
                return !HaveInteractable(ItemHandler.Item.DashSwitch)
                    ? DashCollisionResults.NormalCollision
                    : orig(self, player, v);
            }
        }

        private static class ModSeeker
        {
            internal static void Awake(On.Celeste.Seeker.orig_Awake orig, Seeker self, Scene scene)
            {
                // takes the death subrouting and runs it.
                if (!HaveInteractable(ItemHandler.Item.Seekers))
                {
                    Entity entity = new Entity(self.Position);
                    DeathEffect component = new DeathEffect(Color.HotPink, self.Center - self.Position)
                    {
                        OnEnd = delegate { entity.RemoveSelf(); }
                    };
                    entity.Add(component);
                    entity.Depth = -1000000;
                    scene.Add(entity);
                    //Audio.Play("event:/game/05_mirror_temple/seeker_death", self.Position);
                    self.RemoveSelf();
                    self.dead = true;
                }
            }
        }

        private static class ModCrushBlock
        {
            internal static bool CanActivate(On.Celeste.CrushBlock.orig_CanActivate orig, CrushBlock self, Vector2 v)
            {
                return HaveInteractable(ItemHandler.Item.Kevins) && orig(self, v);
            }
        }

        private static class ModCoreBlock
        {
            internal static void Update(On.Celeste.BounceBlock.orig_Update orig, BounceBlock self)
            {
                if (!HaveInteractable(ItemHandler.Item.CoreBlocks))
                {
                    self.DisableStaticMovers();
                    self.Collidable = false;
                    self.state = BounceBlock.States.Broken;
                    self.respawnTimer = 1f;
                }

                orig(self);
            }
        }

        private static class ModFireball
        {
            internal static void Update(On.Celeste.FireBall.orig_Update orig, FireBall self)
            {
                if (self.iceMode && !HaveInteractable(ItemHandler.Item.Iceballs))
                {
                    self.sprite.Visible = false;
                    self.broken = true;
                    self.Collidable = false;
                }

                orig(self);
            }
        }

        private static class ModPuffer
        {
            internal static void Explode(On.Celeste.Puffer.orig_Explode orig, Puffer self)
            {
                if (!HaveInteractable(ItemHandler.Item.Pufferfish))
                {
                    return;
                }
                orig(self);
            }
        }

        private static class ModPowerBox
        {
            internal static void Break(On.Celeste.LightningBreakerBox.orig_Break orig, LightningBreakerBox self)
            {
                if (!HaveInteractable(ItemHandler.Item.PowerBoxes))
                {
                    // modified code to omit the lightning removal routine.
                    RumbleTrigger.ManuallyTrigger(self.Center.X, 1.2f);
                    self.Tag = Tags.Persistent;
                    self.shakeCounter = 0f;
                    self.shaker.On = false;
                    self.sprite.Play("break");
                    self.Collidable = false;
                    self.DestroyStaticMovers();
                    if (self.pulseRoutine != null)
                    {
                        self.pulseRoutine.Active = false;
                    }

                    return;
                }

                orig(self);
            }
        }

        private static class ModDashTrafficBlock
        {
            internal static IEnumerator Sequence(Func<DashZipMover, IEnumerator> orig, DashZipMover self)
            {
                if (HaveInteractable(ItemHandler.Item.DashTrafficBlocks))
                {
                    yield return new SwapImmediately(orig(self));
                }
            }
        }

        private static class ModCerealBlock
        {
            internal static DashCollisionResults OnDashed(Func<LoopBlock, Player, Vector2, DashCollisionResults> orig,
                LoopBlock self, Player player, Vector2 v)
            {
                return HaveInteractable(ItemHandler.Item.CerealBlocks) switch
                {
                    false => DashCollisionResults.NormalCollision,
                    true => orig(self, player, v)
                };
            }

            internal static void Update(Action<LoopBlock> orig, LoopBlock self)
            {
                if (HaveInteractable(ItemHandler.Item.CerealBlocks))
                {
                    orig(self);
                }
            }
        }

        private static class ModCassetteSwapBlock
        {
            internal static void OnDash(Action<CassetteSwapBlock, Vector2> orig, CassetteSwapBlock self, Vector2 v)
            {
                if (HaveInteractable(ItemHandler.Item.SwapBlocks))
                {
                    orig(self, v);
                }
            }
        }

        private static class ModConnectedMoveBlock
        {
            internal static bool MoveCheck(Func<ConnectedMoveBlock, Vector2, bool> orig, ConnectedMoveBlock self,
                Vector2 v)
            {
                return !HaveInteractable(ItemHandler.Item.MoveBlocks) || orig(self, v);
            }
        }

        private static class ModVitMoveBlock
        {
            internal static bool MoveCheck(Func<VitMoveBlock, Vector2, bool> orig, VitMoveBlock self, Vector2 v)
            {
                return !HaveInteractable(ItemHandler.Item.MoveBlocks) || orig(self, v);
            }
        }

        private static class ModUnInterruptableNonReturnKevin
        {
            internal static DashCollisionResults OnDashed(
                Func<UninterruptedNRCB, Player, Vector2, DashCollisionResults> orig, UninterruptedNRCB self,
                Player player, Vector2 v)
            {
                if (HaveInteractable(ItemHandler.Item.Kevins))
                {
                    return orig(self, player, v);
                }

                return DashCollisionResults.NormalCollision;
            }
        }

        private static class ModNonReturnKevin
        {
            internal static DashCollisionResults OnDashed(
                Func<NonReturnCrushBlock, Player, Vector2, DashCollisionResults> orig, NonReturnCrushBlock self,
                Player player, Vector2 v)
            {
                if (HaveInteractable(ItemHandler.Item.Kevins))
                {
                    return orig(self, player, v);
                }

                return DashCollisionResults.NormalCollision;
            }
        }

        private static class ModClearPipeHelper
        {
            internal static bool CanTransportEntity(Func<Entity, MarioClearPipeHelper.Direction, bool> orig,
                Entity entity, MarioClearPipeHelper.Direction dir)
            {
                return HaveInteractable(ItemHandler.Item.Pipes) && orig(entity, dir);
            }
        }

        private static class ModInstantTeleport
        {
            internal static void TeleportMaster(Action<InstantTeleportTrigger, Player> orig,
                InstantTeleportTrigger self, Player player)
            {
                if (HaveInteractable(ItemHandler.Item.TeleportFields))
                {
                    orig(self, player);
                }
            }
        }

        private static class ModFHToggleSwapBlock
        {
            internal static void OnDash(Action<FrostHelper.ToggleSwapBlock, Vector2> orig,
                FrostHelper.ToggleSwapBlock self, Vector2 v)
            {
                if (HaveInteractable(ItemHandler.Item.SwapBlocks))
                {
                    orig(self, v);
                }
            }
        }

        private static class ModSJToggleSwapBlock
        {
            internal static void OnPlayerDashed(Action<StrawberryJam2021.Entities.ToggleSwapBlock, Vector2> orig,
                ToggleSwapBlock self, Vector2 v)
            {
                if (HaveInteractable(ItemHandler.Item.SwapBlocks))
                {
                    orig(self, v);
                }
            }
        }

        private static class ModDashToggleBlock
        {
            internal static DashCollisionResults OnDashed(
                Func<StationBlock, Player, Vector2, DashCollisionResults> orig, StationBlock self, Player player,
                Vector2 v)
            {
                return HaveInteractable(ItemHandler.Item.DashToggleBlocks) switch
                {
                    false => DashCollisionResults.NormalCollision,
                    true => orig(self, player, v)
                };
            }
        }

        private static class ModPlatformJellyfish
        {
            internal static void Update(Action<PlatformJelly> orig, PlatformJelly self)
            {
                orig(self);
                if (!HaveInteractable(ItemHandler.Item.PlatformJellyfish))
                {
                    JumpThru ridablePlatform = (JumpThru)(typeof(PlatformJelly).GetField("ridablePlatform", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self));
                    ridablePlatform.Collidable = false;
                }
            }
        }

        private static class ModDreamMoveBlock
        {
            internal static bool MoveCheck(Func<DreamMoveBlock, Vector2, bool> orig, DreamMoveBlock self, Vector2 speed)
            {
                return !HaveInteractable(ItemHandler.Item.MoveBlocks) || orig(self, speed);
            }
        }
        
        private static class ModBounceMoveBlock
        {
            internal static bool MoveCheck(Func<BounceMoveBlock, float, bool> orig, BounceMoveBlock self, float move)
            {
                return !HaveInteractable(ItemHandler.Item.MoveBlocks) || orig(self, move);
            }
        }
        
        private static class ModBowlPuffer
        {
            internal static void Explode(Action<BowlPuffer, bool> orig, BowlPuffer self, bool playsound)
            {
                if (!HaveInteractable(ItemHandler.Item.BowlPuffers))
                {
                    return;
                }
                orig(self, playsound);
            }
        }
        
        private static class ModGrabSwitchGate
        {
            internal static void OpenClose(Action<GrabTempleGate> orig, GrabTempleGate self)
            {
                if (!HaveInteractable(ItemHandler.Item.GrabToggleGates))
                {
                    return;
                }
                orig(self);
            }
        }
    }
}