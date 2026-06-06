using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OutlineMaterialGlowMusic : MonoBehaviour
{
    private int roundnessAndroid = 0;
    private Material imageMat;
    private int alphaID = Shader.PropertyToID("_Alpha");

    void Awake()
    {
        imageMat = GetComponent<Image>().material;
        if (Application.platform == RuntimePlatform.Android && PauseTheGame.Instance.isTVPlatform == false)
        {
            using AndroidJavaClass mbJava = new("com.mb28.treeoflife.MBJava");
            roundnessAndroid = mbJava.CallStatic<int>("cornerRadius", AndroidApplication.currentActivity);
            imageMat.SetFloat("_Roundness", Mathf.Clamp(Mathf.InverseLerp(0, 150, roundnessAndroid) * 0.05f, 0, 200));
        }
        else
            imageMat.SetFloat("_Roundness", 0f);

        imageMat.SetFloat(alphaID, 0);
    }

    void Update()
    {
        if (MusicCircleSpawner.Instance.IsStarted)
            imageMat.SetFloat(alphaID, Mathf.Clamp01(AudioPeer.audioBandBuffer[3] - 0.3f));            
    }
}
