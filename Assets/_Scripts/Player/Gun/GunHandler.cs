using System.Collections;
using System.Linq;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform gun;
    [SerializeField] private Transform gunPointer;
    [SerializeField] private Transform playerBody;
    [SerializeField] private BulletProjectile bulletPrefab;
    [SerializeField] private Transform projectilesPoolParent;
    [SerializeField] private Transform walkImage;
    [SerializeField] private Transform aimImage;
    [SerializeField] private Player player;

    [Header("Settings")]
    [SerializeField] private float fireCooldown = 1f;
    [SerializeField] private float aimTime = 0.2f;
    [SerializeField] private float returnAimTime = 0.5f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private string ammoId = "0";

    [Header("Pool")]
    [SerializeField] private int poolSize = 10;

    private ObjectPool<BulletProjectile> _bulletPool;

    private bool canShoot = true;
    private Coroutine returnCoroutine;
    private Vector3 _shootDirection = Vector3.zero;

    private void Awake()
    {
        _bulletPool = new ObjectPool<BulletProjectile>(bulletPrefab, poolSize, projectilesPoolParent);
    }

    private void Update()
    {
        HandleAimImages();
    }

    public void Shoot()
    {
        int totalAmmo = InventoryManager.Instance.Inventory.GetTotalAmount(ammoId);
        if (totalAmmo <= 0)
        {
            Debug.Log("Нет патронов!");
            return;
        }

        Transform nearestTarget = FindNearestTarget();
        if (!canShoot || nearestTarget == null) return;

        if (returnCoroutine != null)
            StopCoroutine(returnCoroutine);

        EventBus.Publish(new ShootButtonPressedEvent(nearestTarget));
        StartCoroutine(ShootRoutine(nearestTarget));
    }

    private IEnumerator ShootRoutine(Transform target)
    {
        canShoot = false;
        aimImage.gameObject.SetActive(false);

        yield return StartCoroutine(AimAtTarget(target));

        EventBus.Publish(new PlayerShootEvent(
            this.transform.position,
            (target.position - gun.position).normalized,
            target));

        bool consumed = InventoryManager.Instance.Inventory.TryConsume(ammoId, 1);
        if (!consumed)
        {
            Debug.Log("Патроны закончились!");
            yield break;
        }
        EventBus.Publish(new InventoryUpdatedEvent());

        BulletProjectile bullet = _bulletPool.Get();
        bullet.transform.position = gunPointer.position;
        bullet.transform.rotation = gunPointer.rotation;
        bullet.SetPool(_bulletPool);
        bullet.Launch(_shootDirection);

        yield return new WaitForSeconds(fireCooldown);

        returnCoroutine = StartCoroutine(ReturnToDefaultRotation());

        canShoot = true;
    }

    private IEnumerator AimAtTarget(Transform target)
    {
        _shootDirection = (target.position - gun.position).normalized;

        float angle = Mathf.Atan2(_shootDirection.y, _shootDirection.x) * Mathf.Rad2Deg;
        if (playerBody.localScale.x < 0) angle -= 180;

        Quaternion targetRot = Quaternion.Euler(0, 0, angle);

        float t = 0f;

        while (t < aimTime)
        {
            gun.rotation = Quaternion.Lerp(gun.rotation, targetRot, t / aimTime);
            t += Time.deltaTime;
            yield return null;
        }

        gun.rotation = targetRot;
    }

    private void HandleAimImages()
    {
        Transform nearestTarget = FindNearestTarget();
        Vector3 direction = Vector3.zero;

        if (nearestTarget != null)
        {
            aimImage.gameObject.SetActive(true);
            walkImage.gameObject.SetActive(false);

            direction = (nearestTarget.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            aimImage.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            aimImage.gameObject.SetActive(false);
            walkImage.gameObject.SetActive(true);

            Vector3 moveDir = player.GetDirection();
            if (moveDir.sqrMagnitude > 0.1f)
                direction = moveDir.normalized;
            else
                direction = Vector3.right * (playerBody.localScale.x > 0 ? 1 : -1);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            walkImage.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private IEnumerator ReturnToDefaultRotation()
    {
        Quaternion startRotation = gun.rotation;
        Quaternion defaultRotation = Quaternion.identity;

        float t = 0f;

        while (t < returnAimTime)
        {
            gun.rotation = Quaternion.Lerp(startRotation, defaultRotation, t / returnAimTime);
            t += Time.deltaTime;
            yield return null;
        }

        gun.rotation = defaultRotation;
    }


    private Transform FindNearestTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, targetMask);

        if (hits.Length == 0) return null;

        Collider2D nearest = hits
            .OrderBy(h => Vector2.Distance(transform.position, h.transform.position))
            .FirstOrDefault();

        return nearest != null && nearest.TryGetComponent<IDamageable>(out _) ? nearest.transform : null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
#endif
}