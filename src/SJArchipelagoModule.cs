using System;
using Celeste.Mod.SJArchipelago.Items;

namespace Celeste.Mod.SJArchipelago;

public class SJArchipelagoModule : EverestModule
{
    public static SJArchipelagoModule Instance { get; private set; }

    public override Type SettingsType => typeof(SJArchipelagoModuleSettings);
    public static SJArchipelagoModuleSettings Settings => (SJArchipelagoModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(SJArchipelagoModuleSession);
    public static SJArchipelagoModuleSession Session => (SJArchipelagoModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(SJArchipelagoModuleSaveData);
    public static SJArchipelagoModuleSaveData SaveData => (SJArchipelagoModuleSaveData) Instance._SaveData;


    public SJArchipelagoModule()
    {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel("AP", LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel("AP", LogLevel.Info);
#endif
    }

    public override void Load()
    {
        // TODO: apply any hooks that should always be active
        foreach (LoadableItemMod item in EntityBehavior.LoadedItemBehaviorMods)
        {
            item.Load();
        }
    }

    public override void LoadContent(bool firstLoad)
    {
        EntityBehavior.ModItemUpdate.CustomLoad();
    }

    public override void Unload()
    {
        // TODO: unapply any hooks applied in Load()
        foreach (LoadableItemMod item in EntityBehavior.LoadedItemBehaviorMods)
        {
            item.Unload();
        }
        EntityBehavior.ModItemUpdate.CustomUnload();
    }
}
