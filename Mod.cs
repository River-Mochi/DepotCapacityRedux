// File: Mod.cs
// Entrypoint: registers settings, locales, and the ECS systems.

namespace DispatchBoss
{
    using Colossal;                       // IDictionarySource
    using Colossal.IO.AssetDatabase;      // AssetDatabase.LoadSettings
    using Colossal.Localization;          // LocalizationManager
    using Colossal.Logging;               // ILog, defines shared s_Log
    using Game;                           // UpdateSystem, GameManager, SystemUpdatePhase
    using Game.Modding;                   // IMod, ModSetting base
    using Game.SceneFlow;
    using System;                         // Exception (localization wrapper)
    using System.Reflection;              // Metadata: Assembly version

    /// <summary>Mod entry point: registers settings, locales, and ECS systems.</summary>
    public sealed class Mod : IMod
    {
        // ---- PUBLIC CONSTANTS / METADATA ----
        public const string ModName = "Dispatch Boss";
        public const string ShortName = "Dispatch Boss";
        public const string ModId = "DispatchBoss";
        public const string ModTag = "[DB]";

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
            AddLocaleSource("fr-FR", new LocaleFR(setting));
            AddLocaleSource("es-ES", new LocaleES(setting));
            AddLocaleSource("de-DE", new LocaleDE(setting));
            AddLocaleSource("it-IT", new LocaleIT(setting));
            AddLocaleSource("ja-JP", new LocaleJA(setting));
            AddLocaleSource("ko-KR", new LocaleKO(setting));
            AddLocaleSource("pl-PL", new LocalePL(setting));
            AddLocaleSource("pt-BR", new LocalePT_BR(setting));
            AddLocaleSource("zh-HANS", new LocaleZH_CN(setting));    // Simplified Chinese
            AddLocaleSource("zh-HANT", new LocaleZH_HANT(setting));  // Traditional Chinese

            AssetDatabase.global.LoadSettings(ModId, setting, new Setting(this));
            setting.RegisterInOptionsUI();

            // Systems
            updateSystem.UpdateAfter<TransitSystem>(SystemUpdatePhase.PrefabUpdate);
            updateSystem.UpdateAfter<MaintenanceSystem>(SystemUpdatePhase.PrefabUpdate);
            updateSystem.UpdateAfter<LaneWearSystem>(SystemUpdatePhase.PrefabUpdate);

            // Extractors (TransportCompanyData.m_MaxTransports)
            updateSystem.UpdateAfter<IndustrySystem>(SystemUpdatePhase.PrefabUpdate);

            // Allow transit lines range to be 1-and higher than vanilla
            // Policy tuner: also better in PrefabUpdate so it applies immediately while paused/Options UI
            updateSystem.UpdateAfter<VehicleCountPolicyTunerSystem>(SystemUpdatePhase.PrefabUpdate);

            // Prefab scan: must work even while Options UI is open
            updateSystem.UpdateAt<PrefabScanSystem>(SystemUpdatePhase.PrefabUpdate);

            // Debug probe: logs LaneCondition.m_Wear deltas (runtime)
            updateSystem.UpdateAt<LaneWearProbeSystem>(SystemUpdatePhase.GameSimulation);


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

        //---------------
        // HELPERS
        //---------------

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

        // Helper to localize Status text.
        internal static string L(string id, string fallback)
        {
            try
            {
                LocalizationManager? lm = GameManager.instance?.localizationManager;
                if (lm != null &&
                    lm.activeDictionary != null &&
                    lm.activeDictionary.TryGetValue(id, out string result))
                {
                    return result;
                }
            }
            catch
            {
                // ignore and fall back
            }

            return fallback;
        }

    }
}
