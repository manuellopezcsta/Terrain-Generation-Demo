using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    // Para el Menu
    public GameObject debugScreen;
    public GameObject carPrefab;
    public GameObject heliPrefab;
    private ThirdPersonCCv2 movementScript;

    // Car
    private GameObject car;
    // Heli
    private GameObject heli;

    // Para el Boton de PJ
    public Transform playerPos;
    [SerializeField] Vector3 offset = new Vector3(0, 5, 5);
    [SerializeField] Vector3 heliOffset = new Vector3(0, 5, 5);
    public GameObject player;
    private CharacterController playerCharacterController;

    // Para el Toggle;
    public Toggle toggle;
    public TPCinemachineFLCameraV2 toggleOption; 

    void Start()
    {
        playerCharacterController = player.GetComponent<CharacterController>();
        movementScript = player.GetComponent<ThirdPersonCCv2>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F3))
        {
            debugScreen.SetActive(!debugScreen.activeSelf);
            movementScript.enabled = !movementScript.isActiveAndEnabled;
        }
    }

    public void SummonCarButton()
    {
        // If a car exists destroy it
        if(car != null)
        {
            Destroy(car);
        }
        //Debug.Log(playerPos);
        car = Instantiate(carPrefab, playerPos.position + offset, playerPos.rotation);
        GameObject level = GameObject.Find("Level");
        car.transform.parent = level.transform;
    }

    public void SummonHeliButton()
    {
        // If a heli exists destroy it
        if(heli != null)
        {
            Destroy(heli);
        }
        //Debug.Log(playerPos);
        heli = Instantiate(heliPrefab, playerPos.position + heliOffset, playerPos.rotation);
        GameObject level = GameObject.Find("Level");
        heli.transform.parent = level.transform;
    }
    

    public void RecenterPlayer()
    {
        // APARENTEMENTE HAY QUE DESHABILITAR EL CHARACTER CONTROLER 1 SEGUNDITO PARA QUE ANDE EL CAMBIO DE POSICION.

        //Debug.Log("PP: " + playerPos.localPosition);
        playerCharacterController.enabled = false;
        playerPos.localPosition = new Vector3(playerPos.localPosition.x, 15f, playerPos.localPosition.z); 
        playerCharacterController.enabled = true;
        //Debug.Log("RECENTERED AT: " + new Vector3(playerPos.localPosition.x, 15f, playerPos.localPosition.z));
        //Debug.Log(playerPos.position);
    }

    public void OnToggle()
    {
        toggleOption.hideMouseOnClick = toggle.isOn;
    }
}
