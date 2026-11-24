using FMOD;
using System.Collections.Generic;

namespace Celeste.Mod.SJArchipelago;

public class SJArchipelagoModuleSaveData : EverestModuleSaveData
{
    public string ConnectionData { get; set; }
    public int UnlockedDifficulty { get; set; } = 0;
    public int Strawberries { get; set; } = 0;
    public HashSet<string> UnlockedMapSIDs { get; set; } = new();
    public HashSet<long> UnlockedInteractables { get; set; } = new();
    public HashSet<long> UnlockedLocations { get; set; } = new();
    public Dictionary<string, string> MapSwapMappings { get; set; } = new();
}

