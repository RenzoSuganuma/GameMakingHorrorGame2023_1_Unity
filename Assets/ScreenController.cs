using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    [SerializeField] int targetFPS = 60; // �ύX�������ڕW��FPS�l

    void Start()
    {
        Application.targetFrameRate = targetFPS;
    }
}
