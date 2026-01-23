using System;
using UnityEngine;

public class IndicateurCouleur : MonoBehaviour
{
    public static IndicateurCouleur Instance { get; private set; }
    
    public GameObject objetIndicateur;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
    public void ChangeCouleur(Color couleurObjet)
    {
        Renderer rend = objetIndicateur.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = couleurObjet;
        }
    }
}
