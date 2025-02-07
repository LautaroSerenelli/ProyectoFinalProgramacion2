using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }

    [Header("Estado del Juego")]
    public int puntuacion;
    public int vidaJugadorMax;
    public int vidaJugador;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        vidaJugadorMax = vidaJugador;
    }

    public void AgregarPuntos(int puntos)
    {
        puntuacion += puntos;
        Debug.Log("Puntuación: " + puntuacion);
    }

    public void PerderVida(int daño)
    {
        vidaJugador -= daño;
        Debug.Log("Vida del jugador: " + vidaJugador);

        if (vidaJugador <= 0)
        {
            GameOver();
        }
    }

    public void RestaurarVida(int cantidad)
    {
        vidaJugador += cantidad;
        Debug.Log("Vida restaurada: " + vidaJugador);
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
    }
}