using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public TransformAnchor PlayerTransformAnchor;

    public PolygonCollider2D cameraBoundsCollider;

    [Header("Settings")]
    public bool cameraIsStationary = false; // ¡û NEW TOGGLE

    private Transform player;
    private CinemachineCamera vcam;
    private CinemachinePositionComposer composer;

    private bool characterLoaded;

    void Start()
    {
        vcam = GetComponentInChildren<CinemachineCamera>();
        composer = vcam.GetComponent<CinemachinePositionComposer>();

        // Instant follow
        composer.Damping = Vector3.zero;
        composer.TargetOffset = Vector3.zero;
    }

    private void OnEnable()
    {
        PlayerTransformAnchor.OnAnchorProvided += SetupProtagonistVirtualCamera;
    }

    private void OnDisable()
    {
        PlayerTransformAnchor.OnAnchorProvided -= SetupProtagonistVirtualCamera;
    }

    void LateUpdate()
    {
        if (cameraIsStationary)
            return; // ¡û STOP CAMERA MOVING ENTIRELY

        if (!characterLoaded || player == null)
            return;

        Vector3 desiredPos = player.position;

        // No boundaries ¡ú just follow
        if (cameraBoundsCollider == null)
        {
            vcam.Follow.position = desiredPos;
            return;
        }

        float camHalfHeight = mainCamera.orthographicSize;
        float camHalfWidth = camHalfHeight * mainCamera.aspect;

        Vector2 camCenter = desiredPos;
        Vector2 closest = cameraBoundsCollider.ClosestPoint(camCenter);

        if ((closest - camCenter).sqrMagnitude > 0.0001f)
        {
            desiredPos = new Vector3(closest.x, closest.y, desiredPos.z);
        }

        vcam.Follow.position = desiredPos;
    }

    public void SetupProtagonistVirtualCamera()
    {
        player = PlayerTransformAnchor.Value;

        if (!cameraIsStationary)
        {
            vcam.Follow = player;
            vcam.OnTargetObjectWarped(player, Vector3.zero);
        }

        characterLoaded = true;
    }
}
