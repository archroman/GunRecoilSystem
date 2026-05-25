using UnityEngine;

namespace Weapon
{
    public sealed class WeaponMagazine : MonoBehaviour
    {
        private int _currentAmmo;
        private int _currentReserve;
        private WeaponData _config;

        public int CurrentAmmo => _currentAmmo;
        public int CurrentReserve => _currentReserve;

        public void Initialize(WeaponData config)
        {
            _config = config;
            _currentAmmo = config.MaxAmmoInMagazine;
            _currentReserve = config.TotalReserveAmmo;
        }

        public bool HasAmmo() => _currentAmmo > 0;
        public bool HasReserve() => _currentReserve > 0;
        public bool IsFull() => _config != null && _currentAmmo >= _config.MaxAmmoInMagazine;

        public void SpendAmmo()
        {
            if (_currentAmmo > 0) _currentAmmo--;
        }

        public void LoadAmmo(int amount)
        {
            _currentAmmo += amount;
            _currentReserve -= amount;
        }

        public int GetNeededAmmo()
        {
            if (_config == null) return 0;
            return _config.MaxAmmoInMagazine - _currentAmmo;
        }
    }
}