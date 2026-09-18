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
using FlaglinesAndSuch;
using FrostHelper;
using Monocle;
using MonoMod.RuntimeDetour;
using VivHelper.Entities;
using VivHelper.Entities.CurvedStuff;
// wow thats a lot of imports, im sure there wouldnt be any conflicts
using DashZipMover = Celeste.Mod.StrawberryJam2021.Entities.DashZipMover;
using CItems = Celeste.Mod.SJArchipelago.Items.EntityHandler.Items;
using ExpiringDashRefill = Celeste.Mod.StrawberryJam2021.Entities.ExpiringDashRefill;
using Entity = Monocle.Entity;
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
    
    public static bool HaveInteractable(EntityHandler.Items item)
    {
        SJArchipelagoModuleSettings.ReceiveSpecificItems receiveItems = SJArchipelagoModule.Settings.ReceiveItems;
        if (SJArchipelagoModule.Settings.DisableAllItems) return false;
        return item switch
        {
            CItems.IntroCrushers => receiveItems.ReceiveIntroCrushers,
            CItems.Springs => receiveItems.ReceiveSprings,
            CItems.TrafficBlocks => receiveItems.ReceiveTrafficBlocks,
            CItems.DashCrystals => receiveItems.ReceiveDashCrystals,
            CItems.DoubleDashCrystals => receiveItems.ReceiveDoubleDashCrystals,
            CItems.CassetteBlocks => receiveItems.ReceiveCassetteBlocks,
            CItems.DreamBlocks => receiveItems.ReceiveDreamBlocks,
            CItems.StrawberrySeeds => receiveItems.ReceiveStrawberrySeeds,
            CItems.Coins => receiveItems.ReceiveCoins,
            CItems.SinkingPlatforms => receiveItems.ReceiveSinkingPlatforms,
            CItems.MovingPlatforms => receiveItems.ReceiveMovingPlatforms,
            CItems.BlueClouds => receiveItems.ReceiveBlueClouds,
            CItems.PinkClouds => receiveItems.ReceivePinkClouds,
            CItems.GreenBubbles => receiveItems.ReceiveGreenBubbles,
            CItems.RedBubbles => receiveItems.ReceiveRedBubbles,
            CItems.MoveBlocks => receiveItems.ReceiveMoveBlocks,
            CItems.WhiteBlock => receiveItems.ReceiveWhiteBlock,
            CItems.SwapBlocks => receiveItems.ReceiveSwapBlocks,
            CItems.DashSwitch => receiveItems.ReceiveDashSwitch,
            CItems.Seekers => receiveItems.ReceiveSeekers,
            CItems.TheoCrystals => receiveItems.ReceiveTheoCrystals,
            CItems.Feathers => receiveItems.ReceiveFeathers,
            CItems.Kevins => receiveItems.ReceiveKevins,
            CItems.Bumpers => receiveItems.ReceiveBumpers,
            CItems.BadelineOrbs => receiveItems.ReceiveBadelineOrbs,
            CItems.CoreBlocks => receiveItems.ReceiveCoreBlocks,
            CItems.Iceballs => receiveItems.ReceiveIceballs,
            CItems.CoreSwitches => receiveItems.ReceiveCoreSwitches,
            CItems.Pufferfish => receiveItems.ReceivePufferfish,
            CItems.Jellyfish => receiveItems.ReceiveJellyfish,
            CItems.PowerBoxes => receiveItems.ReceivePowerBoxes,
            CItems.Birds => receiveItems.ReceiveBirds,

            CItems.DashTrafficBlocks => receiveItems.ReceiveDashTrafficBlocks,
            CItems.CerealBlockBumps => receiveItems.ReceiveCerealBlockBumps,
            CItems.CerealBlockClouds => receiveItems.ReceiveCerealBlockClouds,
            CItems.DreamDashCrystals => receiveItems.ReceiveDreamDashCrystals,
            CItems.BlueSprings => receiveItems.ReceiveBlueSprings,
            CItems.BlueBubbles => receiveItems.ReceiveBlueBubbles,
            CItems.CassetteZippers => receiveItems.ReceiveCassetteZippers,
            CItems.SingleJumpCrystals => receiveItems.ReceiveSingleJumpCrystals,
            CItems.TripleJumpCrystals => receiveItems.ReceiveTripleJumpCrystals,
            CItems.GravityTriggers => receiveItems.ReceiveGravityTriggers,
            CItems.BlueTimeCrystals => receiveItems.ReceiveBlueTimeCrystals,
            CItems.GrayTimeCrystals => receiveItems.ReceiveGrayTimeCrystals,
            CItems.DashCrystalShards => receiveItems.ReceiveDashCrystalShards,
            CItems.TinyStrawberries => receiveItems.ReceiveTinyStrawberries,
            CItems.Roses => receiveItems.ReceiveRoses,
            CItems.Pipes => receiveItems.ReceivePipes,
            CItems.TeleportFields => receiveItems.ReceiveTeleportFields,
            CItems.Ziplines => receiveItems.ReceiveZiplines,
            CItems.DashToggleBlocks => receiveItems.ReceiveDashToggleBlocks,
            CItems.CrystalBombs => receiveItems.ReceiveCrystalBombs,
            CItems.PurpleBubbles => receiveItems.ReceivePurpleBubbles,
            CItems.DashSprings => receiveItems.ReceiveDashSprings,
            CItems.PlatformJellyfish => receiveItems.ReceivePlatformJellyfish,
            CItems.GrayBooster => receiveItems.ReceiveGrayBooster,
            CItems.Skateboard => receiveItems.ReceiveSkateboard,
            CItems.Umbrella => receiveItems.ReceiveUmbrella,
            CItems.FakeHearts => receiveItems.ReceiveFakeHearts,
            CItems.WormholeBoosters => receiveItems.ReceiveWormholeBoosters,
            CItems.Portals => receiveItems.ReceivePortals,
            CItems.PinkBubbles => receiveItems.ReceivePinkBubbles,
            CItems.GravitySprings => receiveItems.ReceiveGravitySprings,
            CItems.SwitchCrates => receiveItems.ReceiveSwitchCrates,
            _ => true
        };
    }

    public class ModItemCollision : LoadableItemMod
    {
        public override void Load()
        {
            On.Monocle.Collide.Check_Entity_Entity += modCollide_EntityCheck;
            On.Celeste.Solid.HasPlayerRider += modSolid_HasPlayerRider;
        }

        public override void Unload()
        {
            On.Monocle.Collide.Check_Entity_Entity -= modCollide_EntityCheck;
            On.Celeste.Solid.HasPlayerRider -= modSolid_HasPlayerRider;
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
            // player is usually a, (if not then its theo, or jellyfish, or pufferfish or allat). ill sort out exceptions as they come along
            switch (b)
            {
                // modded
                case DreamTunnelRefill:
                    return HaveInteractable(CItems.DreamDashCrystals);
                
                case NoDashRefillSpring:
                    return HaveInteractable(CItems.BlueSprings);
                
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
                        false => HaveInteractable(CItems.BlueBubbles),
                        true => HaveInteractable(CItems.PurpleBubbles)
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
                    if (field == null) extraJumps = 1;
                    else extraJumps = (int)field.GetValue(jumpRefill);
                    if (extraJumps == 1) return HaveInteractable(CItems.SingleJumpCrystals);
                    return HaveInteractable(CItems.TripleJumpCrystals);
                
                case GravityTrigger:
                    return HaveInteractable(CItems.GravityTriggers);
                
                case TimeCrystal timeCrystal:
                    field = (typeof(TimeCrystal).GetField("untilDash",
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
                    bool untilDash = (bool)field.GetValue(timeCrystal);
                    if (untilDash) return HaveInteractable(CItems.BlueTimeCrystals);
                    return HaveInteractable(CItems.GrayTimeCrystals);

                case RefillShard:
                    return HaveInteractable(CItems.DashCrystalShards);
                
                case MultiRoomStrawberrySeed:
                    return HaveInteractable(CItems.TinyStrawberries);
                
                case FlagTouchSwitch:
                    return HaveInteractable(CItems.Coins);
                
                case ZipLine:
                    return HaveInteractable(CItems.Ziplines);
                
                case ResettingRefill resettingRefill:
                    field = typeof(ResettingRefill).GetField("extraJump", BindingFlags.Instance | BindingFlags.NonPublic);
                    bool extraJump = (bool)field.GetValue(resettingRefill);
                    field = typeof(ResettingRefill).GetField("dashes", BindingFlags.Instance | BindingFlags.NonPublic);
                    int dashes = (int)field.GetValue(resettingRefill);
                    if (extraJump) return HaveInteractable(CItems.SingleJumpCrystals);
                    if (dashes == 1) return HaveInteractable(CItems.DashCrystals);
                    if (dashes == 2) return HaveInteractable(CItems.DoubleDashCrystals);
                    // idk how we'd end up here but just in case
                    return true;
                
                case ExpiringDashRefill:
                    return HaveInteractable(CItems.DashCrystals);
                
                case CrystalBomb:
                    return HaveInteractable(CItems.CrystalBombs);
                
                case DashSpring:
                    return HaveInteractable(CItems.DashSprings);
                
                case PlatformJelly:
                    return HaveInteractable(CItems.PlatformJellyfish);
                
                case GrayBooster:
                    return HaveInteractable(CItems.GrayBooster);
                
                case TriggerTrigger trigger:
                    if (HaveInteractable(CItems.Skateboard)) return true;
                    if (trigger.SceneAs<Level>().Session.Area.SID != "StrawberryJam2021/3-Advanced/mmm") return true;
                    string level = trigger.SceneAs<Level>().Session.Level;
                    if (level is "a00" or "a01") return false;
                    return true;
                
                case CustomFakeHeart:
                    return HaveInteractable(CItems.FakeHearts);
                
                case Portal:
                    return HaveInteractable(CItems.Portals);
                
                case MovingTouchSwitch:
                    return HaveInteractable(CItems.Coins);
                
                case SpringGreen:
                    return HaveInteractable(CItems.Springs);
                
                case PurpleBooster:
                    return HaveInteractable(CItems.PinkBubbles);
                
                case GravitySpring:
                    return HaveInteractable(CItems.GravitySprings);
                
                case RefillWall:
                    return HaveInteractable(CItems.DashCrystals);
                
                case SwitchCrate:
                    return HaveInteractable(CItems.SwitchCrates);
                
                // these classes are internal and i cannot reference them directly, which means a string comparison must be used
                case Object obj when obj.GetType().FullName == "Celeste.Mod.StrawberryJam2021.Entities.TripleBoostFlower":
                    return HaveInteractable(CItems.Roses);
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.StrawberryJam2021.Triggers.PocketUmbrellaTrigger":
                    if (HaveInteractable(CItems.Umbrella)) return true;
                    field = obj.GetType().GetField("Enable", BindingFlags.Instance | BindingFlags.NonPublic);
                    field.SetValue(obj, false);
                    return false;
                
                case Object obj when obj.GetType().FullName == "Celeste.Mod.StrawberryJam2021.Entities.WormholeBooster":
                    return HaveInteractable(CItems.WormholeBoosters);


                //vanilla
                case Spring:
                    return HaveInteractable(CItems.Springs);
                
                case Refill { twoDashes: false }:
                    return HaveInteractable(CItems.DashCrystals);
                
                case Refill { twoDashes: true }:
                    return HaveInteractable(CItems.DoubleDashCrystals);
                
                case StrawberrySeed:
                    return HaveInteractable(CItems.StrawberrySeeds);
                
                case TouchSwitch:
                    return HaveInteractable(CItems.Coins);
                
                case Booster { red: false }:
                    return HaveInteractable(CItems.GreenBubbles);
                
                case Booster { red: true }:
                    return HaveInteractable(CItems.RedBubbles);
                
                case WhiteBlock:
                    return HaveInteractable(CItems.WhiteBlock);
                
                case TheoCrystal:
                    return HaveInteractable(CItems.TheoCrystals);
                
                case FlyFeather:
                    return HaveInteractable(CItems.Feathers);
                
                case Bumper:
                    return HaveInteractable(CItems.Bumpers);
                
                case BadelineBoost:
                    return HaveInteractable(CItems.BadelineOrbs);
                
                case CoreModeToggle:
                    return HaveInteractable(CItems.CoreSwitches);
                
                case Puffer:
                    return HaveInteractable(CItems.Pufferfish);
                
                case Glider:
                    return HaveInteractable(CItems.Jellyfish);
                
                case FlingBird:
                    return HaveInteractable(CItems.Birds);
                
                default:
                    return true;
            }
        }

        private static bool modSolid_HasPlayerRider(On.Celeste.Solid.orig_HasPlayerRider orig, Solid self)
        {
            if (!orig(self)) return false;

            List<Type> zipMovers = new List<Type>
            {
                typeof(ZipMover),
                typeof(CustomCurvedZipMover),
                typeof(ConnectedZipMover),
                typeof(CassetteZipMover),
                typeof(LinkedZipMover),
                typeof(LinkedZipMoverNoReturn),
            };
            if (zipMovers.Contains(self.GetType()) && !HaveInteractable(CItems.TrafficBlocks)) return false;
            return true;
        }
    }

    public class ModItemUpdate : LoadableItemMod
    {
        private static List<Hook> _customHooks = new();

        public override void Load()
        {
            On.Celeste.IntroCrusher.Sequence += ModIntroCrusher.Sequence;
            // On.Celeste.ZipMover.Update += ModZipMover.Update;
            using (new DetourConfigContext(new DetourConfig("SJAP/CassetteBlockUpdateNonCollidable").WithPriority(1)).Use())
            {
                On.Celeste.CassetteBlock.Update += ModCassetteBlock.Update;
            }
            using (new DetourConfigContext(new DetourConfig("SJAP/DisableCassetteBlockShiftSize").WithPriority(1)).Use())
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
        }

        public static void CustomLoad()
        {
            //_customHooks.Add(new Hook( typeof(CassetteZipMover).GetMethod("Sequence", BindingFlags.Instance | BindingFlags.NonPublic), ModCassetteZipper.Sequence));
            //_customHooks.Add(new Hook( typeof(LinkedZipMoverNoReturn).GetMethod("Sequence", BindingFlags.Instance | BindingFlags.NonPublic), ModLinkedNonReturnZipMover.Sequence));
            //_customHooks.Add(new Hook( typeof(LinkedZipMover).GetMethod("Sequence", BindingFlags.Instance | BindingFlags.NonPublic), ModLinkedZipMover.Sequence));
            _customHooks.Add(new Hook( typeof(DashZipMover).GetMethod("Sequence", BindingFlags.Instance | BindingFlags.NonPublic), ModDashTrafficBlock.Sequence));
            
            _customHooks.Add(new Hook( typeof(LoopBlock).GetMethod("OnDashed", BindingFlags.Instance | BindingFlags.NonPublic), ModCerealBlock.OnDashed));
            _customHooks.Add(new Hook( typeof(LoopBlock).GetMethod("Update", BindingFlags.Instance | BindingFlags.Public), ModCerealBlock.Update));
            
            _customHooks.Add(new Hook( typeof(CassetteSwapBlock).GetMethod("OnDash", BindingFlags.Instance | BindingFlags.NonPublic), ModCassetteSwapBlock.OnDash));
            _customHooks.Add(new Hook( typeof(ToggleSwapBlock).GetMethod("OnPlayerDashed", BindingFlags.Instance | BindingFlags.NonPublic), ModSJToggleSwapBlock.OnPlayerDashed));
            _customHooks.Add(new Hook( typeof(FrostHelper.ToggleSwapBlock).GetMethod("OnDash", BindingFlags.Instance | BindingFlags.NonPublic), ModFHToggleSwapBlock.OnDash));

            _customHooks.Add(new Hook( typeof(ConnectedMoveBlock).GetMethod("MoveCheck", BindingFlags.Instance | BindingFlags.NonPublic), ModConnectedMoveBlock.MoveCheck));
            _customHooks.Add(new Hook( typeof(VitMoveBlock).GetMethod("MoveCheck", BindingFlags.Instance | BindingFlags.NonPublic), ModVitMoveBlock.MoveCheck));
            
            _customHooks.Add(new Hook( typeof(NonReturnCrushBlock).GetMethod("OnDashed", BindingFlags.Instance | BindingFlags.Public), ModNonReturnKevin.OnDashed));
            _customHooks.Add(new Hook( typeof(UninterruptedNRCB).GetMethod("OnDashed", BindingFlags.Instance | BindingFlags.Public), ModUnInterruptableNonReturnKevin.OnDashed));
            
            _customHooks.Add(new Hook( typeof(MarioClearPipeHelper).GetMethod("CanTransportEntity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static), ModClearPipeHelper.CanTransportEntity));
            _customHooks.Add(new Hook( typeof(InstantTeleportTrigger).GetMethod("TeleportMaster", BindingFlags.Instance | BindingFlags.NonPublic), ModInstantTeleport.TeleportMaster));
            _customHooks.Add(new Hook( typeof(StationBlock).GetMethod("OnDashed", BindingFlags.Instance | BindingFlags.NonPublic), ModDashToggleBlock.OnDashed));
            
            _customHooks.Add(new Hook( typeof(PlatformJelly).GetMethod("Update", BindingFlags.Instance | BindingFlags.Public), ModPlatformJellyfish.Update));
        }

        public override void Unload()
        {
            On.Celeste.IntroCrusher.Sequence -= ModIntroCrusher.Sequence;
            On.Celeste.ZipMover.Update -= ModZipMover.Update;
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
        }

        public static void CustomUnload()
        {
            foreach (Hook hook in _customHooks)
            {
                hook.Dispose();
            }
        }

        private static class ModIntroCrusher
        {
            internal static IEnumerator Sequence(On.Celeste.IntroCrusher.orig_Sequence orig, IntroCrusher self)
            {
                if (HaveInteractable(CItems.IntroCrushers))
                {
                    yield return new SwapImmediately(orig(self));
                }

                yield return null;
            }
        }

        private static class ModZipMover
        {
            internal static void Update(On.Celeste.ZipMover.orig_Update orig, ZipMover self)
            {
                if (HaveInteractable(CItems.TrafficBlocks))
                {
                    orig(self);
                }
            }
        }

        private static class ModCassetteBlock
        {
            internal static void Update(On.Celeste.CassetteBlock.orig_Update orig, CassetteBlock self)
            {
                orig(self);
                if (HaveInteractable(CItems.CassetteBlocks)) return;
                self.Collidable = false;
                self.DisableStaticMovers();
                self.SetActivatedSilently(false);
            }

            internal static void ShiftSize(On.Celeste.CassetteBlock.orig_ShiftSize orig, CassetteBlock self, int shift)
            {
                // literally do nothing
            }
        }

        private static class ModDreamBlock
        {
            internal static bool Update(On.Celeste.Player.orig_DreamDashCheck orig, Player self, Vector2 v)
            {
                return HaveInteractable(CItems.DreamBlocks) && orig(self, v);
            }
        }

        private static class ModSinkingPlatform
        {
            internal static void Update(On.Celeste.SinkingPlatform.orig_Update orig, SinkingPlatform self)
            {
                if (!HaveInteractable(CItems.SinkingPlatforms))
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
                if (!HaveInteractable(CItems.MovingPlatforms))
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
                if ((!self.fragile && !HaveInteractable(CItems.BlueClouds)) ||
                    (self.fragile && !HaveInteractable(CItems.PinkClouds)))
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
                return !HaveInteractable(CItems.MoveBlocks) || orig(self, v);
            }
        }

        private static class ModSwapBlock
        {
            internal static void OnDash(On.Celeste.SwapBlock.orig_OnDash orig, SwapBlock self, Vector2 v)
            {
                if (HaveInteractable(CItems.SwapBlocks))
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
                return !HaveInteractable(CItems.DashSwitch)
                    ? DashCollisionResults.NormalCollision
                    : orig(self, player, v);
            }
        }

        private static class ModSeeker
        {
            internal static void Awake(On.Celeste.Seeker.orig_Awake orig, Seeker self, Scene scene)
            {
                // takes the death subrouting and runs it.
                if (!HaveInteractable(CItems.Seekers))
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
                return HaveInteractable(CItems.Kevins) && orig(self, v);
            }
        }

        private static class ModCoreBlock
        {
            internal static void Update(On.Celeste.BounceBlock.orig_Update orig, BounceBlock self)
            {
                if (!HaveInteractable(CItems.CoreBlocks))
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
                if ((self.iceMode && !HaveInteractable(CItems.Iceballs)))
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
                if (HaveInteractable(CItems.Pufferfish))
                {
                    orig(self);
                }
            }
        }

        private static class ModPowerBox
        {
            internal static void Break(On.Celeste.LightningBreakerBox.orig_Break orig, LightningBreakerBox self)
            {
                if (!HaveInteractable(CItems.PowerBoxes))
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
                if (HaveInteractable(CItems.DashTrafficBlocks))
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
                return HaveInteractable(CItems.CerealBlockBumps) switch
                {
                    false => DashCollisionResults.NormalCollision,
                    true => orig(self, player, v)
                };
            }

            internal static void Update(Action<LoopBlock> orig, LoopBlock self)
            {
                if (HaveInteractable(CItems.CerealBlockClouds))
                {
                    orig(self);
                }
            }
        }

        private static class ModCassetteZipper
        {
            internal static IEnumerator Sequence(Func<CassetteZipMover, IEnumerator> orig, CassetteZipMover self)
            {
                if (HaveInteractable(CItems.CassetteZippers))
                {
                    yield return new SwapImmediately(orig(self));
                }
            }
        }
        
        private static class ModCassetteSwapBlock
        {
            internal static void OnDash(Action<CassetteSwapBlock, Vector2> orig, CassetteSwapBlock self, Vector2 v)
            {
                if (HaveInteractable(CItems.SwapBlocks))
                {
                    orig(self, v);
                }
            }
        }

        private static class ModConnectedMoveBlock
        {
            internal static bool MoveCheck(Func<ConnectedMoveBlock, Vector2, bool> orig, ConnectedMoveBlock self, Vector2 v)
            {
                return !HaveInteractable(CItems.MoveBlocks) || orig(self, v);
            }
        }
        private static class ModVitMoveBlock
        {
            internal static bool MoveCheck(Func<VitMoveBlock, Vector2, bool> orig, VitMoveBlock self, Vector2 v)
            {
                return !HaveInteractable(CItems.MoveBlocks) || orig(self, v);
            }
        }

        private static class ModUnInterruptableNonReturnKevin
        {
            internal static DashCollisionResults OnDashed(
                Func<UninterruptedNRCB, Player, Vector2, DashCollisionResults> orig, UninterruptedNRCB self,
                Player player, Vector2 v)
            {
                if (HaveInteractable(CItems.Kevins))
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
                if (HaveInteractable(CItems.Kevins))
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
                return HaveInteractable(CItems.Pipes) && orig(entity, dir);
            }
        }

        private static class ModLinkedZipMover
        {
            internal static IEnumerator Sequence(Func<LinkedZipMover, IEnumerator> orig, LinkedZipMover self)
            {
                if (HaveInteractable(CItems.TrafficBlocks))
                {
                    yield return new SwapImmediately(orig(self));
                }
            }
        }

        private static class ModLinkedNonReturnZipMover
        {
            internal static IEnumerator Sequence(Func<LinkedZipMoverNoReturn, IEnumerator> orig,
                LinkedZipMoverNoReturn self)
            {
                if (HaveInteractable(CItems.TrafficBlocks))
                {
                    yield return new SwapImmediately(orig(self));
                }
            }
        }

        private static class ModInstantTeleport
        {
            internal static void TeleportMaster(Action<InstantTeleportTrigger, Player> orig,
                InstantTeleportTrigger self, Player player)
            {
                if (HaveInteractable(CItems.TeleportFields))
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
                if (HaveInteractable(CItems.SwapBlocks))
                {
                    orig(self, v);
                }
            }
        }
        
        private static class ModSJToggleSwapBlock
        {
            internal static void OnPlayerDashed(Action<ToggleSwapBlock, Vector2> orig, ToggleSwapBlock self, Vector2 v)
            {
                if (HaveInteractable(CItems.SwapBlocks))
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
                return HaveInteractable(CItems.DashToggleBlocks) switch
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
                if (!HaveInteractable(CItems.PlatformJellyfish))
                {
                    JumpThru ridablePlatform = (JumpThru)(typeof(PlatformJelly).GetField("ridablePlatform", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self));
                    ridablePlatform.Collidable = false;

                    /*i couldnt get this to work
                     MethodInfo destroyAnimationRoutine = typeof(PlatformJelly).GetMethod("DestroyAnimationRoutine", BindingFlags.NonPublic | BindingFlags.Instance);
                    self.Add(new Coroutine(destroyAnimationRoutine.Invoke(self, null)));*/
                }
            }
        }
    }
}