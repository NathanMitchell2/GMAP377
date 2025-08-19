using UnityEngine;

public class WebProjectile : MonoBehaviour
{
    public float lifeTime = 5f;
    public float splashRange = 0.5f;
    public float slowDuration = 3f;
    public float slowFactor = 0.5f;
    public EnemyAI spider;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Apply slow + add to web stack
            carControler carControler = other.gameObject.GetComponentInChildren<carControler>();
            if (carControler != null)
            {
                carControler.ApplySlow(slowDuration, slowFactor);

                if (spider != null)
                    spider.webStack = Mathf.Min(spider.webStack + 1, spider.maxWebStacks);
            }
        }

        Destroy(gameObject);
    }
}
