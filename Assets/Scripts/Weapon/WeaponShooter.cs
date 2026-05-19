using Bullet;
using UnityEngine;
using Core;
using Player;

namespace Weapon
{
    internal sealed class WeaponShooter : MonoBehaviour
    {
        [SerializeField] private WeaponInventory _inventory;
        [SerializeField] private Camera _playerCamera;

        [SerializeField] private ObjectPool _bulletPool; 
        [SerializeField] private ObjectPool _impactPool;  

        [SerializeField] private float _maxShotDistance = 100f;

        private float _nextFireTime;

        private void Start()
        {
            if (_playerCamera == null) _playerCamera = Camera.main;
        }

        private void Update()
        {
            if (_inventory == null) return;

            WeaponData activeWeapon = _inventory.CurrentWeapon;
            if (activeWeapon == null) return;

            if (Input.GetKey(KeyCode.Mouse0) && Time.time >= _nextFireTime)
            {
                _nextFireTime = Time.time + activeWeapon.FireRate;
                Shoot();
            }
        }

        private void Shoot()
        {
            WeaponData activeWeapon = _inventory.CurrentWeapon;
            if (activeWeapon == null || _playerCamera == null) return;

            FPSLook cameraLook = GetComponentInParent<FPSLook>();
            if (cameraLook != null)
            {
                cameraLook.AddRecoil(Mathf.Abs(activeWeapon.RecoilData.VerticalKick), activeWeapon.RecoilData.HorizontalSpread);
            }

            Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out RaycastHit hit, _maxShotDistance))
            {
                targetPoint = hit.point;
                SpawnImpact(hit);
            }
            else
            {
                targetPoint = ray.origin + ray.direction * _maxShotDistance;
            }

            SpawnBullet(activeWeapon.MuzzlePoint.position, targetPoint);
        }

        private void SpawnBullet(Vector3 start, Vector3 end)
        {
            if (_bulletPool == null) return;

            GameObject bulletObj = _bulletPool.Get(start, Quaternion.identity);

            if (bulletObj.TryGetComponent(out BulletTracer tracer))
            {
                tracer.Launch(_bulletPool, end);
            }
        }

        private void SpawnImpact(RaycastHit hit)
        {
            if (_impactPool == null) return;

            Quaternion spawnRotation = Quaternion.LookRotation(hit.normal);
            GameObject impactObj = _impactPool.Get(hit.point, spawnRotation);

            if (impactObj.TryGetComponent(out PooledImpact impactTimer))
            {
                impactTimer.Initialize(_impactPool);
            }
        }
    }
}