// File: Systems/ExtractorTransportCompanySystem.cs
// Purpose: Apply ExtractorMaxTrucksScalar to targeted Industrial transport companies (TransportCompanyData.m_MaxTransports).
// Targets:
// - Industrial_*Extractor* (your original intent)
// - PLUS Industrial_*Coal* / Industrial_*Stone* / *Mine* / *Quarry* (fixes coal/stone cases that don't include "Extractor")
// Notes:
// - Runs once when enabled (Settings.Apply enables it).
// - Stores original values in-memory so changing the slider during gameplay doesn't stack.
// - Skips companies where OriginalMaxTransports == 0 (no vehicles to control).

namespace DispatchBoss
{
    using Game;
    using Game.Companies;        // TransportCompanyData
    using Game.Prefabs;          // PrefabSystem, PrefabBase
    using System;
    using System.Collections.Generic;
    using Unity.Collections;
    using Unity.Entities;

    public sealed partial class ExtractorTransportCompanySystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem = null!;
        private EntityQuery m_Query;

        // Original max transports per prefab entity (RAM only; resets on game restart).
        private readonly Dictionary<Entity, int> m_OriginalMax = new Dictionary<Entity, int>();

        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            m_Query = GetEntityQuery(
                ComponentType.ReadOnly<PrefabData>(),
                ComponentType.ReadWrite<TransportCompanyData>());

            RequireForUpdate(m_Query);

            Enabled = false; // one-shot: Settings.Apply enables it
        }

        protected override void OnGameLoadingComplete(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            bool isRealGame =
                mode == GameMode.Game &&
                (purpose == Colossal.Serialization.Entities.Purpose.NewGame || purpose == Colossal.Serialization.Entities.Purpose.LoadGame);

            if (!isRealGame)
            {
                return;
            }

            // Keep originals for the session (so the slider can be changed without stacking).
            Enabled = true;
        }

        protected override void OnUpdate()
        {
            Enabled = false;

            if (Mod.Settings == null)
            {
                return;
            }

            bool verbose = Mod.Settings.EnableDebugLogging;

            float scalarF = Mod.Settings.ExtractorMaxTrucksScalar;
            if (scalarF <= 0f)
            {
                scalarF = 1f;
            }

            int scalar = (int)Math.Round(scalarF);
            if (scalar < 1) scalar = 1;
            if (scalar > (int)Setting.CargoStationMaxScalar) scalar = (int)Setting.CargoStationMaxScalar;

            int matched = 0;
            int changed = 0;
            int skippedZero = 0;

            using NativeArray<Entity> entities = m_Query.ToEntityArray(Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                Entity e = entities[i];

                string name = GetPrefabNameSafe(e);
                if (!IsTargetIndustrialExtractorCompany(name))
                {
                    continue;
                }

                TransportCompanyData tc = EntityManager.GetComponentData<TransportCompanyData>(e);

                if (!m_OriginalMax.TryGetValue(e, out int original))
                {
                    original = tc.m_MaxTransports;
                    m_OriginalMax[e] = original;
                }

                // If original is 0, there's nothing meaningful to scale.
                if (original == 0)
                {
                    skippedZero++;
                    continue;
                }

                matched++;

                int desired = original * scalar;
                if (desired < 0) desired = 0;

                if (tc.m_MaxTransports != desired)
                {
                    tc.m_MaxTransports = desired;
                    EntityManager.SetComponentData(e, tc);
                    changed++;

                    if (verbose)
                    {
                        Mod.s_Log.Info($"{Mod.ModTag} Extractor trucks: '{name}' Base={original} x{scalar} -> {desired}");
                    }
                }
            }

            Mod.s_Log.Info($"{Mod.ModTag} Extractor trucks: scalar={scalar} matched={matched} changed={changed} skippedZero={skippedZero}");
        }

        private static bool IsTargetIndustrialExtractorCompany(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            // Must be Industrial_*
            if (!name.StartsWith("Industrial_", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Primary: *Extractor*
            if (name.IndexOf("Extractor", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            // Fix: some extractors (especially coal/stone) are often named Mine/Quarry variants.
            // Include Coal / Stone explicitly plus Mine / Quarry as a safety net.
            if (name.IndexOf("Coal", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (name.IndexOf("Stone", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (name.IndexOf("Quarry", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (name.IndexOf("Mine", StringComparison.OrdinalIgnoreCase) >= 0) return true;

            return false;
        }

        private string GetPrefabNameSafe(Entity prefabEntity)
        {
            try
            {
                if (m_PrefabSystem != null &&
                    m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase pb))
                {
                    return pb.name ?? "(unnamed)";
                }
            }
            catch
            {
                // ignore
            }

            return $"PrefabEntity={prefabEntity.Index}:{prefabEntity.Version}";
        }
    }
}
