using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{
    public Transform m_target;
    [SerializeField] private int speed;
    public float m_rotateSpeed = 1000;

    private void Update()
    {
        transform.RotateAround(m_target.position, Vector3.forward, m_rotateSpeed * -Input.GetAxis("Horizontal") * Time.deltaTime);
        transform.Translate(Input.GetAxis("Vertical") * Vector3.forward * speed * Time.deltaTime);
        //transform.LookAt(m_target);
    }
}
