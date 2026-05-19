using UnityEngine;

namespace Player
{
    internal sealed class FPSLook : MonoBehaviour
    {
        [SerializeField] private float _mouseSensitivity = 2f;

        [SerializeField] private Transform _playerBody;

        [SerializeField] private float _snappiness = 10f;
        [SerializeField] private float _returnSpeed = 5f;

        private float _xRotation = 0f;
    
        private Vector3 _recoilRotation;
        private Vector3 _currentRecoilRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

            _recoilRotation = Vector3.Lerp(_recoilRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
            _currentRecoilRotation = Vector3.Lerp(_currentRecoilRotation, _recoilRotation, _snappiness * Time.deltaTime);

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); 

            transform.localRotation = Quaternion.Euler(_xRotation + _currentRecoilRotation.x, _currentRecoilRotation.y, 0f);

            if (_playerBody != null)
                _playerBody.Rotate(Vector3.up * mouseX);
        }

        public void AddRecoil(float verticalKick, float horizontalSpread)
        {
            float randomHorizontal = Random.Range(-horizontalSpread, horizontalSpread);
            _recoilRotation += new Vector3(-verticalKick, randomHorizontal, 0f);
        }
    }
}