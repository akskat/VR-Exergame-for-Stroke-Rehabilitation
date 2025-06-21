using UnityEngine;

public class PickupableObject : MonoBehaviour
{
    public GameManager gameManager;

    // Her har vi vår data: en ColorShapeData med "ballColor"
    public ColorShapeData colorData = new ColorShapeData();

    private bool isHeld = false;
    private Transform holder;
    private float spawnTime;

    // For å vise riktig farge i scenen:
    public Renderer objectRenderer;

    void Start()
    {
        spawnTime = Time.time;

        if (!objectRenderer)
        {
            objectRenderer = GetComponentInChildren<Renderer>();
        }
    }

    public void RandomizeColor()
    {
        // Velg en av fire enum-verdier
        BallColor[] possibleColors = { BallColor.Red, BallColor.Blue, BallColor.Yellow, BallColor.Green };
        BallColor chosenColor = possibleColors[Random.Range(0, possibleColors.Length)];

        colorData.ballColor = chosenColor;

        // Tildel en Material ut fra chosenColor
        Color matColor = Color.white;
        switch (chosenColor)
        {
            case BallColor.Red:    matColor = Color.red;    break;
            case BallColor.Blue:   matColor = Color.blue;   break;
            case BallColor.Yellow: matColor = Color.yellow; break;
            case BallColor.Green:  matColor = Color.green;  break;
        }
        if (objectRenderer != null)
        {
            objectRenderer.material.color = matColor;
        }
    }

    void Update()
    {
        // Hvis vi har en "holder" (f.eks. håndcollider), kan vi følge den i world space
        // Du kan også la transform.SetParent(...) styre dette i hierarkiet
        if (isHeld && holder != null)
        {
            transform.position = holder.position;
        }
    }

    // Håndterer automatisk "plukk" når HandCollider berører
    private void OnTriggerEnter(Collider other)
    {
        if (!isHeld && other.CompareTag("HandCollider"))
        {
            isHeld = true;
            holder = other.transform;

            // Fester objektet fysisk i hierarkiet til "holder"
            transform.SetParent(holder, worldPositionStays: true);

            // (Valgfritt) For at ballen skal "snappe" til håndcolliderens local pos:
            // transform.localPosition = Vector3.zero;
        }
    }

    // Hvis du IKKE vil slippe i det øyeblikket du forlater collidern,
    // kan du kommentere ut koden under:
    private void OnTriggerExit(Collider other)
    {
        if (isHeld && other.transform == holder)
        {
            // Kommenter bort for at ballen ikke skal slippe ved exit:
            // isHeld = false;
            // holder = null;
            // transform.SetParent(null, true);
        }
    }

    public void DestroyThisObject(bool correctPlacement)
    {
        float timeUsed = Time.time - spawnTime;

        if (gameManager != null)
        {
            gameManager.OnObjectDestroyed(correctPlacement, timeUsed, this.gameObject);
        }
        Destroy(gameObject);
    }
}
