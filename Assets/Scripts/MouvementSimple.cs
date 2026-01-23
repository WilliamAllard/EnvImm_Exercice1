using UnityEngine;

// Faite par Gemini
public class MouvementSimple : MonoBehaviour
{
    [Header("Paramètres de mouvement")]
    public float vitesse = 5f;        // Vitesse de déplacement
    public float amplitude = 3f;      // Distance max par rapport au centre

    private Vector3 positionInitiale;

    void Start()
    {
        // On mémorise la position de départ pour osciller autour
        positionInitiale = transform.position;
    }

    void Update()
    {
        // Calcul du mouvement de va-et-vient avec Mathf.Sin
        // Sin(Time.time) renvoie une valeur entre -1 et 1
        float mouvementX = Mathf.Sin(Time.time * vitesse) * amplitude;

        // On applique la nouvelle position
        transform.position = new Vector3(positionInitiale.x + mouvementX, positionInitiale.y, positionInitiale.z);
    }
}
