using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverPrisms : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Prism References")]
    [SerializeField] private GameObject leftPrism;
    [SerializeField] private GameObject rightPrism;

    [Header("Animation Settings")]
    [SerializeField] private float floatDistance = 10f;
    [SerializeField] private float floatSpeed = 4f;    

    private Vector3 leftStartPos;
    private Vector3 rightStartPos;
    private bool isHovered = false;

    private void Start()
    {
        // Store original positions so they know where to return
        if (leftPrism != null) leftStartPos = leftPrism.transform.localPosition;
        if (rightPrism != null) rightStartPos = rightPrism.transform.localPosition;

        // Ensure they start hidden
        SetPrismsActive(false);
    }

    private void Update()
    {
        if (isHovered)
        {
            // Mathf.PingPong creates a smooth back-and-forth value
            float offset = Mathf.PingPong(Time.time * floatSpeed, floatDistance);

            // Move the left prism to the left, right prism to the right
            if (leftPrism != null)
                leftPrism.transform.localPosition = leftStartPos + new Vector3(-offset, 0f, 0f);

            if (rightPrism != null)
                rightPrism.transform.localPosition = rightStartPos + new Vector3(offset, 0f, 0f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        SetPrismsActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        SetPrismsActive(false);

        // Snap them back to their starting positions immediately
        if (leftPrism != null) leftPrism.transform.localPosition = leftStartPos;
        if (rightPrism != null) rightPrism.transform.localPosition = rightStartPos;
    }

    private void SetPrismsActive(bool state)
    {
        if (leftPrism != null) leftPrism.SetActive(state);
        if (rightPrism != null) rightPrism.SetActive(state);
    }
}
