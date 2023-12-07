using UnityEngine;
using Cinemachine;
using System.Collections;

public class AdventurePlayerMovement : MonoBehaviour
{
    [Space]
    [Header("Player Attributes")]
    [SerializeField] public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private Vector3 lastMoveDirection = Vector3.zero;
    private Coroutine currentMovementCoroutine; // Almacena la corutina actual.


    [Space]
    [Header("Animation/State")]
    private string actualState;
    private bool walking = false;


    [Space]
    [Header("Components")]
    [SerializeField] private Animator anim;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject referenciaY;
    public bool mobile;
    private void Start()
    {
        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }

        
        anim = GetComponent<Animator>();
        anim.SetFloat("anglex", 1);
    }

    private void Update()
    {
        ChangeScale();
        if (mobile)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    targetPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    targetPosition.z = 0f;

                    // Verifica si el personaje está colisionando con un Collider con etiqueta "Collisions"
                    if (IsCollidingWithOutOfMapCollider(this.transform.position, targetPosition))
                    {

                        Vector3 moveDirection = (targetPosition - transform.position).normalized;

                        if (currentMovementCoroutine != null)
                        {
                            StopCoroutine(currentMovementCoroutine);
                        }

                        currentMovementCoroutine = StartCoroutine(MoveToDestination(moveDirection));

                        // Actualizar la animación del personaje.
                        UpdateAnimation();
                    }

                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) // Verifica si se hizo clic con el botón izquierdo del mouse
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = 0f;
                // Verifica si el personaje está colisionando con un Collider con etiqueta "Collisions"
                if (!IsCollidingWithOutOfMapCollider(this.transform.position, targetPosition))
                {

                    Vector3 moveDirection = (targetPosition - transform.position).normalized;

                    if (currentMovementCoroutine != null)
                    {
                        StopCoroutine(currentMovementCoroutine);
                    }

                    currentMovementCoroutine = StartCoroutine(MoveToDestination(moveDirection));

                    // Actualizar la animación del personaje.
                    UpdateAnimation();
                }

            }
        }
       
        
        // Verifica si el personaje está colisionando con un Collider con etiqueta "Collisions"
        if (IsCollidingWithOutOfMapCollider(this.transform.position, targetPosition))
        {
            StopCoroutine(currentMovementCoroutine);
            walking = false;
        }

        AnimationUpdate();
    }

    private void ChangeScale()
    {
        if (this.transform.position.y > referenciaY.transform.position.y)
        {
            float nuevaEscala = 1f - (this.transform.position.y - referenciaY.transform.position.y) * 0.25f;
            this.transform.localScale = new Vector3(nuevaEscala, nuevaEscala, 1);
        }
        
    }

    private IEnumerator MoveToDestination(Vector3 moveDirection)
    {
        walking = true;
        lastMoveDirection = moveDirection;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }

        walking = false;
        //virtualCamera.Follow = transform;
    }

    private void UpdateAnimation()
    {
        // Configurar la velocidad de la animación según la velocidad de movimiento.
        //animator.SetFloat("Speed", isMoving ? moveSpeed : 0f);

        // Configurar la dirección de la animación.
        //arriba
        //derecha
        if (lastMoveDirection.x > 0f)
        {
            anim.SetFloat("anglex", 1);
        }

        //izquierda
        else if (lastMoveDirection.x < 0f)
        {
            anim.SetFloat("anglex", -1);
        }
        Debug.Log(lastMoveDirection);
    }

    void AnimationUpdate()
    {
        if (!walking)
        {
            ChangeAnimationState("Idle");
        }

        if (walking)
        {
            ChangeAnimationState("Walk");
        }

    }

    void ChangeAnimationState(string newState)
    {

        if (actualState == newState) return;


        anim.Play(newState);

        actualState = newState;
    }

    // Función para verificar si un rayo desde "startPosition" hacia "endPosition" colisiona con el Collider etiquetado como "Collisions"
    private bool IsCollidingWithOutOfMapCollider(Vector3 startPosition, Vector3 endPosition)
    {
        Vector2 direction = endPosition - startPosition;
        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction.normalized, 0.2f, LayerMask.GetMask("Collisions"));
        // Dibuja el rayo en el Editor
        Debug.DrawRay(startPosition, direction.normalized * 0.25f, Color.red, 1f);
        if (hit == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}