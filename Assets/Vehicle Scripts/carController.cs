using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Axel
{
    Front,
    Rear
}
[Serializable]
public struct Wheel // Es como una clase con propiedades
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel; // Para saber si es delantero o trasero.
}

public class carController : MonoBehaviour
{
    [SerializeField] float maxAcceleration = 20f;
    [SerializeField] float turnSensivility = 1f;
    [SerializeField] float maxSteerAngle = 45f;
    [SerializeField] List<Wheel> wheels;
    [SerializeField] Vector3 _centerOfMass;
    float brakes = 0;
    [SerializeField] float brakePower = 500;

    private float inputX, inputY;
    private Rigidbody rb;

    // Para las funciones de entrar y salir.
    private GameObject player;
    private ThirdPersonCCv2 playerScript;
    [SerializeField] public bool insideCar;    


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = _centerOfMass;

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<ThirdPersonCCv2>();
    }

    void Update()
    {
        brakes = 0;
        //AnimateWheels();
        GetInputs();
        Brake();
        //Debug.Log(brakes);
        // Checkear is grounded y presionar tecla y insideHelicopter = true
        if(Input.GetKeyDown(KeyCode.H) && insideCar)
        {
            ExitCar();
        }
    }

    void FixedUpdate()
    {
        if(insideCar)
        {
            Move();
            Turn();
            playerScript.MovePlayerTo(transform);
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
                EnterCar();
            }    
        }
    }

    void EnterCar()
    {        
        // Teleport Player a ubicacion de heli
        playerScript.MovePlayerTo(transform);
        player.transform.rotation = transform.rotation;
        // Desabilitar objeto modelo de Player
        //player.transform.GetChild(0).gameObject.SetActive(false);
        // Desabilitar Char controler y el script
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<ThirdPersonCCv2>().enabled = false;
        Debug.Log("DONE ENTERING");
        insideCar = true;
    }

    void ExitCar()
    {
        // Mover player?? un toque a la derecha o izq
        player.transform.Translate(new Vector3(5, 2, 0));
        // Habilitar player
        //player.transform.GetChild(0).gameObject.SetActive(true);
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<ThirdPersonCCv2>().enabled = true;
        Debug.Log("DONE EXITING");
        insideCar = false;
        playerScript.TurnMsgOffline();
    }


    void Move()
    {
        foreach(var wheel in wheels)
        {
            wheel.collider.motorTorque = inputY * maxAcceleration * 500 *Time.deltaTime;
        }
    }

    void Turn()
    {
        foreach( var wheel in wheels)
        {
            if(wheel.axel == Axel.Front)
            {
                var _steerAngle = inputX * turnSensivility * maxSteerAngle;
                wheel.collider.steerAngle = _steerAngle;
            }
        }
    }

    void Brake()
    {
        if(Input.GetKey(KeyCode.Space) == true)
        {
            brakes = brakePower;
            //Debug.Log("Braking");
        }
        foreach (var wheel in wheels)
            {
                //if(wheel.axel == Axel.Rear)
                //{
                    wheel.collider.brakeTorque = brakes;                    
                //}
            }
    }

    void GetInputs()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
    }

    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion _rotation;
            Vector3 _position;
            wheel.collider.GetWorldPose(out _position, out _rotation);
            wheel.model.transform.position = _position;
            wheel.model.transform.rotation = _rotation;
        }
    }
}
