using UnityEngine;
using System.Collections;

public class Shelf : MonoBehaviour
{
    [Header("Hyllas farge")]
    // I Inspector velger du en av 4 enum-verdier (Red, Blue, Yellow, Green)
    public BallColor requiredColor;

    public float holdTime = 3f;

    private Coroutine placeCoroutine;
    private PickupableObject currentPickup;

    private Renderer shelfRenderer;
    private Color originalShelfColor;

    private Color originalPickupColor;

    void Start()
    {
        shelfRenderer = GetComponent<Renderer>();
        if (shelfRenderer) originalShelfColor = shelfRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        var pickup = other.GetComponent<PickupableObject>();
        if (pickup != null)
        {
            currentPickup = pickup;

            // Mørklegge
            if (pickup.objectRenderer != null)
            {
                originalPickupColor = pickup.objectRenderer.material.color;
                pickup.objectRenderer.material.color = originalPickupColor * 0.5f;
            }
            if (shelfRenderer != null)
            {
                shelfRenderer.material.color = originalShelfColor * 0.5f;
            }

            if (placeCoroutine == null)
            {
                placeCoroutine = StartCoroutine(CheckPlacement(pickup));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var pickup = other.GetComponent<PickupableObject>();
        if (pickup == currentPickup)
        {
            // revert
            if (pickup.objectRenderer != null)
            {
                pickup.objectRenderer.material.color = originalPickupColor;
            }
            if (shelfRenderer) shelfRenderer.material.color = originalShelfColor;

            currentPickup = null;

            if (placeCoroutine != null)
            {
                StopCoroutine(placeCoroutine);
                placeCoroutine = null;
            }
        }
    }

    IEnumerator CheckPlacement(PickupableObject obj)
    {
        float timer = 0f;
        while (timer < holdTime)
        {
            if (!obj || !obj.gameObject.activeSelf) yield break;
            timer += Time.deltaTime;
            yield return null;
        }

        // 3 sek => forsvinn
        bool correctPlacement = false;
        if (obj.colorData != null && obj.colorData.ballColor == requiredColor)
        {
            correctPlacement = true;
        }

        obj.DestroyThisObject(correctPlacement);

        // Revert hyllefarge
        if (shelfRenderer) shelfRenderer.material.color = originalShelfColor;

        currentPickup = null;
        placeCoroutine = null;
    }
}
