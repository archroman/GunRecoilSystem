using Data;
using UnityEngine;

namespace Core
{
    internal sealed class RecoilController : MonoBehaviour
    {
        private Vector3 _targetRotation;
        private Vector3 _currentRotation;
        private RecoilData _currentRecoilData;

        public Vector3 CurrentRotation => _currentRotation;

        public void SetRecoilData(RecoilData recoilData)
        {
            _currentRecoilData = recoilData;
        }

        public void ApplyRecoil()
        {
            if (_currentRecoilData == null) return;

            float randomHorizontalSpread = Random.Range(-_currentRecoilData.HorizontalSpread, _currentRecoilData.HorizontalSpread);
            _targetRotation += new Vector3(_currentRecoilData.VerticalKick, randomHorizontalSpread, 0f);
        }

        private void Update()
        {
            if (_currentRecoilData == null) return;

            _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, _currentRecoilData.ReturnSpeed * Time.deltaTime);
            _currentRotation = Vector3.Lerp(_currentRotation, _targetRotation, _currentRecoilData.Snappiness * Time.deltaTime);

            transform.localRotation = Quaternion.Euler(_currentRotation);
        }
    }
}