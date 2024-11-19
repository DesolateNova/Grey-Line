using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NanoCatalystBehavior : MonoBehaviour
{
    public LayerMask Buildings;
    // Start is called before the first frame update
    void Start()
    {
        this.name = "NanoCatalyst";
        Spawn(transform.position, GameManager.nanoBots);
    }

    // Update is called once per frame
    void Update()
    {
        Spawn(transform.position, GameManager.nanoBots);
    }

    public static void Spawn(Vector2 position, GameObject spawn)
    {
        Collider2D checker;
        string ancestor;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                checker = Physics2D.OverlapBox(position + new Vector2(i, j), GameManager.spawnPlot / 2, 0);
                if (checker != null)
                    ancestor = GameManager.baseAncestor(checker);
                else
                    ancestor = "none";

                int roll = Random.Range(0, 101);

                if ((checker == null || checker.name == "NanoCatalyst" || ancestor == "Turret") && roll > 85)
                {
                    Instantiate(spawn, position + new Vector2(i, j), Quaternion.identity);
                }
            }
        }
    }
}
