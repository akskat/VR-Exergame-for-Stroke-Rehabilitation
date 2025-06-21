using UnityEngine;
using System.Collections;
using UnityEngine.UI; // for "Text"

public class GameManager : MonoBehaviour
{
    [Header("Spawning av objekter")]
    public GameObject pickupablePrefab;
    public SpawnPoint[] spawnPoints;

    [Tooltip("Hvor lang tid etter at en ball forsvinner før neste dukker opp.")]
    public float spawnDelayAfterDespawn = 2f;

    [Header("Poeng og scoring")]
    public ScoreManager scoreManager;

    [Header("Tid UI")]
    [Tooltip("Dra inn 'TimeText' (UI) fra VRCanvas -> TimeText")]
    public Text timeText;

    private GameObject currentBall;
    private float spawnTimeForCurrentBall = 0f; // for tid siden sist ball ble spawnet

    void Start()
    {
        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        while (true)
        {
            // 1) Vent litt før første ball
            yield return new WaitForSeconds(1f);

            // 2) Spawn en ball
            SpawnBall();

            // 3) Vent til ballen er destruert
            while (currentBall != null)
            {
                yield return null;
            }

            // 4) Vent litt etter at ball forsvant
            yield return new WaitForSeconds(spawnDelayAfterDespawn);
        }
    }

    void SpawnBall()
    {
        if (spawnPoints.Length == 0) return;

        // velg random spawnpoint
        var sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 pos = sp.GetSpawnPosition();
        Quaternion rot = Quaternion.identity;

        // Instansier prefab
        currentBall = Instantiate(pickupablePrefab, pos, rot);

        // Logg spawn-time
        spawnTimeForCurrentBall = Time.time;

        // Oppsett av ball-skript
        var pickup = currentBall.GetComponent<PickupableObject>();
        if (pickup)
        {
            pickup.RandomizeColor();
            pickup.gameManager = this;
        }
    }

    // Kalles av ballen (PickupableObject) når den forsvinner
    public void OnObjectDestroyed(bool correctPlacement, float timeUsed, GameObject theBall)
    {
        // Sjekk om det var "vår" ball
        if (theBall == currentBall)
        {
            currentBall = null;
        }

        // Oppdater poeng
        if (scoreManager != null)
        {
            scoreManager.UpdateScore(correctPlacement, timeUsed);
        }
    }

    void Update()
    {
        // Oppdater "TimeText" mens ballen er i scenen
        if (currentBall != null)
        {
            float timeSinceSpawn = Time.time - spawnTimeForCurrentBall;
            if (timeText != null)
            {
                timeText.text = "Time: " + timeSinceSpawn.ToString("F1");
            }
        }
        else
        {
            // ingen ball -> blank / 0
            if (timeText != null)
            {
                timeText.text = "Time: 0.0";
            }
        }
    }
}
