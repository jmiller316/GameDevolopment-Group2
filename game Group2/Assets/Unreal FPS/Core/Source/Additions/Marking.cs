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
	public enum MarkItem { Grab, Ladder, Water, EnterWater }

	public class Marking : MonoBehaviour
	{
		[SerializeField] private MarkItem markItem;

		public MarkItem GetMark ()
		{
			return markItem;
		}

		public bool CompareMark (MarkItem mark)
		{
			return (this.markItem == mark);
		}
	}
}