using System;
using UnityEngine;

namespace OmicronParticles
{
    public abstract class ParticleEffect<T> : MonoBehaviour where T : Enum
    {
        [SerializeField]
        private T _type;
        [SerializeField]
        private ParticleSystem[] _particleSystems;

        public T Type => _type;

        public void Emit(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;

            for (int i = 0; i < _particleSystems.Length; i++)
            {
                var particleSystem = _particleSystems[i];
                int amount = Mathf.RoundToInt(particleSystem.emission.GetBurst(0).count.constant);
                if (amount > 0)
                    particleSystem.Emit(amount);
            }
        }
    }
}