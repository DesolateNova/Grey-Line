using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerManagement : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public Button TierOneButton, PenetratorButton, BombardButton, TransistorButton, HelpButton;
    [SerializeField] private GameObject TRT, PT, BMB, TR;
    [SerializeField] private float powerGainDelay;
    [SerializeField] private int TRT_COST , PT_COST, BMB_COST, TR_COST;

    private int power;
    readonly private int POWER_CAP = 450, BASE_POWER_GAIN = 5;
    private float timer;
    private readonly float TRT_DELAY = 1f, PT_DELAY = 3f, BMB_DELAY = 5f, TR_DELAY = 1f;
    private Vector3 spawnLocation, spawnOffset = new Vector3(0, 1.5f, 0), PBaseLocation;

    private static bool AllowButtonPush;
    private static float spawnTimer;



    // Start is called before the first frame update
    void Start()
    {
        power = 50;
        timer = powerGainDelay;
        TierOneButton.onClick.AddListener(SpawnT1Turret);
        TierOneButton.GetComponentInChildren<TextMeshProUGUI>().SetText($"{TRT_COST} EP");
        PenetratorButton.onClick.AddListener(SpawnPenTurret);
        PenetratorButton.GetComponentInChildren<TextMeshProUGUI>().SetText($"{PT_COST} EP");
        BombardButton.onClick.AddListener(SpawnBombard);
        BombardButton.GetComponentInChildren<TextMeshProUGUI>().SetText($"{BMB_COST} EP");
        TransistorButton.onClick.AddListener(SpawnTransistor);
        TransistorButton.GetComponentInChildren<TextMeshProUGUI>().SetText($"{TR_COST} EP");
        HelpButton.onClick.AddListener(openHelp);
    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;
        spawnTimer -= Time.deltaTime;
        PBaseLocation = GameObject.Find("PBase").transform.position;
        spawnLocation = PBaseLocation + spawnOffset;
        text.text = $"Current EP: {power}"; 

        if (timer <= 0)
        {
            if (power > POWER_CAP)
                power += 0;
            else
            {
            power += (BASE_POWER_GAIN + Random.Range(0, 7));
            timer = powerGainDelay;
            }
        }

        if (spawnTimer <= 0)
            AllowButtonPush = true;
        if (spawnTimer > 0)
            AllowButtonPush = false;

    }

    private void SpawnT1Turret()
    {
        if (!GetInteractAllowance() || GetCost(TRT_COST) < 0) return;
        else
        {
            SetSpawnTimer(TRT_DELAY);
            power -= TRT_COST;
            Instantiate(TRT, spawnLocation, Quaternion.identity);
        }
    }

    private void SpawnPenTurret()
    {
        if (!GetInteractAllowance() || GetCost(PT_COST) < 0) return;
        else
        {
            SetSpawnTimer(PT_DELAY);
            power -= PT_COST;
            Instantiate(PT, spawnLocation, Quaternion.identity);
        }
    }

    private void SpawnBombard()
    {
        if (!GetInteractAllowance() || GetCost(BMB_COST) < 0) return;
        else
        {
            SetSpawnTimer(BMB_DELAY);
            power -= BMB_COST;
            Instantiate(BMB, spawnLocation, Quaternion.identity);
        }
    }

    private void SpawnTransistor()
    { 
        if (!GetInteractAllowance() || GetCost(TR_COST) < 0) return;
        else
        {
            SetSpawnTimer(TR_DELAY);
            power -= TR_COST;
            Instantiate(TR, spawnLocation, Quaternion.identity);
        }
    }

    private void SetSpawnTimer(float time)
    {
        spawnTimer = time;
    }

    private bool GetInteractAllowance()
    {
        return AllowButtonPush;
    }

    private int GetCost(int num)
    {
        return power - num;
    }

    private void openHelp()
    {
        if (GameObject.Find("Explanation") == null)
        {
            GameObject.Find("Help Button").transform.GetChild(1).gameObject.SetActive(true);
        }
        else
            GameObject.Find("Explanation").SetActive(false);
    }



}
