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
using Celeste.Mod.BGswitch;
using FlaglinesAndSuch;
using FrostHelper;
using Celeste.Mod.EmHelper.Entities;
using Monocle;
using MonoMod.RuntimeDetour;
using VivHelper.Entities;
using VivHelper.Entities.CurvedStuff;
using BouncySpikes = Celeste.Mod.HonlyHelper.BouncySpikes;
// wow thats a lot of imports, im sure there wouldnt be any conflicts
using DashZipMover = Celeste.Mod.StrawberryJam2021.Entities.DashZipMover;
using ExpiringDashRefill = Celeste.Mod.StrawberryJam2021.Entities.ExpiringDashRefill;
using Entity = Monocle.Entity;
using GrabTempleGate = Celeste.Mod.StrawberryJam2021.Entities.GrabTempleGate;
using InstantTeleportTrigger = VivHelper.Triggers.InstantTeleportTrigger;
using ToggleSwapBlock = Celeste.Mod.StrawberryJam2021.Entities.ToggleSwapBlock;
using Item = Celeste.Mod.SJArchipelago.Items.ItemHandler.Item;

namespace Celeste.Mod.SJArchipelago.Items;

internal class EntityBehavior
{
    public static List<LoadableItemMod> LoadedItemBehaviorMods { get; } =
    [
        new ModItemCollision(),
        new ModItemUpdate()
    ];
    
    public static bool HaveInteractable(Item item)
    {
        SJArchipelagoModuleSettings.ReceiveSpecificItems receiveItems = SJArchipelagoModule.Settings.ReceiveItems;
        if (SJArchipelagoModule.Settings.DisableAllItems) return false;
        return item switch
        {
            Item.IntroCrushers => receiveItems.ReceiveIntroCrushers,
            Item.Springs => receiveItems.ReceiveSprings,
            Item.TrafficBlocks => receiveItems.ReceiveTrafficBlocks,
            Item.DashCrystals => receiveItems.ReceiveDashCrystals,
            Item.DoubleDashCrystals => receiveItems.ReceiveDoubleDashCrystals,
            Item.CassetteBlocks => receiveItems.ReceiveCassetteBlocks,
            Item.DreamBlocks => receiveItems.ReceiveDreamBlocks,
            Item.StrawberrySeeds => receiveItems.ReceiveStrawberrySeeds,
            Item.Coins => receiveItems.ReceiveCoins,
            Item.SinkingPlatforms => receiveItems.ReceiveSinkingPlatforms,
            Item.MovingPlatforms => receiveItems.ReceiveMovingPlatforms,
            Item.BlueClouds => receiveItems.ReceiveBlueClouds,
            Item.PinkClouds => receiveItems.ReceivePinkClouds,
            Item.GreenBubbles => receiveItems.ReceiveGreenBubbles,
            Item.RedBubbles => receiveItems.ReceiveRedBubbles,
            Item.MoveBlocks => receiveItems.ReceiveMoveBlocks,
            Item.SwapBlocks => receiveItems.ReceiveSwapBlocks,
            Item.DashSwitches => receiveItems.ReceiveDashSwitch,
            Item.Seekers => receiveItems.ReceiveSeekers,
            Item.TheoCrystals => receiveItems.ReceiveTheoCrystals,
            Item.Feathers => receiveItems.ReceiveFeathers,
            Item.Kevins => receiveItems.ReceiveKevins,
            Item.Bumpers => receiveItems.ReceiveBumpers,
            Item.BadelineOrbs => receiveItems.ReceiveBadelineOrbs,
            Item.CoreBlocks => receiveItems.ReceiveCoreBlocks,
            Item.Iceballs => receiveItems.ReceiveIceballs,
            Item.CoreSwitches => receiveItems.ReceiveCoreSwitches,
            Item.Pufferfish => receiveItems.ReceivePufferfish,
            Item.Jellyfish => receiveItems.ReceiveJellyfish,
            Item.PowerBoxes => receiveItems.ReceivePowerBoxes,
            Item.Birds => receiveItems.ReceiveBirds,

            Item.DashTrafficBlocks => receiveItems.ReceiveDashTrafficBlocks,
            Item.CerealBlocks => receiveItems.ReceiveCerealBlocks,
            Item.DreamDashCrystals => receiveItems.ReceiveDreamDashCrystals,
            Item.BlueSprings => receiveItems.ReceiveBlueSprings,
            Item.BlueBubbles => receiveItems.ReceiveBlueBubbles,
            Item.CassetteZippers => receiveItems.ReceiveCassetteZippers,
            Item.SingleJumpCrystals => receiveItems.ReceiveSingleJumpCrystals,
            Item.TripleJumpCrystals => receiveItems.ReceiveTripleJumpCrystals,
            Item.GravityTriggers => receiveItems.ReceiveGravityTriggers,
            Item.BlueTimeCrystals => receiveItems.ReceiveBlueTimeCrystals,
            Item.GrayTimeCrystals => receiveItems.ReceiveGrayTimeCrystals,
            Item.DashCrystalShards => receiveItems.ReceiveDashCrystalShards,
            Item.TinyStrawberries => receiveItems.ReceiveTinyStrawberries,
            Item.Roses => receiveItems.ReceiveRoses,
            Item.Pipes => receiveItems.ReceivePipes,
            Item.TeleportFields => receiveItems.ReceiveTeleportFields,
            Item.Ziplines => receiveItems.ReceiveZiplines,
            Item.DashToggleBlocks => receiveItems.ReceiveDashToggleBlocks,
            Item.CrystalBombs => receiveItems.ReceiveCrystalBombs,
            Item.PurpleBubbles => receiveItems.ReceivePurpleBubbles,
            Item.DashSprings => receiveItems.ReceiveDashSprings,
            Item.PlatformJellyfish => receiveItems.ReceivePlatformJellyfish,
            Item.GrayBubbles => receiveItems.ReceiveGrayBooster,
            Item.Skateboard => receiveItems.ReceiveSkateboard,
            Item.Umbrella => receiveItems.ReceiveUmbrella,
            Item.FakeHearts => receiveItems.ReceiveFakeHearts,
            Item.WormholeBubbles => receiveItems.ReceiveWormholeBoosters,
            Item.Portals => receiveItems.ReceivePortals,
            Item.MomentumSprings => receiveItems.ReceiveMomentumSprings,
            Item.PinkBubbles => receiveItems.ReceivePinkBubbles,
            Item.GravitySprings => receiveItems.ReceiveGravitySprings,
            Item.PowerSwitchCrates => receiveItems.ReceivePowerSwitchCrates,
            Item.Batteries => receiveItems.ReceiveBatteries,
            Item.FloatingBubbles => receiveItems.ReceiveFloatingBubbles,
            Item.DashBoostFields => receiveItems.ReceiveDashBoostFields,
            Item.PropellerBoxes => receiveItems.ReceivePropellerBoxes,
            Item.CoreBubbles => receiveItems.ReceiveCoreBubbles,
            Item.FlowerSprings => receiveItems.ReceiveFlowerSprings,
            Item.FlowerHyperBoosts => receiveItems.ReceiveFlowerHyperBoosts,
            Item.CloudCrystals => receiveItems.ReceiveCloudCrystals,
            Item.JellyCrystals => receiveItems.ReceiveJellyCrystals,
            Item.RewindCrystals => receiveItems.ReceiveRewindCrystals,
            Item.BowlPuffers => receiveItems.ReceiveBowlPuffers,
            Item.GrayStopwatches => receiveItems.ReceiveGrayStopwatches,
            Item.BlueStopwatches => receiveItems.ReceiveBlueStopwatches,
            Item.GreenStopwatches => receiveItems.ReceiveGreenStopwatches,
            Item.SkyLanterns => receiveItems.ReceiveSkyLanterns,
            Item.GlitchWalls => receiveItems.ReceiveGlitchWalls,
            Item.GrabToggleGates => receiveItems.ReceiveGrabToggleGates,
            Item.BackgroundToggleSwitches => receiveItems.ReceiveBackgroundToggleSwitches,
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
            // player is usually param a (if not then its theo, or jellyfish, or pufferfish or allat). ill sort out exceptions as they come along
            switch (b)
            {
                // modded
                case DreamTunnelRefill:
                    return HaveInteractable(Item.DreamDashCrystals);

                case NoDashRefillSpring:
                    return HaveInteractable(Item.BlueSprings);

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
                        false => HaveInteractable(Item.BlueBubbles),
                        true => HaveInteractable(Item.PurpleBubbles)
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
                        return HaveInteractable(Item.SingleJumpCrystals);
                    }

                    return HaveInteractable(Item.TripleJumpCrystals);

                case GravityTrigger:
                    return HaveInteractable(Item.GravityTriggers);

                case TimeCrystal timeCrystal:
                    field = typeof(TimeCrystal).GetField("untilDash", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    bool untilDash = (bool)field.GetValue(timeCrystal);
                    if (untilDash) return HaveInteractable(Item.BlueTimeCrystals);
                    return HaveInteractable(Item.GrayTimeCrystals);

                case RefillShard:
                    return HaveInteractable(Item.DashCrystalShards);

                case MultiRoomStrawberrySeed:
                    return HaveInteractable(Item.TinyStrawberries);

                case FlagTouchSwitch:
                    return HaveInteractable(Item.Coins);

                case ZipLine:
                    return HaveInteractable(Item.Ziplines);

                case ResettingRefill resettingRefill:
                    field = typeof(ResettingRefill).GetField("extraJump", BindingFlags.Instance | BindingFlags.NonPublic);
                    bool extraJump = (bool)field.GetValue(resettingRefill);
                    field = typeof(ResettingRefill).GetField("dashes", BindingFlags.Instance | BindingFlags.NonPublic);
                    int dashes = (int)field.GetValue(resettingRefill);
                    if (extraJump) return HaveInteractable(Item.SingleJumpCrystals);
                    if (dashes == 1) return HaveInteractable(Item.DashCrystals);
                    if (dashes == 2) return HaveInteractable(Item.DoubleDashCrystals);
                    // idk how we'd end up here but just in case
                    return true;

                case ExpiringDashRefill:
                    return HaveInteractable(Item.DashCrystals);

                case CrystalBomb:
                    return HaveInteractable(Item.CrystalBombs);

                case DashSpring:
                    return HaveInteractable(Item.DashSprings);

                case PlatformJelly:
                    return HaveInteractable(Item.PlatformJellyfish);

                case GrayBooster:
                    return HaveInteractable(Item.GrayBubbles);

                case TriggerTrigger trigger:
                    if (HaveInteractable(Item.Skateboard)) return true;
                    areaSid = SaveData.Instance.CurrentSession_Safe.Area.SID;
                    if (areaSid != "StrawberryJam2021/3-Advanced/mmm") return true;
                    string level = trigger.SceneAs<Level>().Session.Level;
                    if (level is "a00" or "a01") return false;
                    return true;

                case CustomFakeHeart:
                    return HaveInteractable(Item.FakeHearts);
                
                case BouncySpikes:
                    return HaveInteractable(Item.Springs);

                case Portal:
                    return HaveInteractable(Item.Portals);

                case MovingTouchSwitch:
                    return HaveInteractable(Item.Coins);

                case SpringGreen:
                    return HaveInteractable(Item.MomentumSprings);

                case PurpleBooster:
                    return HaveInteractable(Item.PinkBubbles);

                case GravitySpring:
                    return HaveInteractable(Item.GravitySprings);

                case RefillWall refillWall:
                    field = typeof(RefillWall).GetField("twoDashes", BindingFlags.Instance | BindingFlags.NonPublic);
                    bool twoDashes = (bool)field.GetValue(refillWall);
                    return twoDashes ? HaveInteractable(Item.DoubleDashCrystals) : HaveInteractable(Item.DashCrystals);

                case SwitchCrate:
                    return HaveInteractable(Item.PowerSwitchCrates);

                case Battery:
                    return HaveInteractable(Item.Batteries);

                case FloatingBubble:
                    return HaveInteractable(Item.FloatingBubbles);

                case DashBoostField:
                    return HaveInteractable(Item.DashBoostFields);

                case CoreBooster:
                    return HaveInteractable(Item.CoreBubbles);

                case ShroomRefill:
                    return HaveInteractable(Item.DashCrystals);

                case ExtendedVariantTrigger trigger:
                    areaSid = SaveData.Instance.CurrentSession_Safe.Area.SID;
                    if (areaSid == "StrawberryJam2021/4-Expert/DanTKO")
                    {
                        field = typeof(ExtendedVariantTrigger).GetField("variantChange", BindingFlags.Instance | BindingFlags.NonPublic);
                        ExtendedVariantsModule.Variant variant = (ExtendedVariantsModule.Variant)field.GetValue(trigger);
                        if (variant == ExtendedVariantsModule.Variant.BounceEverywhere)
                        {
                            return HaveInteractable(Item.FlowerSprings);
                        }

                        if (variant is ExtendedVariantsModule.Variant.HyperdashSpeed
                            or ExtendedVariantsModule.Variant.WallBouncingSpeed)
                        {
                            return HaveInteractable(Item.FlowerHyperBoosts);
                        }
                    }

                    if (areaSid == "StrawberryJam2021/4-Expert/KAERRA")
                    {
                        field = typeof(ExtendedVariantTrigger).GetField("variantChange", BindingFlags.Instance | BindingFlags.NonPublic);
                        ExtendedVariantsModule.Variant variant = (ExtendedVariantsModule.Variant)field.GetValue(trigger);

                        if (variant is ExtendedVariantsModule.Variant.Gravity)
                        {
                            return HaveInteractable(Item.GravityTriggers);
                        }
                    }

                    return true;

                case CardinalBumper:
                    return HaveInteractable(Item.Bumpers);

                case MonumentBooster monumentBooster:
                    field = typeof(MonumentBooster).GetField("red", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if ((bool)field.GetValue(monumentBooster))
                    {
                        return HaveInteractable(Item.RedBubbles);
                    }

                    return HaveInteractable(Item.GreenBubbles);

                case CloudRefill:
                    return HaveInteractable(Item.CloudCrystals);

                case JellyRefill:
                    return HaveInteractable(Item.JellyCrystals);

                case StopwatchRefill:
                    return HaveInteractable(Item.RewindCrystals);

                case BowlPuffer:
                    return HaveInteractable(Item.BowlPuffers);

                case CustomizableRefill:
                    return HaveInteractable(Item.DashCrystals);

                case BGModeToggle:
                    return HaveInteractable(Item.BackgroundToggleSwitches);
                
                case StaticBumper:
                    return HaveInteractable(Item.Bumpers);
                
                case VortexBumper:
                    return HaveInteractable(Item.Bumpers);
                
                case CustomGlider:
                    return HaveInteractable(Item.Jellyfish);

                // these classes are internal and i cannot reference them directly, which means a string comparison must be used
                case Object obj when obj.GetType().FullName == "Celeste.Mod.StrawberryJam2021.Entities.TripleBoostFlower":
                return HaveInteractable(Item.Roses);
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.StrawberryJam2021.Triggers.PocketUmbrellaTrigger":
                if (HaveInteractable(Item.Umbrella)) return true;
                field = obj.GetType().GetField("Enable", BindingFlags.Instance | BindingFlags.NonPublic);
                field.SetValue(obj, false);
                return false;
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.StrawberryJam2021.Entities.WormholeBooster":
                return HaveInteractable(Item.WormholeBubbles);
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.PandorasBox.PropellerBox":
                return HaveInteractable(Item.PropellerBoxes);
                
                case Object obj when obj.GetType().FullName == "FrostHelper.PlusOneRefill":
                return HaveInteractable(Item.DashCrystals);
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.StrawberryJam2021.Entities.SkyLantern":
                return HaveInteractable(Item.SkyLanterns);
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.Spirialis.Stopwatch":
                // TODO: this does not work. you need to disable the actual grab to toggle time functionality.
                field = obj.GetType().GetField("stopType", BindingFlags.Instance | BindingFlags.NonPublic);
                stopType = (int)field.GetValue(obj);
                switch (stopType)
                {
                    case 0:
                        return HaveInteractable(Item.GrayStopwatches);
                    case 1:
                        return HaveInteractable(Item.BlueStopwatches);
                    case 2:
                        return HaveInteractable(Item.GreenStopwatches);
                }
                return true;
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.Spirialis.TimeGrabTrigger":
                field = obj.GetType().GetField("stopType", BindingFlags.Instance | BindingFlags.NonPublic);
                stopType = (int)field.GetValue(obj);
                switch (stopType)
                {
                    case 0:
                        return HaveInteractable(Item.GrayStopwatches);
                    case 1:
                        return HaveInteractable(Item.BlueStopwatches);
                    case 2:
                        return HaveInteractable(Item.GreenStopwatches);
                }
                return true;
            
                //vanilla
                case Spring:
                return HaveInteractable(Item.Springs);
                
                case Refill { twoDashes: false }:
                return HaveInteractable(Item.DashCrystals);
                
                case Refill { twoDashes: true }:
                return HaveInteractable(Item.DoubleDashCrystals);
                
                case StrawberrySeed:
                return HaveInteractable(Item.StrawberrySeeds);
                
                case TouchSwitch:
                return HaveInteractable(Item.Coins);
                
                case Booster { red: false }:
                return HaveInteractable(Item.GreenBubbles);
                
                case Booster { red: true }:
                return HaveInteractable(Item.RedBubbles);
                
                case TheoCrystal:
                return HaveInteractable(Item.TheoCrystals);
                
                case FlyFeather:
                return HaveInteractable(Item.Feathers);
                
                case Bumper:
                return HaveInteractable(Item.Bumpers);
                
                case BadelineBoost:
                return HaveInteractable(Item.BadelineOrbs);
                
                case CoreModeToggle:
                return HaveInteractable(Item.CoreSwitches);
                
                case Puffer:
                return HaveInteractable(Item.Pufferfish);
                
                case Glider:
                return HaveInteractable(Item.Jellyfish);
                
                case FlingBird:
                return HaveInteractable(Item.Birds);
                
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
            if (ZipMovers.Contains(self.GetType()) && !HaveInteractable(Item.TrafficBlocks))
            {
                return null;
            }
            if (self.GetType() == typeof(GlitchWall) && !HaveInteractable(Item.GlitchWalls))
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
            CustomHooks.Add(new Hook(typeof(MultiNodeMovingPlatform).GetMethod("Update", BindingFlags.Instance | BindingFlags.Public), ModMultiNodeMovingPlatform.Update));
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
                if (HaveInteractable(Item.IntroCrushers))
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
                if (HaveInteractable(Item.CassetteBlocks)) return;
                self.Collidable = false;
                self.DisableStaticMovers();
                self.SetActivatedSilently(false);
            }

            internal static void ShiftSize(On.Celeste.CassetteBlock.orig_ShiftSize orig, CassetteBlock self, int shift)
            {
                if (HaveInteractable(Item.CassetteBlocks))
                {
                    orig(self, shift);
                }
            }
        }

        private static class ModDreamBlock
        {
            // this does NOT work btw, it locks all checkpoints and transitions back to the hub rooms for gyms.
            // TODO: fix this.
            private static List<string> gymRooms = new List<string>()
            {
                "StrawberryJam2021/0-Gyms/1-Beginner=^.^=1-hub",
                "StrawberryJam2021/0-Gyms/2-Intermediate=^.^=1-hub",
                "StrawberryJam2021/0-Gyms/3-Advanced=^.^=1-hub",
                "StrawberryJam2021/0-Gyms/4-Expert=^.^=1-hub",
                "StrawberryJam2021/0-Gyms/5-Grandmaster=^.^=1-hub",
            };
            internal static bool Update(On.Celeste.Player.orig_DreamDashCheck orig, Player self, Vector2 v)
            {
                if (!orig(self, v))
                {
                    return false;
                }

                var sid = SaveData.Instance.CurrentSession_Safe.Area.SID;
                var level = self.SourceData.Level.Name;
                var fullId = sid + "=^.^=" + level;

                return gymRooms.Contains(fullId) || HaveInteractable(Item.DreamBlocks);
            }
        }

        private static class ModSinkingPlatform
        {
            internal static void Update(On.Celeste.SinkingPlatform.orig_Update orig, SinkingPlatform self)
            {
                if (!HaveInteractable(Item.SinkingPlatforms))
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
                if (!HaveInteractable(Item.MovingPlatforms))
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
                if ((!self.fragile && !HaveInteractable(Item.BlueClouds)) ||
                    (self.fragile && !HaveInteractable(Item.PinkClouds)))
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
                return !HaveInteractable(Item.MoveBlocks) || orig(self, v);
            }
        }

        private static class ModSwapBlock
        {
            internal static void OnDash(On.Celeste.SwapBlock.orig_OnDash orig, SwapBlock self, Vector2 v)
            {
                if (HaveInteractable(Item.SwapBlocks))
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
                return !HaveInteractable(Item.DashSwitches)
                    ? DashCollisionResults.NormalCollision
                    : orig(self, player, v);
            }
        }

        private static class ModSeeker
        {
            internal static void Awake(On.Celeste.Seeker.orig_Awake orig, Seeker self, Scene scene)
            {
                // takes the death subrouting and runs it.
                if (!HaveInteractable(Item.Seekers))
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
                return HaveInteractable(Item.Kevins) && orig(self, v);
            }
        }

        private static class ModCoreBlock
        {
            internal static void Update(On.Celeste.BounceBlock.orig_Update orig, BounceBlock self)
            {
                if (!HaveInteractable(Item.CoreBlocks))
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
                if (self.iceMode && !HaveInteractable(Item.Iceballs))
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
                if (!HaveInteractable(Item.Pufferfish))
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
                if (!HaveInteractable(Item.PowerBoxes))
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
                if (HaveInteractable(Item.DashTrafficBlocks))
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
                return HaveInteractable(Item.CerealBlocks) switch
                {
                    false => DashCollisionResults.NormalCollision,
                    true => orig(self, player, v)
                };
            }

            internal static void Update(Action<LoopBlock> orig, LoopBlock self)
            {
                if (HaveInteractable(Item.CerealBlocks))
                {
                    orig(self);
                }
            }
        }

        private static class ModCassetteSwapBlock
        {
            internal static void OnDash(Action<CassetteSwapBlock, Vector2> orig, CassetteSwapBlock self, Vector2 v)
            {
                if (HaveInteractable(Item.SwapBlocks))
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
                return !HaveInteractable(Item.MoveBlocks) || orig(self, v);
            }
        }

        private static class ModVitMoveBlock
        {
            internal static bool MoveCheck(Func<VitMoveBlock, Vector2, bool> orig, VitMoveBlock self, Vector2 v)
            {
                return !HaveInteractable(Item.MoveBlocks) || orig(self, v);
            }
        }

        private static class ModUnInterruptableNonReturnKevin
        {
            internal static DashCollisionResults OnDashed(
                Func<UninterruptedNRCB, Player, Vector2, DashCollisionResults> orig, UninterruptedNRCB self,
                Player player, Vector2 v)
            {
                if (HaveInteractable(Item.Kevins))
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
                if (HaveInteractable(Item.Kevins))
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
                return HaveInteractable(Item.Pipes) && orig(entity, dir);
            }
        }

        private static class ModInstantTeleport
        {
            // the only ones i want to disable are in vertigo.
            private static string vertigoSID = "StrawberryJam2021/2-Intermediate/Evilleafy";
            internal static void TeleportMaster(Action<InstantTeleportTrigger, Player> orig,
                InstantTeleportTrigger self, Player player)
            {
                if (HaveInteractable(Item.TeleportFields))
                {
                    orig(self, player);
                }
                var sid = SaveData.Instance.CurrentSession_Safe.Area.SID;
                if (sid != vertigoSID)
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
                if (HaveInteractable(Item.SwapBlocks))
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
                if (HaveInteractable(Item.SwapBlocks))
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
                return HaveInteractable(Item.DashToggleBlocks) switch
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
                if (!HaveInteractable(Item.PlatformJellyfish))
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
                return !HaveInteractable(Item.MoveBlocks) || orig(self, speed);
            }
        }
        
        private static class ModBounceMoveBlock
        {
            internal static bool MoveCheck(Func<BounceMoveBlock, float, bool> orig, BounceMoveBlock self, float move)
            {
                return !HaveInteractable(Item.MoveBlocks) || orig(self, move);
            }
        }
        
        private static class ModBowlPuffer
        {
            internal static void Explode(Action<BowlPuffer, bool> orig, BowlPuffer self, bool playsound)
            {
                if (!HaveInteractable(Item.BowlPuffers))
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
                if (!HaveInteractable(Item.GrabToggleGates))
                {
                    return;
                }
                orig(self);
            }
        }
        
        private static class ModMultiNodeMovingPlatform
        {
            internal static void Update(Action<MultiNodeMovingPlatform> orig, MultiNodeMovingPlatform self)
            {
                if (!HaveInteractable(Item.MovingPlatforms))
                {
                    self.Collidable = false;
                    return;
                }
                orig(self);
            }
        }
    }
}