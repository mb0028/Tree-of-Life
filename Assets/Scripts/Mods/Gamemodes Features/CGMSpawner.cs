using System.Collections;
using TMPro;
using UnityEngine;

public class CGMSpawner : MonoBehaviour
{
    public static CGMSpawner Instance;

    public GameObject Enemy;
    public GameObject LosePanel;
    public float enemiesSpeed = 1;
    public float spawnrate = 0.6f;
    public float treeHP = 10;
    public bool isWin = false;
    public int kills = 0;

    [Header("refs")]
    public GameObject winPanel;
    public GameObject tree;
    public TextMeshProUGUI treeHPText;
    public TextMeshProUGUI scoreText;
    public AudioSource audioSource;

    public float record;
    private float lastFrameTreeHP;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        lastFrameTreeHP = treeHP;
        StartCoroutine(SpawnEnemy());
    }

    void Update()
    {
        if (treeHP != lastFrameTreeHP)
        {
            float difference = treeHP - lastFrameTreeHP;
            if (difference < 0)
                tree.transform.Find("Tree Damaged Particle").GetComponent<ParticleSystem>().Play();
            lastFrameTreeHP = treeHP;
        }

        treeHPText.text = $"HP: {treeHP}";
        scoreText.text = $"Score: {kills}";
    }

    public void Lose()
    {
        StopAllCoroutines();
        tree.transform.Find("Tree destroy ptcl").GetComponent<ParticleSystem>().Play();
        LosePanel.SetActive(true);
        audioSource.Stop();
    }

    public IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnrate);

            Vector2 spawnpos = new(Random.Range(-18, 18), Random.Range(-18, 18));
            float distance = (spawnpos - Vector2.zero).magnitude;

            if (distance >= 8 && distance <= 16)
                Instantiate(Enemy, new Vector3(spawnpos.x, spawnpos.y, transform.position.z), transform.rotation);
        }
    }
}
