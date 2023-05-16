using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridStatus : MonoBehaviour,IPointerClickHandler
{
    public bool isSelect = false;
    // Start is called before the first frame update

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PiecesMove.isMoveStage)
        {
            isSelect = true;
            Debug.Log("ˆÚ“®æ‚É‘I‚Î‚ê‚½");
        }
        
    }
}
