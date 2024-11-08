using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class GameManager : MonoBehaviour
{
    
    private static GameManager instance;
    public static GameObject nanoBots;
    readonly public static string[] variants = {"var1", "var2", "var3", "var4", "var5"};
    

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();


                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(GameManager).Name);
                    instance = singletonObject.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && gameObject != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
