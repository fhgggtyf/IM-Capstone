using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator _animator;
    private string _currentAnim;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = gameObject.GetComponentInChildren<Animator>();
    }

    public void ChangeAnimState(string newAnim, float speed)
    {
        if (_currentAnim == newAnim) return;

        ChangePlaySpeed(speed);

        _animator.Play(newAnim);

        _currentAnim = newAnim;
    }

    public void ChangePlaySpeed(float speed)
    {
        if (speed < 0) _animator.StartPlayback();
        else _animator.StopPlayback();

        _animator.speed = speed;
    }


}
