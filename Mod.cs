// Mod.cs
// Entrypoint for Depot Capacity Redux; registers settings, locales, and the ECS system.

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

            // 1) create settings
            Setting setting = new Setting(this);
            Settings = setting;

            // 2) register locales (top-level classes, NOT Setting.LocaleEN)
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

            // 3) load saved settings from our fixed path
            AssetDatabase.global.LoadSettings(ModId, setting, new Setting(this));

            // 4) show in Options → Mods
            setting.RegisterInOptionsUI();

            // 5) make sure our ECS system runs after prefab data is ready
            updateSystem.UpdateAfter<DepotCapacityReduxSystem>(SystemUpdatePhase.PrefabUpdate);

            // 6) if a world is already running, apply once right now
            World world = World.DefaultGameObjectInjectionWorld;
            if (world != null)
            {
                DepotCapacityReduxSystem system = world.GetExistingSystemManaged<DepotCapacityReduxSystem>();
                if (system != null)
                {
                    system.Enabled = true;
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
