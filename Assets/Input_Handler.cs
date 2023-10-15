using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Input_Handler : MonoBehaviour
{
    public float MoveInput { get; private set; }
    public bool LeftClick {  get; private set; }
    public bool RightClick { get; private set; }
    private void Update()
    {
        MoveInput = Input.GetAxis("Horizontal");
        LeftClick = Input.GetMouseButtonDown(0);
        RightClick = Input.GetMouseButtonUp(1);
    }
}
