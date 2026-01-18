using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [SerializeField] private TransformAnchor _playerTransformAnchor;
    [SerializeField] private float _flipRotationTime = 0.5f;
    [SerializeField] private Player _player;
    [SerializeField] private TransformEventChannelSO _playerInstantiatedChannel;

    protected Movement Movement
    {
        get => movement ?? _player.Core.GetCoreComponent(ref movement);
    }

    private Movement movement;

    private void OnEnable()
    {
        _playerInstantiatedChannel.OnEventRaised += AddPlayer;
    }

    private void OnDisable()
    {
        _playerInstantiatedChannel.OnEventRaised -= AddPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = _playerTransformAnchor.Value.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 30 * Time.deltaTime);
    }

    void AddPlayer(Transform value)
    {
        _player = value.gameObject.GetComponent<Player>();
    }
}
