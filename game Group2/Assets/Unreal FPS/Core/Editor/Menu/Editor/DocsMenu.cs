/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEngine;

namespace UnrealFPS.Editor
{
    public static class DocsMenu
    {
        public static string LocalURL = System.Uri.EscapeUriString("file:///" + Application.dataPath + "/Unreal FPS/Documentation/Unreal FPS Manual.pdf");
        public static string InternetURL = "https://docs.google.com/document/d/1H6NnAOCbOH2GLAAi3XyTcFFI0szRGHgk8ysuvuly9Ag/edit?usp=sharing";

        [MenuItem("Unreal FPS/Documentation", false, 1)]
        private static void OpenAPI()
        {
            string URL = (Application.internetReachability != NetworkReachability.NotReachable) ? InternetURL : LocalURL;
            Application.OpenURL(URL);
        }
    }
}