// File: Utils/ScalarMath.cs
// Purpose: Centralized clamping + scaling helpers to keep math consistent across systems (numbers).
// Notes:
// - Uses truncation to int (matches previous behavior) to minimize behavioral surprises.
// - Provides "allowZero" scaling for prefabs that legitimately have 0 base values.

namespace DispatchBoss
{
    using System;

    internal static class ScalarMath
    {
        internal static float Clamp(float v, float min, float max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }

        internal static int ClampInt(int v, int min, int max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }

        internal static float PercentToScalarClamped(float percent, float minPercent, float maxPercent)
        {
            percent = Clamp(percent, minPercent, maxPercent);
            return percent / 100f;
        }

        internal static float ClampScalar(float scalar, float minScalar, float maxScalar)
        {
            return Clamp(scalar, minScalar, maxScalar);
        }

        /// <summary>
        /// Scales an integer base value by a scalar, truncates to int, then clamps to a minimum.
        /// If allowZero=true and baseValue<=0, returns 0.
        /// </summary>
        internal static int MulIntTruncate(int baseValue, float scalar, int minIfBasePositive, bool allowZero)
        {
            if (allowZero && baseValue <= 0)
            {
                return 0;
            }

            if (baseValue < 1)
            {
                baseValue = 1;
            }

            // Guard against negative scalars (should be clamped before calling, but be safe).
            if (scalar < 0f)
            {
                scalar = 0f;
            }

            double raw = baseValue * (double)scalar;
            if (raw > int.MaxValue)
            {
                return int.MaxValue;
            }

            int v = (int)raw; // truncate (matches previous behavior)
            if (v < minIfBasePositive)
            {
                v = minIfBasePositive;
            }

            return v;
        }

        internal static int MulIntTruncateMin1(int baseValue, float scalar)
        {
            return MulIntTruncate(baseValue, scalar, minIfBasePositive: 1, allowZero: false);
        }

        internal static int MulIntTruncateAllowZeroMin1(int baseValue, float scalar)
        {
            return MulIntTruncate(baseValue, scalar, minIfBasePositive: 1, allowZero: true);
        }
    }
}
