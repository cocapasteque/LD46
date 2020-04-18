using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Singleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (mInstance == null && !isQuitting)
            {
                mInstance = FindObjectOfType(typeof(T)) as T;
                if (mInstance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    mInstance = go.AddComponent<T>();
                    Debug.LogWarning("Creating " + go.name + " since one does not exist in the scene currently");
                }
            }
            return mInstance;
        }
    }

    public static bool HasInstance
    {
        get
        {
            //if we dont have one we try to find one
            if (mInstance == null)
            {
                mInstance = FindObjectOfType(typeof(T)) as T;
            }
            return mInstance != null;
        }
    }

    static T mInstance;

    protected static bool isQuitting = false;
    protected virtual void OnApplicationQuit()
    {
        isQuitting = true;
    }

}
