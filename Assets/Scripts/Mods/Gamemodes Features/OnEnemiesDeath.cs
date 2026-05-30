using System.Threading.Tasks;
using UnityEngine;

public class OnEnemiesDeath : MonoBehaviour
{
    public ParticleSystem deathParticle;

    public async void OnDeath(CGMEnemies e)
    {
        Destroy(e);
        GameObject p = Instantiate(deathParticle.gameObject);
        p.GetComponent<ParticleSystem>().Play();

        //await Task.Delay(600);
        Destroy(p);
        Destroy(gameObject);
    }
}



