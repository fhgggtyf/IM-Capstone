using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : Character
{
    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    public VoidEventChannelSO playerIsDeadChannel;
    [SerializeField]
    private VoidEventChannelSO patrolRouteSet;

    public PatrolRoute patrolRoute;

    // Runtime memory for the loop
    public int[] checkedIndices;
    public Vector2 moveTarget;
    public float arriveDistance = 0.01f;

    public bool TargetAssigned = false;

    [NonSerialized] public int lastPatrolIndex = -1;

    [NonSerialized] public bool nonIdle = false;
    [NonSerialized] public bool targetIsDead = false;

    // The world position where the enemy last heard the player. This is set by
    // hearing/vision conditions and used by the investigate actions to move to the
    // correct location. It is reset when the investigation completes.
    [NonSerialized] public Vector2 lastHeardPosition;

    // Flag indicating that a noise stimulus was detected and the enemy should
    // transition into the investigation state. Conditions set this flag and
    // investigate actions should reset it once the enemy has responded.
    [NonSerialized] public bool hasHeardPlayer;

    // Flag indicating that the current investigation cycle has finished. Once set
    // the investigation to patrol condition will transition the enemy back to
    // patrolling and reset this flag.
    [NonSerialized] public bool investigationComplete;

    [NonSerialized] public bool playerIsInSight = false;

    private void InitCheckedIndices()
    {
        if (patrolRoute != null)
        {
            Debug.Log(patrolRoute);
            checkedIndices = new int[patrolRoute.Points.Length];
        }
    }

    protected virtual void OnEnable()
    {

        patrolRouteSet.OnEventRaised += InitCheckedIndices;
    
        playerIsDeadChannel.OnEventRaised += TargetDead;
    }

    protected virtual void OnDisable()
    {
        patrolRouteSet.OnEventRaised -= InitCheckedIndices;
        playerIsDeadChannel.OnEventRaised -= TargetDead;
    }

    private void TargetDead() => targetIsDead = true;

    public virtual void OnDrawGizmos()
    {

    }

}
