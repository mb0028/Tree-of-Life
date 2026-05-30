using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OutlineMaterialGlowMusic : MonoBehaviour
{
    private int roundnessAndroid = 0;
    private Material imageMat;

    void Awake()
    {
        imageMat = GetComponent<Image>().material;
        if (Application.platform == RuntimePlatform.Android && PauseTheGame.Instance.isTVPlatform == false)
        {
            try
            {
                using AndroidJavaClass mbJava = new("com.mb28.treeoflife.MBJava");
                roundnessAndroid = mbJava.CallStatic<int>("cornerRadius", AndroidApplication.currentActivity);
                imageMat.SetFloat("_Roundness", Mathf.Clamp(Mathf.InverseLerp(0, 150, roundnessAndroid) * 0.05f, 0, 200));
            }
            catch (System.Exception e)
            {
                TreeNotification.Unregistered("ERROR", 3);
                GUIUtility.systemCopyBuffer = e.ToString();
                imageMat.SetFloat("_Roundness", 0f);
            }
        }
        else
            imageMat.SetFloat("_Roundness", 0f);
    }

    void Update()
    {
        if (MusicCircleSpawner.Instance.IsStarted)
            imageMat.SetFloat("_Alpha", Mathf.Clamp01(AudioPeer.audioBandBuffer[3] - 0.3f));
        else
            imageMat.SetFloat("_Alpha", 0);
    }
}
