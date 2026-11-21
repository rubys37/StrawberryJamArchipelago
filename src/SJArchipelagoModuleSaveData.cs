using FMOD;
using System.Collections.Generic;

namespace Celeste.Mod.SJArchipelago;

public class SJArchipelagoModuleSaveData : EverestModuleSaveData
{
    public int UnlockedDifficulty { get; set; } = 0;
    public HashSet<string> UnlockedMapSIDs { get; set; } = new HashSet<string>();
}
