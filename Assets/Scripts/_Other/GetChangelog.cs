using System;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetChangelog : MonoBehaviour
{
    public GameObject[] tabs;
    public Color noColor = new Color(0,0,0,0);
    public Color selectedColor = new Color(0, 0, 0, 0);
    public TextMeshProUGUI creditsAndChangelog;
    public string fileNameInStreaming;
    public string fileNameInPresistent;


    void Start()
    {
        fileNameInStreaming = Path.Combine(Application.streamingAssetsPath, "Changelog.txt");
        fileNameInPresistent = Path.Combine(Application.persistentDataPath, "Changelog.txt");

        CopyFromStreamToPersistent();
    }

    private async void CopyFromStreamToPersistent()
    {
        using UnityWebRequest wGetChangelog = UnityWebRequest.Get(fileNameInStreaming);
        await wGetChangelog.SendWebRequest();

        byte[] fileBytes = wGetChangelog.downloadHandler.data;
        try {
            await Task.Run(async () => await File.WriteAllBytesAsync(fileNameInPresistent, fileBytes) ); 
            creditsAndChangelog.text = await File.ReadAllTextAsync(fileNameInPresistent);
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }

    public void TMPNextPage(int thisTab)
    {
        creditsAndChangelog.pageToDisplay = thisTab;

        for (int i = 1; i < tabs.Length + 1; i++)
            tabs[i -1].GetComponent<Image>().color = noColor;

        tabs[thisTab -1].GetComponent<Image>().color = selectedColor;
    }

}
