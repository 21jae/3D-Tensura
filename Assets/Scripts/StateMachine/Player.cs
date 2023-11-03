using UnityEngine;

[RequireComponent (typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [field: Header("Reference")]
    [field: SerializeField] public PlayerSO Data { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public PlayerInput Input { get; private set; }
    public Transform MainCameraTransfrom { get; private set; }
    
    private PlayerMovementStateMachine movementStateMachine;



    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Input = GetComponent<PlayerInput>();

        MainCameraTransfrom = Camera.main.transform;

        movementStateMachine = new PlayerMovementStateMachine(this);
    }

    private void Start()
    {
        movementStateMachine.ChangeState(movementStateMachine.IdlingState);
    }

    private void Update()
    {
        movementStateMachine.HandleInput();

        movementStateMachine.Update();
    }

    private void FixedUpdate()
    {
        movementStateMachine.PhysicsUpdate();
    }
}
