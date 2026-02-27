using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ReactionDepot : MonoBehaviour
{
    // Le socket est sur ce même GameObject
    private XRSocketInteractor socket;

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    void OnEnable()
    {
        // S'abonner à l'événement : un objet vient d'être déposé
        socket.selectEntered.AddListener(OnObjetDepose);

        // S'abonner à l'événement : un objet vient d'être retiré
        socket.selectExited.AddListener(OnObjetRetire);
    }

    void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnObjetDepose);
        socket.selectExited.RemoveListener(OnObjetRetire);
    }

    private void OnObjetDepose(SelectEnterEventArgs args)
    {
        // args.interactableObject est l'objet qui vient d'entrer dans le socket
        GameObject objetDepose = args.interactableObject.transform.gameObject;
        Debug.Log("Objet déposé : " + objetDepose.name);
    }

    private void OnObjetRetire(SelectExitEventArgs args)
    {
        Debug.Log("Objet retiré du support");
    }
}