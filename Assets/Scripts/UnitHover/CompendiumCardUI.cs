using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompendiumCardUI : MonoBehaviour
{
    public Image unitSpriteImage;
    public TextMeshProUGUI nameText;
    public GameObject crownIcon; // Optional: Assign a little crown Image if you want!

    private UnitDefinition myDef;
    private UnitDetailModal modal;
    private bool isDiscovered;

    public void Initialize(UnitDefinition def, bool discovered, bool crowned, UnitDetailModal modalRef)
    {
        myDef = def;
        isDiscovered = discovered;
        modal = modalRef;

        unitSpriteImage.sprite = def.unitSprite;

        if (isDiscovered)
        {
            unitSpriteImage.color = Color.white;
            nameText.text = def.unitName;
            if (crownIcon != null) crownIcon.SetActive(crowned);
        }
        else
        {
            unitSpriteImage.color = Color.black;
            nameText.text = "???";
            if (crownIcon != null) crownIcon.SetActive(false);
        }
    }

    public void OnCardClicked()
    {
        if (isDiscovered && modal != null) modal.OpenModal(myDef);
    }

    // This is the missing getter needed for the Filter System!
    public UnitDefinition GetDefinition() => myDef;
}
