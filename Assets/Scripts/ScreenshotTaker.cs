using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenshotTaker : MonoBehaviour
{
    public void _TakeScreenshot()
    {
        StartCoroutine(CaptureAndSave());
    }

    private IEnumerator CaptureAndSave()
    {
        yield return new WaitForEndOfFrame();

        string fileName = "Tree At " + System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png";

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            string windowsSavePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), fileName);
            ScreenCapture.CaptureScreenshot(windowsSavePath);
            TreeNotification.ScreenshotNotif("Saved in: " + windowsSavePath);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            Texture2D screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();
            byte[] bytes = screenshotTexture.EncodeToPNG();
            Destroy(screenshotTexture);
        
            string albumName = "Tree of Life";
            TreeNotification.ScreenshotNotif($"Screenshot Saved\n0/DCIM/Tree of Life/{fileName}");
            NativeGallery.SaveImageToGallery(bytes, albumName, fileName, null);
        }
        
        yield break;
    }
}