using Data;
using UnityEngine;

namespace Weapon
{
    [RequireComponent(typeof(WeaponMagazine))]
    public sealed class WeaponData : MonoBehaviour
    {
        [SerializeField] private RecoilData _recoilData;
        [SerializeField] private float _fireRate = 0.1f;
        [SerializeField] private Transform _muzzlePoint; 

        [Header("Ammo Config")]
        [SerializeField] private int _maxAmmoInMagazine = 30; 
        [SerializeField] private int _totalReserveAmmo = 90;   
        [SerializeField] private float _reloadTime = 2.0f;     

        public RecoilData RecoilData => _recoilData;
        public float FireRate => _fireRate;
        public Transform MuzzlePoint => _muzzlePoint;
        
        public int MaxAmmoInMagazine => _maxAmmoInMagazine;
        public int TotalReserveAmmo => _totalReserveAmmo;
        public float ReloadTime => _reloadTime;

        public WeaponMagazine Magazine { get; private set; }

        private void Awake()
        {
            Magazine = GetComponent<WeaponMagazine>();
            if (Magazine != null)
            {
                Magazine.Initialize(this);
            }
        }

        public void Activate() => gameObject.SetActive(true);
        public void Deactivate() => gameObject.SetActive(false);
    }
}