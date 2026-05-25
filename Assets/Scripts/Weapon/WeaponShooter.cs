using Bullet;
using UnityEngine;
using Core;
using Player;
using System.Collections;

namespace Weapon
{
    internal sealed class WeaponShooter : MonoBehaviour
    {
        [SerializeField] private WeaponInventory _inventory;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private RecoilController _recoilController; 

        [SerializeField] private ObjectPool _bulletPool; 
        [SerializeField] private ObjectPool _impactPool;  

        [SerializeField] private float _maxShotDistance = 100f;

        private float _nextFireTime;
        private bool _isReloading = false; 

        private void Start()
        {
            if (_playerCamera == null) _playerCamera = Camera.main;
        }

        private void Update()
        {
            if (_inventory == null) return;

            WeaponData activeWeapon = _inventory.CurrentWeapon;
            if (activeWeapon == null) return;

            WeaponMagazine magazine = activeWeapon.Magazine;
            if (magazine == null) return;

            if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
            {
                if (!magazine.IsFull() && magazine.HasReserve())
                {
                    StartCoroutine(ReloadRoutine(activeWeapon, magazine));
                }
            }

            if (Input.GetKey(KeyCode.Mouse0) && Time.time >= _nextFireTime && !_isReloading)
            {
                if (magazine.HasAmmo())
                {
                    _nextFireTime = Time.time + activeWeapon.FireRate;
                    Shoot(activeWeapon, magazine);
                }
                else if (magazine.HasReserve())
                {
                    StartCoroutine(ReloadRoutine(activeWeapon, magazine));
                }
            }
        }

        private void Shoot(WeaponData activeWeapon, WeaponMagazine magazine)
        {
            magazine.SpendAmmo();
            Debug.Log($"Патроны: {magazine.CurrentAmmo} / {magazine.CurrentReserve}");

            FPSLook cameraLook = GetComponentInParent<FPSLook>();
            if (cameraLook != null)
            {
                cameraLook.AddRecoil(Mathf.Abs(activeWeapon.RecoilData.VerticalKick), activeWeapon.RecoilData.HorizontalSpread);
            }

            if (_recoilController != null)
            {
                _recoilController.ApplyRecoil();
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

        private IEnumerator ReloadRoutine(WeaponData activeWeapon, WeaponMagazine magazine)
        {
            _isReloading = true;
            Debug.Log("Перезарядка...");

            yield return new WaitForSeconds(activeWeapon.ReloadTime);

            int ammoNeeded = magazine.GetNeededAmmo();
            int ammoToLoad = Mathf.Min(ammoNeeded, magazine.CurrentReserve);

            magazine.LoadAmmo(ammoToLoad);

            _isReloading = false;
            Debug.Log("Перезарядка завершена!");
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