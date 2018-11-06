/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnrealFPS.Editor
{
    public enum MoveType { Footsteps, Climb }

    public class MovementSoundManager : EditorWindow
    {
        private static Vector2 WindowSize = new Vector2(500, 250);
        private List<FootstepSurface> surfaces;
        private MoveType moveType;
        private int selGridInt = 0;
        private string[] selStrings;
        private Vector2 scrollPos1;
        private Vector2 scrollPos2;

        //Initialize window and open it
        public static void Open()
        {
            MovementSoundManager window = (MovementSoundManager) GetWindow(typeof(MovementSoundManager), true, "Movement Sound Manager");
            window.position = new Rect(
                (Screen.currentResolution.width / 2) - (WindowSize.x / 2),
                (Screen.currentResolution.height / 2) - (WindowSize.y / 2),
                WindowSize.x,
                WindowSize.y);
            window.Show();
        }

        private void OnEnable()
        {
            if (surfaces == null)
                surfaces = new List<FootstepSurface>();

            if (GameObject.FindGameObjectWithTag("Player") != null)
                surfaces = GameObject.FindGameObjectWithTag("Player").GetComponent<FPController>().SurfaceList;
            GenerateGrid();
        }

        #region GUI
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            FootstepSurfaceListGroup();
            FootstepSurfaceEditLayout();
            GUILayout.EndHorizontal();
            if (GUI.changed)
            {
                if (surfaces == null)
                    surfaces = new List<FootstepSurface>();

                if (GameObject.FindGameObjectWithTag("Player") != null)
                    surfaces = GameObject.FindGameObjectWithTag("Player").GetComponent<FPController>().SurfaceList;

                UpdateMoveType();
            }

        }

        private void FootstepSurfaceListGroup()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(150));
            scrollPos1 = GUILayout.BeginScrollView(scrollPos1);
            moveType = (MoveType) EditorGUILayout.EnumPopup(moveType);
            GUILayout.Space(3);
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
            if (selStrings != null)
                selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 1);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                surfaces.Add(new FootstepSurface() { SurfaceName = "New FootstepSurface", FootStepsSound = new AudioClip[1] { null } });
            }
            if (GUILayout.Button("-", GUILayout.Width(19)))
            {
                surfaces.RemoveAt(selGridInt);
                if (selGridInt > 0) { selGridInt -= 1; }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void FootstepSurfaceEditLayout()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            scrollPos2 = GUILayout.BeginScrollView(scrollPos2);

            if (surfaces.Count > 0)
            {
                FootstepSurface surface = surfaces[selGridInt];
                List<AudioClip> audioClips = new List<AudioClip>();
                if (surface.FootStepsSound.Length > 0)
                {
                    for (int i = 0; i < surface.FootStepsSound.Length; i++)
                    {
                        audioClips.Add(surface.FootStepsSound[i]);
                    }
                }
                GUILayout.Label(surface.SurfaceName, EditorStyles.boldLabel);

                GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });

                surface.SurfaceName = EditorGUILayout.TextField("FootstepSurface Name", surface.SurfaceName);
                for (int i = 0; i < audioClips.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Sound");
                    audioClips[i] = (AudioClip) EditorGUILayout.ObjectField(audioClips[i], typeof(AudioClip), true);
                    if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(15)))
                    {
                        audioClips.RemoveAt(i);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Sound"))
                {
                    audioClips.Add(null);
                }
                GUILayout.EndHorizontal();

                surface.FootStepsSound = audioClips.ToArray();

                surfaces[selGridInt] = surface;
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void GenerateGrid()
        {
            selStrings = new string[surfaces.Count];
            for (int i = 0; i < surfaces.Count; i++)
            {
                selStrings[i] = surfaces[i].SurfaceName;
            }
        }

        private void UpdateMoveType()
        {
            if (moveType == MoveType.Footsteps && GameObject.FindGameObjectWithTag("Player").GetComponent<FPController>() != null)
            {
                surfaces = GameObject.FindGameObjectWithTag("Player").GetComponent<FPController>().SurfaceList;
            }

            if (moveType == MoveType.Climb && GameObject.FindGameObjectWithTag("Player").GetComponent<FPController>() != null)
            {
                surfaces = GameObject.FindGameObjectWithTag("Player").GetComponent<FPController>().FPClimb.SurfaceList;
            }
            GenerateGrid();
        }
        #endregion

    }
}