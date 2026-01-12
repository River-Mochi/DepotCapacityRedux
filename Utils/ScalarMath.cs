// File: Utils/ScalarMath.cs
// Purpose: Centralized clamping + scaling helpers to keep math consistent across systems (numbers).
// Notes:
// - Uses rounding to int (player-facing sliders feel better than truncation).
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
        /// Scales an integer base value by a scalar, rounds to int, then clamps to a minimum.
        /// If allowZero=true and baseValue<=0, returns 0.
        /// </summary>
        internal static int ScaleIntRounded(int baseValue, float scalar, int minIfBasePositive, bool allowZero)
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

            int v = (int)Math.Round(raw, MidpointRounding.AwayFromZero);

            if (v < minIfBasePositive)
            {
                v = minIfBasePositive;
            }

            return v;
        }

        /// <summary>Rounded scale, minimum result 1 (for typical “must be at least 1” capacities).</summary>
        internal static int ScaleIntRoundedMin1(int baseValue, float scalar)
        {
            return ScaleIntRounded(baseValue, scalar, minIfBasePositive: 1, allowZero: false);
        }

        /// <summary>
        /// Rounded scale, minimum result 1 when base is positive; returns 0 only when baseValue<=0.
        /// </summary>
        internal static int ScaleIntRoundedAllowZeroMin1(int baseValue, float scalar)
        {
            return ScaleIntRounded(baseValue, scalar, minIfBasePositive: 1, allowZero: true);
        }
    }
}
