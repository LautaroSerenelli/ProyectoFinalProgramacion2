using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    public float vidaMax;
    public float vida;

    public Slider slider;
    private GameManager gameManager;
    
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        vida = gameManager.vidaJugador;
        vidaMax = gameManager.vidaJugadorMax;

        slider.value = vida / vidaMax;
    }
}