using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class EnergyPylonBehavior : MonoBehaviour
{
    [SerializeField] float range;

    private GameObject[] Catalysts;
    private Vector3 currentLocation;
    private int remainingCatalysts;
    private float CHARGE_UP_TIME = 10f;
    private static float timer, beamTimer;
    private GameObject obliterationCannon, currentClosest;
    private LayerMask validTargets;


    // Start is called before the first frame update
    void Start()
    {
        Catalysts = GameObject.FindGameObjectsWithTag("Catalyst");
        obliterationCannon = GameObject.Find("ObliterationCannon");
        validTargets = LayerMask.GetMask("Catas", "Buildings");
        remainingCatalysts = Catalysts.Length;
        gameObject.transform.Find("RangeFinder").transform.localScale = new Vector3(1, 1, 0) * (range * 2);
        timer = CHARGE_UP_TIME;
        
    }

    // Update is called once per frame
    void Update()
    {
        Catalysts = GameObject.FindGameObjectsWithTag("Catalyst");
        if (Catalysts.Length > 0)
        {
            gameObject.transform.GetChild(0).Find("PivotPoint").transform.rotation = Quaternion.identity;
            currentClosest = findClosestEnemy(Catalysts);
            currentLocation = transform.position;

            foreach (var target in Catalysts)
            {
                if (GetDistance(target, currentLocation) < GetDistance(currentClosest, currentLocation))
                    currentClosest = target;
            }

            if (GetDistance(currentClosest, currentLocation) < range)
                ObliterationCannon(currentClosest);
            else
            {
                beamTimer -= Time.deltaTime;
                if (beamTimer < 0)
                {
                    ColorModifier("Beam1", 0);
                    ColorModifier("Beam2", 0);
                    ColorModifier("Burst", 0);
                    ColliderSetter("Burst", false);
                }
                timer = CHARGE_UP_TIME;
            }


            if (Input.GetMouseButton(2))
                GameManager.DisplayRange(gameObject, true);
            else
                GameManager.DisplayRange(gameObject, false);
        }
        else
            GameManager.EndRound();
    }

    private void ObliterationCannon(GameObject target)
    {
        Vector3 enemyDirection = target.transform.position - currentLocation;
        float firingAngle = (Mathf.Atan2(enemyDirection.y, enemyDirection.x) * Mathf.Rad2Deg) - 90;
        float movement = Mathf.MoveTowardsAngle(transform.eulerAngles.z, firingAngle, 13f * Time.deltaTime);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, range, validTargets);
        Debug.DrawRay(transform.position, transform.up * range, Color.red);
        GameObject aquiredTarget;

        if (hit.collider != null)
        {
            aquiredTarget = hit.collider.gameObject;
        }
        else aquiredTarget = null;

        if (Vector3.Distance(new Vector3(0, 0, transform.position.z), new Vector3(0, 0, movement)) > 0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, movement);
        }

        if ( timer <= 0 && aquiredTarget.name == "NanoCatalyst")
        {
            ColorModifier("Beam1", 1);
            ColorModifier("Beam2", 1);
            ColorModifier("Burst", 1);
            ColorModifier("Charge", 0);
            ColliderSetter("Burst", true);
            beamTimer = 0.5f;
            timer -= Time.deltaTime;
        }
        else if (aquiredTarget != null && aquiredTarget.name == "NanoCatalyst")
        {
            timer -= Time.deltaTime;
           
            float percentage = 1 - timer / CHARGE_UP_TIME;
            ColorModifier("Charge", percentage) ;
        }
    }

    private GameObject findClosestEnemy(GameObject[] enemies)
    {
        if (enemies != null)
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
        return null;
    }
    
    private float GetDistance(GameObject target, Vector3 currentLocation)
    {
        return Vector3.Distance(target.transform.position, currentLocation);
    }

    private void ColorModifier(string str, float value)
    {
        GameObject.Find(str).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, value);
    }

    private bool BurstIsActive()
    {
        return GameObject.Find("Burst").GetComponent<Collider2D>().enabled;
    }

    private void ColliderSetter(string str, bool toggle)
    {
        GameObject.Find(str).GetComponent<Collider2D>().enabled = toggle;
 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(BurstIsActive());
        if ((other.tag == "Enemy" || other.tag == "Catalyst"))
        {
            if (BurstIsActive())
                Destroy(other.gameObject);
            else
                Destroy(gameObject);
        }
    }

}

