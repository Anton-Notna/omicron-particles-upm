using System;
using System.Collections.Generic;
using UnityEngine;

namespace OmicronParticles
{
    public class ParticleEffects<T> where T : Enum
    {
        private static readonly string _path = $"ParticleEffects/{typeof(T).Name}";

        private static Transform _root;
        private static Dictionary<T, ParticleEffect<T>> _prefabs;
        private static readonly Dictionary<T, ParticleEffect<T>> _spawnedEffects = new Dictionary<T, ParticleEffect<T>>();

        private static Transform Root
        {
            get
            {
                if (_root == null)
                {
                    _root = new GameObject($"{typeof(T).Name}Root").transform;
                    GameObject.DontDestroyOnLoad(_root.gameObject);
                }

                return _root;
            }
        }

        private static Dictionary<T, ParticleEffect<T>> Prefabs
        {
            get
            {
                if (_prefabs == null)
                {
                    _prefabs = new Dictionary<T, ParticleEffect<T>>();
                    ParticleEffect<T>[] prefabs = Resources.LoadAll<ParticleEffect<T>>(_path);
                    for (int i = 0; i < prefabs.Length; i++)
                    {
                        if (_prefabs.ContainsKey(prefabs[i].Type))
                        {
                            Debug.LogError($"There is more than one {prefabs[i].Type} effect. Prefabs: {_prefabs[prefabs[i].Type].gameObject.name}, {prefabs[i].gameObject.name}");
                            continue;
                        }

                        _prefabs.Add(prefabs[i].Type, prefabs[i]);
                    }
                }

                return _prefabs;
            }
        }

        public static void Emit(T type, Vector3 position, Quaternion rotation) => Get(type)?.Emit(position, rotation);

        private static ParticleEffect<T> Get(T type)
        {
            if (_spawnedEffects.TryGetValue(type, out ParticleEffect<T> effect) == false)
            {
                effect = Spawn(type);
                _spawnedEffects.Add(type, effect);
            }

            return effect;
        }

        private static ParticleEffect<T> Spawn(T type)
        {
            if (Prefabs.TryGetValue(type, out ParticleEffect<T> prefab) == false)
            {
                Debug.LogError(new NullReferenceException($"There is no {type} effect in Resources/{_path}"));
                return null;
            }

            return GameObject.Instantiate(prefab, Root);
        }
    }
}