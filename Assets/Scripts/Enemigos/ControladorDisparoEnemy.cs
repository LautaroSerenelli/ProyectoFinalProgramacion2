using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorDisparoEnemy : MonoBehaviour
{
    [SerializeField] private int daño;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instancia.PerderVida(daño);
        }
    }
}