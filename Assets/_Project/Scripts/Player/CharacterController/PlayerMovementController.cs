using KBCore.Refs;
using UnityEditor;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovementController : ValidatedMonoBehaviour
{
    #region Fields
    [SerializeField, Anywhere] InputReader inputReader;
    [SerializeField, Self] Rigidbody rb;
    [field: SerializeField] public Transform GroundCheckPoint { get; protected set; }
    [field: SerializeField] public bool Grounded { get; protected set; }
    [SerializeField] bool onSlope;
    RaycastHit slopeHit;
    Vector2 inputVector;
    Vector3 gravity;
    [field: SerializeField] public float GroundCheckRadius { get; protected set; } = 0.4f;
    #endregion

    private void Awake()
    {
        rb.useGravity = false;
        rb.freezeRotation = true;
        inputReader.Jump += OnJump;
        inputReader.Move += OnMove;
    }
    private void OnDestroy()
    {
        inputReader.Move -= OnMove;
        inputReader.Jump -= OnJump;
    }
    private void Update()
    {
        //ground and slope check
        GroundCheck();
    }
    void GroundCheck()
    {
        Grounded = Physics.CheckSphere(GroundCheckPoint.position, GroundCheckRadius, GlobalPlayerConfig.GroundLayerMask);
        if (Grounded)
        {
            SlopeCheck();
        }
        else
        {
            onSlope = false;
        }
    }
    void SlopeCheck()
    {
        Physics.Raycast(GroundCheckPoint.position, -GroundCheckPoint.up, out slopeHit, transform.localScale.x / 2, GlobalPlayerConfig.GroundLayerMask);
        onSlope = slopeHit.normal != Vector3.up;
    }
    private void FixedUpdate()
    {
        gravity = new Vector3(0, rb.velocity.y, 0);
        if (!Grounded)
        {
            //apply gravity
            gravity -= transform.up * GlobalPlayerConfig.Gravity;
        }
        Vector3 dir = (transform.forward * inputVector.y + transform.right * inputVector.x).normalized * GlobalPlayerConfig.PlayerSpeed;
        if (onSlope)
        {
            dir = Vector3.ProjectOnPlane(dir, slopeHit.normal);
        }
        rb.velocity = gravity + dir;
    }
    public void OnMove(Vector2 inputVector)
    {
        this.inputVector = inputVector;
    }

    public void OnJump()
    {
        if (Grounded)
        {
            rb.AddForce(transform.up * GlobalPlayerConfig.JumpForce, ForceMode.Impulse);
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerMovementController))]
    public class GroundCheckDebug : Editor
    {
        private void OnSceneGUI()
        {
            var p = (PlayerMovementController)target;
            Handles.color = Color.yellow;
            Handles.DrawWireArc(p.GroundCheckPoint.position, Vector3.up, p.transform.forward, 360, p.GroundCheckRadius);
            Handles.DrawWireArc(p.GroundCheckPoint.position, p.transform.forward, Vector3.up, 360, p.GroundCheckRadius);
            Handles.DrawWireArc(p.GroundCheckPoint.position, p.transform.right, Vector3.up, 360, p.GroundCheckRadius);
        }
    }
#endif
}