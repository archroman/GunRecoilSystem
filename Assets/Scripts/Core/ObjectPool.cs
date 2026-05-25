using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    internal sealed class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _initialSize = 30;

        private readonly Queue<GameObject> _poolQueue = new Queue<GameObject>();

        private void Start()
        {
            if (_prefab == null) return;

            for (int i = 0; i < _initialSize; i++)
            {
                GameObject obj = Instantiate(_prefab, transform);
                obj.SetActive(false);
                _poolQueue.Enqueue(obj);
            }
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            GameObject obj;

            if (_poolQueue.Count > 0)
            {
                obj = _poolQueue.Dequeue();
            }
            else
            {
                obj = Instantiate(_prefab, transform);
            }

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
            return obj;
        }

        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);

            if (!_poolQueue.Contains(obj))
            {
                _poolQueue.Enqueue(obj);
            }
        }
    }
}