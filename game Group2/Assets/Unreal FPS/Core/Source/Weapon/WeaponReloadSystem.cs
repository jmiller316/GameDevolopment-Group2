/* =====================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System.Collections;
using UnityEngine;

namespace UnrealFPS
{
	/// <summary>
	/// 
	/// </summary>
	public enum ReloadType { Default, Sequential }

	/// <summary>
	/// 
	/// </summary>
	[RequireComponent(typeof(WeaponAttackSystem))]
	[RequireComponent(typeof(AudioSource))]
	public class WeaponReloadSystem : MonoBehaviour
	{
		[SerializeField] private ReloadType reloadType;
		[SerializeField] private int bulletCount;
		[SerializeField] private int clipCount;
		[SerializeField] private int maxBulletCount;
		[SerializeField] private int maxClipCount;

		//Default
		[SerializeField] private float reloadTime;
		[SerializeField] private float emptyReloadTime;

		//Sequential
		[SerializeField] private float startTime;
		[SerializeField] private float iterationTime;

		private bool isReloading;

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		private void Update()
		{
			if (SimpleInputManager.GetReload() && !ClipsIsEmpty)
			{
				isReloading = true;
				switch (reloadType)
				{
					case ReloadType.Default:
						DefaultReload();
						break;
					case ReloadType.Sequential:
						SequentialReload();
						break;
				}
			}
		}

		/// <summary>
		/// Calculate bullet and clip count
		/// </summary>
		public virtual void ReCalculateAmmo()
		{
			if (clipCount >= maxBulletCount)
			{
				clipCount -= (maxBulletCount - bulletCount);
				bulletCount = maxBulletCount;
			}
			else if (clipCount < maxBulletCount)
			{
				bulletCount = clipCount + bulletCount;
				clipCount = 0;
			}
		}

		/// <summary>
		/// Default reload handler
		/// </summary>
		protected virtual void DefaultReload()
		{
			if (!BulletsIsEmpty)
				StartCoroutine(Reload(reloadTime));
			else
				StartCoroutine(Reload(emptyReloadTime));
		}

		/// <summary>
		/// Sequential reload handler
		/// </summary>
		protected virtual void SequentialReload()
		{
			StartCoroutine(Reload(CalculateMaxTime(bulletCount, maxBulletCount, startTime, iterationTime)));
		}

		/// <summary>
		/// Calculate bullet and clip count after a specified time
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public IEnumerator Reload(float time)
		{
			yield return new WaitForSeconds(time);
			ReCalculateAmmo();
			isReloading = false;
			yield break;
		}

		/// <summary>
		/// Calculates the maximum time required for recharging
		/// </summary>
		/// <param name="bulletCount"></param>
		/// <param name="maxBulletCount"></param>
		/// <param name="startTime"></param>
		/// <param name="iterationTime"></param>
		/// <returns></returns>
		public virtual float CalculateMaxTime(float bulletCount, float maxBulletCount, float startTime, float iterationTime)
		{
			float totalTime;
			float requiredBullet = maxBulletCount - bulletCount;
			totalTime = iterationTime * requiredBullet;
			totalTime += startTime;
			return totalTime;
		}

		public bool IsReloading
		{
			get
			{
				return isReloading;
			}

			protected set
			{
				isReloading = value;
			}
		}

		/// <summary>
		/// Bullets
		/// </summary>
		/// <returns>Bullet Count</returns>
		public int BulletCount
		{
			get
			{
				return bulletCount;
			}
			set
			{
				if (value <= maxBulletCount)
					bulletCount = value;
				else
					bulletCount = maxBulletCount;
			}
		}

		/// <summary>
		/// Clips
		/// </summary>
		/// <returns>Clip Count</returns>
		public int ClipCount
		{
			get
			{
				return clipCount;
			}
			set
			{
				if (value <= maxClipCount)
					clipCount = value;
				else
					clipCount = maxClipCount;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int MaxBulletCount
		{
			get
			{
				return maxBulletCount;
			}

			set
			{
				maxBulletCount = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int MaxClipCount
		{
			get
			{
				return maxClipCount;
			}

			set
			{
				maxClipCount = value;
			}
		}

		public bool ClipsIsEmpty
		{
			get
			{
				return (clipCount <= 0);
			}
		}

		public bool BulletsIsEmpty
		{
			get
			{
				return (bulletCount <= 0);
			}
		}

		public ReloadType W_ReloadType { get { return reloadType; } set { reloadType = value; } }
	}
}