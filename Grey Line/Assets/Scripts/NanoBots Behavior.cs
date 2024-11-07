using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NanoBotsBehavior : MonoBehaviour
{
    [SerializeField] private float spawnRate;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = spawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            NanoCatalystBehavior.Spawn(transform.position, this.gameObject);
            timer = spawnRate;
        }
    }
}
