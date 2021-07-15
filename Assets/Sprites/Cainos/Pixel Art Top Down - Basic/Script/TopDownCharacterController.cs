using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;


public class TopDownCharacterController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private InputReader inputReader;
    public float speed;
    private Rigidbody2D rb;
    private bool canMove = true;

    [Header("Interact")]
    [SerializeField] private Transform frontSide;
    [SerializeField] private float interactOverlapRadius;

    [Header("Dig")]
    [SerializeField] private GameObject digTestCanvas;
    private bool isTryingToDig = false, isHoldingMouseButton = false;
    private int currentTestPosition = 0;
    [SerializeField] private UnityEngine.UI.Image fillStage1to2, fillStage2to3;
    [SerializeField] private Vector2 stage1To2Multiplier, stage2to3Multiplier;
    [SerializeField] GameObject toDig;

    [Header ("Animations")]
    private Animator animator;
    [SerializeField] private AnimationStateReference seatheW;
    [SerializeField] private float unseatheTime;

    #region Input Reader Events

    private void OnEnable()
    {
        //inputReader.onMoveEvent += vectorValue => movementDirection = vectorValue;
        inputReader.onMoveEvent += Move;
        inputReader.onInteractEvent += StartInteract;
        inputReader.onDigEvent += ctx => IsHoldingDig = ctx;
    }

    private void OnDisable()
    {
        //inputReader.onMoveEvent -= vectorValue => movementDirection = vectorValue;
        inputReader.onMoveEvent -= Move;
        inputReader.onInteractEvent -= StartInteract;
        inputReader.onDigEvent -= ctx => IsHoldingDig = ctx;

    }

    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void StartInteract()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(frontSide.position, interactOverlapRadius);

        if (hit != null)
        {
            foreach(Collider2D colCheck in hit)
            {
                if(colCheck.GetComponent<IDiggable>() != null)
                {
                    TryToDig(colCheck.gameObject);
                }
                else if (colCheck.TryGetComponent(out IInteractable toInteract))
                {
                    toInteract.OnInteract();
                }
            }
        }
    }

    private void TryToDig(GameObject GOtoDig)
    {
        ResetDigTest();
        digTestCanvas.SetActive(true);
        isTryingToDig = true;
        toDig = GOtoDig;

    }

    private void DigIt()
    {
        ResetDigTest();
        toDig.GetComponent<IInteractable>().OnInteract();
        digTestCanvas.SetActive(false);
        isTryingToDig = false;
    }
    private void ResetDigTest()
    {
        currentTestPosition = 0;
        fillStage1to2.fillAmount = 0;
        fillStage2to3.fillAmount = 0;
    }
    private bool IsHoldingDig
    {
        get => isHoldingMouseButton;
        set => isHoldingMouseButton = value;
    }
    private void Update()
    {
        if (isTryingToDig)
        {
            if (IsHoldingDig) //it needs to hold
            {
                if(inputReader.OnDigTest(digTestCanvas) == 1)
                {
                    if (currentTestPosition != 1)
                    {
                        inputReader.SetDigMousePosition(stage1To2Multiplier);
                        currentTestPosition = 1;

                    }
                }

                if (currentTestPosition == 1)
                {
                    if (fillStage1to2.fillAmount >= 1)
                    {
                        inputReader.SetDigMousePosition(stage2to3Multiplier);
                        currentTestPosition = 2;
                    }
                    else
                    {
                        fillStage1to2.fillAmount = inputReader.GetTargetMousePosition().y / inputReader.GetCurrentMousePosition().y;
                    }
                }

                if(currentTestPosition == 2)
                {
                    if (fillStage2to3.fillAmount >= 1)
                    {
                        DigIt();
                    }
                    else
                    {
                        float checkTestX = inputReader.GetTargetMousePosition().x / inputReader.GetCurrentMousePosition().x;
                        float checkTestY = inputReader.GetTargetMousePosition().y / inputReader.GetCurrentMousePosition().y;
                        fillStage2to3.fillAmount = checkTestX / checkTestY;
                    }
                }


            }
            else
            {
                currentTestPosition = 0;

            }
        }
    }

    private void Move(Vector2 direction, bool isMoving)
    {
        if (canMove)
        {
            animator.SetBool("IsMoving", isMoving);

            if (isMoving)
            {
                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", direction.y);
            }

            rb.velocity = speed * direction;
        }
    }

    IEnumerator UnseatheWeaponAnimation()
    {
        float journey = 0;

        while(unseatheTime > journey)
        {
            if (animator.GetBool("IsMoving")) //If the player moves again, then the animation will not be played.
                break;
            

            journey += Time.deltaTime;
            
            yield return null;
        }

        if(journey >= unseatheTime)
            seatheW.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractable toInteract))
        {
            StartCoroutine(toInteract.ShowInteractable());
        }

      
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable toInteract))
        {
            StartCoroutine(toInteract.HideInteractable());
        }

        if (collision.GetComponent<IDiggable>() != null)
        {
            if (isTryingToDig)
            {
                isTryingToDig = false;
                digTestCanvas.SetActive(false);
                ResetDigTest();
            }
        }
    }
}

