using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    [SerializeField] int targetFPS = 60; // 変更したい目標のFPS値

    void Start()
    {
        Application.targetFrameRate = targetFPS;
    }
}
