using System;
using UnityEngine;

public class EnemySignals : MonoBehaviour
{
    public event Action IdleFinished;

    public void RaiseIdleFinished() => IdleFinished?.Invoke();
}
