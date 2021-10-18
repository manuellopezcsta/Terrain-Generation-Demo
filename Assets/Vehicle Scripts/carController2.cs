using UnityEngine;
using System.Collections;

public class carController2 : MonoBehaviour {

	private WheelCollider[] wheels;

	public float maxAngle = 30;
	public float maxTorque = 300;
	public GameObject wheelShape;

	// here we find all the WheelColliders down in the hierarchy

    //float brakes = 0;
    //[SerializeField] float brakePower = 500;

    private GameObject player;
    private ThirdPersonCCv2 playerScript;
    [SerializeField] public bool insideCar;    


	public void Start()
	{
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<ThirdPersonCCv2>();


		wheels = GetComponentsInChildren<WheelCollider>();

		for (int i = 0; i < wheels.Length; ++i) 
		{
			var wheel = wheels [i];

			// create wheel shapes only when needed
			if (wheelShape != null)
			{
				var ws = GameObject.Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;

				if(wheel.transform.localPosition.x < 0f)
				{
					var holder = ws.transform.localScale;
					ws.transform.localScale = new Vector3(holder.x * -1, holder.y, holder.z);
				}
			}
		}
	}

	// this is a really simple approach to updating wheels
	// here we simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero
	// this helps us to figure our which wheels are front ones and which are rear
	public void Update()
	{
        if(insideCar)
        {
            MoveAndTurn();
        }

        foreach (WheelCollider wheel in wheels)
		{
			if (wheelShape) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);

				// assume that the only child of the wheelcollider is the wheel shape
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}
		}
		

        // Para salir del auto
        if(Input.GetKeyDown(KeyCode.H) && insideCar)
        {
            ExitCar();
        }        
	}

    private void MoveAndTurn()
    {
        float angle = maxAngle * Input.GetAxis("Horizontal");
		float torque = maxTorque * Input.GetAxis("Vertical");

        foreach (WheelCollider wheel in wheels)
		{
			// a simple car where front wheels steer while rear ones drive
			if (wheel.transform.localPosition.z > 0)
				wheel.steerAngle = angle;

			if (wheel.transform.localPosition.z < 0)
				wheel.motorTorque = torque;

			// update visual wheels if any
			if (wheelShape) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);

				// assume that the only child of the wheelcollider is the wheel shape
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}

		}
    }

    void FixedUpdate()
    {
        if(insideCar)
        {
            MoveAndTurn();
            playerScript.MovePlayerTo(transform);
            player.transform.rotation = transform.rotation;
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
}
