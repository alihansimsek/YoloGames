using UnityEngine;

public class PlayerController : MonoBehaviour   //handles movement, touches and collisions
{

    public delegate void PlayerStateChange();
    public static event PlayerStateChange PlayerDeathEvent;
    public static event PlayerStateChange PlayerSwipeEvent;
    public static event PlayerStateChange PlayerResetEvent;
    public static event PlayerStateChange PlayerNextLevelEvent;

    [SerializeField]
    private float swipeMinimumDistance = .2f;
    [SerializeField]
    private float swipeMaximumTime = 1f;
    [SerializeField, Range(0f, 1f)]
    private float directionThreshold = .9f;
    [SerializeField]
    private float speed = 50f;
    private InputManager inputManager;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float startTime;
    private float endTime;
    private Rigidbody rb;
    private Vector3 originalPosition;
    private MeshRenderer playerMesh;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }
    private void Start()
    {
        playerMesh = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
    }
    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }
    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }
    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
    }
    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }
    private void DetectSwipe()
    {
        if (PlayerSwipeEvent != null)
            PlayerSwipeEvent();
        if(Vector3.Distance(startPosition, endPosition) >= swipeMinimumDistance && (endTime - startTime) <= swipeMaximumTime)
        {
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }
    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            //Debug.Log("Swipe Up");
            rb.AddForce(Camera.main.transform.forward * speed, ForceMode.VelocityChange);
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            //Debug.Log("Swipe Down");
            rb.AddForce(-Camera.main.transform.forward * speed, ForceMode.VelocityChange);
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            //Debug.Log("Swipe Right");
            rb.AddForce(Camera.main.transform.right * speed, ForceMode.VelocityChange);
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            //Debug.Log("Swipe Left");
            rb.AddForce(-Camera.main.transform.right * speed, ForceMode.VelocityChange);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("OutOfMap"))
        {
            //Debug.Log("Died");
            playerMesh.enabled = false;
            rb.velocity = Vector3.zero;
            if (PlayerDeathEvent != null)
                PlayerDeathEvent();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FinishLine"))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            if(PlayerNextLevelEvent != null)
                PlayerNextLevelEvent();
        }
    }
    public void NextLevel()
    {
        rb.constraints = RigidbodyConstraints.None;
    }
    public void ResetPlayerPosition()
    {
        playerMesh.enabled = true;
        transform.position = originalPosition;
        rb.velocity = Vector3.zero;
        PlayerResetEvent();
    }
}
