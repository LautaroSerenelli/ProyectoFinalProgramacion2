using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float vida;

    public void TomarDaño (float daño)
    {
        vida -= daño;

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Comportamiento() { }
}