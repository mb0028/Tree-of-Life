using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpModdingPanel : MonoBehaviour
{
    public GameObject[] tabs;
    public GameObject[] panels;

    public Color noColor = new(0,0,0,0);
    public Color selectedColor = new(0, 0, 0, 0);

    public TextMeshProUGUI winText;

    void Start()
    {
        if (winText != null) { winText.text = $"Open C:/Users/{System.Environment.UserDomainName}/Documents/Tree of Life Mods/\nAnd Change Files!\n\nRead Documentation in the left Tabs"; }
    }

    public void OnTabClicked(int thisTab)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].GetComponent<Image>().color = noColor;
            panels[i].SetActive(false);
        }

        tabs[thisTab].GetComponent<Image>().color = selectedColor;
        panels[thisTab].SetActive(true);
    }

    public void ClosePanel()
    {
        Destroy(this.gameObject);
    }

    public void StartCustomGamemode()
    {
        SceneManager.LoadScene(0);
    }

}
