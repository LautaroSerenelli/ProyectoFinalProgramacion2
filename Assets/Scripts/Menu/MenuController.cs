using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public RectTransform[] botones;
    public Vector2[] posicionDestino;
    public float duracion;
    public GameObject menuConfig;

    public void AbrirConfigMenu()
    {
        StartCoroutine(MoverBotones(true));
    }

    public void CerrarConfigMenu()
    {
        StartCoroutine(MoverBotones(false));
    }

    private IEnumerator MoverBotones(bool abrir)
    {
        float tiempo = 0f;

        Vector2[] posicionesIniciales = new Vector2[botones.Length];
        Vector2[] posicionesFinales = new Vector2[botones.Length];

        for (int i = 0; i < botones.Length; i++)
        {
            posicionesIniciales[i] = botones[i].anchoredPosition;
            posicionesFinales[i] = abrir ? posicionesIniciales[i] + posicionDestino[i] : posicionesIniciales[i] - posicionDestino[i];
        }

        for (int i = 0; i < botones.Length; i++)
        {
            while (tiempo < duracion)
            {
                tiempo += Time.deltaTime;

                float t = Mathf.Clamp01(tiempo / duracion);
                
                botones[i].anchoredPosition = Vector2.Lerp(posicionesIniciales[i], posicionesFinales [i], t);

                yield return null;
            }

            tiempo = 0f;
        }

        menuConfig.SetActive(true);
    }
}