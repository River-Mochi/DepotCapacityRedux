// File: Utils/PrefabComponentUtil.cs
// Purpose: prefab components lookup (TransportDepot, PublicTransport, etc.).
// Notes:
// - Centralizes PrefabSystem.TryGetPrefab + PrefabBase.TryGet.
// - PrefabBase.TryGet<T> requires T : ComponentBase.

namespace DispatchBoss
{
    using Game.Prefabs;
    using Unity.Entities;

    internal static class PrefabComponentUtil
    {
        internal static bool TryGetPrefabBase(PrefabSystem prefabSystem, Entity prefabEntity, out PrefabBase prefabBase)
        {
            prefabBase = null!;
            if (prefabSystem == null)
            {
                return false;
            }

            return prefabSystem.TryGetPrefab(prefabEntity, out prefabBase);
        }

        internal static bool TryGetComponent<T>(PrefabSystem prefabSystem, Entity prefabEntity, out T component)
            where T : ComponentBase
        {
            component = null!;

            if (!TryGetPrefabBase(prefabSystem, prefabEntity, out PrefabBase prefabBase))
            {
                return false;
            }

            return prefabBase.TryGet(out component);
        }
    }
}
