// Mod.cs
// Entrypoint for Adjust Transit Capacity; registers settings, locales, and the ECS system.

namespace AdjustTransitCapacity
{
    using Colossal.IO.AssetDatabase;
    using Colossal.Logging;
    using Game;
    using Game.Modding;
    using Game.SceneFlow;
    using Unity.Entities;

    /// <summary>Mod entry point: registers settings, locales, and the ECS system.</summary>
    public sealed class Mod : IMod
    {
        // ----- Public constants / metadata -----
        public const string ModName = "Adjust Transit Capacity";
        public const string ModId = "AdjustTransitCapacity";
        public const string ModTag = "[ATC]";
        public const string ModVersion = "1.2.7";
        private static bool s_BannerLogged;

        // ----- Logger & public properties ----
        public static readonly ILog Log =
            LogManager.GetLogger(ModId).SetShowsErrorsInUI(false);

        public static Setting? Settings;

        public void OnLoad(UpdateSystem updateSystem)
        {
            // metadata banner (once only)
            Log.Info($"{ModName} v{ModVersion} OnLoad");
            if (!s_BannerLogged)
                s_BannerLogged = true;


            // Settings (must exist before locales so labels resolve)
            var setting = new Setting(this);
            Settings = setting;

            // Register locales BEFORE Options UI
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
                Log.Warn($"{ModTag} LocalizationManager not found; settings UI texts may be missing.");
            }

            // Load saved settings
            AssetDatabase.global.LoadSettings(ModId, setting, new Setting(this));

            // Register in options UI
            setting.RegisterInOptionsUI();

            // ECS system scheduling
            updateSystem.UpdateAfter<AdjustTransitCapacitySystem>(SystemUpdatePhase.PrefabUpdate);

            // Apply immediately for already-running city
            World world = World.DefaultGameObjectInjectionWorld;
            if (world != null)
            {
                var system = world.GetExistingSystemManaged<AdjustTransitCapacitySystem>();
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
