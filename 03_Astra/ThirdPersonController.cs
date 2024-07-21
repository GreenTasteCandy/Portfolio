using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

/*
 * 3인칭 프로젝트 에셋에 들어가 있는 3인칭 컨트롤러 스크립트
 * 캐릭터의 전체적인 이동 및 조작에 관련된 스크립트이다
 * 
 * StarterAssetsInputs 스크립트에서 플레이어의 조작을 받아와서 동작하는 방식으로 작동된다
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        public AudioClip[] weaponAudioClips;
        public AudioClip[] skillAudioClips;
        public AudioClip[] hitAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDAttack;
        private int _animIDSkill;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        private PlayerActing playerAct;
        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

        public Vector3 movePoint;
        private Vector3 LookPoint;
        private int weaponNum;
        private bool isHit;
        private bool isSlash;

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            playerAct = GetComponent<PlayerActing>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            //업데이트 함수에서 모든 조작 함수들이 처리된다

            if (GameManager.ins.isAtivite)
            {
                if (!GameManager.ins.isWork)
                {
                    JumpAndGravity();
                    GroundedCheck();
                    Move();
                    AttackAnim();
                    UseSkill();
                    playerAct.PlayDance();
                    playerAct.SwitchWeapon();
                    playerAct.GatheringWork();
                }
                else
                {
                    playerAct.GatheringCancel();
                }
            }
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void OnTriggerEnter(Collider other)
        {
            //적에게 피해를 입을 시
            if (other.gameObject.CompareTag("EnemyWeapon"))
            {
                if (!isHit)
                {
                    var index = Random.Range(0, hitAudioClips.Length);
                    AudioSource.PlayClipAtPoint(hitAudioClips[index], transform.TransformPoint(_controller.center), GameSetting.sfxValue);
                    float dmg = other.transform.parent.GetComponent<EnemySystem>().damage;
                    GameManager.ins.health -= dmg;

                    if (GameManager.ins.health <= 0)
                    {
                        StopAllCoroutines();
                        GameManager.ins.isAtivite = false;
                        _animator.SetBool("Attack", false);
                        _animator.SetBool("isMining", false);
                        _animator.SetBool("isWork", false);
                        _animator.SetBool("isGathering", false);
                        _animator.SetTrigger("Dead");
                    }
                    
                    StartCoroutine(HitEffect());
                }
            }
        }

        IEnumerator HitEffect()
        {
            isHit = true;
            Transform bullet = GameManager.ins.poolManager.GetPool(1).transform;
            bullet.position = transform.position + Vector3.up;

            yield return new WaitForSeconds(1.03f);

            bullet.gameObject.SetActive(false);
            isHit = false;
        }

        private void AssignAnimationIDs()
        {
            //애니메이션 파라미터를 받아오는 부분
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDAttack = Animator.StringToHash("Attack");
            _animIDSkill = Animator.StringToHash("Skill");
        }

        private void GroundedCheck() //현재 땅을 밣고 있는지 체크하는 함수
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation() //카메라 회전을 담당하는 함수
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            LookPoint = new Vector3(_input.look.x, 0.0f, _input.look.y);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move() //캐릭터의 이동을 담당하는 함수
        {
            //이동 속도, 스프린트 속도 및 스프린트가 눌린 경우를 기준으로 목표 속도 설정
            float targetSpeed;

            if (GameManager.ins.stamina > 0f)
                targetSpeed = _input.move.sqrMagnitude >= 0.8f ? SprintSpeed + GameManager.ins.speed : MoveSpeed + GameManager.ins.speed;
            else
                targetSpeed = MoveSpeed + GameManager.ins.speed;


            //제거, 교체 또는 반복이 용이하도록 설계된 단순한 가속 및 감속

            // 참고: Vector2의 == 연산자는 근사를 사용하므로 부동 소수점 오류가 발생하지 않으며 크기보다 저렴합니다
            // 입력이 없으면 목표 속도를 0으로 설정합니다
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // 선수들의 현재 수평 속도에 대한 참조
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // 목표 속도까지 가속 또는 감속
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // 선형 결과가 아닌 곡선 결과를 생성하여 보다 유기적인 속도 변화를 제공합니다
                // Lerp의 T는 고정되어 있으므로 속도를 고정할 필요가 없습니다
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // 소수점 세 자리까지 반올림하는 속도
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // 입력 방향의 정규화
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // 카메라 위치를 기준으로 입력 방향을 향하도록 회전합니다
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void AttackAnim() //캐릭터의 공격 기능
        {
            if (GameManager.ins.stamina < 0)
            {
                _animator.SetBool(_animIDAttack, false);
                return;
            }

            if (Grounded)
            {
                bool isAttack = _input.targetOn;

                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDAttack, isAttack);
                    _animator.SetFloat("attackSpeed", 1f * GameManager.ins.rate);
                }

                if (!isHit)
                {
                    if (isAttack)
                    {
                        // if there is an input and camera position is not fixed
                        if (_input.target.sqrMagnitude >= _threshold && !LockCameraPosition)
                        {
                            //Don't multiply mouse input by Time.deltaTime;
                            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                            _cinemachineTargetYaw += _input.target.x * deltaTimeMultiplier;
                            _cinemachineTargetPitch += _input.target.y * deltaTimeMultiplier;
                        }

                        // clamp our rotations so our values are limited 360 degrees
                        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
                        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                        LookPoint = new Vector3(_input.target.x, 0.0f, _input.target.y);

                        // Cinemachine will follow this target
                        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);

                        if (weaponNum == 1)
                        {
                            GameManager.ins.crosshair.SetActive(isAttack);
                        }

                        if (GameManager.ins.isTPS)
                        {
                            transform.rotation = Quaternion.Euler(0.0f, _mainCamera.transform.eulerAngles.y, 0.0f);
                        }
                        else
                        {
                            _targetRotation = Mathf.Atan2(LookPoint.x, LookPoint.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

                            // 카메라 위치를 기준으로 입력 방향을 향하도록 회전합니다
                            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                        }

                        switch (playerAct.weaponNum)
                        {

                            case 0: //쌍검
                                if (!playerAct.isSlash)
                                {
                                    AudioSource.PlayClipAtPoint(weaponAudioClips[playerAct.weaponNum], transform.TransformPoint(_controller.center), GameSetting.sfxValue);
                                    StartCoroutine(playerAct.SlashEffect(false));
                                }
                                break;

                            case 1: //레이저 소총
                            case 3: //플라즈마 권총
                                if (!playerAct.isSlash)
                                {
                                    AudioSource.PlayClipAtPoint(weaponAudioClips[playerAct.weaponNum], transform.TransformPoint(_controller.center), GameSetting.sfxValue);
                                    Vector3 dir = new Vector3(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
                                    StartCoroutine(playerAct.ShotBullet(dir, false));
                                }
                                break;

                            case 2: //대검
                                if (!playerAct.isSlash)
                                {
                                    AudioSource.PlayClipAtPoint(weaponAudioClips[playerAct.weaponNum], transform.TransformPoint(_controller.center), GameSetting.sfxValue);
                                    StartCoroutine(playerAct.SlashEffect2(false));
                                }
                                break;

                        }
                    }
                }
            }
        }

        private void UseSkill()
        {
            if (GameManager.ins.isUseSkill)
            {
                return;
            }

            if (GameManager.ins.stamina < GameManager.ins.weaponTable.weapon[playerAct.weaponNum].skillCost)
            {
                _animator.SetBool(_animIDSkill, false);
                return;
            }

            if (Grounded)
            {

                bool isSkill = _input.skill;

                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDSkill, isSkill);
                    _animator.SetFloat("attackSpeed", 1f * GameManager.ins.rate);
                }

                if (!isHit && isSkill)
                {
                    switch (playerAct.weaponNum)
                    {

                        case 0: //쌍검
                            if (!playerAct.isSlash)
                            {
                                AudioSource.PlayClipAtPoint(skillAudioClips[playerAct.weaponNum], transform.TransformPoint(_controller.center), GameSetting.sfxValue);
                                GameManager.ins.stamina -= GameManager.ins.weaponTable.weapon[playerAct.weaponNum].skillCost;
                                StartCoroutine(playerAct.SlashEffect(true));
                                GameManager.ins.StartCoolDown();
                            }
                            break;

                        case 1: //레이저 소총
                        case 3: //플라즈마 권총
                            if (!playerAct.isSlash)
                            {
                                AudioSource.PlayClipAtPoint(skillAudioClips[playerAct.weaponNum], transform.TransformPoint(_controller.center), GameSetting.sfxValue);
                                GameManager.ins.stamina -= GameManager.ins.weaponTable.weapon[playerAct.weaponNum].skillCost;
                                Vector3 dir = new Vector3(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
                                StartCoroutine(playerAct.ShotBullet(dir, true));
                                GameManager.ins.StartCoolDown();
                            }
                            break;

                        case 2: //대검
                            if (!playerAct.isSlash)
                            {
                                AudioSource.PlayClipAtPoint(skillAudioClips[playerAct.weaponNum], transform.TransformPoint(_controller.center), GameSetting.sfxValue);
                                GameManager.ins.stamina -= GameManager.ins.weaponTable.weapon[playerAct.weaponNum].skillCost;
                                StartCoroutine(playerAct.SlashEffect2(true));
                                GameManager.ins.StartCoolDown();
                            }
                            break;

                    }
                }
            }
        }
        
        private void JumpAndGravity() //점프 및 중력에 관련한 기능
        {
            if (GameManager.ins.stamina <= 0)
                return;

            if (Grounded) //땅을 밟고 있을 때, 이때는 점프를 누르면 위로 올라가게 된다
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else //땅을 안 밟고 있을 때,이때는 중력이 작동한다
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), GameSetting.sfxValue);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), GameSetting.sfxValue);
            }
        }
    }
}