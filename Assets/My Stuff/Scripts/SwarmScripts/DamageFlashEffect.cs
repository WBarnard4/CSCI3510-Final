using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Required for Coroutines

public class DamageFlashEffect : MonoBehaviour
{
    public Image flashImage; // Assign your UI Image here in the Inspector
    public float flashDuration = 0.1f; // How long the flash stays at full intensity
    public float fadeDuration = 0.2f; // How long it takes to fade back to transparent
    public Color flashColor = Color.red; // Color of the flash

    private Coroutine currentFlashCoroutine;

    // Call this method when the player takes damage
    public void TriggerFlash()
    {
        if (currentFlashCoroutine != null)
        {
            StopCoroutine(currentFlashCoroutine);
        }
        currentFlashCoroutine = StartCoroutine(FlashScreen());
    }

    private IEnumerator FlashScreen()
    {
        // Instantly set to full flash color
        flashImage.color = flashColor;

        // Hold at full intensity for a short duration
        yield return new WaitForSeconds(flashDuration);

        // Fade back to transparent
        float timer = 0f;
        Color startColor = flashColor;
        Color endColor = new Color(flashColor.r, flashColor.g, flashColor.b, 0f); // Transparent version of flashColor

        while (timer < fadeDuration)
        {
            flashImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure it's fully transparent at the end
        flashImage.color = endColor;
    }
}