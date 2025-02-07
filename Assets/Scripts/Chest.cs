using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chest : MonoBehaviour
{
    [SerializeField] private Sprite spriteAbierto;
    private SpriteRenderer spriteRenderer;
    private bool estaAbierto = false;

    [SerializeField] private bool increaseVel = false;
    [SerializeField] private bool increaseLife = false;
    [SerializeField] private bool increaseDamage = false;
    [SerializeField] private bool increaseCriticPos = false;
    [SerializeField] private bool increaseCriticDam = false;
    [SerializeField] private bool decreaseFireRate = false;

    [Header("UI")]
    public GameObject textPrefab;
    private GameManager gameManager;
    public Canvas canvas;
    private Vector3 posicionInicialTexto;
    private bool initialPositionSet = false;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !estaAbierto)
        {
            AbrirCofre();
            CharacterController characterController = other.GetComponent<CharacterController>();
            ControladorDisparo controladorDisparo = other.GetComponent<ControladorDisparo>();
            if (increaseVel)
            {
                characterController.multiVel += 0.1f;
            }
            if (increaseLife)
            {
                gameManager.vidaJugadorMax += 2;
                gameManager.vidaJugador += 2;
            }
            if (increaseDamage)
            {
                controladorDisparo.IncreaseDamage(10f);
            }
            if (increaseCriticPos)
            {
                controladorDisparo.IncreaseCriticChance(1f);
            }
            if (increaseCriticDam)
            {
                controladorDisparo.increaseCriticDamage(0.1f);
            }
            if (decreaseFireRate)
            {
                controladorDisparo.reduccionFireRate += 0.1f;
            }
        }
    }

    private void AbrirCofre()
    {
        estaAbierto = true;
        spriteRenderer.sprite = spriteAbierto;

        StartCoroutine(ShowUpgrades());
    }

    IEnumerator ShowUpgrades()
    {
        if (increaseVel)
        {
            StartCoroutine(ShowUpgradeText("Multi Vel +0.1"));
            yield return new WaitForSeconds(0.3f);
        }
        if (increaseLife)
        {
            StartCoroutine(ShowUpgradeText("Life +2"));
            yield return new WaitForSeconds(0.3f);
        }
        if (increaseDamage)
        {
            StartCoroutine(ShowUpgradeText("Damage +10"));
            yield return new WaitForSeconds(0.3f);
        }
        if (increaseCriticPos)
        {
            StartCoroutine(ShowUpgradeText("Critic Chance +1%"));
            yield return new WaitForSeconds(0.3f);
        }
        if (increaseCriticDam)
        {
            StartCoroutine(ShowUpgradeText("Multi Critic Damage +0.1"));
            yield return new WaitForSeconds(0.3f);
        }
        if (decreaseFireRate)
        {
            StartCoroutine(ShowUpgradeText("Fire Rate -0.1"));
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator ShowUpgradeText (string upgrade)
    {
        GameObject textObject = Instantiate(textPrefab, canvas.transform);
        textObject.transform.SetParent(canvas.transform, false);
        RectTransform recTransform = textObject.GetComponent<RectTransform>();
        Vector3 worldPosition = transform.position;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);

        TextMeshProUGUI textComponent = textObject.GetComponent<TextMeshProUGUI>();
        textComponent.text = upgrade;

        float time = 0f;
        Vector3 initialPosition = screenPoint;
        if (!initialPositionSet)
        {
            posicionInicialTexto = initialPosition;
            initialPositionSet = true;
        }
        else
        {
            initialPosition = posicionInicialTexto;
        }
        posicionInicialTexto = new Vector3(posicionInicialTexto.x, posicionInicialTexto.y - 50f, posicionInicialTexto.z);
        initialPosition = posicionInicialTexto;
        Vector3 targetPosition = initialPosition + Vector3.up * 100f;

        while (time < 2f)
        {
            float t = Mathf.Clamp01(time / 2f);
            recTransform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }

        float fadeTime = 0.5f;
        Color startColor = textComponent.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        time = 0f;
        while (time < fadeTime)
        {
            textComponent.color = Color.Lerp(startColor, endColor, time/fadeTime);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(textObject);
    }
}