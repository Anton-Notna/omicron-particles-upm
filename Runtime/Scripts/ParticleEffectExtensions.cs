using System;
using UnityEngine;

namespace OmicronParticles
{
    public static class ParticleEffectExtensions
    {
        public static void Emit<T>(this T type, Transform origin) where T : Enum =>
            Emit(type, origin.position, origin.rotation);

        public static void Emit<T>(this T type, Vector3 position) where T : Enum =>
            Emit(type, position, Quaternion.identity);

        public static void Emit<T>(this T type, Ray ray) where T : Enum =>
            Emit(type, ray.origin, ToRotation(ray.direction));

        public static void Emit<T>(this T type, Vector3 position, Vector3 forward) where T : Enum =>
            Emit(type, position, ToRotation(forward));

        public static void Emit<T>(this T type, Vector3 position, Quaternion rotation) where T : Enum =>
            ParticleEffects<T>.Emit(type, position, rotation);

        private static Quaternion ToRotation(Vector3 forward)
        {
            if (forward == Vector3.zero)
                return Quaternion.identity;

            forward = forward.normalized;

            Vector3 up = forward == Vector3.up ? Vector3.forward : Vector3.up;
            return Quaternion.LookRotation(forward, up);
        }
    }
}