using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolatileStorage : MonoBehaviour
{
#region Singleton implementation
    private static VolatileStorage _instance = null;
    public static VolatileStorage GetInstance()
    {
        if (_instance != null)
        {
            return _instance;
        }
        else
        {
            _instance = new GameObject("VolatileStorage").AddComponent<VolatileStorage>();
            DontDestroyOnLoad(_instance.gameObject);
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
#endregion

    public enum CauseOfDeath { None, Life, Money };

    public int lives = 0;
    public int score = 0;
    public int money = 0;
    public CauseOfDeath causeoOfDeath = CauseOfDeath.None;

}