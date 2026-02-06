using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class AnimationManager : MonoBehaviour
{
    private Animator _animator;
    private string _currentAnim;
    private string _facing;

    [SerializeField] private Movement Movement;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _facing = Movement.FacingDirection.ToString();
    }

    public void ChangeAnimState(string newAnim, float speed)
    {
        // build final clip/state name like "Walk_Down"
        string final = $"{newAnim}_{_facing}"; // "Walk_Down", "Idle_Left", etc.

        if (_currentAnim == newAnim)
        {
            Debug.Log($"Already playing {final}");
            return;
        }

        ChangePlaySpeed(speed);

        _animator.Play(final);

        _currentAnim = final;
    }

    public void ChangePlaySpeed(float speed)
    {
        _animator.speed = Mathf.Max(0f, speed);
    }


}
