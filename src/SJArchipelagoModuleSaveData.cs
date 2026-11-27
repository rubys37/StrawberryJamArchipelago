using FMOD;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace Celeste.Mod.SJArchipelago;

public class SJArchipelagoModuleSaveData : EverestModuleSaveData
{
    
    public string ConnectionData { get; set; }
    
    public int Strawberries { get; set; } = 0;
    public int BeginnerHearts { get; set; } = 0;
    public int IntermediateHearts { get; set; } = 0;
    public int AdvancedHearts { get; set; } = 0;
    public int ExpertHearts { get; set; } = 0;
    public int GrandmasterHearts { get; set; } = 0;
    public Dictionary<int, int> RequiredHearts { get; set; } = new();
    public HashSet<string> UnlockedMapSIDs { get; set; } = new();
    public HashSet<string> DisallowedMapSIDs { get; set; } = new();
    public Dictionary<string, string> MapSwapMappings { get; set; } = new();
    public int ProgressiveDifficulties { get; set; } = 0;
    public List<int> EnabledDifficulties { get; set; } = Enumerable.Range(1, 17).ToList();
    [YamlIgnore] public int UnlockedDifficulty => EnabledDifficulties[ProgressiveDifficulties];

    public HashSet<long> UnlockedInteractables { get; set; } = new();
    public HashSet<long> CheckedLocations { get; set; } = new();
}

