using System.Collections;
using TMPro;
using UnityEngine;

public class TreeNotification : MonoBehaviour
{
    private static WaitForSecondsRealtime _waitForSecondsRealtime0_5 = new(0.5f);
    public static TreeNotification Instance;
    public GameObject winNotifications;

    void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); return; }
    }

    public static void ScreenshotNotif(string notifContent)
    {
        TreeNotification.Instance.InGameNotification(notifContent);
    }

    public static void NewRecordNotif(int record, string gamemode)
    {
        TreeNotification.Instance.InGameNotification($"New Record!\n{record} is your new record for {gamemode}!");
    }

    public static void Unregistered(string message, int duration)
    {
        TreeNotification.Instance.InGameNotification(message, duration);
    }

    public void InGameNotification(string content, int duration = 3)
    {
        StartCoroutine(ShowInGameNotif(content, duration));
    }
    
    private IEnumerator ShowInGameNotif(string content, int duration = 3)
    {
        winNotifications.GetComponent<CanvasGroup>().alpha = 1;
        winNotifications.SetActive(false);
        winNotifications.SetActive(true);
        winNotifications.GetComponentInChildren<TextMeshProUGUI>().text = content;

        yield return new WaitForSecondsRealtime(duration);
        
        // winNotifications.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        yield return _waitForSecondsRealtime0_5;

        winNotifications.SetActive(false);

        yield break;
    }
}
