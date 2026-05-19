using UnityEngine;

namespace Core
{
    internal sealed class PooledImpact : MonoBehaviour
    {
        [SerializeField] private float _lifetime = 1.5f;
        
        private ObjectPool _originPool;
        private float _lifeTimer;

        public void Initialize(ObjectPool pool)
        {
            _originPool = pool;
            _lifeTimer = _lifetime;
        }

        private void Update()
        {
            _lifeTimer -= Time.deltaTime;
            
            if (_lifeTimer <= 0f)
            {
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
}