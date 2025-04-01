using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllersManager : MonoBehaviour
{

    private List<Gamepad> connectedGamepads = new List<Gamepad>();
    private int _assignedJoystick;

    private void Awake()
    {
    }

    private void Update()
    {
        UpdateGamepadList();
    }

    public void UpdateGamepadList()
    {
        connectedGamepads.Clear();
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad != null && !connectedGamepads.Contains(gamepad))
                connectedGamepads.Add(gamepad);
        }
    }

    public Gamepad GetGamepad(int index)
    {
        if (connectedGamepads.Count == 0)
            return null; // Sem joystick, pode usar teclado como fallback

        _assignedJoystick = index;
        return connectedGamepads[_assignedJoystick];
    }

    public bool ChangeCamera(){
         if (connectedGamepads.Count == 0)
            return Input.GetKeyDown(KeyCode.C);
        return connectedGamepads[_assignedJoystick].rightShoulder.isPressed;
    }
    public bool ShootCharge(){
        // return connectedGamepads[_assignedJoystick].leftTrigger.value > 0;
        return connectedGamepads[_assignedJoystick].buttonWest.IsActuated();
    }
    public bool ShootAction(){
        return connectedGamepads[_assignedJoystick].buttonWest.isPressed;
    }
    public float HorizontalMovement(){
        if (connectedGamepads.Count == 0)
            return Input.GetAxis("Horizontal");
        return connectedGamepads[_assignedJoystick].leftStick.value.x;
    }
    public float VerticalMovement(){
        if (connectedGamepads.Count == 0)
            return Input.GetAxis("Vertical");
        return connectedGamepads[_assignedJoystick].leftStick.value.y;
    }
    public float HorizontalCameraMovement(){
        return connectedGamepads[_assignedJoystick].rightStick.value.x;
    }
    public float VericalCameraMovement(){
        return connectedGamepads[_assignedJoystick].rightStick.value.y;
    }
    // public Gamepad GetGamepad(){
    //     if (connectedGamepads.Count == 0) return null;
    //     return connectedGamepads[_assignedJoystick];
    // }
    public void VibrateController(){
        if (connectedGamepads.Count == 0) return;
        StartCoroutine(SetVibration(connectedGamepads[_assignedJoystick], 1f));
    }
    private IEnumerator SetVibration(Gamepad controller, float time){
        controller.SetMotorSpeeds(0.123f, 0.245f);
        yield return new WaitForSeconds(time);
        StopVibration();

    }
    public void StopVibration(){
        // StopCoroutine("SetVibration");
        if (connectedGamepads.Count == 0) return;
        connectedGamepads[_assignedJoystick].SetMotorSpeeds(0f, 0f);
    }
}
