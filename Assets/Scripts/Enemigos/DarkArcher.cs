using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkArcher : EnemyController
{
    [Header("Arquero Oscuro Stats")]
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private int daño;
    [SerializeField] private int dañoCritico;
    [SerializeField] private float probabilidadCritico;

    [Header("Comportamiento Arquero Oscuro")]
    [SerializeField] private float intervaloDisparo;
    [SerializeField] private float distanciaSegura;
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private float rangoDisparo;
    [SerializeField] private float tiempoTeletransporte;

    private Transform jugador;
    private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    private float tiempoSiguienteDisparo;
    private float siguienteTiempoTeletransporte;
    private Coroutine corutinaDaño;

    public LayerMask obstacleLayer;
    private Vector2 fleeDirection;

    public void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        siguienteTiempoTeletransporte = Time.time + tiempoTeletransporte;
    }

    public void Update()
    {
        Comportamiento();

        ActualizarAnimacion();

        if (Vector2.Distance(transform.position, jugador.position) > rangoDisparo)
        {
            if (Time.time >= tiempoSiguienteDisparo)
            {
                DispararProyectil();
                tiempoSiguienteDisparo = Time.time + intervaloDisparo;
            }
        }

        if (Time.time >= siguienteTiempoTeletransporte)
        {
            Teletransportarse();
            siguienteTiempoTeletransporte = Time.time + tiempoTeletransporte;
        }
    }

    private void ActualizarAnimacion()
    {
        animator.SetFloat("X", rb.velocity.x);
        animator.SetFloat("Y", rb.velocity.y);

        bool estaMoviendose = rb.velocity != Vector2.zero;
        animator.SetBool("Walk", estaMoviendose);
        animator.SetBool("Idle", !estaMoviendose);
    }

    public override void Comportamiento()
    {
        if (Vector2.Distance(transform.position, jugador.position) < distanciaSegura)
        {
            DireccionAlejarse();
            // EvadirObstaculos();
            // MoverEnemigo();
        }
        else if (Vector2.Distance(transform.position, jugador.position) > distanciaSegura * 2)
        {
            DetenerMovimiento();
        }
    }

    private void DireccionAlejarse()
    {
        Vector2 direccionAlejar = (transform.position - jugador.position).normalized;
        rb.velocity = direccionAlejar * velocidadMovimiento;
        // fleeDirection = direccionAlejar;
    }

    // private void EvadirObstaculos()
    // {
    //     RaycastHit2D hit = Physics2D.Raycast(transform.position, fleeDirection, 8f, obstacleLayer);

    //     if (hit.collider != null)
    //     {
    //         Vector2 perpendicular = Vector2.Perpendicular(fleeDirection);
    //         Vector2 newDirection = Vector2.zero;

    //         if(!Physics2D.Raycast(transform.position, perpendicular, 8f, obstacleLayer))
    //         {
    //             newDirection = perpendicular;
    //         }
    //         else if(!Physics2D.Raycast(transform.position, -perpendicular, 8f, obstacleLayer))
    //         {
    //             newDirection = -perpendicular;
    //         }

    //         if(newDirection != Vector2.zero)
    //         {
    //             fleeDirection = newDirection.normalized;
    //         }
    //     }
    // }

    // private void MoverEnemigo()
    // {
    //     transform.position += (Vector3)(fleeDirection * velocidadMovimiento * Time.deltaTime);
    // }

    private void DetenerMovimiento()
    {
        rb.velocity = Vector2.zero;
    }

    private void DispararProyectil()
    {
        Vector2 direccionDisparo = (jugador.position - transform.position).normalized;
        GameObject proyectil = Instantiate(proyectilPrefab, transform.position, Quaternion.identity);
        proyectil.GetComponent<Rigidbody2D>().velocity = direccionDisparo * 5f; // Ajusta la velocidad del proyectil
    }

    private void Teletransportarse()
    {
        Vector2 nuevaPosicion = (Vector2)transform.position + Random.insideUnitCircle * 2f;
        transform.position = nuevaPosicion;
    }

    private bool CalcularDañoCritico()
    {
        float randomChance = Random.value;
        return randomChance <= probabilidadCritico;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (corutinaDaño != null)
            {
                StopCoroutine(corutinaDaño);
            }

            corutinaDaño = StartCoroutine(AplicarDañoContacto());
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (corutinaDaño != null)
            {
                StopCoroutine(corutinaDaño);
                corutinaDaño = null;
            }
        }
    }

    private IEnumerator AplicarDañoContacto()
    {
        while (true)
        {
            int dañoFinal = CalcularDañoCritico() ? dañoCritico : daño;
            GameManager.Instancia.PerderVida(dañoFinal);
            yield return new WaitForSeconds(0.2f);
        }
    }
}