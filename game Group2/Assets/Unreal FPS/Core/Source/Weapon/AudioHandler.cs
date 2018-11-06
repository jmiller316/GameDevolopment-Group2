/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEngine;

namespace UnrealFPS
{
	/// <summary>
	/// AudioHandler class used for playing specific sound in weapon animation
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class AudioHandler : MonoBehaviour
	{
		private AudioSource audioSource;

		/// <summary>
		/// Start is called on the frame when a script is enabled just before
		/// any of the Update methods is called the first time.
		/// </summary>
		protected virtual void Start()
		{
			audioSource = GetComponent<AudioSource>();
		}

		/// <summary>
		/// Play specific sound
		/// </summary>
		/// <param name="audioClip"></param>
		public virtual void PlaySound(AudioClip audioClip)
		{
			if (audioSource == null)
				return;

			audioSource.clip = audioClip;
			audioSource.Play();
		}
	}
}