using UnityEngine;

public class DisableOnPlatforms : MonoBehaviour
{
    public bool onAndroid = false;
    public bool onWindows = false;
    public bool onEditor = false;

    void Awake()
    {
        if (onAndroid == true && Application.platform == RuntimePlatform.Android)
            this.gameObject.SetActive(false);

        if (onWindows == true && Application.platform == RuntimePlatform.WindowsPlayer)
            this.gameObject.SetActive(false);

        if (onEditor == true && Application.platform == RuntimePlatform.WindowsEditor)
            this.gameObject.SetActive(false);
    }

}
