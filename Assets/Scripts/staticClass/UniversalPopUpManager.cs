using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UniversalPopupManager : MonoBehaviour
{
    public static UniversalPopupManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private CanvasGroup popupCanvasGroup;
    [SerializeField] private TextMeshProUGUI popupText;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private float defaultDisplayDuration = 2.5f;

    private Coroutine activePopupCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (popupCanvasGroup != null)
            {
                popupCanvasGroup.alpha = 0f;
                popupCanvasGroup.gameObject.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void ShowPopup(string message, float duration = -1f)
    {
        if (Instance != null)
        {
            float time = duration > 0 ? duration : Instance.defaultDisplayDuration;
            Instance.DisplayPopup(message, time);
        }
        else
        {
            Debug.LogWarning($"UniversalPopupManager is missing! Could not show message: {message}");
        }
    }

    private void DisplayPopup(string message, float duration)
    {
        if (activePopupCoroutine != null)
        {
            StopCoroutine(activePopupCoroutine);
        }

        activePopupCoroutine = StartCoroutine(PopupRoutine(message, duration));
    }

    private IEnumerator PopupRoutine(string message, float duration)
    {
        popupText.SetText(TextIconUtility.ParseDescription(message));
        popupCanvasGroup.gameObject.SetActive(true);
        popupCanvasGroup.transform.localScale = Vector3.one * 0.9f;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            popupCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            popupCanvasGroup.transform.localScale = Vector3.Lerp(Vector3.one * 0.9f, Vector3.one, t);
            yield return null;
        }
        popupCanvasGroup.alpha = 1f;

        yield return new WaitForSeconds(duration);

        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            popupCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        popupCanvasGroup.alpha = 0f;
        popupCanvasGroup.gameObject.SetActive(false);
    }
}
