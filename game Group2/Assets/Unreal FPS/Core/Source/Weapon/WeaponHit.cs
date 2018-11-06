/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System.Collections.Generic;
using UnityEngine;

namespace UnrealFPS
{
    [System.Serializable]
    public struct WeaponHitParams
    {
        public string SurfaceName;
        public GameObject[] HitDecal;
        public AudioClip[] HitSound;
    }

    public static class WeaponHit
    {
        /// <summary>
        /// Create hit effect by Ray
        /// </summary>
        /// <param name="rayCastHit"></param>
        /// <param name="audioSource"></param>
        public static void OnRay(List<WeaponHitParams> hitEffects, RaycastHit rayCastHit, AudioSource audioSource)
        {
            if (rayCastHit.collider.sharedMaterial != null)
            {
                string materialName = rayCastHit.collider.sharedMaterial.name;
                for (int i = 0; i < hitEffects.Count; i++)
                {
                    if (materialName == hitEffects[i].SurfaceName)
                    {
                        if (hitEffects[i].HitDecal.Length > 0)
                        {
                            int randomDecal = Random.Range(0, hitEffects[i].HitDecal.Length);
                            GameObject spawnedDecal = Object.Instantiate(hitEffects[i].HitDecal[randomDecal], rayCastHit.point, Quaternion.LookRotation(rayCastHit.normal));
                            spawnedDecal.transform.SetParent(rayCastHit.collider.transform);
                        }

                        if (hitEffects[i].HitSound.Length > 0)
                        {
                            int randomSound = Random.Range(0, hitEffects[i].HitSound.Length);
                            audioSource.PlayOneShot(hitEffects[i].HitSound[randomSound]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create hit effect by collision
        /// </summary>
        /// <param name="collision"></param>
        /// <param name="contactPoint"></param>
        /// <param name="audioSource"></param>
        public static void OnCollision(List<WeaponHitParams> hitEffects, Collision collision, ContactPoint contactPoint, AudioSource audioSource)
        {
            if (collision.collider.sharedMaterial != null)
            {
                string materialName = collision.collider.sharedMaterial.name;
                for (int i = 0; i < hitEffects.Count; i++)
                {
                    if (materialName == hitEffects[i].SurfaceName)
                    {
                        if (hitEffects[i].HitDecal.Length > 0)
                        {
                            int randomDecal = Random.Range(0, hitEffects[i].HitDecal.Length);
                            GameObject spawnedDecal = Object.Instantiate(hitEffects[i].HitDecal[randomDecal], contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
                            spawnedDecal.transform.SetParent(collision.collider.transform);
                        }

                        if (hitEffects[i].HitSound.Length > 0)
                        {
                            int randomSound = Random.Range(0, hitEffects[i].HitSound.Length);
                            audioSource.PlayOneShot(hitEffects[i].HitSound[randomSound]);
                        }
                    }
                }
            }
        }
    }
}