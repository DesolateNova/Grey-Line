using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private GameObject laser;
    [SerializeField] private float laserSpeed;

    public GameObject LaserType => laser;
    private float xTraversal, yTraversal, shootAngle;
    Vector2 turretPos;

    // Start is called before the first frame update
    void Start()
    {
        turretPos = transform.position;
        shootAngle = (transform.eulerAngles.z + 90);
        transform.rotation = Quaternion.Euler(0, 0, shootAngle);

        if (shootAngle == 90 || shootAngle == 270)
        {
            xTraversal = 0;
            yTraversal = 1;
        }
        else if (shootAngle == 0 || shootAngle == 180)
        {
            xTraversal = 1;
            yTraversal = 0;
        }
        else 
        {
            yTraversal = Mathf.Sin(shootAngle * Mathf.Deg2Rad);
            xTraversal = Mathf.Cos(shootAngle * Mathf.Deg2Rad);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (new Vector3(xTraversal, yTraversal, 0) * laserSpeed) * Time.deltaTime;


        Vector3 laserPos = laser.transform.position;
        if (laserPos.x > (turretPos.x + 26f) || laserPos.y > (turretPos.y + 26f)){
            Destroy(gameObject);
        }
        else if (laserPos.x < (turretPos.x - 26f) || laserPos.y < (turretPos.y - 26f)){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
