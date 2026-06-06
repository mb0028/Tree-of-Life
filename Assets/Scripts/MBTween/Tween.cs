using System;
using UnityEngine;

public interface ITween
{
    object Target { get; }
    bool IsComplete { get; }
    bool IsPaused { get; }
    bool IgnoreTimescale { get; }
    string Id { get; }
    bool IsTargetDestroyed();
    void Pause();
    void Resume();
    void Update();
}

public class Tween<T> : ITween
{
    private T startValue;
    private T endValue;
    private float duration;
    private float elapsedTime;
    private Action<T> onTweenUpdate;
    private EaseMode easeMode = EaseMode.Linear;

    public object Target { get; private set; }
    public bool IsComplete { get; private set; }
    public bool IsPaused { get; private set; }
    public bool IgnoreTimescale { get; private set; }
    public string Id { get; private set; }

    public Tween(object target, string id, T startVal, T endVal, float duration, Action<T> onUpdate, EaseMode ease)
    {
        Target = target;
        Id = id;
        startValue = startVal;
        endValue = endVal;
        this.duration = duration;
        onTweenUpdate = onUpdate;
        easeMode = ease;
        IgnoreTimescale = false;

        MBTweenManager.Instance.AddTween<T>(this);
    }

    public void Update()
    {
        if (IsComplete || IsPaused || IsTargetDestroyed())
            return;

        elapsedTime += IgnoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;
        float t = elapsedTime / duration;

        onTweenUpdate.Invoke(Interpolate(startValue, endValue, Ease(easeMode, t)));

        if (elapsedTime >= duration)
            IsComplete = true;
    }

    private T Interpolate(T start, T end, float t)
    {
        if (start is float startFloat && end is float endFloat)
            return (T)(object)Mathf.LerpUnclamped(startFloat, endFloat, t);

        if (start is Vector3 startVec3 && end is Vector3 endVec3)
            return (T)(object)Vector3.LerpUnclamped(startVec3, endVec3, t);

        if (start is Color startCol && end is Color endCol)
            return (T)(object)Color.Lerp(startCol, endCol, t);

        throw new InvalidOperationException();
    }

    public bool IsTargetDestroyed()
    {
        if (Target.GetType() == null)
            return true;
        if (Target is MonoBehaviour mono && mono == null)
            return true;
        if (Target is GameObject g && g == null)
            return true;
        if (Target is Delegate del && del.Target == null)
            return true;
        if (Target is RectTransform rt && rt == null)
            return true;
        if (Target is AudioSource aus && aus == null)
            return true;

        return false;
    }

    public void Pause() => IsPaused = true;
    public void Resume() => IsPaused = false;
    
    public Tween<T> Unscaled()
    {
        IgnoreTimescale = true;
        return this;
    }

    /// <summary> Easing mode maths from https://easings.net/# </summary>
    public static float Ease(EaseMode ease, float t) => ease switch
    {
        EaseMode.Linear => t,
        EaseMode.EaseOutCirc => OutCirc(t),
        EaseMode.EaseOutBack => OutBack(t),
        EaseMode.EaseOutBounce => OutBounce(t),
        _ => t,
    };
    private const float c1 = 1.7015f;
    private const float c3 = c1 + 1;
    private const float n1 = 7.5625f;
    private const float d1 = 2.75f;
    private static float OutCirc(float t) => Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));
    private static float OutBack(float t) => 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    private static float OutBounce(float t)
    {
        if (t < 1 / d1)
            return n1 * t * t;
        else if (t < 2 / d1)
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        else if (t < 2.5 / d1)
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        else 
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
    }
}

public enum EaseMode
{
    Linear,
    EaseOutCirc,
    EaseOutBack,
    EaseOutBounce,
}