using Cinemachine;
using UnityEngine;
 
public class TPCinemachineFLCameraV2 : MonoBehaviour
{
    // This scripts goes inside the cinemachine Camera object.
    private CinemachineFreeLook _cinemachineFreeLook;
    private string _inputAxisNameX;
    private string _inputAxisNameY;
    public bool hideMouseOnClick = false;

 
    void Awake()
    {
        _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        _inputAxisNameX = _cinemachineFreeLook.m_XAxis.m_InputAxisName;
        _inputAxisNameY = _cinemachineFreeLook.m_YAxis.m_InputAxisName;
    }
 
    void Update()
    {
        //
        if(Input.GetMouseButton(1) && hideMouseOnClick)
        {
            Cursor.lockState = CursorLockMode.Locked;
        } else{
            Cursor.lockState = CursorLockMode.None;
        }
        //
        _cinemachineFreeLook.m_XAxis.m_InputAxisName = Input.GetMouseButton(1) ? _inputAxisNameX : "";
        _cinemachineFreeLook.m_YAxis.m_InputAxisName = Input.GetMouseButton(1) ? _inputAxisNameY : "";
    }
}
