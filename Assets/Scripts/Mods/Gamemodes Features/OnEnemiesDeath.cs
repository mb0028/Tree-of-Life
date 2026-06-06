using System.Threading.Tasks;
using UnityEngine;

public class OnEnemiesDeath : MonoBehaviour
{
    public ParticleSystem deathParticle;

    public async void OnDeath(CGMEnemies e)
    {
        Destroy(e);
        deathParticle.Play();
        gameObject.transform.ScaleTo(Vector3.zero, 0.6f);
        await Task.Delay(600);
        Destroy(gameObject);
    }
}



