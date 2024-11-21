using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D.Animation;

public class GameManager : MonoBehaviour
{
    
    private static SpriteResolver nanoBody, nanoDeco;
    private static GameManager instance;

    public static GameObject nanoBots, selectedItem, fallbackObject;
    readonly public static string[] variants = {"var1", "var2", "var3", "var4", "var5"};
    public static Vector2 spawnPlot;
    public static Vector3 mousePos;
    static Tilemap rooftops;
    static List<Vector3Int> transparentableTiles = new List<Vector3Int>(0);
    static private Vector3Int currentClosest;
    static Color originalRGB;
    public Dictionary<int, int> tethers;
    private bool isSelected;
    
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

        nanoBots = Resources.Load<GameObject>("Prefabs/NanoBots");
        nanoBody = nanoBots.transform.GetChild(0).GetComponent<SpriteResolver>();
        nanoDeco = nanoBots.transform.GetChild(1).GetComponent<SpriteResolver>();
        spawnPlot = new Vector2(0.66f, 0.66f);
        rooftops = GameObject.Find("Rooftops").GetComponent<Tilemap>();
        tethers = new Dictionary<int, int>();
        fallbackObject = new GameObject("Empty");
        isSelected = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetTransparentableTiles(rooftops);
        GetOriginalRGB(rooftops);
        currentClosest = GetNearestTransparentable(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        NanoRandomizer();

        mousePos = Input.mousePosition;
        mousePos.z = 0 - Camera.main.transform.position.z;
        Vector3 pointer = GetPointerLocation();


        if (Input.GetMouseButtonDown(0))
        {
            GameObject itemAtClick = SelectItem(pointer);
            itemAtClick = baseAncestor(itemAtClick);

            if (itemAtClick.name != "Empty")
            {
                isSelected = true;
                selectedItem = itemAtClick;
            }
            else if (Input.GetMouseButtonDown(0)  && isSelected)
            {
                WayPoint.setWayPoint(selectedItem);
            }


        }

        if (Input.GetMouseButtonDown(1))
        {
            isSelected = false;
        }

    }

    //###################################################################################################################################

    public static string baseAncestor(Collider2D item)
    {
        Transform ancestor = item.transform;

        while (ancestor.parent != null)
            ancestor = ancestor.parent;

        return ancestor.name;
    }

    public static GameObject baseAncestor(GameObject item)
    {
        Transform ancestor = item.transform;

        while (ancestor.parent != null)
            ancestor = ancestor.parent;

        return ancestor.gameObject;
    }

    private static void NanoRandomizer()
    {
        nanoBody.GetComponent<SpriteResolver>().SetCategoryAndLabel("Variants", GameManager.variants[Random.Range(0, 5)]);
        nanoDeco.GetComponent<SpriteResolver>().SetCategoryAndLabel("Variants", GameManager.variants[Random.Range(0, 3)]);
    }


//###################################################################################################################################

    public static GameObject SelectItem(Vector3 mousePos) {
        RaycastHit2D item = Physics2D.Raycast(mousePos, mousePos, 0);
        return item.collider != null ? item.collider.gameObject : fallbackObject;
    }

    public static Vector3 GetPointerLocation()
    {
        GameObject pointer = GameObject.Find("Pointer");

        pointer.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int nearest = GetNearestTransparentable(pointer.transform.position);
        CheckForTransparentables(pointer.transform.position, nearest, false);

        return pointer.transform.position;
    }

    private static void CheckForTransparentables(Vector3 pointerLocation, Vector3Int nearest, bool isTransparent)
    {
        if (isTransparent) return;
        Vector3Int nearestTransparentable = GetNearestTransparentable(pointerLocation);
        currentClosest = nearestTransparentable;

        if (Vector3.Distance(pointerLocation, nearestTransparentable) < 0.75)
        {
            rooftops.color = new Color(originalRGB.r, originalRGB.g, originalRGB.b, 0.25f);
            CheckForTransparentables(pointerLocation, nearestTransparentable, true);
        }
        else if (Vector3.Distance(pointerLocation, nearestTransparentable) > 0.75f)
        {
            rooftops.color = new Color(originalRGB.r, originalRGB.g, originalRGB.b, 1f);
        }
    }

    private static Vector3Int GetNearestTransparentable(Vector3 pointerLocation)
    {
        float closestDist = Vector3.Distance(pointerLocation, transparentableTiles[0]);
        Vector3Int closest = transparentableTiles[0];

        foreach (Vector3Int transparentable in transparentableTiles)
            if (Vector3.Distance(pointerLocation, transparentable) < closestDist)
            {
                closest = transparentable;
                closestDist = Vector3.Distance(pointerLocation, transparentable);
            }

        currentClosest = closest;
        return closest;
    }

    private static void GetTransparentableTiles(Tilemap tilemap)
    {
        foreach (Vector3Int cell in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(cell))
            {
                transparentableTiles.Add(cell);
            }
        }
    }

    private static void GetOriginalRGB(Tilemap tilemap)
    {
        originalRGB = tilemap.color;
    }

//###################################################################################################################################

    public static void ToggleColliders(GameObject obj)
    {
        if (obj.GetComponent<Collider2D>() != null && obj.GetComponent<Collider2D>().gameObject.name != "Burst")
        {
            obj.GetComponent<Collider2D>().enabled = !obj.GetComponent<Collider2D>().enabled;
        }

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            ToggleColliders(obj.transform.GetChild(i).gameObject);
        }
    }
    public static void DisplayRange(GameObject turret, bool toggle)
    {
        if (turret.transform.Find("RangeFinder")  != null)
            turret.transform.Find("RangeFinder").GetComponent<SpriteRenderer>().enabled = toggle;
    }

    public static void EndRound()
    {
        Debug.Log("Congrats you won the round");
        Debug.Break();
    }

}
