using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControladorDisparo : MonoBehaviour
{
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject bala;
    [SerializeField] private float distancia = 2;
    [SerializeField] private float fireRate;
    [SerializeField] private float nextFireTime = 0;
    public float reduccionFireRate = 0;
    public float newDamage = 10;
    public float criticChance = 0;
    public float criticDamage = 1.1f;

    public Slider slider;
    public float tiempoCarga = 1f;
    public Vector3 offset;
    public Transform characterController;
    
    private void Update()
    {
        tiempoCarga += Time.deltaTime;
        slider.transform.position = Camera.main.WorldToScreenPoint(characterController.position + offset);
        Vector3 posicionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionMouse.z = 0;

        float distanciaAlJugador = Vector3.Distance(transform.position, posicionMouse);
        
        if (distanciaAlJugador > 1f)
        {
            Vector3 direccion = posicionMouse - controladorDisparo.position;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg + 90f;
            controladorDisparo.rotation = Quaternion.Euler(new Vector3(0, 0, angulo));
            controladorDisparo.position = transform.position + direccion.normalized * distancia;
        }
        else
        {
            Vector3 direccion = posicionMouse - transform.position;
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg + 90f;
            controladorDisparo.rotation = Quaternion.Euler(new Vector3(0, 0, angulo));
            controladorDisparo.position = transform.position + direccion.normalized / 2f;
        }

        slider.value = Mathf.Clamp(tiempoCarga / fireRate, 0f, 1f);

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Disparar();
            tiempoCarga = 0f;
            nextFireTime = Time.time + fireRate - reduccionFireRate;
        }
    }

    private void Disparar()
    {
        Instantiate(bala, controladorDisparo.position, controladorDisparo.rotation);
        bala.GetComponent<BalaController>().SetDamage(newDamage, criticChance, criticDamage);
    }

    public void IncreaseDamage(float amount)
    {
        newDamage += amount;
    }

    public void IncreaseCriticChance(float amount)
    {
        criticChance += amount;
    }

    public void increaseCriticDamage(float amount)
    {
        criticDamage += amount;
    }
}