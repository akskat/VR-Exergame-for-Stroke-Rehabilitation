using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class EEGLabelUI : MonoBehaviour
{
    private Text txt;

    void Awake ()
    {
        txt = GetComponent<Text>();
        txt.horizontalOverflow   = HorizontalWrapMode.Overflow;
        txt.verticalOverflow     = VerticalWrapMode.Overflow;
        txt.resizeTextForBestFit = true;
        txt.text = "EEG: None (venter …)";
        Debug.Log("<color=orange>[EEG-UI]</color> Awake – init tekst satt");
    }

    public void SetLabel (string lab)
    {
        string disp = string.IsNullOrEmpty(lab) ? "-" : lab;
        txt.text = $"EEG: {disp}";
        Debug.Log($"<color=orange>[EEG-UI]</color> SetLabel('{disp}')");
    }
}
