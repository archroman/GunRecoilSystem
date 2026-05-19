using Core;
using UnityEngine;

namespace Weapon
{
    internal sealed class WeaponInventory : MonoBehaviour
    {
        [SerializeField] private RecoilController _recoilController;

        private WeaponData[] _weapons; 
        private int _currentWeaponIndex = 0;

        public WeaponData CurrentWeapon => _weapons.Length > 0 ? _weapons[_currentWeaponIndex] : null;

        private void Start()
        {
            _weapons = GetComponentsInChildren<WeaponData>(true);

            if (_weapons.Length > 0)
            {
                InitializeWeapons();
                EquipWeapon(_currentWeaponIndex);
            }
        }

        private void Update()
        {
            if (_weapons.Length == 0) return;

            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
            if (scroll > 0f) ChangeWeapon(1);
            else if (scroll < 0f) ChangeWeapon(-1);

            HandleNumericInput();
        }

        private void ChangeWeapon(int direction)
        {
            _weapons[_currentWeaponIndex].Deactivate();

            _currentWeaponIndex += direction;
            if (_currentWeaponIndex >= _weapons.Length) _currentWeaponIndex = 0;
            if (_currentWeaponIndex < 0) _currentWeaponIndex = _weapons.Length - 1;

            EquipWeapon(_currentWeaponIndex);
        }

        private void HandleNumericInput()
        {
            for (int i = 0; i < _weapons.Length; i++)
            {
                if (i >= 9) break; 

                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    if (i == _currentWeaponIndex) return;

                    _weapons[_currentWeaponIndex].Deactivate();
                    _currentWeaponIndex = i;
                    EquipWeapon(_currentWeaponIndex);
                    break;
                }
            }
        }

        private void EquipWeapon(int index)
        {
            WeaponData targetWeapon = _weapons[index];
            targetWeapon.Activate();

            if (_recoilController != null)
            {
                _recoilController.SetRecoilData(targetWeapon.RecoilData);
            }
        }

        private void InitializeWeapons()
        {
            foreach (WeaponData weapon in _weapons)
            {
                weapon.Deactivate();
            }
        }
    }
}