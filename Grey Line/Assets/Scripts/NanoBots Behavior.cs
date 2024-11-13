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
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject nanoBots = GameObject.Find("Body");
        GameObject deco = GameObject.Find("Deco");
        nanoBots.GetComponent<SpriteResolver>().SetCategoryAndLabel("Variants", GameManager.variants[Random.Range(0, 5)]);
        deco.GetComponent<SpriteResolver>().SetCategoryAndLabel("Variants", GameManager.variants[Random.Range(0, 3)]);
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
