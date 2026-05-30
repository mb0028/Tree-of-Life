using UnityEngine;

public class MusicCircleAbility : MonoBehaviour
{

    public GameObject circle;
    public CircleCollider2D circleCollider;
    public GameObject treeOnFire;
    private float sspeed;

    void OnEnable()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material glow = renderer.material;
        Color TheColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        Color newColor = TheColor * 2.5f;
        glow.SetColor("_Color", newColor);
        circle.transform.localScale *= MusicCircleSpawner.Instance.baseScale * Random.Range(0.8f, 1.2f);
    }

    void Update()
    {
        sspeed = Time.deltaTime * MusicCircleSpawner.Instance.shrinkSpeed;

        if (circle.transform.localScale.x >= 0.05f && circle.transform.localScale.y >= 0.05f)
        {
            circle.transform.localScale -= new Vector3(sspeed, sspeed, transform.localScale.z);
            circleCollider.radius += sspeed * 3;
            // Debug.Log(circleCollider.radius.ToString());
        }
        else
        {
            if (MusicCircleSpawner.Instance.IsStarted) { Instantiate(treeOnFire, transform.position, transform.rotation); }
            Destroy(this.gameObject);
            MusicCircleSpawner.Instance.Lose();
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("mouse"))
        {
            MusicCircleSpawner.Instance.score += 1;
            MusicCircleSpawner.Instance.scoreText.text = "Score: " + MusicCircleSpawner.Instance.score;
            Destroy(this.gameObject);
        }
    }




}
