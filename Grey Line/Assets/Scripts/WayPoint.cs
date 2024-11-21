using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{

    private static GameObject wayPointPrefab;
    private static Dictionary<int, GameObject> existingTethers = new Dictionary<int, GameObject>();

    private GameObject tetheredWayPoint, tetheredObject;
    private Vector3 wayPointPos, tetheredObjectPos;
    private bool isMoving;
    private int tetheredObjectID;

    private void Init(int ID, GameObject wayPoint, GameObject item)
    {
        this.name = "WayPoint";
        existingTethers.Add(ID, wayPoint);
        this.tetheredWayPoint = wayPoint;
        this.tetheredObject = item;
        this.tetheredObjectID = ID;
        GameManager.ToggleColliders(tetheredObject);
        isMoving = true;
    }

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tetheredObject == null)
        {
            existingTethers.Remove(tetheredObjectID);
            Destroy(tetheredWayPoint);
        }


        tetheredObjectPos = tetheredObject.transform.position;
        wayPointPos = tetheredWayPoint.transform.position;
        Vector3 targetPos = wayPointPos - tetheredObjectPos;


        if (isMoving)
        {
            tetheredObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetPos);
            tetheredObject.transform.position += targetPos.normalized * Time.deltaTime;
        }
        if (Vector3.Distance(tetheredObjectPos, wayPointPos) < 0.1f)
        {

            isMoving = false;
            GameManager.ToggleColliders(tetheredObject);
            Destroy(tetheredWayPoint);
            existingTethers.Remove(tetheredObjectID);
        }

        tetheredObjectPos = tetheredObject.transform.position;
    }

    public static void setWayPoint(GameObject obj)
    {
        if (obj.tag != "Player") return;

        int ID = obj.GetInstanceID();
        GameObject wp;

        if (wayPointPrefab == null)
            wayPointPrefab = Resources.Load<GameObject>("Prefabs/WayPoint");

        if (existingTethers.TryGetValue(ID, out var existingWayPoint))
            existingWayPoint.transform.position = GameManager.GetPointerLocation();
        else
        {
            wp = Instantiate(wayPointPrefab, GameManager.GetPointerLocation(), Quaternion.identity);
            wp.GetComponent<WayPoint>().Init(ID, wp, obj);
        }
    }
}
