using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject _cameraObj;
    private Vector3 _specificVector;
    private float _smoothSpeed = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        _cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        _specificVector = new Vector3(transform.position.x, transform.position.y, _cameraObj.transform.position.z);
        _cameraObj.transform.position = Vector3.Lerp(_cameraObj.transform.position, _specificVector, _smoothSpeed * Time.deltaTime);
    }
}
