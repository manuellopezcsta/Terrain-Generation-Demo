using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerZero : MonoBehaviour {

    [SerializeField]
    private TerrainController terrainController;
    //public Camera cam;
    [SerializeField]
    private CinemachineFreeLook cam;
    [SerializeField]
    private float distance = 10;

    private void FixedUpdate() {

        if (Vector3.Distance(Vector3.zero, transform.position) > distance) {
            //CamHolder.SetActive(false);
            cam.OnTargetObjectWarped(transform, -transform.position);
            //cam.transform.position -= transform.position;//            
            terrainController.Level.position -= transform.position;
            transform.position = Vector3.zero;//only necessary if player isn't a child of the level
        }
    }
}