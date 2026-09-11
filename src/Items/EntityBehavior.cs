using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;

using Celeste.Mod.CommunalHelper.DashStates;
using Celeste.Mod.CommunalHelper.Entities;
using Celeste.Mod.MaxHelpingHand.Entities;
using ExtendedVariants.Entities.ForMappers;
using Celeste.Mod.GravityHelper.Triggers;
using vitmod;
using FrostTempleHelper.Entities;
using Celeste.Mod.StrawberryJam2021.Entities;
using FrostHelper.Entities.Boosters;

using Monocle;
using MonoMod.RuntimeDetour;

namespace Celeste.Mod.SJArchipelago.Items;


internal class EntityBehavior
{
    public static List<LoadableItemMod> LoadedItemBehaviorMods { get; } =
    [
        new ModItemCollision(),
        new ModItemUpdate()
    ];
    
    
    public static bool HaveInteractable(Enum items)
    {
        return SJArchipelagoModule.Settings.Receiveallitems;
    }

    public class ModItemCollision : LoadableItemMod
    {
        public override void Load()
        {
            On.Monocle.Collide.Check_Entity_Entity += modCollide_EntityCheck; 
        }

        public override void Unload()
        {
            On.Monocle.Collide.Check_Entity_Entity -= modCollide_EntityCheck;
        }

        /* Handles every entity that should have no collision when disabled by the ap
        TODO: Add a list of every entity handled here*/
        private static bool modCollide_EntityCheck(On.Monocle.Collide.orig_Check_Entity_Entity orig, Entity a, Entity b)
        {
            if (!orig(a, b))
            {
                return false;
            }
            // this makes debugging easier i promise
            if (b.GetType() == typeof(SolidTiles)) return true;
            switch (b)
            {
                // modded
                case DreamTunnelRefill:
                    return HaveInteractable(EntityHandler.Items.DreamDashCrystals);
                case NoDashRefillSpring:
                    return HaveInteractable(EntityHandler.Items.BlueSprings);
                case BlueBooster:
                    return HaveInteractable(EntityHandler.Items.BlueBubbles);
                case JumpRefill jumpRefill:
                    // i shamelessly copypasted this reflection from stackoverflow
                    Type t = jumpRefill.GetType();
                    FieldInfo fi = null;
                    while (fi == null && t != null)
                    {
                        fi = t.GetField("extraJumps", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        t = t.BaseType;
                    }
                    int extraJumps;
                    if (fi == null) extraJumps = 1;
                    else extraJumps = (int)fi.GetValue(jumpRefill);
                    if (extraJumps == 1) return HaveInteractable(EntityHandler.Items.SingleJumpCrystals);
                    return HaveInteractable(EntityHandler.Items.TripleJumpCrystals);
                case GravityTrigger:
                    return HaveInteractable(EntityHandler.Items.GravityTriggers);
                case TimeCrystal:
                    return HaveInteractable(EntityHandler.Items.TimeCrystals);
                case RefillShard:
                    return HaveInteractable(EntityHandler.Items.DashCrystalShards);
                case MultiRoomStrawberrySeed:
                    return HaveInteractable(EntityHandler.Items.TinyStrawberries);
                // for some reason, triple boost flower is an internal class
                case Object obj when obj.GetType().FullName == "Celeste.Mod.StrawberryJam2021.Entities.TripleBoostFlower":
                    return HaveInteractable(EntityHandler.Items.Roses);
                //vanilla
                case Spring:
                    return HaveInteractable(EntityHandler.Items.Spring);
                case Refill { twoDashes: false }:
                    return HaveInteractable(EntityHandler.Items.DashCrystals);
                case Refill { twoDashes: true }:
                    return HaveInteractable(EntityHandler.Items.DoubleDashCrystals);
                case StrawberrySeed:
                    return HaveInteractable(EntityHandler.Items.StrawberrySeeds);
                case TouchSwitch:
                    return HaveInteractable(EntityHandler.Items.Coins);
                case Booster { red: false }:
                    return HaveInteractable(EntityHandler.Items.GreenBubbles);
                case Booster { red: true }:
                    return HaveInteractable(EntityHandler.Items.RedBubbles);
                case WhiteBlock:
                    return HaveInteractable(EntityHandler.Items.WhiteBlock);
                case TheoCrystal:
                    return HaveInteractable(EntityHandler.Items.TheoCrystals);
                case FlyFeather:
                    return HaveInteractable(EntityHandler.Items.Feathers);
                case Bumper:
                    return HaveInteractable(EntityHandler.Items.Bumpers);
                case BadelineBoost:
                    return HaveInteractable(EntityHandler.Items.BadelineOrbs);
                case CoreModeToggle:
                    return HaveInteractable(EntityHandler.Items.CoreSwitches);
                case Puffer:
                    return HaveInteractable(EntityHandler.Items.Pufferfish);
                case Glider:
                    return HaveInteractable(EntityHandler.Items.Jellyfish);
                case FlingBird:
                    return HaveInteractable(EntityHandler.Items.Birds);
                default:
                    return true;
            }
        }
    }

    public class ModItemUpdate : LoadableItemMod
    {
        private static List<Hook> _customHooks = new List<Hook>();
        public override void Load()
        {
            On.Celeste.IntroCrusher.Sequence += ModIntroCrusher.Sequence;
            On.Celeste.ZipMover.Update += ModZipMover.Update;
            On.Celeste.CassetteBlock.Update += ModCassetteBlock.Update;
            On.Celeste.Player.DreamDashCheck += ModDreamBlock.Update;
            On.Celeste.SinkingPlatform.Update += ModSinkingPlatform.Update;
            On.Celeste.MovingPlatform.Update += ModMovingPlatform.Update;
            On.Celeste.Cloud.Update += ModCloud.Update;
            On.Celeste.MoveBlock.MoveCheck += ModMoveBlock.MoveCheck;
            On.Celeste.SwapBlock.OnDash += ModSwapBlock.OnDash;
            On.Celeste.DashSwitch.OnDashed += ModDashSwitch.OnDashed;
            On.Celeste.Seeker.Awake += ModSeeker.Awake;
            On.Celeste.CrushBlock.CanActivate += ModCrushBlock.CanActivate;
            On.Celeste.BounceBlock.Update += ModCoreBlock.Update;
            On.Celeste.FireBall.Update += ModFireball.Update;
            On.Celeste.Puffer.Explode += ModPuffer.Explode;
            On.Celeste.LightningBreakerBox.Break += ModPowerBox.Break;

        }

        public static void CustomLoad()
        {
            // using reflection to get access to modded methods
            _customHooks.Add(new Hook(typeof(DashZipMover).GetMethod("Sequence", BindingFlags.Instance | BindingFlags.NonPublic), ModDashTrafficBlock.Sequence));
            _customHooks.Add(new Hook(typeof(ConnectedMoveBlock).GetMethod("MoveCheck", BindingFlags.Instance | BindingFlags.NonPublic), ModConnectedMoveBlock.MoveCheck));
            _customHooks.Add(new Hook(typeof(ConnectedMoveBlock).GetMethod("MoveCheck", BindingFlags.Instance | BindingFlags.NonPublic), ModConnectedMoveBlock.MoveCheck));
            _customHooks.Add(new Hook(typeof(ConnectedDreamBlock).GetMethod("MoveCheck", BindingFlags.Instance | BindingFlags.NonPublic), ModConnectedMoveBlock.MoveCheck));
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
                if (HaveInteractable(EntityHandler.Items.IntroCrusher))
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
                if (HaveInteractable(EntityHandler.Items.TrafficBlock))
                {
                    orig(self); 
                }
            }
        }

        private static class ModCassetteBlock 
        {
            internal static void Update(On.Celeste.CassetteBlock.orig_Update orig, CassetteBlock self)
            {
                Enum id = self.Index switch
                {
                    0 => EntityHandler.Items.BlueCassetteBlock,
                    1 => EntityHandler.Items.PinkCassetteBlock,
                    2 => EntityHandler.Items.YellowCassetteBlock,
                    3 => EntityHandler.Items.GreenCassetteBlock,
                    _ => EntityHandler.Items.BlueCassetteBlock
                };
                if (HaveInteractable(id))
                {
                    orig(self);
                }
                else
                {
                    self.ShiftSize(-1);
                    self.SetActivatedSilently(false);
                }
            }
        }

        private static class ModDreamBlock
        {
            internal static bool Update(On.Celeste.Player.orig_DreamDashCheck orig, Player self, Vector2 v)
            {
                return HaveInteractable(EntityHandler.Items.DreamBlock) && orig(self, v);
            }
        }

        private static class ModSinkingPlatform
        {
            internal static void Update(On.Celeste.SinkingPlatform.orig_Update orig, SinkingPlatform self)
            {
                if (!HaveInteractable(EntityHandler.Items.SinkingPlatforms))
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
                if (!HaveInteractable(EntityHandler.Items.MovingPlatforms))
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
                if ((!self.fragile && !HaveInteractable(EntityHandler.Items.BlueClouds)) || (self.fragile && !HaveInteractable(EntityHandler.Items.PinkClouds)))
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
                return !HaveInteractable(EntityHandler.Items.MoveBlocks) || orig(self, v);
            }
        }

        private static class ModSwapBlock
        {
            internal static void OnDash(On.Celeste.SwapBlock.orig_OnDash orig, SwapBlock self, Vector2 v)
            {
                if (!HaveInteractable(EntityHandler.Items.SwapBlocks))
                {
                    return;
                }
                orig(self, v);
            }
        }
        
        private static class ModDashSwitch
        {
            internal static DashCollisionResults OnDashed(On.Celeste.DashSwitch.orig_OnDashed orig, DashSwitch self, Player player, Vector2 v)
            {
                return !HaveInteractable(EntityHandler.Items.DashSwitch) ? DashCollisionResults.NormalCollision : orig(self, player, v);
            }
        }
        
        private static class ModSeeker
        {
            internal static void Awake(On.Celeste.Seeker.orig_Awake orig, Seeker self, Scene scene)
            {
                // takes the death subrouting and runs it.
                if (!HaveInteractable(EntityHandler.Items.Seekers))
                {
                    Entity entity = new Entity(self.Position);
                    DeathEffect component = new DeathEffect(Color.HotPink, self.Center - self.Position)
                    {
                        OnEnd = delegate
                        {
                            entity.RemoveSelf();
                        }
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
                return HaveInteractable(EntityHandler.Items.Kevins) && orig(self, v);
            }
        }
        
        private static class ModCoreBlock
        {
            internal static void Update(On.Celeste.BounceBlock.orig_Update orig, BounceBlock self)
            {
                if (!HaveInteractable(EntityHandler.Items.CoreBlocks))
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
                if ((self.iceMode && !HaveInteractable(EntityHandler.Items.Iceballs)))
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
                if (HaveInteractable(EntityHandler.Items.Pufferfish))
                {
                    orig(self);
                }
            }
        }
        
        private static class ModPowerBox
        {
            internal static void Break(On.Celeste.LightningBreakerBox.orig_Break orig, LightningBreakerBox self)
            {
                if (!HaveInteractable(EntityHandler.Items.PowerBoxes))
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
                
                if (HaveInteractable(EntityHandler.Items.DashTrafficBlocks))
                {
                    yield return new SwapImmediately(orig(self));
                }
            }
        }
        
        private static class ModConnectedMoveBlock
        {
            internal static bool MoveCheck(Func<ConnectedMoveBlock, bool> orig, ConnectedMoveBlock self)
            {
                return HaveInteractable(EntityHandler.Items.MoveBlocks) && orig(self);
            }
        }
    }
}

