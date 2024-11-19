using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class NanoBotsBehavior : MonoBehaviour
{
    [SerializeField] private float spawnRate;
    private float timer;

    void Awake()
    {
        timer = spawnRate;
        this.name = "NanoBot";
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            NanoCatalystBehavior.Spawn(transform.position, GameManager.nanoBots);
            timer = spawnRate;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" || (other.transform.position == this.transform.position && other.name != "NanoCatalyst"))
        {
            Destroy(other.gameObject);
        }
    }

}
