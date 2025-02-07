using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurarVida : MonoBehaviour
{
    private GameManager gameManager;
    public int vida;
    public bool usado = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !usado)
        {
            vida = gameManager.vidaJugadorMax - gameManager.vidaJugador;
            gameManager.RestaurarVida(vida);
            usado = true;
        }
    }
}