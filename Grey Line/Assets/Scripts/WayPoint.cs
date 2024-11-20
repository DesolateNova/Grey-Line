using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{

    private static GameObject wayPoint, tetheredObject;
    private static Dictionary<int, GameObject> tether = new Dictionary<int, GameObject>();
    private Vector3 wayPointPos, tetheredObjectPos;
    private static bool isMoving;
    private static int wayPointID;



    // Start is called before the first frame update
    void Awake()
    {
        this.name = "WayPoint";
        wayPointPos = this.transform.position;
        tetheredObjectPos = tetheredObject.transform.position;
        wayPointID = wayPoint.GetInstanceID();
        tether.Add(wayPointID, tetheredObject);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(tether.Keys.ToString());
        Vector3 targetPos = wayPointPos - tetheredObjectPos;
        if (isMoving)
        {
            tetheredObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetPos);
            tetheredObject.transform.position += targetPos.normalized * Time.deltaTime;
        }

        if (Vector3.Distance(tetheredObjectPos, wayPointPos) < 0.1f)
        {

            isMoving = false;
            Destroy(gameObject);
            tether.Remove(wayPointID);
        }

        tetheredObjectPos = tetheredObject.transform.position;
    }

    public static void setWayPoint(GameObject obj)
    {
        tetheredObject = obj;
        if (wayPoint == null)
            wayPoint = Resources.Load<GameObject>("Prefabs/WayPoint");

        //If waypoint already exists move the way point to pointer position
        if (tether.ContainsKey(wayPointID))
            wayPoint.transform.position = GameManager.GetPointerLocation();
        else
            Instantiate(wayPoint, GameManager.GetPointerLocation(), Quaternion.identity);
        isMoving = true;
    }
}
