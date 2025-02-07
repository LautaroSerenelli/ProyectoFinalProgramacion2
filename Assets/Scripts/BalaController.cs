using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BalaController : MonoBehaviour
{
    [SerializeField] private float velocidad;
    public float daño;
    public float chanceCritic;
    public float damageCritic;
    private float dañoFinal;

    private void Update()
    {
        transform.Translate(Vector2.down * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            calculateDamage();
            other.GetComponent<EnemyController>().TomarDaño(dañoFinal);
            Destroy(gameObject);
        }
    }

    public void SetDamage(float newDamage, float criticChance, float criticDamage)
    {
        daño = newDamage;
        chanceCritic = criticChance;
        damageCritic = criticDamage;
    }

    public void calculateDamage()
    {
        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= chanceCritic)
        {
            dañoFinal = daño * damageCritic;
        }
        else
        {
            dañoFinal = daño;
        }
    }
}