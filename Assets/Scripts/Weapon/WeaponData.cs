using Data;
using UnityEngine;

namespace Weapon
{
    internal sealed class WeaponData : MonoBehaviour
    {
        [SerializeField] private RecoilData _recoilData;
        [SerializeField] private float _fireRate = 0.1f;
        
        [SerializeField] private Transform _muzzlePoint; 
    
        public RecoilData RecoilData => _recoilData;
        public float FireRate => _fireRate;
        
        public Transform MuzzlePoint => _muzzlePoint;

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}