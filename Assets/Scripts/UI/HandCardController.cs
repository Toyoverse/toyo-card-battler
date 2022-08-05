using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandCardController : MonoBehaviour
{
    //value
    public Vector3 cardUp;
    public float duration;
    
    //Private values
    private RectTransform hoverCard;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnHover()
    {
        hoverCard = GetComponent<RectTransform>();
        hoverCard.DOMove(hoverCard.position + cardUp, duration);
    }
    
}