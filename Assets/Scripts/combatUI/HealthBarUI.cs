using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private Image shieldFill;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI healthTitleText;

    public void SetValues(int currentHP, int maxHP, int shield)
    {
        healthFill.fillAmount = (float)currentHP / maxHP;
        shieldFill.fillAmount = Mathf.Clamp01((float)shield / maxHP);
    }

    public void SetHoverUIValues(int currentHP, int maxHP, int shield)
    {
        healthFill.fillAmount = (float)currentHP / maxHP;
        shieldFill.fillAmount = Mathf.Clamp01((float)shield / maxHP);

        if (healthText != null)
        {
            if (shield > 0)
            {
                healthText.text = $"{currentHP} <color=#FFD700>+{shield}</color> / {maxHP}";
            }
            else
            {
                // Format: 260 / 260
                healthText.text = $"{currentHP} / {maxHP}";
            }
        }
    }

    public void SetTextVisible(bool visible)
        {
         if (healthText != null)
             healthText.gameObject.SetActive(visible);
         if (healthTitleText != null)
             healthTitleText.gameObject.SetActive(visible);
        healthTitleText.SetText(TextIconUtility.ParseDescription("[MAXHEALTH] HP"));
    }
}
