using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Monocle;
using Celeste.Mod.CommunalHelper.DashStates;
using Celeste.Mod.MaxHelpingHand.Entities;
using ExtendedVariants.Entities.ForMappers;
using Celeste.Mod.GravityHelper.Triggers;
using vitmod;
using Celeste.Mod.StrawberryJam2021.Entities;
using MonoMod.RuntimeDetour;

namespace Celeste.Mod.SJArchipelago.Items;

internal class EntityBehavior
{
    public static List<LoadableItemMod> LoadedItemMods { get; } =
    [
        new ModItemCollision(),
        new ModItemUpdate()
    ];
    
    
    public static bool HaveInteractable(int id)
    {
        if (id == 0xCA12103) return !SJArchipelagoModule.Settings.Receiveallitems;
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
                    return HaveInteractable(0xCA12100);
                case NoDashRefillSpring:
                    return HaveInteractable(0xCA12101);
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
                    if (extraJumps == 1) return HaveInteractable(0xCA12102);
                    return HaveInteractable(0xCA12103);
                case GravityTrigger:
                    return HaveInteractable(0xCA12104);
                case TimeCrystal:
                    return HaveInteractable(0xCA12105);
                case RefillShard:
                    return HaveInteractable(0xCA12106);
                case MultiRoomStrawberrySeed:
                    return HaveInteractable(0xCA12107);
                // for some reason, triple boost flower is an internal class
                case Object obj when obj.GetType().FullName == "Celeste.Mod.StrawberryJam2021.Entities.TripleBoostFlower":
                    return HaveInteractable(0xCA12108);
                //vanilla
                case Spring:
                    return HaveInteractable(0xCA12000);
                case TouchSwitch:
                    return HaveInteractable(0xCA12005);
                // sinking platforms do not work this way (this code is never reached)
                case MovingPlatform:
                    return HaveInteractable(0xCA12006);
                case Booster { red: false }:
                    return HaveInteractable(0xCA12007);
                case Booster { red: true }:
                    return HaveInteractable(0xCA1200B);
                // clouds never reach this code
                case Cloud { fragile: false }:
                    return HaveInteractable(0xCA12008);
                case Cloud { fragile: true }:
                    return HaveInteractable(0xCA12010);
                case FlyFeather:
                    return HaveInteractable(0xCA1200D);
                case Bumper:
                    return HaveInteractable(0xCA1200E);
                case BadelineBoost:
                    return HaveInteractable(0xCA12011);
                case CoreModeToggle:
                    return HaveInteractable(0xCA12013);
                // pufferfish still explode
                case Puffer:
                    return HaveInteractable(0xCA12015);
                case Glider:
                    return HaveInteractable(0xCA12016);
                case Refill { twoDashes: false }:
                    return HaveInteractable(0xCA12018);
                case Refill { twoDashes: true }:
                    return HaveInteractable(0xCA12019);
                // sinking platforms do not work this way
                case SinkingPlatform:
                    return HaveInteractable(0xCA12020);
                case FlingBird:
                    return HaveInteractable(0xCA12023);
                default:
                    return true;
            }
        }
    }

    private class ModItemUpdate : LoadableItemMod
    {
        public override void Load()
        {
            On.Celeste.ZipMover.Update += ModZipMover.Update;
            On.Celeste.CassetteBlock.Update += ModCassetteBlock.Update;
        }

        public override void Unload()
        {
            throw new System.NotImplementedException();
        }
        
        private static class ModZipMover
        {
            internal static void Update(On.Celeste.ZipMover.orig_Update orig, ZipMover self)
            {
                if (HaveInteractable(0xCA12001))
                {
                    orig(self);
                }
            }
        }

        private static class ModCassetteBlock 
        {
            internal enum BlockColor
            {
                Pink = 0xCA12002,
                Blue = 0xCA12003,
                Yellow = 0xCA1201A,
                Green = 0xCA1201B
            }

            internal static void Update(On.Celeste.CassetteBlock.orig_Update orig, CassetteBlock self)
            {
                BlockColor bc = self.color.R switch
                {
                    240 => BlockColor.Pink,
                    73 => BlockColor.Blue,
                    252 => BlockColor.Yellow,
                    56 => BlockColor.Green,
                    _ => BlockColor.Pink
                };
                if (HaveInteractable((int)bc))
                {
                    orig(self);
                }
                else
                {
                    if (self.Activated)
                    {
                        self.ShiftSize(-1);
                        self.SetActivatedSilently(false);
                    }
                }
            }
        }

        private static class ModDreamBlock
        {
            private static bool ModPlayer_DreamDashCheck(On.Celeste.Player.orig_DreamDashCheck orig, Player self, Microsoft.Xna.Framework.Vector2 dir)
            {
                if (HaveInteractable(0xCA12004))
                {
                    return orig(self, dir);
                }
                return false;
            }
        }
    }
}

