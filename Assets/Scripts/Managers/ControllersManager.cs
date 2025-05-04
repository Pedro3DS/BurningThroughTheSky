using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllersManager : MonoBehaviour
{

    private List<Gamepad> connectedGamepads = new List<Gamepad>();
    private int _assignedJoystick;
    public static ControllersManager Instance = null;

    private void Awake()
    {
        if(!Instance){
            Instance = this;
        }else Destroy(Instance);
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
    public bool ShootAction(int joystickIndex){
        if(joystickIndex == 10) return Input.GetKeyDown(KeyCode.Space);
        return connectedGamepads[joystickIndex].buttonWest.isPressed;
    }
    public float HorizontalMovement(int joystickIndex){
        if (connectedGamepads.Count == 0 || joystickIndex == 10)
            return Input.GetAxis("Horizontal");
        return connectedGamepads[joystickIndex].leftStick.value.x;
    }
    public float VerticalMovement(int joystickIndex){
        if (connectedGamepads.Count == 0 || joystickIndex == 10)
            return Input.GetAxis("Vertical");
        return connectedGamepads[joystickIndex].leftStick.value.y;
    }
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
