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
        public const string SettingsPath = "ModsSettings/DepotCapacityRedux/DepotCapacityRedux";

        public static readonly ILog Log =
            LogManager.GetLogger(ModId).SetShowsErrorsInUI(false);

        public static Setting? Settings;

        public void OnLoad(UpdateSystem updateSystem)
        {
            Log.Info($"{ModName} v{ModVersion} OnLoad");

            // 1) Settings
            Setting setting = new Setting(this);
            Settings = setting;

            // 2) Locales
            var lm = GameManager.instance?.localizationManager;
            if (lm != null)
            {
                lm.AddSource("en-US", new Setting.LocaleEN(setting));
                lm.AddSource("fr-FR", new Setting.LocaleFR(setting));
                lm.AddSource("es-ES", new Setting.LocaleES(setting));
                lm.AddSource("de-DE", new Setting.LocaleDE(setting));
                lm.AddSource("zh-HANS", new Setting.LocaleZH(setting));
            }
            else
            {
                Log.Warn("LocalizationManager not found; settings UI texts may be missing.");
            }

            // 3) Load saved settings from fixed path
            AssetDatabase.global.LoadSettings(SettingsPath, setting, new Setting(this));

            // 4) Show in Options Menu
            setting.RegisterInOptionsUI();

            // 5) Schedule system after prefab update
            updateSystem.UpdateAfter<DepotCapacityReduxSystem>(SystemUpdatePhase.PrefabUpdate);

            // 6) if world already exists, apply once
            World world = World.DefaultGameObjectInjectionWorld;
            if (world != null)
            {
                DepotCapacityReduxSystem system =
                    world.GetExistingSystemManaged<DepotCapacityReduxSystem>();
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
