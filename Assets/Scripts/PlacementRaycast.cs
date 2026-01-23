using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementRaycast : MonoBehaviour
{
    private PlayerInputActions inputActions;
    
    public IndicateurCouleur indicateurCouleur;
    
    public GameObject objetAPlacer; // Assignez un prefab dans l'inspecteur
    
    private Color couleurObjet = Color.red;
    
    public LayerMask layerMask;

    private int nombreObjetPlacer;

    public int maxObjet = 10;
    
    void Awake()
    {
        // Créer une instance des Input Actions
        inputActions = new PlayerInputActions();
        indicateurCouleur.ChangeCouleur(couleurObjet);
    }
    
    void OnEnable()
    {
        // IMPORTANT : Activer les actions
        inputActions.Players.Click_Add.performed += OnInputClickAdd;
        inputActions.Players.Click_Remove.performed += OnInputClickRemove;
        inputActions.Players.Color_Switch.performed += OnInputColorChange;
        inputActions.Enable();
    }

    void OnDisable()
    {
        // IMPORTANT : Désactiver pour éviter les fuites mémoire
        inputActions.Players.Click_Add.performed -= OnInputClickAdd;
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
