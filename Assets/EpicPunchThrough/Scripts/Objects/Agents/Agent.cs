﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent( typeof(Animator), typeof(PhysicsBody))]
public class Agent : MonoBehaviour
{
    #region Animation Fields

    [SerializeField] protected RuntimeAnimatorController baseController;

    protected Animator _animator;
    public Animator animator {
         get {
            return _animator;
        }
    }
    public RuntimeAnimatorController animatorController {
        get {
            return animator.runtimeAnimatorController;
        }
        set {
            animator.runtimeAnimatorController = value;
        }
    }
    protected bool stopAnimatorControllerTransition = false;

    protected string transitionStateName = "ControllerTransition";
    protected string transitionBool = "transitionController";

    protected Transform graphicsChild;
    protected Transform cosmetics;
    protected DirectionIndicator directionIndicator;

    protected ParticleController particleController;
    public ParticleEmitter slideParticle;

    protected Dictionary<string, Transform> anchors = new Dictionary<string, Transform>();

    protected int animationVariation = 0;

    #endregion

    #region Technique Fields

    protected List<Technique> techniques = new List<Technique>();
    protected List<Technique> activatingTechniques = new List<Technique>();

    protected Technique _activeTechnique;
    public virtual Technique activeTechnique {
        get {
            return _activeTechnique;
        }
        set {
            _activeTechnique = value;
        }
    }

    #endregion

    #region Agent State Fields

    protected int _team = 0;
    public int Team {
        get {
            return _team;
        }
        set {
            _team = value;
        }
    }

    private bool _isFacingRight = false;
    public bool isFacingRight {
        get {
            return _isFacingRight;
        }
        set {
            _isFacingRight = value;
            graphicsChild.localScale = new Vector3( isFacingRight ? -1 : 1, 1, 1 );
        }
    }

    public enum State {
        Any, // For technique triggers only - Agents should never be in this state
        Grounded,
        InAir,
        WallSliding,
        OnCeiling,
        Flinched,
        Launched
    }
    protected State _state;
    public virtual State state {
        get {
            return _state;
        }
        set {
            if( ValidActiveTechnique() ) {
                _activeTechnique.OnStateChange( _state, value );
            }

            // Default state behaviour
            physicsBody.useGravity = true;
            graphicsChild.eulerAngles = Vector3.zero;

            switch( value ) {
                case State.Grounded:
                    physicsBody.useGravity = false;
                    physicsBody.velocity = new Vector3(physicsBody.velocity.x, Mathf.Max(physicsBody.velocity.y, 0), 0);
                    physicsBody.frictionCoefficients = EnvironmentManager.Instance.GetEnvironment().groundFriction;
                    slideParticle = CreateEmitter("slide", GetAnchor("FloorAnchor").position, 0f, transform);
                    break;
                case State.InAir:
                    onLayer = 1;
                    physicsBody.frictionCoefficients = EnvironmentManager.Instance.GetEnvironment().airFriction;
                    if( slideParticle != null ) {
                        slideParticle.End();
                        slideParticle.transform.parent = null;
                    }
                    break;
                case State.WallSliding:
                    physicsBody.velocity = new Vector3(0, physicsBody.velocity.y, 0);
                    physicsBody.frictionCoefficients = EnvironmentManager.Instance.GetEnvironment().wallFriction;
                    break;
                case State.OnCeiling:
                    physicsBody.velocity = new Vector3(physicsBody.velocity.x, Mathf.Min(physicsBody.velocity.y, 0), 0);
                    break;
                case State.Flinched:
                    physicsBody.velocity = new Vector3(0, 0, 0);
                    physicsBody.frictionCoefficients = EnvironmentManager.Instance.GetEnvironment().airFriction;
                    if( slideParticle != null ) {
                        slideParticle.End();
                        slideParticle.transform.parent = null;
                    }

                    animationVariation = UnityEngine.Random.Range(0, 3);
                    break;
                case State.Launched:
                    onLayer = 1;
                    physicsBody.velocity = new Vector3(0, 0, 0);
                    physicsBody.frictionCoefficients = EnvironmentManager.Instance.GetEnvironment().airFriction;
                    if( slideParticle != null ) {
                        slideParticle.End();
                        slideParticle.transform.parent = null;
                    }

                    animationVariation = UnityEngine.Random.Range(0, 1);
                    break;
            }

            _state = value;
        }
    }

    bool groundFound = false;
    bool wallFound = false;
    bool ceilingFound = false;

    public enum Action {
        None,
        MoveForward,
        MoveBack,
        MoveUp,
        MoveDown,
        AttackForward,
        AttackDown,
        AttackBack,
        AttackUp,
        Block,
        Jump,
        Dash,
        Clash,
        Hit
    }
    protected List<Action> actions = new List<Action>();
    public Action[] ActionSequence {
         get {
            return actions.ToArray();
        }
    }
    public Action lastAction {
         get {
            if( actions.Count == 0 ) { return Action.None; }
            return actions[actions.Count - 1];
        }
    }

    private float _activeActionValue = 0;
    public float activeActionValue {
        get {
            return _activeActionValue;
        }
    }

    protected Vector2 _aimDirection = Vector2.up;
    public Vector2 aimDirection {
        get {
            return _aimDirection;
        }
        set {
            if( value.magnitude <= 0 ) {
                _aimDirection = Vector2.up;
            }
            _aimDirection = value.normalized;

            directionIndicator.transform.eulerAngles = new Vector3( 0, 0, Vector2.SignedAngle(Vector2.up, _aimDirection) );
        }
    }
    public enum AimSegment {
        Up,
        Forward,
        Down,
        Back
    }
    public AimSegment aimSegment {
        get {
            float angle = Vector2.SignedAngle(Vector2.up, aimDirection);

            AimSegment segment;
            if( angle >= -45 && angle <= 45 ) {
                segment = AimSegment.Up;
            } else if( angle >= 45 && angle <= 135 ) {
                segment = ( isFacingRight ? AimSegment.Back : AimSegment.Forward );
            } else if( angle <= -45 && angle >= -135 ) {
                segment = ( isFacingRight ? AimSegment.Forward : AimSegment.Back );
            } else {
                segment = AimSegment.Down;
            }

            return segment;
        }
    }

    protected float stateTimestamp = 0;

    #endregion

    #region Physics Fields

    protected PhysicsBody _physicsBody;
    public PhysicsBody physicsBody {
        get {
            return _physicsBody;
        }
    }
    protected Transform _environmentColliders;
    public Transform environmentColliders {
        get {
            return _environmentColliders;
        }
    }

    protected BoxCollider _bodyCollider;
    protected TriggerCheck _groundCheck;
    protected TriggerCheck _rightWallCheck;
    protected TriggerCheck _leftWallCheck;
    protected TriggerCheck _ceilingCheck;
    protected TriggerCheck _backgroundCheck;

    protected bool passThrough = false;
    protected bool wallCollide = false;

    public float onLayer = 1;
    protected float backgroundDistance = 20;
    protected List<Collider> backgroundColliders = new List<Collider>();

    protected BoxCollider[] boundaryColliders;

	#endregion

	#region Vital Force Fields

    ulong _maxVF = 1000000;
    ulong _currentVF = 1000000;
    ulong _activeFactor = 4;
    double _chargingVF = 0;
    ulong _criticalSoul = 20;

    public ulong maxVF {
        get {
            return _maxVF;
        }
        set {
            if( value <= ulong.MaxValue ) {
                _maxVF = value;
            } else {
                _maxVF = ulong.MaxValue;
            }
        }
    }
    public ulong currentVF {
        get {
            return _currentVF;
        }
        set {
            ulong cappedValue = Math.Max(Math.Min(value, maxVF), 0);
            _currentVF = value;
        }
    }
    public double VFRemaining {
        get {
            return (double)currentVF / (double)maxVF;
        }
    }

    public ulong activeVF {
        get {
            return (ulong)(_maxVF / _activeFactor);
        }
    }
    public ulong activeFactor {
        get {
            return _activeFactor;
        }
        set {
            if( value > 0 ) {
                _activeFactor = value;
            } else {
                _activeFactor = 1;
            }
        }
    }

    public double chargingVF {
        get {
            return _chargingVF;
        }
        set {
            if( chargingVF <= _currentVF ) {
                _chargingVF = value;
            } else {
                _chargingVF = _currentVF;
            }
        }
    }
    public double chargeRate {
        get {
            return (double)activeVF * AgentManager.Instance.settings.chargeRate;
        }
    }

    public ulong criticalVF {
         get {
            return _maxVF / _criticalSoul;
        }
    }
    public ulong criticalSoul {
        get {
            return _criticalSoul;
        }
    }

    public double breakLimit {
        get {
            return (double)activeVF * AgentManager.Instance.settings.breakFactor;
        }
    }


    #endregion

	#region Initialization

	protected bool didInit = false;
    private void Start()
    {
        if( !didInit ) {
            Init();
        }
    }

    private void OnDestroy()
    {
        AgentManager.Instance.UnregisterAgent(this);

        if( boundaryColliders != null ) {
            for( int i = boundaryColliders.Length - 1; i >= 0; i-- ) {
                Destroy(boundaryColliders[i]);
            }
            boundaryColliders = null;
        }
    }

    public virtual void Init( RuntimeAnimatorController baseController, ParticleController particleController, int team )
    {
        this.baseController = baseController;
        this.particleController = particleController;
        this._team = team;

        Init();
    }
    public virtual void Init()
    {
        _animator = GetComponent<Animator>();
        animatorController = baseController;
        animator.logWarnings = false;

        anchors["rig"] = FindChildByDepth(transform, "Rig");

        _physicsBody = GetComponent<PhysicsBody>();
        _environmentColliders = FindTransform("EnvironmentColliders");

        _bodyCollider = FindComponent<BoxCollider>("EnvironmentColliders/BodyCollider");
        _groundCheck = FindTriggerCheck( "EnvironmentColliders/GroundCheck", SetGroundFound );
        _leftWallCheck = FindTriggerCheck("EnvironmentColliders/LeftWallCheck", SetLeftWallFound );
        _rightWallCheck = FindTriggerCheck( "EnvironmentColliders/RightWallCheck", SetRightWallFound );
        _ceilingCheck = FindTriggerCheck( "EnvironmentColliders/CeilingCheck", SetCeilingFound );
        _backgroundCheck = FindTriggerCheck( "EnvironmentColliders/BackgroundCheck", BackgroundFound );

        graphicsChild = FindTransform("Graphics");
        cosmetics = FindTransform("Graphics/Cosmetics");
        directionIndicator = FindComponent<DirectionIndicator>("DirectionIndicator");
        directionIndicator.gameObject.SetActive(false);

        state = AgentManager.Instance.settings.initialAgentState;

        OnEnvironmentChange( null, EnvironmentManager.Instance.GetEnvironment() );
        EnvironmentManager.Instance.environmentChanged += OnEnvironmentChange;

        _criticalSoul = (ulong)AgentManager.Instance.settings.criticalSoulFactor;
        currentVF = maxVF;

        AgentManager.Instance.RegisterAgent(this);

        didInit = true;
    }
    protected virtual TriggerCheck FindTriggerCheck( string name, UnityAction<bool, Collider> action )
    {
        TriggerCheck check = FindComponent<TriggerCheck>(name);
        check.onTrigger.AddListener(action);

        return check;
    }

    public virtual void OnEnvironmentChange( Environment previousEnvironment, Environment nextEnvironment )
    {
        if( boundaryColliders != null ) {
            for( int i = boundaryColliders.Length - 1; i >= 0; i-- ) {
                Destroy(boundaryColliders[i]);
            }
        }

        int colliderCount = ( nextEnvironment.agentBounds.topBound ? 1 : 0 )
                            + ( nextEnvironment.agentBounds.rightBound ? 1 : 0 )
                            + ( nextEnvironment.agentBounds.bottombound ? 1 : 0 )
                            + ( nextEnvironment.agentBounds.leftBound ? 1 : 0 );
        boundaryColliders = new BoxCollider[colliderCount];
        colliderCount = 0;
        if( nextEnvironment.agentBounds.topBound ) {
            float yPosition = nextEnvironment.agentBounds.maxY + (AgentManager.Instance.settings.verticalBoundarySize.y / 2);
            Vector3 position = new Vector3( transform.position.x, yPosition, transform.position.z);

            CreateBoundary(colliderCount, "_TopBoundary", true, position);

            colliderCount++;
        }

        if( nextEnvironment.agentBounds.rightBound ) {
            float xPosition = nextEnvironment.agentBounds.maxX + (AgentManager.Instance.settings.horizontalBoundarySize.x / 2);
            Vector3 position = new Vector3( xPosition, transform.position.y, transform.position.z);

            CreateBoundary(colliderCount, "_RightBoundary", true, position);

            colliderCount++;
        }

        if( nextEnvironment.agentBounds.bottombound ) {
            float yPosition = nextEnvironment.agentBounds.minY - (AgentManager.Instance.settings.verticalBoundarySize.y / 2);
            Vector3 position = new Vector3( transform.position.x, yPosition, transform.position.z);

            CreateBoundary(colliderCount, "_BottomBoundary", true, position);

            colliderCount++;
        }

        if( nextEnvironment.agentBounds.leftBound ) {
            float xPosition = nextEnvironment.agentBounds.minX - (AgentManager.Instance.settings.horizontalBoundarySize.x / 2);
            Vector3 position = new Vector3( xPosition, transform.position.y, transform.position.z);

            CreateBoundary(colliderCount, "_LeftBoundary", false, position);
        }
    }
    void CreateBoundary(int index, string name, bool isVertical, Vector3 position)
    {
        if( index < 0 || index >= boundaryColliders.Length ) { return; }

        boundaryColliders[index] = new GameObject(gameObject.name + name).AddComponent<BoxCollider>();
        boundaryColliders[index].gameObject.layer = LayerMask.NameToLayer("Boundary");
        boundaryColliders[index].transform.parent = transform;
        boundaryColliders[index].transform.parent = null;

        boundaryColliders[index].size = ( isVertical ? AgentManager.Instance.settings.verticalBoundarySize : AgentManager.Instance.settings.horizontalBoundarySize );

        Follow followScript = boundaryColliders[index].gameObject.AddComponent<Follow>();
        followScript.target = transform;
        followScript.followX = isVertical;
        followScript.followY = !isVertical;
        followScript.onFixedUpdate = true;

        boundaryColliders[index].transform.position = position;
    }

    #endregion

    #region Update

    public virtual void DoUpdate( GameManager.UpdateData data )
    {
        if( slideParticle != null ) {
            slideParticle.enabled = false;
        }
        CheckState();

        HandleTechniques();
        if( ValidActiveTechnique() ) {
            activeTechnique.Update(data, _activeActionValue);
        } else {
            if( slideParticle != null ) { slideParticle.enabled = true; }
            HandlePhysics(data);
            HandleAnimation();
        }
    }
    protected virtual void EnableTriggerChecks()
    {
        if( state == State.WallSliding ) {
            _leftWallCheck.doDetect= true;
            _rightWallCheck.doDetect = true;
        } else if( physicsBody.velocity.x > 0 ) {
            _rightWallCheck.doDetect = true;
            _leftWallCheck.doDetect= false;
        } else if( physicsBody.velocity.x < 0 ) {
            _leftWallCheck.doDetect = true;
            _rightWallCheck.doDetect = false;
        } else {
            _leftWallCheck.doDetect= false;
            _rightWallCheck.doDetect = false;
            wallFound = false;
        }
        if( state == State.OnCeiling || physicsBody.velocity.y > 0 ) {
            _ceilingCheck.doDetect = true;
        } else {
            _ceilingCheck.doDetect = false;
            ceilingFound = false;
        }
        if ( state == State.Grounded || physicsBody.velocity.y < 0 ) {
            _groundCheck.doDetect = true;
        } else {
            _groundCheck.doDetect = false;
            groundFound = false;
        }
    }
    public virtual void CheckState()
    {
        switch( state ) {
            case State.InAir:
                if( groundFound ) {
                    state = State.Grounded;
                } else if( wallFound ) {
                    state = State.WallSliding;
                } else if( ceilingFound ) {
                    state = State.OnCeiling;
                }
                break;
            case State.Grounded:
                if( !groundFound ) {
                    state = State.InAir;
                }
                break;
            case State.Flinched:
                if( Time.time >= stateTimestamp ) {
                    animator.SetBool("Flinched", false);
                    state = State.InAir;
                }
                break;
            case State.Launched:
                if( Time.time >= stateTimestamp ) {
                    animator.SetBool("Launched", false);
                    animator.SetBool("Fallen", false);
                    state = State.InAir;
                } else {
			        if (groundFound && physicsBody.velocity.y < 1) { // && physicsBody.velocity.magnitude < 30) {
                        physicsBody.velocity = new Vector3(physicsBody.velocity.x, 0f, 0f);
				        graphicsChild.rotation = Quaternion.identity;
				        animator.SetBool("Fallen", true);
                        if( slideParticle != null ) { slideParticle.enabled = true; }
                        else {
                            slideParticle = CreateEmitter("slide", GetAnchor("FloorAnchor").position, 0f, transform);
                        }
			        } else if (wallFound) {
                        physicsBody.velocity = new Vector3(0f, physicsBody.velocity.y, 0f);
				        graphicsChild.rotation = Quaternion.identity;
				        animator.SetBool("Fallen", true);
                        if( slideParticle != null ) { slideParticle.enabled = true; }
                        else {
                            slideParticle = CreateEmitter("slide", GetAnchor("FloorAnchor").position, 0f, transform);
                        }
			        } else {
				        animator.SetBool("Fallen", false);
                        float rotation = Vector3.SignedAngle(Vector3.right, physicsBody.velocity, Vector3.forward);
                        if( isFacingRight ) {
                            rotation += 180;
                        }

                        graphicsChild.eulerAngles = new Vector3(0, 0, rotation);
                    }
		        }
		break;
            case State.WallSliding:
                if( groundFound ) {
                    state = State.Grounded;
                } else if( !wallFound ) {
                    state = State.InAir;
                }
                break;
            case State.OnCeiling:
                if( groundFound ) {
                    state = State.Grounded;
                } else if( wallFound ) {
                    state = State.WallSliding;
                } else if( !ceilingFound ) {
                    state = State.InAir;
                }
                break;
        }
    }

    protected virtual void HandleTechniques()
    {
        if( animator.GetBool(transitionBool) ) {
            TransitionTechnique(activeTechnique, false);
        }

        ActivateTechniques();
    }
    protected virtual void ActivateTechniques()
    {
        if( activatingTechniques.Count == 1 ) {
            TransitionTechnique(activatingTechniques[0], false);
            activatingTechniques.Clear();
        } else if( activatingTechniques.Count > 1 ) {
            Technique chosenTech = null;
            int longestSequence = 0;
            foreach( Technique tech in activatingTechniques ) {
                if( tech.techTrigger.sequence.Length > longestSequence ) {
                    chosenTech = tech;
                    longestSequence = tech.techTrigger.sequence.Length;
                } else if( tech.techTrigger.sequence.Length == longestSequence ) {
                    if( Array.IndexOf(chosenTech.techTrigger.states, State.Any) != -1 && Array.IndexOf(tech.techTrigger.states, State.Any) == -1 ) {
                        chosenTech = tech;
                        longestSequence = tech.techTrigger.sequence.Length;
                    }
                }
            }

            TransitionTechnique( chosenTech, false );
            activatingTechniques.Clear();
        }
    }

    public virtual void HandleAnimation()
    {
        switch( state ) {
            case State.Grounded:
                animator.SetBool( "Grounded", true );
                float xVelocity = Mathf.Abs(physicsBody.velocity.x / 7f);
                animator.SetFloat( "speed", xVelocity );
                break;
            case State.Flinched:
                animator.SetBool( "Flinched", true );
                animator.SetFloat( "Variation", animationVariation );
                break;
            case State.Launched:
                animator.SetBool( "Launched", true );
                animator.SetFloat( "Variation", animationVariation );
                break;
            default:
                animator.SetBool("Grounded", false );
                float yVelocity = (physicsBody.velocity.y / 10f);
                yVelocity = yVelocity > 0 ? Mathf.Clamp(yVelocity, 0.5f, 1f): Mathf.Clamp(yVelocity + 1, 0, 0.5f);
                animator.SetFloat("YDirection", yVelocity);
                break;
        }
    }

    #endregion

    #region Physics Methods

    public virtual void HandlePhysics( GameManager.UpdateData data )
    {
        HandlePhysics(data, null, null);
    }
    public virtual void HandlePhysics( GameManager.UpdateData data, Vector3? frictionOverride )
    {
        HandlePhysics(data, frictionOverride, null);
    }
    public virtual void HandlePhysics( GameManager.UpdateData data, Vector3? frictionOverride, Vector3? gravityOverride )
    {
        HandleTriggerChecks();

        physicsBody.HandleGravity( data, gravityOverride );
        physicsBody.HandleFriction( frictionOverride );
    }
    protected virtual void HandleTriggerChecks()
    {
        //EnableTriggerChecks();

        if( backgroundDistance < onLayer ) {
            backgroundDistance = onLayer;
        }

        if( state == State.Launched ) {
            _bodyCollider.transform.localScale = new Vector3(1, 1, backgroundDistance);
            _groundCheck.transform.localScale = new Vector3(1, 1, 1);
            _rightWallCheck.transform.localScale = new Vector3(1, 1, 1);
            _leftWallCheck.transform.localScale = new Vector3(1, 1, 1);
            _ceilingCheck.transform.localScale = new Vector3(1, 1, 1);
        } else if( groundFound || wallFound ) {
            _bodyCollider.transform.localScale = new Vector3(1, 1, onLayer);
            _groundCheck.transform.localScale = new Vector3(1, 1, onLayer);
            _rightWallCheck.transform.localScale = new Vector3(1, 1, onLayer);
            _leftWallCheck.transform.localScale = new Vector3(1, 1, onLayer);
            _ceilingCheck.transform.localScale = new Vector3(1, 1, onLayer);
        } else {
            _bodyCollider.transform.localScale = new Vector3(1, 1, 1);
            
            if( physicsBody.velocity.y > 0 ) { 
                _groundCheck.transform.localScale = new Vector3(1, 1, 1);
            } else {
                _groundCheck.transform.localScale = new Vector3(1, 1, (passThrough ? 1 : backgroundDistance) );
            }

            _rightWallCheck.transform.localScale = new Vector3(1, 1, (wallCollide ? backgroundDistance : 1) );
            _leftWallCheck.transform.localScale = new Vector3(1, 1, (wallCollide ? backgroundDistance : 1) );
            _ceilingCheck.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    protected virtual void SetGroundFound( bool state, Collider other )
    {
        if( other == null ) { return; }

        bool isBoundary = other.gameObject.layer == LayerMask.NameToLayer("Boundary");
        bool isProp = other.gameObject.layer == LayerMask.NameToLayer("Prop");
        if( isBoundary ) {
            if( other.GetComponent<Follow>().target != transform ) {
                return;
            }
        }
        if( !isBoundary && !isProp ) { return; }
        if( other.bounds.max.y - transform.position.y > 1 )  { return; }

        _groundCheck.triggerCount += state ? 1 : -1;
        groundFound = _groundCheck.triggerCount > 0;
        
        if( state ) {
            Debug.Log(gameObject.name + " groundFound");
            transform.position = new Vector3(transform.position.x, transform.position.y + (other.bounds.max.y - _groundCheck.transform.position.y), transform.position.z);
            if( isBoundary ) {
                onLayer = 1;
            } else {
                onLayer = (transform.position.z - other.transform.position.z) * -2;
            }
        }
    }
    protected virtual void SetLeftWallFound( bool state, Collider other )
    {
        if( other == null ) { return; }

        bool isBoundary = other.gameObject.layer == LayerMask.NameToLayer("Boundary");
        bool isProp = other.gameObject.layer == LayerMask.NameToLayer("Prop");
        if( isBoundary ) {
            if( other.GetComponent<Follow>().target != transform ) {
                return;
            }
        }
        if( !isBoundary && !isProp ) { return; }

        _leftWallCheck.triggerCount += state ? 1 : -1;
        wallFound = _leftWallCheck.triggerCount > 0;
        if( isFacingRight != true ) { isFacingRight = true; }

        if( state ) {
            Debug.Log(gameObject.name + " leftWallFound");
            float offset = (other.bounds.max.x - _leftWallCheck.transform.position.x);
            transform.position = new Vector3(transform.position.x  + offset, transform.position.y, transform.position.z);
            if( isBoundary ) {
                onLayer = 1;
            } else {
                onLayer = (transform.position.z - other.transform.position.z) * -2;
            }
        }
    }
    protected virtual void SetRightWallFound( bool state, Collider other )
    {
        if( other == null ) { return; }

        bool isBoundary = other.gameObject.layer == LayerMask.NameToLayer("Boundary");
        bool isProp = other.gameObject.layer == LayerMask.NameToLayer("Prop");
        if( isBoundary ) {
            if( other.GetComponent<Follow>().target != transform ) {
                return;
            }
        }
        if( !isBoundary && !isProp ) { return; }

        _rightWallCheck.triggerCount += state ? 1 : -1;
        wallFound = _rightWallCheck.triggerCount > 0;
        if( isFacingRight == true ) { isFacingRight = false; }

        if( state ) {
            Debug.Log(gameObject.name + " rightWallFound");
            float offset = (other.bounds.min.x - _rightWallCheck.transform.position.x);
            transform.position = new Vector3(transform.position.x  + offset, transform.position.y, transform.position.z);
            if( isBoundary ) {
                onLayer = 1;
            } else {
                onLayer = (transform.position.z - other.transform.position.z) * -2;
            }
        }
    }
    protected virtual void SetCeilingFound( bool state, Collider other )
    {
        if( other == null ) { return; }

        bool isBoundary = other.gameObject.layer == LayerMask.NameToLayer("Boundary");
        bool isProp = other.gameObject.layer == LayerMask.NameToLayer("Prop");
        if( isBoundary ) {
            if( other.GetComponent<Follow>().target != transform ) {
                return;
            }
        }
        if( !isBoundary && !isProp ) { return; }

        _ceilingCheck.triggerCount += state ? 1 : -1;
        ceilingFound = _ceilingCheck.triggerCount > 0;

        if( state ) {
            Debug.Log(gameObject.name + " ceilingFound");
            transform.position = new Vector3(transform.position.x, transform.position.y + (other.bounds.min.y - _ceilingCheck.transform.position.y), transform.position.z);
            if( isBoundary ) {
                onLayer = 1;
            } else {
                onLayer = (transform.position.z - other.transform.position.z) * -2;
            }
        }
    }
    protected virtual void BackgroundFound( bool state, Collider other )
    {
        if( other == null ) { return; }

        if( other.gameObject.layer == LayerMask.NameToLayer("Boundary") ) {
            return;
        }
        bool isProp = other.gameObject.layer == LayerMask.NameToLayer("Prop");
        if( !isProp ) { return; }

        if( state ) {
            backgroundColliders.Add(other);
        } else {
            backgroundColliders.Remove(other);
        }

        float foremost = 0f;
        foreach( Collider background in backgroundColliders ) {
            if( background.transform.position.z < foremost ) {
                foremost = background.transform.position.z;
            }
        }
        backgroundDistance = (transform.position.z - foremost + 1) * -2;
        if( backgroundDistance < onLayer ) {
            backgroundDistance = onLayer;
        }
    }

    #endregion

    #region Animation Methods

    public void SetSkin( CharacterSkin skin )
    {
        graphicsChild.Find("Sprites/head").GetComponent<SpriteRenderer>().color = skin.headColor;
        graphicsChild.Find("Sprites/torso").GetComponent<SpriteRenderer>().color = skin.torsoColor;
        graphicsChild.Find("Sprites/hip").GetComponent<SpriteRenderer>().color = skin.hipColor;
        graphicsChild.Find("Sprites/neck").GetComponent<SpriteRenderer>().color = skin.neckColor;
        graphicsChild.Find("Sprites/lowerTorso").GetComponent<SpriteRenderer>().color = skin.lowerTorsoColor;

        graphicsChild.Find("Sprites/rightHand").GetComponent<SpriteRenderer>().color = skin.rightHandColor;
        graphicsChild.Find("Sprites/rightElbow").GetComponent<SpriteRenderer>().color = skin.rightElbowColor;
        graphicsChild.Find("Sprites/rightShoulder").GetComponent<SpriteRenderer>().color = skin.rightShoulderColor;
        graphicsChild.Find("Sprites/rightLowerArm").GetComponent<SpriteRenderer>().color = skin.rightLowerArmColor;
        graphicsChild.Find("Sprites/rightUpperArm").GetComponent<SpriteRenderer>().color = skin.rightUpperArmColor;

        graphicsChild.Find("Sprites/leftHand").GetComponent<SpriteRenderer>().color = skin.leftHandColor;
        graphicsChild.Find("Sprites/leftElbow").GetComponent<SpriteRenderer>().color = skin.leftElbowColor;
        graphicsChild.Find("Sprites/leftShoulder").GetComponent<SpriteRenderer>().color = skin.leftShoulderColor;
        graphicsChild.Find("Sprites/leftLowerArm").GetComponent<SpriteRenderer>().color = skin.leftLowerArmColor;
        graphicsChild.Find("Sprites/leftUpperArm").GetComponent<SpriteRenderer>().color = skin.leftUpperArmColor;

        graphicsChild.Find("Sprites/rightUpperLeg").GetComponent<SpriteRenderer>().color = skin.rightUpperLegColor;
        graphicsChild.Find("Sprites/rightLowerLeg").GetComponent<SpriteRenderer>().color = skin.rightLowerLegColor;
        graphicsChild.Find("Sprites/rightKnee").GetComponent<SpriteRenderer>().color = skin.rightKneeColor;
        graphicsChild.Find("Sprites/rightFoot").GetComponent<SpriteRenderer>().color = skin.rightFootColor;

        graphicsChild.Find("Sprites/leftUpperLeg").GetComponent<SpriteRenderer>().color = skin.leftUpperLegColor;
        graphicsChild.Find("Sprites/leftLowerLeg").GetComponent<SpriteRenderer>().color = skin.leftLowerLegColor;
        graphicsChild.Find("Sprites/leftKnee").GetComponent<SpriteRenderer>().color = skin.leftKneeColor;
        graphicsChild.Find("Sprites/leftFoot").GetComponent<SpriteRenderer>().color = skin.leftFootColor;
    }

    public void AnimationEvent(AnimationEvent e)
    {
        if( ValidActiveTechnique() ) {
            activeTechnique.OnAnimationEvent(e);
        }
    }

    #endregion

    #region Technique Methods

    public delegate void TechniqueEvent(Technique technique);
    public event TechniqueEvent TechniqueChange;

    public virtual void AddTechnique( Technique technique )
    {
        techniques.Add(technique);
    }
    public virtual void RemoveTechnique( Technique technique )
    {
        techniques.Remove(technique);
    }

    public virtual void AddActivatingTechnique( Technique technique )
    {
        activatingTechniques.Add( technique );
    }

    public virtual void TransitionTechnique( Technique technique = null, bool blend = true )
    {
        if( activeTechnique != technique ) {
            if( ValidActiveTechnique() ) {
                activeTechnique.Exit();
            }
        }

        if( blend && !animator.GetCurrentAnimatorStateInfo(0).IsName(transitionStateName) ) {
            animator.SetBool(transitionBool, true);
        } else {
            if( technique == null ) {
                animatorController = baseController;
                animator.SetBool(transitionBool, false);
            } else {
                animatorController = technique.animatorController;
                animator.SetBool(transitionBool, false);
            }
        }

        activeTechnique = technique;
        if( technique != null && technique.IsValid() ) {
            technique.Activate();
        }

        TechniqueEvent handler = TechniqueChange;
        if( handler != null ) {
            handler(activeTechnique);
        }
    }

    public virtual bool ValidActiveTechnique()
    {
        return ( activeTechnique != null && activeTechnique.IsValid() );
    }

    #endregion

    #region Action Methods
    
    public class ActionEventArgs {
        public bool activated { get; set; }
        public float value { get; private set; }

        public ActionEventArgs( float value ) {
            activated = false;
            this.value = value;
        }
    }

    public delegate void ActionEvent( ActionEventArgs e );

    public event ActionEvent MoveForward;
    public event ActionEvent MoveBack;
    public event ActionEvent MoveUp;
    public event ActionEvent MoveDown;
    public event ActionEvent AttackForward;
    public event ActionEvent AttackDown;
    public event ActionEvent AttackBack;
    public event ActionEvent AttackUp;
    public event ActionEvent Block;
    public event ActionEvent Jump;
    public event ActionEvent Dash;
    public event ActionEvent Clash;
    public event ActionEvent Hit;

    public void SubscribeToActionEvent(Action action, ActionEvent callback, bool unSubscribe = false)
    {
        if( !unSubscribe ) {
            switch( action ) {
                case Action.MoveForward:
                    MoveForward += callback;
                    break;
                case Action.MoveBack:
                    MoveBack += callback;
                    break;
                case Action.MoveUp:
                    MoveUp += callback;
                    break;
                case Action.MoveDown:
                    MoveDown += callback;
                    break;
                case Action.AttackForward:
                    AttackForward += callback;
                    break;
                case Action.AttackDown:
                    AttackDown += callback;
                    break;
                case Action.AttackBack:
                    AttackBack += callback;
                    break;
                case Action.AttackUp:
                    AttackUp += callback;
                    break;
                case Action.Block:
                    Block += callback;
                    break;
                case Action.Jump:
                    Jump += callback;
                    break;
                case Action.Dash:
                    Dash += callback;
                    break;
                case Action.Clash:
                    Clash += callback;
                    break;
                case Action.Hit:
                    Hit += callback;
                    break;
                default:
                    Debug.LogError("Action not implemented");
                    break;
            }
        } else {
            switch( action ) {
                case Action.MoveForward:
                    MoveForward -= callback;
                    break;
                case Action.MoveBack:
                    MoveBack -= callback;
                    break;
                case Action.MoveUp:
                    MoveUp -= callback;
                    break;
                case Action.MoveDown:
                    MoveDown -= callback;
                    break;
                case Action.AttackForward:
                    AttackForward -= callback;
                    break;
                case Action.AttackDown:
                    AttackDown -= callback;
                    break;
                case Action.AttackBack:
                    AttackBack -= callback;
                    break;
                case Action.AttackUp:
                    AttackUp -= callback;
                    break;
                case Action.Block:
                    Block -= callback;
                    break;
                case Action.Jump:
                    Jump -= callback;
                    break;
                case Action.Dash:
                    Dash -= callback;
                    break;
                case Action.Clash:
                    Clash -= callback;
                    break;
				case Action.Hit:
                    Hit -= callback;
                    break;
                default:
                    Debug.LogError("Action not implemented");
                    break;
            }
        }
    }

    public void PerformAction( Action action, float value )
    {
        bool escape = false;
        if( ValidActiveTechnique()
            && !activeTechnique.ValidateAction(action, value) )
        { escape = true; }

        if( value == 0 ) { 
            if( action == lastAction ) {
                _activeActionValue = 0; 
            }
            escape = true; 
        }
        if( escape ) { return; }

        ActionEvent handler = ActionToActionEvent(action);
        if( handler == null ) { return; }
        ActionEventArgs e = new ActionEventArgs(value);
        handler(e);

        if( e.activated ) {
            actions.Add(action);

            if( actions.Count > AgentManager.Instance.settings.actionSequenceLength ) {
                actions.RemoveAt(0);
            }

            _activeActionValue = value;
        }
        
        return;
    }

    public void ReceiveHit(Vector3 pushVector, double breakForce, Vector3 launchVector)
    {
        if( ValidActiveTechnique() ) {
            activeTechnique.OnHit(pushVector, breakForce, launchVector);
        } else {
            ProcessHit(pushVector, breakForce, launchVector);
        }
    }
    public void ProcessHit(Vector3 pushVector, double breakForce, Vector3 launchVector)
    {
        if( breakLimit > breakForce ) {
            state = State.Flinched;
            stateTimestamp = Time.time + 0.2f;

            physicsBody.AddVelocity(pushVector);
            if( groundFound ) {
                physicsBody.useGravity = false;
                physicsBody.velocity = new Vector3(physicsBody.velocity.x, Mathf.Max(physicsBody.velocity.y, 0), 0);
            }
        } else {
            state = State.Launched;
            stateTimestamp = Time.time + 1f;

            if( launchVector.x < 0 ) {
                isFacingRight = true;
            } else if( launchVector.x > 0 ) {
                isFacingRight = false;
            }

            physicsBody.AddVelocity(launchVector);
        }
    }

    #endregion

    #region Utility

    public virtual void SetName( string name )
    {
        gameObject.name = name;
        if( boundaryColliders != null ) {
            for( int i = 0; i < boundaryColliders.Length; i++ ) {
                string boundaryName = boundaryColliders[i].name;
                int underScoreIndex = boundaryName.LastIndexOf("_");
                boundaryColliders[i].name = gameObject.name + boundaryName.Substring( underScoreIndex + 1, boundaryName.Length - underScoreIndex - 1 );
            }
        }
    }

    public Transform FindTransform( string name )
    {
        Transform target = transform.Find(name);

        if( target == null ) {
            Debug.LogError($"Agent {gameObject.name} does not have {name} transform");

            string[] names = name.Split('/');
            target = transform;
            foreach( String n in names ) {
                Transform newTarget = target.Find(n);
                if( newTarget == null ) {
                    newTarget = new GameObject(n).transform;
                    newTarget.parent = target;
                    newTarget.localPosition = Vector3.zero;
                }
                target = newTarget;
            }
            
        }

        return target;
    }
    public T FindComponent<T>( string name )
        where T: Component
    {
        Transform target = FindTransform(name);
        T component = target.GetComponent<T>();
        if( component == null ) {
            Debug.LogError($"Agent {gameObject.name} did not find component of type {typeof(T).Name} in object {name}");
            component = target.gameObject.AddComponent<T>();
        }

        return component;
    }

    public virtual ParticleEmitter CreateEmitter( string particleName, Vector3 pos, float angle, Transform parent = null )
    {
        if( particleController == null ) { return null; }

        ParticleEmitter emitter = ParticleManager.Instance.CreateEmitter( pos, angle, parent, particleController.GetParticles(particleName) );

        return emitter;
    }

    public virtual Transform GetAnchor( string name )
    {
        string lowerName = name.ToLower();

        if( anchors.ContainsKey(lowerName) ) { return anchors[lowerName]; }

        Transform anchor = FindChildByDepth( anchors["rig"], name );
        if( anchor != null ) { anchors[lowerName] = anchor; }

        return anchor;
    }
    protected virtual Transform FindChildByDepth( Transform node, string name )
    {
        for( int i = 0; i < node.childCount; i++ ) {
            if( node.GetChild(i).name == name ) {
                return node.GetChild(i);
            } else {
                Transform found = FindChildByDepth(node.GetChild(i), name);
                if( found != null ) { return found; }
            }
        }

        return null;
    }

    public virtual ActionEvent ActionToActionEvent( Action action )
    {
        ActionEvent handler = null;
        switch( action ) {
            case Action.MoveForward:
                handler = MoveForward;
                break;
            case Action.MoveBack:
                handler = MoveBack;
                break;
            case Action.MoveUp:
                handler = MoveUp;
                break;
            case Action.MoveDown:
                handler = MoveDown;
                break;
            case Action.AttackForward:
                handler = AttackForward;
                break;
            case Action.AttackDown:
                handler = AttackDown;
                break;
            case Action.AttackBack:
                handler = AttackBack;
                break;
            case Action.AttackUp:
                handler = AttackUp;
                break;
            case Action.Block:
                handler = Block;
                break;
            case Action.Jump:
                handler = Jump;
                break;
            case Action.Dash:
                handler = Dash;
                break;
            case Action.Clash:
                handler = Clash;
                break;
            case Action.Hit:
                handler = Hit;
                break;
            default:
                Debug.LogError("Action not implemented: " + action);
                break;
        }

        return handler;
    }

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        if( !GameManager.Instance.DebugOn ) { return; }

        Gizmos.color = Color.blue;
        if( _groundCheck.doDetect ) {  Gizmos.DrawWireCube( _groundCheck.transform.position, ((BoxCollider)_groundCheck.collider).size ); }
        if( _rightWallCheck.doDetect ) {  Gizmos.DrawWireCube( _rightWallCheck.transform.position, ((BoxCollider)_rightWallCheck.collider).size ); }
        if( _leftWallCheck.doDetect ) {  Gizmos.DrawWireCube( _leftWallCheck.transform.position, ((BoxCollider)_leftWallCheck.collider).size ); }
        if( _ceilingCheck.doDetect ) {  Gizmos.DrawWireCube( _ceilingCheck.transform.position, ((BoxCollider)_ceilingCheck.collider).size ); }
    }

    #endregion
}