/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

using System;
using UnityEngine;

namespace UnrealFPS
{
    [Serializable]
    public class NGMouseLook
    {
        [Range(0.0f, 50.0f)] public static float XSensitivity = 2f;
        [Range(0.0f, 50.0f)] public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        [Range(-360.0f, 360.0f)] public float MinimumX = -90F;
        [Range(-360.0f, 360.0f)] public float MaximumX = 90F;
        public bool smooth;
        [Range(0.0f, 50.0f)] public float smoothTime = 5f;
        public bool lockCursor = true;

        [HideInInspector] public Quaternion m_CharacterTargetRot;
        [HideInInspector] public Quaternion m_CameraTargetRot;
        public bool m_cursorIsLocked = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="camera"></param>
        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="camera"></param>
        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;
            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);



            if (smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }
            UpdateCursorLock();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if (!lockCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor)
                InternalLockUpdate();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InternalLockUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_cursorIsLocked = true;
            }
            else if (Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}