using UnityEngine;
using System.Collections;

// Author: Pierre CAMILLI

[System.Serializable]
public struct Bounds1D {

    #region Getter Setter
    [SerializeField]
    private float m_min;
    /// <summary>
    /// Minimal value
    /// </summary>
    public float Min { get { return m_min; }
        set
        {
            if (m_max < value)
                m_min = (restrict ? m_max : m_max = value);
            else
                m_min = value;
        }
    }

    [SerializeField]
    private float m_max;
    /// <summary>
    /// Maximal value
    /// </summary>
    public float Max { get { return m_max; }
        set
        {
            if (m_min > value)
                m_max = (restrict ? m_min : m_min = value);
            else
                m_max = value;
        }
    }

    /// <summary>
    /// Indicates whether or not the values push the other
    /// </summary>
    public bool restrict;
    
    public void Set(float min, float max)
    {
        if(min <= max)
        {
            m_min = min;
            m_max = max;
        }else
        {
            m_max = min;
            m_min = max;
        }
    }

    /// <summary>
    /// Difference between max and min value
    /// </summary>
    public float Difference { get { return Max - Min; } }
    #endregion

    /// <summary>
    /// One dimensional bounds
    /// </summary>
    /// <param name="min">Minimal value of the bounds</param>
    /// <param name="max">Maximal value of the bounds</param>
    /// <param name="restrict">Indicates whether or not the values push the other</param>
    Bounds1D(float min = 0.0f, float max = 0.0f, bool restrict = true)
    {
        if (min <= max)
        {
            m_min = min;
            m_max = max;
        }
        else
        {
            m_max = min;
            m_min = max;
        }
        this.restrict = restrict;
    }

    #region Operators

    public static Bounds1D operator -(Bounds1D bounds)
    {
        return new Bounds1D(-bounds.Max, -bounds.Min, bounds.restrict);
    }

    public static Bounds1D operator +(Bounds1D bounds, float value)
    {
        return new Bounds1D(bounds.Min + value, bounds.Max + value, bounds.restrict);
    }

    public static Bounds1D operator -(Bounds1D bounds, float value)
    {
        return bounds + (-value);
    }

    public static Bounds1D operator *(Bounds1D bounds, float value)
    {
        return new Bounds1D(bounds.Min * value, bounds.Max * value, bounds.restrict);
    }

    public static Bounds1D operator /(Bounds1D bounds, float value)
    {
        return bounds * (1f / value);
    }

    public static Bounds1D operator +(Bounds1D bounds1, Bounds1D bounds2)
    {
        return new Bounds1D(bounds1.Min + bounds2.Min, bounds1.Max + bounds2.Max, bounds1.restrict || bounds2.restrict);
    }

    public static Bounds1D operator -(Bounds1D bounds1, Bounds1D bounds2)
    {
        return bounds1 + (-bounds2);
    }

    #endregion

    #region Methods
    /// <summary>
    /// Clamp a value between the bounds
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float Clamp(float value)
    {
        return Mathf.Clamp(value, Min, Max);
    }

    /// <summary>
    /// Calculates the linear parameter t that produces the interpolant value within the range [a, b].
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float InverseLerp(float value)
    {
        return Mathf.InverseLerp(Min, Max, value);
    }

    /// <summary>
    /// Linearly interpolate value between the bounds
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float Lerp(float value)
    {
        return Mathf.Lerp(Min, Max, value);
    }

    /// <summary>
    /// Same as Lerp but makes sure the values interpolate correctly when they wrap around
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float LerpAngle(float value)
    {
        return Mathf.LerpAngle(Min, Max, value);
    }

    /// <summary>
    /// Linearly interpolate value between the bounds
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float LerpUnclamped(float value)
    {
        return Mathf.LerpUnclamped(Min, Max, value);
    }

    /// <summary>
    /// Increase the value of both bounds by the specificated value
    /// </summary>
    /// <param name="value"></param>
    public void Translate(float value)
    {
        m_min += value;
        m_max += value;
    }
    #endregion
}
