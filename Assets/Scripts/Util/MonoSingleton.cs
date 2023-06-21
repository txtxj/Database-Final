using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = (T)this;
        }
    }

    protected void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}