using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnrealFPS
{
    [Serializable]
    public struct FootstepSurface
    {
        public string SurfaceName;
        public AudioClip[] FootStepsSound;
    }

    /// <summary>
    /// 
    /// </summary>
    public class FootstepsSoundSystem
    {
        /// <summary>
        /// Handle sound by ray
        /// </summary>
        /// <param name="player"></param>
        /// <param name="audioSource"></param>
        public static void Play(List<FootstepSurface> surfaceList, Transform player, AudioSource audioSource)
        {
            RaycastHit footstepshit;
            if (Physics.Raycast(player.position, Vector3.down, out footstepshit))
            {
                string materialName;

                if (footstepshit.collider.sharedMaterial != null)
                    materialName = footstepshit.collider.sharedMaterial.name;
                else
                    materialName = "Not Defined";
                
                
                for (int i = 0; i < surfaceList.Count; i++)
                {
                    if (materialName == surfaceList[i].SurfaceName)
                    {
                        if (surfaceList[i].FootStepsSound.Length > 0)
                        {
                            int randomSound = Random.Range(0, surfaceList[i].FootStepsSound.Length);
                            audioSource.PlayOneShot(surfaceList[i].FootStepsSound[randomSound]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handle sound by surface name
        /// </summary>
        /// <param name="surfaceName"></param>
        /// <param name="audioSource"></param>
        public static void Play(List<FootstepSurface> climbSurfaceList, string surfaceName, AudioSource audioSource)
        {
            if (surfaceName == null)
                surfaceName = "Not Defined";
                
            for (int i = 0; i < climbSurfaceList.Count; i++)
            {
                if (surfaceName == climbSurfaceList[i].SurfaceName)
                {
                    if (climbSurfaceList[i].FootStepsSound.Length > 0)
                    {
                        int randomSound = Random.Range(0, climbSurfaceList[i].FootStepsSound.Length);
                        audioSource.PlayOneShot(climbSurfaceList[i].FootStepsSound[randomSound]);
                    }
                }
            }
        }
    }
}
