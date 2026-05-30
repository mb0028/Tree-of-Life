using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string TooltipText;
    public Color TextColor = Color.white;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.showTooltip = true;
        Tooltip.tooltipText = TooltipText;
        Tooltip.textColor = TextColor;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.showTooltip = false;
    }

    void OnDisable()
    {
        OnPointerExit(null);
    }


}
