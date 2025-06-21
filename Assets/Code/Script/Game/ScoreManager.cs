using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("Scoring")]
    [Tooltip("Basispoeng for riktig plassering")]
    public int basePoints = 10;

    [Tooltip("Flere poeng jo raskere man plasserer? (Hvis du vil)")]
    public float timeBonusFactor = 5f;

    [Header("UI for poeng")]
    // Dra inn 'ScoreText' fra VRCanvas -> ScoreText (UI)
    public Text scoreText;  

    private int currentScore = 0;

    void Start()
    {
        UpdateScoreText();
    }

    // Kalles av GameManager: OnObjectDestroyed(correct, time)
    public void UpdateScore(bool correctPlacement, float timeUsed)
    {
        if (correctPlacement)
        {
            int pointsEarned = basePoints;
            // Belønn rask placing
            pointsEarned += Mathf.Max(0, (int)(timeBonusFactor * (10f - timeUsed)));
            currentScore += pointsEarned;
        }
        else
        {
            // -3 for feil plassering
            currentScore -= 3;
        }
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
}
