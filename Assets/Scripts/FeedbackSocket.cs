using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
[RequireComponent(typeof(AudioSource))]
public class FeedbackSocket : MonoBehaviour
{
    [Header("Haptique")]
    [SerializeField] private float amplitudeDepot = 0.8f;
    [SerializeField] private float dureeDepot = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioClip sonDepot;

    private XRSocketInteractor socket;
    private AudioSource audioSource;

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
        audioSource = GetComponent<AudioSource>();

        audioSource.spatialBlend = 1f;
        audioSource.maxDistance = 5f;
    }

    void OnEnable()
    {
        socket.selectEntered.AddListener(OnDepotEntered);
    }

    void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnDepotEntered);
    }

    private void OnDepotEntered(SelectEnterEventArgs args)
    {
        var controller = args.interactableObject.interactorsSelecting[0].transform.GetComponentInParent<XRBaseController>();

        if (controller != null)
        {
            controller.SendHapticImpulse(amplitudeDepot, dureeDepot);
        }

        // 3. Le son (vérification de sécurité pour éviter le NullRef)
        if (audioSource != null && sonDepot != null)
        {
            audioSource.PlayOneShot(sonDepot);
        }
    }
}
