using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{

    private static GameObject wayPoint, tetheredObject;
    private int wayPointId, tetheredId;



    // Start is called before the first frame update
    void Awake()
    {
        this.name = "WayPoint";
        wayPointId = this.gameObject.GetInstanceID();
        tetheredId = tetheredObject.GetInstanceID();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

    }

    public static void setWayPoint(GameObject obj)
    {
        if (wayPoint == null)
            wayPoint = Resources.Load<GameObject>("Prefabs/WayPoint");
        tetheredObject = obj;
        Instantiate(wayPoint, GameManager.GetPointerLocation(), Quaternion.identity);
    }
}
