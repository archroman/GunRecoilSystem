using Core;
using UnityEngine;

namespace Bullet
{
    [RequireComponent(typeof(TrailRenderer))]
    internal sealed class BulletTracer : MonoBehaviour
    {
        [SerializeField] private float _speed = 250f;

        private ObjectPool _originPool;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        
        private float _flyDuration;
        private float _timeTimeSpent;
        private bool _isLaunched;
        private TrailRenderer _trail;

        private void Awake()
        {
            _trail = GetComponent<TrailRenderer>();
        }

        public void Launch(ObjectPool pool, Vector3 target)
        {
            _originPool = pool;
            _startPosition = transform.position;
            _targetPosition = target;
            
            float distance = Vector3.Distance(_startPosition, _targetPosition);
            _flyDuration = distance / _speed;
            _timeTimeSpent = 0f;
            
            _isLaunched = true;

            if (_trail != null)
            {
                _trail.Clear();   
                _trail.enabled = true;
            }
        }

        private void Update()
        {
            if (!_isLaunched) return;

            _timeTimeSpent += Time.deltaTime;
            
            float progress = _timeTimeSpent / _flyDuration;

            transform.position = Vector3.Lerp(_startPosition, _targetPosition, progress);

            if (progress >= 1f)
            {
                transform.position = _targetPosition;
                ForceReturn();
            }
        }

        private void ForceReturn()
        {
            _isLaunched = false;
            
            if (_trail != null)
            {
                _trail.enabled = false; 
            }

            if (_originPool != null)
            {
                _originPool.ReturnToPool(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}