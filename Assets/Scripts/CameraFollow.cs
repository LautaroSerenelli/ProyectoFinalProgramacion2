using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject jugador;
    [SerializeField] private float suavizado;
    [SerializeField] private Vector3 offset;

    public void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Vector3 posicionDeseada = jugador.transform.position + offset;
            Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado);
            transform.position = new Vector3(posicionSuavizada.x, posicionSuavizada.y, -10f);
    }
}