using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesStatus : MonoBehaviour,IPointerClickHandler
{

    public Vector2 startPosition;
    public int[] distination;//‹î‚ÌˆÚ“®‰Â”\æ
    public string type;//‹î‚Ìí—Ş

    public bool isSelect = false;//‚±‚Ì‹î‚ª‘I‚Î‚ê‚Ä‚¢‚é‚©‚Ç‚¤‚©
    
    // Start is called before the first frame update


    public void OnPointerClick(PointerEventData eventData)
    {
        isSelect = true;
        Debug.Log("‘I‚Î‚ê‚½");
    }

    public bool CheckSelected()
    {
        return isSelect;
    }
}
