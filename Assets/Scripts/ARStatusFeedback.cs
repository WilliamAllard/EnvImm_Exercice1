using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARStatusFeedback : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private TMP_Text statusText; // Nécessite un UI Text

    private int planesCount = 0;

    void Update()
    {
        // Compter les plans détectés
        planesCount = planeManager.trackables.count;

        if (planesCount == 0)
        {
            statusText.text = "Recherche de surfaces...";
            statusText.color = Color.yellow;
        }
        else
        {
            statusText.text = $"Prêt ! {planesCount} surface(s) détectée(s)";
            statusText.color = Color.green;
        }
    }
}