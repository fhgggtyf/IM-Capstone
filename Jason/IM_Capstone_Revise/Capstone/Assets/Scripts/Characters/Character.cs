using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public Core Core;
    [SerializeField] public AnimationEventHandler animationEventHandler;

    [NonSerialized] public Vector2 movementVector; //Final movement vector, manipulated by the StateMachine actions
    [NonSerialized] public bool isAttackFinished = true;
    [NonSerialized] public bool stunOver;
}
