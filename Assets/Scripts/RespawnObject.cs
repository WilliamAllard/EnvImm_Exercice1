using System;
using UnityEngine;

// Merci Gemini
public class RespawnObject : MonoBehaviour
{
    public GameObject objectToRespawn;
    
    // On stocke les VALEURS de position et rotation
    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        if (objectToRespawn != null)
        {
            // On enregistre les chiffres au début
            startPosition = objectToRespawn.transform.position;
            startRotation = objectToRespawn.transform.rotation;
        }
    }

    private void Update()
    {
        // On vérifie si l'objet existe toujours avant de tester sa position
        if (objectToRespawn != null && objectToRespawn.transform.position.y < -1)
        {
            // On crée le nouvel objet à la position de départ mémorisée
            GameObject newObject = Instantiate(objectToRespawn, startPosition, startRotation);
            
            // On détruit l'ancien
            Destroy(objectToRespawn);
            
            // IMPORTANT : On remplace la référence par le nouvel objet 
            // pour que l'Update puisse surveiller le nouveau !
            objectToRespawn = newObject;
        }
    }
}
