using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{

    public InputReader inputReader;
    public Camera mainCamera;
    public CinemachineCamera VCam;
    public CinemachineImpulseSource impulseSource;

    [SerializeField] [Range(.5f, 3f)] private float _speedMultiplier = 1f; //TODO: make this modifiable in the game settings											
    [SerializeField] private TransformAnchor _cameraTransformAnchor = default;
    [SerializeField] private TransformAnchor PlayerFollowObjectTransformAnchor = default;
    [SerializeField] private TransformAnchor PlayerTransformAnchor = default;

    [Header("Listening on channels")]
    [Tooltip("The CameraManager listens to this event, fired by protagonist GettingHit state, to shake camera")]
    [SerializeField] private VoidEventChannelSO _camShakeEvent = default;

    public static CameraManager instance;

    [SerializeField] private CinemachineCamera[] _allVirtualCameras;

    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _yOffset = 0;
    [SerializeField] private float _fallYPanTime = 0.35f;
    private float _fallSpeedYDampingChangeThreshold = -20;

    private bool isLerpingYDamping;
    private bool isDecreasingYOffset;
    private bool lerpedFromPlayerFalling;

    private Coroutine _lerpYPanCoroutine;

    private bool characterLoaded;

    private Player player;

    private CinemachineCamera _currentCamera;
    private CinemachinePositionComposer _positionComposer;

    private float _normPanAmount;
    private float _normYOffsetAmount;

    public float FallSpeedYDampingChangeThreshold { get => _fallSpeedYDampingChangeThreshold; set => _fallSpeedYDampingChangeThreshold = value; }
    public bool IsLerpingYDamping { get => isLerpingYDamping; set => isLerpingYDamping = value; }
    public bool LerpedFromPlayerFalling { get => lerpedFromPlayerFalling; set => lerpedFromPlayerFalling = value; }



    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i = 0; i < _allVirtualCameras.Length; i++)
        {
            if (_allVirtualCameras[i].enabled)
            {
                _currentCamera = _allVirtualCameras[i];
                _positionComposer = _currentCamera.GetComponent<CinemachinePositionComposer>();
            }

            _normPanAmount = _positionComposer.Damping.y;
            _normYOffsetAmount = _positionComposer.TargetOffset.y;
        }

        Debug.Log(PlayerFollowObjectTransformAnchor.isSet);
//        if (PlayerFollowObjectTransformAnchor.isSet)
 //           SetupProtagonistVirtualCamera();
    }

    private void OnEnable()
    {
        PlayerFollowObjectTransformAnchor.OnAnchorProvided += SetupProtagonistVirtualCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (characterLoaded)
        {
            if (player.movementVector.y < FallSpeedYDampingChangeThreshold && !IsLerpingYDamping && !LerpedFromPlayerFalling)
            {
                LerpYDamping(true);
            }

            if (player.movementVector.y >= -1f && !IsLerpingYDamping && lerpedFromPlayerFalling)
            {
                lerpedFromPlayerFalling = false;
                LerpYDamping(false);
            }
        }
    }

    private void OnDisable()
    {
        PlayerFollowObjectTransformAnchor.OnAnchorProvided -= SetupProtagonistVirtualCamera;
    }

    public void LerpYDamping(bool isPlayerFalling)
    {
        //_lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
        StartCoroutine(ReduceYOffset(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        isLerpingYDamping = true;

        float startDampAmount = _positionComposer.Damping.y;
        float endDampAmount;

        if (isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = _normPanAmount;
        }

        float elaspedTime = 0f;

        while (elaspedTime < _fallYPanTime)
        {
            elaspedTime += Time.deltaTime;
            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elaspedTime / _fallYPanTime);
            _positionComposer.Damping.y = lerpedPanAmount;

            yield return null;
        }

        isLerpingYDamping = false;

    }

    private IEnumerator ReduceYOffset(bool isPlayerFalling)
    {
        isDecreasingYOffset = true;
        float startYOffsetAmount = _positionComposer.TargetOffset.y;
        float endYOffsetAmount;

        if (isPlayerFalling)
        {
            endYOffsetAmount = _yOffset;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endYOffsetAmount = _normYOffsetAmount;
        }

        float elaspedTime = 0f;

        while (elaspedTime < _fallYPanTime)
        {
            elaspedTime += Time.deltaTime;
            float lerpedYOffsetChangeAmount = Mathf.Lerp(startYOffsetAmount, endYOffsetAmount, elaspedTime / _fallYPanTime);
            _positionComposer.TargetOffset.y = lerpedYOffsetChangeAmount;

            yield return null;
        }

        isDecreasingYOffset = false;

    }

    /// <summary>
    /// Provides Cinemachine with its target, taken from the TransformAnchor SO containing a reference to the player's Transform component.
    /// This method is called every time the player is reinstantiated.
    /// </summary>
    public void SetupProtagonistVirtualCamera()
    {
        instance.player = PlayerTransformAnchor.Value.gameObject.GetComponent<Player>();

        Transform target = PlayerFollowObjectTransformAnchor.Value;

        VCam.Follow = target;
        VCam.OnTargetObjectWarped(target, target.position - VCam.transform.position - Vector3.forward);       

        characterLoaded = true;
    }
}
