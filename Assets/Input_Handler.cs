using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Input_Handler : MonoBehaviour
{
    public float horizontalInput { get; private set; }
    public bool dashInput {  get; private set; }
    public bool refresherLauncher { get; private set; }
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        dashInput = Input.GetMouseButtonDown(0);
        refresherLauncher = Input.GetMouseButtonUp(1);
    }
}
