using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{
    public Transform m_target;
    [SerializeField] private int speed;
    public float m_rotateSpeed = 10;
    private GameObject otherCamera;
    private void Start()
    {
        if (tag == "MainCamera")
            otherCamera = GameObject.FindGameObjectWithTag("SubCamera");
        else
            otherCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    private void Update()
    {
        if (tag == "MainCamera" && otherCamera.activeInHierarchy)
        {
            transform.RotateAround(m_target.position, Vector3.forward, m_rotateSpeed * -Input.GetAxis("Horizontal") * Time.deltaTime);
            transform.Translate(Input.GetAxis("Vertical") * Vector3.forward * speed * Time.deltaTime);
            //transform.LookAt(m_target);
        }
        else if (tag == "SubCamera" && otherCamera.activeInHierarchy)
        {
            transform.RotateAround(m_target.position, Vector3.forward, m_rotateSpeed * -Input.GetAxis("Horizontal2") * Time.deltaTime);
            transform.Translate(Input.GetAxis("Vertical2") * Vector3.forward * speed * Time.deltaTime);
            //transform.LookAt(m_target);
        }
        else
        {
            transform.RotateAround(m_target.position, Vector3.forward, m_rotateSpeed * -Input.GetAxis("Horizontal2") * Time.deltaTime);
            transform.Translate(Input.GetAxis("Vertical2") * Vector3.forward * speed * Time.deltaTime);
            transform.RotateAround(m_target.position, Vector3.forward, m_rotateSpeed * -Input.GetAxis("Horizontal") * Time.deltaTime);
            transform.Translate(Input.GetAxis("Vertical") * Vector3.forward * speed * Time.deltaTime);
        }
    }
}