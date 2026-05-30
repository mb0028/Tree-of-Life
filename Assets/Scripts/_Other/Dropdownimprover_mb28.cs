using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Dropdown))]
public class Dropdownimprover_mb28 : MonoBehaviour
{
    public Button next;
    public Button pervious;
    public GameObject dotsGroup;
    public GameObject dots;
    private TMP_Dropdown dropdown;
    private int maxValues;
    private int cureentValue;

    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        maxValues = dropdown.options.Count;
        cureentValue = dropdown.value;

        next.onClick.AddListener(OnNext);
        pervious.onClick.AddListener(OnPervious);

        for (int i = 0; i < maxValues; i++)
        {
            Instantiate(dots, dotsGroup.GetComponent<RectTransform>()).SetActive(true);
        }
    }

    private void OnNext()
    {
        if (cureentValue < maxValues)
        {
            dropdown.value += 1;
            cureentValue = dropdown.value;
        }
    }

    private void OnPervious()
    {
        if (cureentValue > 0)
        {
            dropdown.value -= 1;
            cureentValue = dropdown.value;
        }
    }

    void Update()
    {
        if (cureentValue == 0)
        {
            next.interactable = true;
            pervious.interactable = false;
        }  else if (cureentValue == maxValues -1) {
            next.interactable = false;
            pervious.interactable = true;
        } else // if (cureentValue != 0 && cureentValue != maxValues)
        {
            next.interactable = true;
            pervious.interactable = true;
        }
    }
 
}
