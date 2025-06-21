using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Tooltip("Radien rundt dette punktet hvor objektet kan spawne.")]
    public float radius = 0.5f;

    public Vector3 GetSpawnPosition()
    {
        // Litt random offset
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        Vector3 spawnPos = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
        return spawnPos;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
