using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NanoCatalystBehavior : MonoBehaviour
{
    [SerializeField] GameObject nanoBots;
    public LayerMask Buildings;
    // Start is called before the first frame update
    void Start()
    {
        Spawn(transform.position, nanoBots);
    }

    // Update is called once per frame
    void Update()
    {
        Spawn(transform.position, nanoBots);
    }

    public static void Spawn(Vector2 position, GameObject spawn)
    {
        Vector3 spawnPlot = new Vector2(1, 1);
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Collider2D checker = Physics2D.OverlapBox(position + new Vector2(i, j), spawnPlot / 2, 0);
                int roll = Random.Range(0, 101);
                if ((checker == null || checker.tag == "Catalyst") && roll > 88)
                {
                    Instantiate(spawn, position + new Vector2(i, j), Quaternion.identity);
                }
            }
        }
    }
}
