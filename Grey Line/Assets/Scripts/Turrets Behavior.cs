using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretsBehavior : MonoBehaviour
{
    [SerializeField] private GameObject laserType;
    [SerializeField] private float timer, swivelSpeed, range; 
    private GameObject laser;
    private Transform hardPoint, muzzle;
    private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        hardPoint = transform.Find("HardPoint");
        muzzle = transform.Find("HardPoint/Muzzle");
        cooldown = timer;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] presentEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        cooldown -= Time.deltaTime;

        if (presentEnemies.Length > 0)
        {
            GameObject target = findClosestEnemy(presentEnemies);
            Vector3 relativeDistance = target.transform.position - transform.position;
            if (relativeDistance.x == 0)
            {
                relativeDistance.x += 0.000001f;
            }
            if (relativeDistance.y == 0)
            {
                relativeDistance.y += 0.000001f;
            }

            float tangentLine = Mathf.Atan2(relativeDistance.x, relativeDistance.y) * Mathf.Rad2Deg;
            if (Vector3.Distance(target.transform.position, transform.position) < range)
            {
                Quaternion facing = Quaternion.Euler(0, 0, -tangentLine);
                if (hardPoint.eulerAngles.z < facing.eulerAngles.z || hardPoint.eulerAngles.z > facing.eulerAngles.z)
                {
                    hardPoint.rotation = Quaternion.RotateTowards(hardPoint.rotation, facing, swivelSpeed * Time.deltaTime);
                }
                else if (cooldown < 0 && (facing.eulerAngles.z == hardPoint.eulerAngles.z))
                {
                    Instantiate(laserType, transform.position, muzzle.transform.rotation);
                    cooldown = timer;
                }
            }
        }
    }

    private GameObject findClosestEnemy(GameObject[] enemies)
    {
        GameObject closestEnemy = enemies[0];
        Vector3 closest = enemies[0].transform.position;
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) < Vector3.Distance(closest, transform.position))
            {
                closest = enemy.transform.position;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
        
    }
}
