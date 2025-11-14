using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    private ObjectPool<BulletProjectile> _pool;
    [SerializeField] private float damage = 1;
    [SerializeField] private float lifeTime = 5;
    [SerializeField] private float bulletSpeed = 5;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetPool(ObjectPool<BulletProjectile> pool)
    {
        _pool = pool;
    }

    public void Launch(Vector3 direction)
    {
        gameObject.SetActive(true);
        _rb.velocity = direction * bulletSpeed;
        Invoke("ReturnToPool", lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var target))
        {
            if (other.gameObject.tag == "Player")
                return;
            target.TakeDamage(damage);
            ReturnToPool();
        }

        if (other.gameObject.tag == "Obstacle")
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        _rb.velocity = Vector2.zero;
        _pool?.ReturnToPool(this);
        gameObject.SetActive(false);
    }
}