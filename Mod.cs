namespace DepotCapacityRedux
{
    using Colossal.IO.AssetDatabase;
    using Colossal.Logging;
    using Game;
    using Game.Modding;
    using Game.SceneFlow;
    using Unity.Entities;

    public sealed class Mod : IMod
    {
        public const string ModName = "Depot Capacity Redux";
        public const string ModId = "DepotCapacityRedux";
        public const string ModTag = "[DCR]";
        public const string ModVersion = "1.2.0";

        // no ".Mod" suffix per your rule
        public static readonly ILog Log =
            LogManager.GetLogger(ModId).SetShowsErrorsInUI(false);

        public static Setting? Settings;

        public void OnLoad(UpdateSystem updateSystem)
        {
            Log.Info($"{ModName} v{ModVersion} OnLoad");

            var setting = new Setting(this);
            Settings = setting;

            // register 5 locales (same pattern as MagicHearse)
            var lm = GameManager.instance?.localizationManager;
            if (lm != null)
            {
                lm.AddSource("en-US", new LocaleEN(setting));
                lm.AddSource("fr-FR", new LocaleFR(setting));
                lm.AddSource("es-ES", new LocaleES(setting));
                lm.AddSource("de-DE", new LocaleDE(setting));
                lm.AddSource("zh-HANS", new LocaleZH(setting));
            }
            else
            {
                Log.Warn("LocalizationManager not found; settings UI texts may be missing.");
            }

            // load saved settings (file name is in [FileLocation] on Setting)
            AssetDatabase.global.LoadSettings(ModId, setting, new Setting(this));

            // show in Options -> Mods
            setting.RegisterInOptionsUI();

            // make sure our system runs after prefabs are ready
            updateSystem.UpdateAfter<DepotCapacityReduxSystem>(SystemUpdatePhase.PrefabUpdate);

            // if world is alive already, trigger a run now
            var world = World.DefaultGameObjectInjectionWorld;
            if (world != null)
            {
                var sys = world.GetExistingSystemManaged<DepotCapacityReduxSystem>();
                if (sys != null)
                {
                    sys.Enabled = true;
                }
            }
        }

        public void OnDispose()
        {
            Log.Info("OnDispose");

            if (Settings != null)
            {
                Settings.UnregisterInOptionsUI();
                Settings = null;
            }
        }
    }
}
