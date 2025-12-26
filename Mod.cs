// File: Mod.cs
// Entrypoint: registers settings, locales, and the ECS systems.

namespace AdjustTransitCapacity
{
    using System;                         // Exception (localization wrapper)
    using System.Reflection;              // Metadata: Assembly version
    using Colossal;                       // IDictionarySource
    using Colossal.IO.AssetDatabase;      // AssetDatabase.LoadSettings
    using Colossal.Localization;          // LocalizationManager
    using Colossal.Logging;               // ILog, defines shared s_Log
    using Game;                           // UpdateSystem, GameManager
    using Game.Modding;                   // IMod, ModSetting base
    using Game.SceneFlow;                 // GameMode, GameManager access

    /// <summary>Mod entry point: registers settings, locales, and ECS systems.</summary>
    public sealed class Mod : IMod
    {
        // ---- PUBLIC CONSTANTS / METADATA ----
        public const string ModName = "Adjust Transit Capacity";
        public const string ShortName = "Adjust Transit";
        public const string ModId = "AdjustTransitCapacity";
        public const string ModTag = "[ATC]";

        public static readonly string ModVersion =
            Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";

        private static bool s_BannerLogged;

        public static readonly ILog s_Log =
            LogManager.GetLogger(ModId).SetShowsErrorsInUI(false);

        public static Setting? Settings;

        public void OnLoad(UpdateSystem updateSystem)
        {
            if (!s_BannerLogged)
            {
                s_BannerLogged = true;
                s_Log.Info($"{ModName} v{ModVersion} OnLoad");
            }

            // Settings first so locale labels can resolve
            Setting setting = new Setting(this);
            Settings = setting;

            // Register ALL languages (keep these lines!)
            AddLocaleSource("en-US", new LocaleEN(setting));
            //AddLocaleSource("fr-FR", new LocaleFR(setting));
            //AddLocaleSource("es-ES", new LocaleES(setting));
            //AddLocaleSource("de-DE", new LocaleDE(setting));
            //AddLocaleSource("it-IT", new LocaleIT(setting));
            //AddLocaleSource("ja-JP", new LocaleJA(setting));
            // AddLocaleSource("ko-KR", new LocaleKO(setting));
            //AddLocaleSource("pl-PL", new LocalePL(setting));
            // AddLocaleSource("pt-BR", new LocalePT_BR(setting));
            //AddLocaleSource("zh-HANS", new LocaleZH_CN(setting));    // Simplified Chinese
            // AddLocaleSource("zh-HANT", new LocaleZH_HANT(setting));  // Traditional Chinese

            AssetDatabase.global.LoadSettings(ModId, setting, new Setting(this));

            setting.RegisterInOptionsUI();

            // Both systems need prefabs ready.
            updateSystem.UpdateAfter<AdjustTransitCapacitySystem>(SystemUpdatePhase.PrefabUpdate);
            updateSystem.UpdateAfter<ServiceVehiclesSystem>(SystemUpdatePhase.PrefabUpdate);
        }

        public void OnDispose()
        {
            s_Log.Info("OnDispose");

            if (Settings != null)
            {
                Settings.UnregisterInOptionsUI();
                Settings = null;
            }
        }

        private static void AddLocaleSource(string localeId, IDictionarySource source)
        {
            if (string.IsNullOrEmpty(localeId))
            {
                return;
            }

            LocalizationManager? lm = GameManager.instance?.localizationManager;
            if (lm == null)
            {
                s_Log.Warn($"AddLocaleSource: No LocalizationManager; cannot add source for '{localeId}'.");
                return;
            }

            try
            {
                lm.AddSource(localeId, source);
            }
            catch (Exception ex)
            {
                s_Log.Warn(
                    $"AddLocaleSource: AddSource for '{localeId}' failed: {ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
