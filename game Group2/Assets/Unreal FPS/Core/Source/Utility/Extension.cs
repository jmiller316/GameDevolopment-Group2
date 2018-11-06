/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System.Text;
using UnityEngine;

namespace UnrealFPS.Utility
{
    public static class Extension
    {
        /// <summary>
        /// Generate random position in the circle with specific radius
        /// </summary>
        /// <param name="radius">Circle radius</param>
        /// <returns>Return Vector3</returns>
        public static Vector3 RandomPositionInCircle(this Vector3 center, float radius)
        {
            Vector2 randomPos = Random.insideUnitCircle * radius;
            return new Vector3(center.x + randomPos.x, center.y, center.z + randomPos.y);
        }

        /// <summary>
        /// Generate random position in the rectangle
        /// </summary>
        /// <param name="lenght">Rectangle lenght</param>
        /// <param name="weight">Rectangle weight</param>
        /// <returns>Return Vector3</returns>
        public static Vector3 RandomPositionInRectangle(this Vector3 center, float lenght, float weight)
        {
            Vector3 position;
            position.x = Random.Range(center.x - weight / 2, center.x + weight / 2);
            position.y = center.y;
            position.z = Random.Range(center.z - lenght / 2, center.z + lenght / 2);
            return position;
        }

        /// <summary>
        /// Add spaces to this line
        /// 
        ///     Note: Spaces are added only between  capital letters.
        /// </summary>
        /// <returns>Return string</returns>
        public static string AddSpaces(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        public static float GetPersent(this float value, float maxValue)
        {
            return (100f / maxValue) * value;
        }
    }
}