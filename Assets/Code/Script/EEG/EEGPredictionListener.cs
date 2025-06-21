using UnityEngine;
using LSL;                      // krever Assets/Plugins/LSL.cs

public class EEGPredictionListener : MonoBehaviour
{
    [Header("LSL-kilde")]
    public string streamName = "MI_Pred";
    public float  timeoutSec = 0f;          // 0 = non-blocking

    [Header("UI")]
    // Dra inn din EEGLabelUI-komponent her i Inspector
    public EEGLabelUI labelUI;

    private StreamInlet inlet;
    private double      lastTs;

    void Start ()
    {
        // Sjekk at labelUI er satt – ellers prøv å finn den automatisk
        if (labelUI == null)
        {
            labelUI = FindObjectOfType<EEGLabelUI>();
            if (labelUI == null)
                Debug.LogError("<color=lime>[EEG-LSL]</color> labelUI ikke satt og ingen EEGLabelUI funnet i scena!");
            else
                Debug.Log("<color=lime>[EEG-LSL]</color> Fant EEGLabelUI via FindObjectOfType.");
        }

        Debug.Log($"<color=lime>[EEG-LSL]</color> Søker etter stream «{streamName}»");
        var infos = LSL.LSL.resolve_stream("name", streamName, 1, 5.0f);
        if (infos.Length == 0)
        {
            Debug.LogError("<color=lime>[EEG-LSL]</color> Fant ikke stream – script deaktivert");
            enabled = false;
            return;
        }

        inlet = new StreamInlet(infos[0]);
        inlet.open_stream();
        Debug.Log("<color=lime>[EEG-LSL]</color> Inlet åpnet OK");
    }

    void Update ()
    {
        if (inlet == null) return;

        var sample = new string[1];
        double ts  = inlet.pull_sample(sample, timeoutSec);

        if (ts > 0 && ts != lastTs)
        {
            lastTs = ts;
            var label = sample[0] ?? "";
            Debug.Log($"<color=lime>[EEG-LSL]</color> Got LSL label: '{label}'");

            if (labelUI != null)
            {
                labelUI.SetLabel(label);
            }
            else
            {
                Debug.LogWarning("<color=lime>[EEG-LSL]</color> labelUI er fortsatt null, kunne ikke oppdatere UI.");
            }
        }
    }

    void OnDestroy ()
    {
        inlet?.close_stream();
    }
}
