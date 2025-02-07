using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinter : EnemyController
{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private int daño;
    [SerializeField] private int dañoCritico;
    [SerializeField] private float intervaloDaño;
    private float tiempoSiguienteDaño;
    [SerializeField] private float probabilidadCritico;

    [Header("Comportamiento Sprinter")]
    [SerializeField] private float tiempoHastaFrenado;
    [SerializeField] private float tiempoDeParpadeo;
    [SerializeField] private float distanciaDash;
    [SerializeField] private float velocidadDash;
    [SerializeField] private int dañoDash;
    [SerializeField] private int dañoCriticoDash;

    private Transform jugador;
    private Rigidbody2D rb;
    private Animator animator;
    public SpriteRenderer spriteRenderer;

    private Vector2 direccionMovimiento;
    private bool estaCorriendo = true;
    private bool realizandoDash = false;
    private Coroutine corutinaDaño;

    public void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();        
        StartCoroutine(CicloDeComportamiento());
    }

    public void Update()
    {
        if (!realizandoDash)
        {
            if (estaCorriendo)
            {
                Vector3 direccion = jugador.position - transform.position;
                direccionMovimiento = direccion.normalized;

                ActualizarAnimacion();

                rb.velocity = direccionMovimiento * velocidadMovimiento;
            }
        }
    }

    private void ActualizarAnimacion()
    {
        animator.SetFloat("X", direccionMovimiento.x);
        animator.SetFloat("Y", direccionMovimiento.y);

        bool estaMoviendose = direccionMovimiento != Vector2.zero;
        animator.SetBool("Walk", estaMoviendose);
        animator.SetBool("Idle", !estaMoviendose);
    }

    private IEnumerator CicloDeComportamiento()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoHastaFrenado);
            estaCorriendo = false;

            StartCoroutine(Parpadeo());
            yield return new WaitForSeconds(tiempoDeParpadeo);

            StartCoroutine(DashHaciaJugador());
            yield return new WaitForSeconds(distanciaDash / velocidadDash);

            estaCorriendo = true;
        }
    }

    private IEnumerator Parpadeo()
    {
        Color colorOriginal = spriteRenderer.material.GetColor("_ColorRemplazo");

        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.material.SetColor("_ColorRemplazo", Color.white);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.material.SetColor("_ColorRemplazo", colorOriginal);
            yield return new WaitForSeconds(0.1f);
        }
        spriteRenderer.material.SetColor("_ColorRemplazo", colorOriginal);
    }

    private IEnumerator DashHaciaJugador()
    {
        realizandoDash = true;
        Vector3 direccionDash = (jugador.position - transform.position).normalized;

        Vector3 destino = transform.position + direccionDash * distanciaDash;

        while (Vector3.Distance(transform.position, destino) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidadDash * Time.deltaTime);
            yield return null;
        }

        realizandoDash = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!realizandoDash)
            {
                if (corutinaDaño != null)
                {
                    StopCoroutine(corutinaDaño);
                }

                corutinaDaño = StartCoroutine(AplicarDaño());
            }
            else
            {
                int dañoFinalDash = CalcularDañoCritico() ? dañoCriticoDash : dañoDash;
                GameManager.Instancia.PerderVida(dañoFinalDash);
            }
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

    private IEnumerator AplicarDaño()
    {
        while (true)
        {
            int dañoFinal = CalcularDañoCritico() ? dañoCritico : daño;
            GameManager.Instancia.PerderVida(dañoFinal);
            yield return new WaitForSeconds(intervaloDaño);
        }
    }

    private bool CalcularDañoCritico()
    {
        float randomChance = Random.value;
        return randomChance <= probabilidadCritico;
    }
}