using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{
    public Transform m_target;//âÒì]ÇÃíÜêSé≤
    [SerializeField] private int speed = 10;//ëOå„Ç∑ÇÈë¨Ç≥
    public float m_rotateSpeed = 30;
    private GameObject otherCamera;
    [SerializeField] private Vector3 axis = new Vector3(1,0,0);

    private void Start()
    {
        if (tag == "MainCamera")
            otherCamera = GameObject.FindGameObjectWithTag("SubCamera");
        else
            otherCamera = GameObject.FindGameObjectWithTag("MainCamera");

        if (axis.x != 0)
            axis = new Vector3(axis.x / axis.x, axis.y, axis.z);
        if (axis.y != 0)
            axis = new Vector3(axis.x, axis.y / axis.y, axis.z);
        if (axis.z != 0)
            axis = new Vector3(axis.x, axis.y, axis.z / axis.z);
    }
    private void Update()
    {
        Vector3 tr = transform.localPosition;
        if (tr.x * tr.x + tr.y * tr.y + tr.z * tr.z < 10)
        {
            transform.Translate(-Vector3.forward);
            return;
        }
        else if (tr.x * tr.x + tr.y * tr.y + tr.z * tr.z > 200)
        {
            transform.Translate(Vector3.forward);
            return;
        }
        if (tag == "MainCamera" && otherCamera.activeInHierarchy)
        {
            transform.RotateAround(m_target.position, axis, m_rotateSpeed * -Input.GetAxis("Horizontal") * Time.deltaTime);
            transform.Translate(Input.GetAxis("Vertical") * Vector3.forward * speed * Time.deltaTime);
            //transform.LookAt(m_target);
        }
        else if (tag == "SubCamera" && otherCamera.activeInHierarchy)
        {
            transform.RotateAround(m_target.position, axis, m_rotateSpeed * -Input.GetAxis("Horizontal2") * Time.deltaTime);
            transform.Translate(Input.GetAxis("Vertical2") * Vector3.forward * speed * Time.deltaTime);
            //transform.LookAt(m_target);
        }
        else
        {
            transform.RotateAround(m_target.position, axis, m_rotateSpeed * -Input.GetAxis("Horizontal3") * Time.deltaTime);
            transform.Translate(Input.GetAxis("Vertical3") * Vector3.forward * speed * Time.deltaTime);
        }
        
    }
}