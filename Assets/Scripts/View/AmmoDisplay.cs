using TMPro;
using UnityEngine;
using Weapon;

namespace View
{
    internal sealed class AmmoDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _ammoText;

        [SerializeField] private WeaponInventory _inventory; 

        private void Update()
        {
            if (_inventory == null || _ammoText == null) return;

            WeaponData activeWeapon = _inventory.CurrentWeapon;

            if (activeWeapon == null)
            {
                _ammoText.text = "- / -";
                return;
            }

            WeaponMagazine magazine = activeWeapon.Magazine;

            if (magazine != null)
            {
                _ammoText.text = $"{magazine.CurrentAmmo} / {magazine.CurrentReserve}";
            }
        }
    }
}