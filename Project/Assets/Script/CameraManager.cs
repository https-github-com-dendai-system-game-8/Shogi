using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{
    public Transform m_target;//âÒì]ÇÃíÜêSé≤
    [SerializeField] private int speed;//ëOå„Ç∑ÇÈë¨Ç≥
    public float m_rotateSpeed = 10;
    private GameObject otherCamera;
    [SerializeField] private Vector3 axis = new Vector3(0,0,1);

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
        if(tag == "MainCamera" && otherCamera.activeInHierarchy)
        {
            transform.RotateAround(m_target.position, axis, m_rotateSpeed * -Input.GetAxis("Horizontal") * Time.deltaTime);
            transform.Translate(Input.GetAxis("Vertical") * m_target.position * speed * Time.deltaTime);
            //transform.LookAt(m_target);
        }
        else if(tag == "SubCamera" && otherCamera.activeInHierarchy)
        {
            transform.RotateAround(m_target.position, axis, m_rotateSpeed * -Input.GetAxis("Horizontal2") * Time.deltaTime);
            transform.Translate(Input.GetAxis("Vertical2") * m_target.position * speed * Time.deltaTime);
            //transform.LookAt(m_target);
        }
        else
        {
            transform.RotateAround(m_target.position, axis, m_rotateSpeed * -Input.GetAxis("Horizontal2") * Time.deltaTime);
            transform.Translate(Input.GetAxis("Vertical2") * m_target.position * speed * Time.deltaTime);
            transform.RotateAround(m_target.position, axis, m_rotateSpeed * -Input.GetAxis("Horizontal") * Time.deltaTime);
            transform.Translate(Input.GetAxis("Vertical") * Vector3.forward * speed * Time.deltaTime);
        }
    }
}
