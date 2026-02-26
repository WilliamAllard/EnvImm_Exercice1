using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementRaycast : MonoBehaviour
{
    private PlayerInputActions inputActions;
    
    public IndicateurCouleur indicateurCouleur;
    
    public GameObject objetAPlacer; // Assignez un prefab dans l'inspecteur
    
    private Color couleurObjet = Color.red;
    
    public LayerMask layerMask;

    private int nombreObjetPlacer;

    public int maxObjet = 10;
    
    [SerializeField] private ARRaycastManager arRaycastManager;
    
    void Awake()
    {
        // Créer une instance des Input Actions
        inputActions = new PlayerInputActions();
        indicateurCouleur.ChangeCouleur(couleurObjet);
    }
    
    void OnEnable()
    {
        // IMPORTANT : Activer les actions
        inputActions.Players.Click_Add.performed += OnTap;
        inputActions.Players.Click_Remove.performed += OnInputClickRemove;
        inputActions.Players.Color_Switch.performed += OnInputColorChange;
        inputActions.Enable();
    }

    void OnDisable()
    {
        // IMPORTANT : Désactiver pour éviter les fuites mémoire
        inputActions.Players.Click_Add.performed -= OnTap;
        inputActions.Players.Click_Remove.performed -= OnInputClickRemove;
        inputActions.Players.Color_Switch.performed -= OnInputColorChange;
        inputActions.Disable();
    }

    void OnInputClickAdd(InputAction.CallbackContext context)
    {
        if (Mouse.current == null) return;

        if (nombreObjetPlacer >= maxObjet) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log("Touché : " + hit.collider.gameObject.name);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.aquamarine, 5f);
            
            Vector3 positionAjustee = hit.point + hit.normal * 0.25f;
            GameObject nouveauObjet = Instantiate(objetAPlacer, positionAjustee, Quaternion.identity);
            Renderer rend = nouveauObjet.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = couleurObjet; 
            }
            ++nombreObjetPlacer;
        }
    }

    private void OnTap(InputAction.CallbackContext context)
    {
        // Obtenir la position du touch/clic
        Vector2 touchPosition = inputActions.Players.Point.ReadValue<Vector2>();
        
        // Liste pour stocker les résultats du raycast AR
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        
        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // Obtenir le plan touché
            ARPlane plane = hits[0].trackable as ARPlane;

            // Adapter selon le type de plan
            Vector3 position = hitPose.position;

            if (plane.alignment == PlaneAlignment.HorizontalUp)
            {
                // Sol ou table - position normale
                position += Vector3.up * 0.25f; // Légèrement au-dessus
            }
            else if (plane.alignment == PlaneAlignment.Vertical)
            {
                // Mur - peut-être coller au mur ?
                position += plane.normal * 0.1f; // Légèrement devant le mur
            }

            GameObject nouveauCube = Instantiate(objetAPlacer, position, Quaternion.identity);

            // Appliquer couleur
            Renderer renderer = nouveauCube.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = couleurObjet;
            }

            nouveauCube.tag = "Cube";

            Debug.Log($"Cube placé sur : {plane.alignment}");
        }
    }
    
    void OnInputClickRemove(InputAction.CallbackContext context)
    {
        if (Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Cube"))
            {
                --nombreObjetPlacer;
                Destroy(hit.transform.gameObject);
            }
        }
    }
    
    private void OnDelete(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = inputActions.Players.Point.ReadValue<Vector2>();

        // On peut utiliser le raycast CLASSIQUE pour toucher les cubes
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Cube"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
    
    void OnInputColorChange(InputAction.CallbackContext context)
    {
        if (context.control.name == "1")
        {
            couleurObjet = Color.red;
        }
        else if (context.control.name == "2")
        {
            couleurObjet = Color.green;
        }
        else if (context.control.name == "3")
        { 
            couleurObjet = Color.blue;
        }
        else if (context.control.name == "4")
        {
            couleurObjet = Color.yellow;
        }
        else if (context.control.name == "5")
        {
            couleurObjet = Color.magenta;
        }
        
        IndicateurCouleur.Instance.ChangeCouleur(couleurObjet);
    }
}
