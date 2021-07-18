using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager bgmInstance;

    private void Awake()
    {
        if (bgmInstance != null && bgmInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        bgmInstance = this;
        DontDestroyOnLoad(this);
    }
}
