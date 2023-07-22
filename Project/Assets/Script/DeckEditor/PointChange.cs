using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointChange : MonoBehaviour
{

    [SerializeField] private int deltaPoint; 
    private EditManager editManager;

    private void Start()
    {
        editManager = FindObjectOfType<EditManager>();
    }
    // Start is called before the first frame update
    public void OnClick()
    {
        editManager.status[editManager.pieceID] += deltaPoint;
    }
}
