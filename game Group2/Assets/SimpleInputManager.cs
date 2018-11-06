using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SimpleInputManager {
    public static KeyCode moveLeft = KeyCode.A;
    public static KeyCode moveRight = KeyCode.D;
    public static KeyCode moveForward = KeyCode.W;
    public static KeyCode moveBackward = KeyCode.S;
    [SerializeField] public static List<KeyCode> weaponList = new List<KeyCode>
        {KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5};
    [SerializeField] public static KeyCode run = KeyCode.LeftShift;
    [SerializeField] public static KeyCode crouch = KeyCode.LeftControl;
    [SerializeField] public static KeyCode interact = KeyCode.F;
    [SerializeField] public static KeyCode reload = KeyCode.R;
    [SerializeField] public static KeyCode interact2 = KeyCode.G;
    [SerializeField] public static KeyCode escape = KeyCode.Escape;
    [SerializeField] public static KeyCode jump = KeyCode.Space;
    public static KeyCode fire = KeyCode.Mouse0;
    public static KeyCode aim = KeyCode.Mouse1;
    public static KeyCode menu = KeyCode.Escape;

    public static float GetHorizontal() {
        if (Input.GetKey(moveLeft)) {
            return -1f;
        }
        else if (Input.GetKey(moveRight)) {
            return 1f;
        }
        return 0f;
    }

    public static float GetVertical() {
        if (Input.GetKey(moveBackward)) {
            return -1f;
        }
        else if (Input.GetKey(moveForward)) {
            return 1f;    
        }
        return 0f;
    }

    public static bool DropWeapon() {
        if (Input.GetKeyDown(interact2)) {
            return true;
        }
        return false;
    }

    public static bool IsRunning() {
        if (Input.GetKey(run)) {
            return true;
        }
        return false;
    }

    public static bool IsJumping() {
        return Input.GetKeyDown(jump);
    }

    public static bool GetFire() {
        return Input.GetKey(fire);
    }

    public static bool GetFireLong() {
        return Input.GetKeyDown(fire);
    }

    public static bool GetAim() {
        return Input.GetKey(aim);
    }

    public static bool GetCrouch() {
        return Input.GetKey(crouch);
    }

    public static float GetMouseX() {
        return Input.GetAxis("Mouse X");
    }

    public static float GetMouseY() {
        return Input.GetAxis("Mouse Y");
    }

    public static bool GetReload() {
        return Input.GetKeyDown(reload);
    }

    public static bool GetInteract() {
        return Input.GetKeyDown(interact);
    }

    public static bool GetInteract2() {
        return Input.GetKeyDown(interact2);
    }

    public static bool GetMenu() {
        return Input.GetKeyDown(menu);
    }
}
