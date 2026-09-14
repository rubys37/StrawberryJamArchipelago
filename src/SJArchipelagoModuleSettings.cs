namespace Celeste.Mod.SJArchipelago;

public class SJArchipelagoModuleSettings : EverestModuleSettings
{
    public bool DisableAllItems { get; set; } = false;
    public ReceiveSpecificItems ReceiveItems { get; set; } = new();
        
    [SettingSubMenu]
    public class ReceiveSpecificItems
    {
        public bool ReceiveIntroCrushers { get; set; } = true;
        public bool ReceiveSprings { get; set; } = true;
        public bool ReceiveTrafficBlocks { get; set; } = true;
        public bool ReceiveDashCrystals { get; set; } = true;
        public bool ReceiveDoubleDashCrystals { get; set; } = true;
        public bool ReceiveCassetteBlocks { get; set; } = true;
        public bool ReceiveDreamBlocks { get; set; } = true;
        public bool ReceiveStrawberrySeeds { get; set; } = true;
        public bool ReceiveCoins { get; set; } = true;
        public bool ReceiveSinkingPlatforms { get; set; } = true;
        public bool ReceiveMovingPlatforms { get; set; } = true;
        public bool ReceiveBlueClouds { get; set; } = true;
        public bool ReceivePinkClouds { get; set; } = true;
        public bool ReceiveGreenBubbles { get; set; } = true;
        public bool ReceiveRedBubbles { get; set; } = true;
        public bool ReceiveMoveBlocks { get; set; } = true;
        public bool ReceiveWhiteBlock { get; set; } = true;
        public bool ReceiveSwapBlocks { get; set; } = true;
        public bool ReceiveDashSwitch { get; set; } = true;
        public bool ReceiveSeekers { get; set; } = true;
        public bool ReceiveTheoCrystals { get; set; } = true;
        public bool ReceiveFeathers { get; set; } = true;
        public bool ReceiveKevins { get; set; } = true;
        public bool ReceiveBumpers { get; set; } = true;
        public bool ReceiveBadelineOrbs { get; set; } = true;
        public bool ReceiveCoreBlocks { get; set; } = true;
        public bool ReceiveIceballs { get; set; } = true;
        public bool ReceiveCoreSwitches { get; set; } = true;
        public bool ReceivePufferfish { get; set; } = true;
        public bool ReceiveJellyfish { get; set; } = true;
        public bool ReceivePowerBoxes { get; set; } = true;
        public bool ReceiveBirds { get; set; } = true;
        
        public bool ReceiveDashTrafficBlocks { get; set; } = true;
        public bool ReceiveCerealBlockBumps { get; set; } = true;
        public bool ReceiveCerealBlockClouds { get; set; } = true;
        public bool ReceiveDreamDashCrystals { get; set; } = true;
        public bool ReceiveBlueSprings { get; set; } = true;
        public bool ReceiveBlueBubbles { get; set; } = true;
        public bool ReceiveCassetteZippers { get; set; } = true;
        public bool ReceiveSingleJumpCrystals { get; set; } = true;
        public bool ReceiveTripleJumpCrystals { get; set; } = true;
        public bool ReceiveGravityTriggers { get; set; } = true;
        public bool ReceiveBlueTimeCrystals { get; set; } = true;
        public bool ReceiveGrayTimeCrystals { get; set; } = true;
        public bool ReceiveDashCrystalShards { get; set; } = true;
        public bool ReceiveTinyStrawberries { get; set; } = true;
        public bool ReceiveRoses { get; set; } = true;
        public bool ReceivePipes { get; set; } = true;
        public bool ReceiveTeleportFields { get; set; } = true;
        public bool ReceiveZiplines { get; set; } = true;
        public bool ReceiveDashToggleBlocks { get; set; } = true;
        public bool ReceiveCrystalBombs { get; set; } = true;
        public bool ReceivePurpleBubbles { get; set; } = true;
        public bool ReceiveDashSprings { get; set; } = true;
        public bool ReceivePlatformJellyfish { get; set; } = true;
        public bool ReceiveGrayBooster { get; set; } = true;
        public bool ReceiveSkateboard { get; set; } = true;
        public bool ReceiveUmbrella { get; set; } = true;
        public bool ReceiveFakeHearts { get; set; } = true;
        public bool ReceiveWormholeBoosters { get; set; } = true;
        public bool ReceivePortals { get; set; } = true;
        public bool ReceivePinkBubbles { get; set; } = true;
    }
}