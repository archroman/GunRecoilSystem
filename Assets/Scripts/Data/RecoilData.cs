using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "NewRecoilData", menuName = "Weapon/Recoil Data")]
    public sealed class RecoilData : ScriptableObject
    {
        [SerializeField] private float _verticalKick = -3f;
        [SerializeField] private float _horizontalSpread = 1.5f;
        [SerializeField] private float _snappiness = 10f;
        [SerializeField] private float _returnSpeed = 5f;

        public float VerticalKick => _verticalKick;
        public float HorizontalSpread => _horizontalSpread;
        public float Snappiness => _snappiness;
        public float ReturnSpeed => _returnSpeed;
    }
}