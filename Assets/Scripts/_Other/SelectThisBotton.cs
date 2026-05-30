using UnityEngine;
using UnityEngine.EventSystems;

public class SelectThisBotton : MonoBehaviour
{
    public bool late = false;

    void OnEnable()
    {
        if (late == false) EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    void Start()
    {
        if (late == true) EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
}
