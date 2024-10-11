using TMPro;
using UnityEngine;

public class PhasePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void UpdatePanelText(string panelName)
    {
        _text.text = panelName;
    }
}