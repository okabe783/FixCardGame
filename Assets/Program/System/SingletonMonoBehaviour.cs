using System;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T I
    {
        get
        {
            if (_instance == null)
            {
                Type t = typeof(T);
                _instance = (T)FindAnyObjectByType(t);
                if (_instance == null)
                {
                    Debug.LogError(t + "をアタッチしているオブジェクトはありません");
                }
            }

            return _instance;
        }
    }
    
    public static bool ExistInstance()
    {
        return _instance != null;
    }

    protected void Awake()
    {
        CheckInstance();
    }
    
    protected bool CheckInstance()
    {
        if (_instance == null)
        {
            _instance = this as T;
            return true;
        }
        
        if (I == this)
        {
            return true;
        }
        
        Destroy(this);
        return false;
    }
}