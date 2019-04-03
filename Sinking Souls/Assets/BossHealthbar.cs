using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class BossHealthbar : MonoBehaviour
{
    [Header("Configuration")]
    public float trailingDelay = 1.5f;
    public float normalSpeed = 0.1f;
    public float trailingSpeed = 0.1f;

    [Header("Setup")]
    public Image healthbar;
    public Image backHealthbar;

    private float maxHealth;
    private float previousHealth;


    protected abstract float GetCurrentHealth();
    protected abstract float GetMaxHealth();

    private void Start()
    {
        maxHealth = GetMaxHealth();
    }

    private void Update()
    {
        if (previousHealth != GetCurrentHealth())
        {
            StartCoroutine(UpdateLife());
        }

        previousHealth = GetCurrentHealth();
    }

    private IEnumerator UpdateLife()
    {
        float currentFill = healthbar.fillAmount;
        float fillTarget = GetCurrentHealth() / maxHealth;

        StartCoroutine(TrailingLife(fillTarget));

        for (float t = 0; t < 1.0f; t += normalSpeed)
        {
            healthbar.fillAmount = Mathf.SmoothStep(currentFill, fillTarget, t);
            yield return null;
        }
    }

    private IEnumerator TrailingLife(float fillTarget)
    {
        yield return new WaitForSecondsRealtime(trailingDelay);

        float currentFill = backHealthbar.fillAmount;

        for (float t = 0; t < 1.0f; t += trailingSpeed)
        {
            backHealthbar.fillAmount = Mathf.SmoothStep(currentFill, healthbar.fillAmount, t);
            yield return null;
        }
    }
}
