using TMPro;
using UnityEngine;

[RequireComponent(typeof(OnEnemiesDeath))]
public class CGMEnemies : MonoBehaviour
{
    public TextMeshPro hpText;
    public SpriteRenderer spriteRenderer; 
    public int index;
    private int randomHP;
    private float randomSize;
    private float randomSpeed;
    private float atk;
    private Vector3 treePosition;
    private string namme;

    void OnEnable()
    {
        index = Random.Range(0, CustomGameLoader.enemies.Count);
        namme = CustomGameLoader.enemies[index].enemyData.name;
        spriteRenderer.sprite = CustomGameLoader.enemies[index].sprite;
        randomSize = 0.2f * Random.Range(CustomGameLoader.enemies[index].enemyData.minScale, CustomGameLoader.enemies[index].enemyData.maxScale);
        randomSpeed = 0.6f * CGMSpawner.Instance.enemiesSpeed * Random.Range(CustomGameLoader.enemies[index].enemyData.minSpeed, CustomGameLoader.enemies[index].enemyData.maxSpeed);
        randomHP = Random.Range(CustomGameLoader.enemies[index].enemyData.minHP, CustomGameLoader.enemies[index].enemyData.maxHPExclusive);
        atk = CustomGameLoader.enemies[index].enemyData.atkPower;
        transform.localScale = new Vector3(randomSize, randomSize, transform.localScale.z);
        treePosition = CGMSpawner.Instance.tree.transform.position;
    }

    void Update()
    {
        hpText.text = $"{namme}\nHP: {randomHP}";

        randomSpeed += Time.deltaTime * 0.065f;
        transform.position = Vector3.LerpUnclamped(transform.position, treePosition, randomSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("tree"))
        {
            Destroy(this.gameObject);
            CGMSpawner.Instance.treeHP -= atk;
            if (CGMSpawner.Instance.treeHP <= 0) {
                CGMSpawner.Instance.Lose();
            }
        }
        if (other.gameObject.CompareTag("mouse"))
        {
            randomHP--;
            if (randomHP <= 0) {
                CGMSpawner.Instance.kills++;
                GetComponent<OnEnemiesDeath>().OnDeath(this); 
            }
        }
    }


}
