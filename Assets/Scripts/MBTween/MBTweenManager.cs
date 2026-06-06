using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MBTweenManager : MonoBehaviour
{
    private static MBTweenManager instance;
    public static MBTweenManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("MB Tween Manager").AddComponent<MBTweenManager>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    private Dictionary<string, ITween> activeTweens = new();

    public void AddTween<T>(ITween tween) => activeTweens[tween.Id] = tween;
    public void RemoveTween<T>(string id) => activeTweens.Remove(id);

    void Update()
    {
        foreach (var t in activeTweens.ToList())
        {
            if (t.Value.IsComplete || t.Value.IsTargetDestroyed())
            {
                activeTweens.Remove(t.Value.Id);
                continue;
            }
            t.Value.Update();
        }
    }
}

public static class MBTweens
{
    public static Tween<Vector3> ScaleTo(this Transform transform, Vector3 end, float duration, EaseMode easeMode = EaseMode.Linear)
    {
        long id = transform.GetHashCode() + Random.Range(int.MinValue, int.MaxValue);
        Tween<Vector3> tween = new(transform, id.ToString(), transform.localScale, end, duration, f =>
        {
            transform.localScale = f;
        }, easeMode);
        return tween;
    }

    public static Tween<float> VolumeTo(this AudioSource audioSource, float end, float duration, EaseMode easeMode = EaseMode.Linear)
    {
        long id = audioSource.GetHashCode() + Random.Range(int.MinValue, int.MaxValue);
        Tween<float> tween = new(audioSource, id.ToString(), audioSource.volume, Mathf.Clamp01(end), duration, f =>
        {
            audioSource.volume = Mathf.Clamp01(f);
        }, easeMode);
        return tween;
    }

    public static Tween<float> FloatTo01(this Material mat, string matNameID, float end, float duration, EaseMode easeMode = EaseMode.Linear)
    {
        long id = mat.GetHashCode() + Random.Range(int.MinValue, int.MaxValue);
        Tween<float> tween = new(mat, id.ToString(), mat.GetFloat(matNameID), end, duration, f =>
        {
            mat.SetFloat(matNameID, Mathf.Clamp01(f));
        }, easeMode);
        return tween;
    }
    public static Tween<float> FloatTo01(this Material mat, int id, float end, float duration, EaseMode easeMode = EaseMode.Linear)
    {
        long idd = mat.GetHashCode() + Random.Range(int.MinValue, int.MaxValue);
        Tween<float> tween = new(mat, idd.ToString(), mat.GetFloat(id), end, duration, f =>
        {
            mat.SetFloat(id, Mathf.Clamp01(f));
        }, easeMode);
        return tween;
    }

    public static Tween<float> FloatTo(Func<float> getFloat, Action<float> setFloat, float end, float duration, EaseMode easeMode = EaseMode.Linear)
    {
        long id = getFloat.Target.GetHashCode() + Random.Range(int.MinValue, int.MaxValue);
        Tween<float> tween = new(getFloat.Target, id.ToString(), getFloat(), end, duration, f =>
        {
            setFloat(f);
        }, easeMode);
        return tween;
    }

    
}