using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heliController : MonoBehaviour
{
    public float Speed;
    float hor;
    float ver;
    float altitude;
    [SerializeField] float altitudeModifier; // From 1 to 5

    // Para las animaciones
    [SerializeField] Animator animator;
    [SerializeField] Transform heliBlades;
    [SerializeField] float bladeSpeed;
    [SerializeField] Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask; // Layer del piso
    [SerializeField] bool isGrounded;

    // Para entrar y salir del Heli
    private GameObject player;
    private ThirdPersonCCv2 playerScript;
    [SerializeField] public bool insideHelicopter;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<ThirdPersonCCv2>();
    }
    void Update ()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
        altitude = Input.GetAxis("Altitude");

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        AnimateHeli();

        if(insideHelicopter)
        {
            // Move Player to helicopter.
            playerScript.MovePlayerTo(transform);
        }
        // Checkear is grounded y presionar tecla y insideHelicopter = true
        if(isGrounded && Input.GetKeyDown(KeyCode.H) && insideHelicopter)
        {
            ExitHeli();
        }
    }

    void FixedUpdate()
    {
        Vector3 heliMovement = new Vector3(hor, 0 , ver).normalized * Speed * Time.deltaTime;
        heliMovement += new Vector3(0, altitude, 0).normalized * altitudeModifier * Time.deltaTime;
        if(insideHelicopter) // If the player is inside the heli move.
        {
            transform.Translate(heliMovement, Space.Self);
        }
        
        
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            //Debug.Log(" Se coliciono con Player");
            if(Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("Se presiono G");
                EnterHeli();
            }    
        }
    }

    void EnterHeli()
    {        
        // Teleport Player a ubicacion de heli
        playerScript.MovePlayerTo(transform);
        // Desabilitar objeto modelo de Player
        player.transform.GetChild(0).gameObject.SetActive(false);
        // Desabilitar Char controler y el script
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<ThirdPersonCCv2>().enabled = false;
        Debug.Log("DONE ENTERING");
        insideHelicopter = true;
    }

    void ExitHeli()
    {
        // Mover player?? un toque a la derecha o izq
        player.transform.Translate(new Vector3(5, 2, 0));
        // Habilitar player
        player.transform.GetChild(0).gameObject.SetActive(true);
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<ThirdPersonCCv2>().enabled = true;
        Debug.Log("DONE EXITING");
        insideHelicopter = false;
        playerScript.TurnMsgOffline();
    }

    void AnimateHeli()
    {
        if(!isGrounded)
        {
            bladeSpeed = 20;
            // Animar heli
            //heliBlades.rotation = Quaternion.Euler(0, 5, 0);
            heliBlades.Rotate(new Vector3(0, 50, 0) * Time.deltaTime * bladeSpeed);
        } else{
            bladeSpeed = 0;
        }

        if(insideHelicopter)
        {
            // Animacion del Heli
            if(hor >= 0.1f)
            {
                // Play 1 animation
                //animator.Play("heliRight");
                animator.SetBool("bRight", true);
                animator.SetBool("bLeft", false);
            } else if (hor <= -0.1f)
            {
                //animator.Play("heliLeft");
                animator.SetBool("bLeft", true);
                animator.SetBool("bRight", false);
            } else{
                //animator.Play("heliIddle");
                animator.SetBool("bRight", false);
                animator.SetBool("bLeft", false);
            }

            if(ver >= 0.1f)
            {
                // Play 1 animation
                //animator.Play("heliFoward");
                animator.SetBool("bFoward", true);
                animator.SetBool("bBackwards", false);
            } else if (ver <= -0.1f)
            {
                // Animacion contraria
                //animator.Play("heliBackwards");
                animator.SetBool("bBackwards", true);
                animator.SetBool("bFoward", false);
            } else{
                //animator.Play("heliIddle");
                animator.SetBool("bFoward", false);
                animator.SetBool("bBackwards", false);
            }
        }
        
    }
}
