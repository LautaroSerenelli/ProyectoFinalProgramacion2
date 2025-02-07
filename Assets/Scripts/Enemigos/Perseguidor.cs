using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perseguidor : EnemyController
{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private int daño;
    [SerializeField] private int dañoCritico;
    [SerializeField] private float intervaloDaño;
    private float tiempoSiguienteDaño;
    [SerializeField] private float probabilidadCritico;

    private Transform jugador;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 direccionMovimiento;
    private Coroutine corutinaDaño;

    public void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        Comportamiento();

        Vector3 direccion = jugador.position - transform.position;
        direccionMovimiento = direccion.normalized;
        animator.SetFloat("X", direccionMovimiento.x);
        animator.SetFloat("Y", direccionMovimiento.y);

        if (direccionMovimiento != Vector2.zero)
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", true);
        }
    }

    public override void Comportamiento()
    {
        transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidadMovimiento * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (corutinaDaño != null)
            {
                StopCoroutine(corutinaDaño);
            }

            corutinaDaño = StartCoroutine(AplicarDaño());
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