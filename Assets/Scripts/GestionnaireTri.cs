using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;

public class GestionnaireTri : MonoBehaviour
{
    public XRSocketInteractor socketSphere;
    public XRSocketInteractor socketCylinder;
    public XRSocketInteractor socketCapsule;
    
    public TMP_Text statusText;

    private bool timerActive;
    private float timer;

    public float scoreInitial = 1000;
    public float pointPerduParsSeconde = 10;
    
    [SerializeField] private XRBaseInputInteractor controleurGauche;
    [SerializeField] private XRBaseInputInteractor controleurDroit;

    private void Awake()
    {
        timer = 0;
        timerActive = true;
    }

    private void Update()
    {
        if (timerActive)
        {
            timer += Time.deltaTime;
            statusText.text = timer.ToString("F2");
        }
    }

    void OnEnable()
    {
        socketSphere.selectEntered.AddListener(OnSphereDepose);
        socketSphere.selectExited.AddListener(OnSphereRetirer);
        
        socketCylinder.selectEntered.AddListener(OnCylinderDepose);
        socketCylinder.selectExited.AddListener(OnCylinderRetirer);
        
        socketCapsule.selectEntered.AddListener(OnCapsuleDepose);
        socketCapsule.selectExited.AddListener(OnCapsuleRetirer);
    }

    void OnDisable()
    {
        socketSphere.selectEntered.RemoveListener(OnSphereDepose);
        socketSphere.selectExited.RemoveListener(OnSphereRetirer);
        
        socketCapsule.selectEntered.RemoveListener(OnCylinderDepose);
        socketCapsule.selectExited.RemoveListener(OnCylinderRetirer);
        
        socketCapsule.selectEntered.RemoveListener(OnCapsuleDepose);
        socketCapsule.selectExited.RemoveListener(OnCapsuleRetirer);
    }

    private void OnSphereDepose(SelectEnterEventArgs args)
    {
        GameObject objetDepose = args.interactableObject.transform.gameObject;
        Debug.Log("Sphere déposé : " + objetDepose.name);
        VerificationSupport();
    }

    private void OnCylinderDepose(SelectEnterEventArgs args)
    {
        GameObject objetDepose = args.interactableObject.transform.gameObject;
        Debug.Log("Cylinder déposé : " + objetDepose.name);
        VerificationSupport();
    }

    private void OnCapsuleDepose(SelectEnterEventArgs args)
    {
        GameObject objetDepose = args.interactableObject.transform.gameObject;
        Debug.Log("Capsule déposé : " + objetDepose.name);
        VerificationSupport();
    }
    
    private void OnSphereRetirer(SelectExitEventArgs args)
    {
        Debug.Log(socketSphere.hasSelection);
        Debug.Log("Sphere retiré du support");
    }

    private void OnCylinderRetirer(SelectExitEventArgs args)
    {
        Debug.Log("Cylinder retiré du support");
    }

    private void OnCapsuleRetirer(SelectExitEventArgs args)
    {
        Debug.Log("Capsule retiré du support");
    }

    private void VerificationSupport()
    {
        if (socketSphere.hasSelection && socketCylinder.hasSelection && socketCapsule.hasSelection)
        {
            int scoreFinial = CalculerScore();
            statusText.text = "Bravo ! Tri complété. Score Final : " + scoreFinial;
            timerActive = false;
            DeclencherVictoireHaptique();
        }
    }

    private int CalculerScore()
    {
        float scoreCalculer = scoreInitial - (timer * pointPerduParsSeconde);
        return Mathf.Max(0, Mathf.RoundToInt(scoreCalculer));
    }
    
    private void DeclencherVictoireHaptique()
    {
        controleurDroit.SendHapticImpulse(1.0f, 2);
        controleurGauche.SendHapticImpulse(1.0f, 2);
    }
}
