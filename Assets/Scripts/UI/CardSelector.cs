using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using Fusion;
using Tools.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelector : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //PathMode
    public PathType pathType;
    public PathMode pathMode;
    
    
    //Paths
    [Tooltip("Readable card position")]
    public Vector3[] cardParentPos;
    [Tooltip("Card start position")]
    public Vector3[] cardIniPos;
    [Tooltip("Left center row positions")]
    public Vector3[] targetGoToMiddle;
    [Tooltip("Card rotation by position")]
    public float[] cardIniRotZ;
    
    [Tooltip("Hand card parent ID number")]
    private int ParentValue;
    [Tooltip("Rotation when in readable position")]
    private Vector3 cardRotateUp;
    [Tooltip("Rotation when displayed in hand")]
    private Vector3 cardRotateDown;

    private Vector3 cardScale;
    public float scaleMultiplier;
    

    private RectTransform cardActualPos;
    private Vector3 cardInitialScale;
    public RectTransform childCard;
    public RectTransform childTop;
    public RectTransform childBottom;

    //Values
    //public float alphaValue;
    public float duration;
    private bool clickActive = false;

    public RectTransform parentPos;
    //private Image changeImage;

    private float alphaZero = 0.0f;
    private float alphaOne = 1.0f;
    
    //IMPROVEMENT 
    //Maybe is better to change the cards from Ui to Sprite
    //so we can use the sorting layer component
    //to put the selected card in front of the others
    
    
    // Start is called before the first frame update
    void Start()
    {
        cardActualPos = GetComponent<RectTransform>();
        childCard = cardActualPos.Find("Mini_card-pivot").GetComponent<RectTransform>();
        childTop = childCard.Find("Mini_card-top").GetComponent<RectTransform>();
        childBottom = childCard.Find("Mini_card-bottom").GetComponent<RectTransform>();
        cardRotateUp = Vector3.zero; 
        //ParentValue = cardActualPos.gameObject.GetComponentInParent<ParentID>().parentId;
        ParentValue = cardActualPos.gameObject.GetComponentInParent<ParentID>().parentId; //Get parent ID to know the right position and rotation
        parentPos = cardActualPos.parent.GetComponent<RectTransform>();
        childCard.gameObject.SetActive(false);
        childTop.GetComponent<RawImage>().DOFade(alphaZero, alphaZero);
        childBottom.GetComponent<RawImage>().DOFade(alphaZero, alphaZero);

    }

    public void OnPointerEnter(PointerEventData eventData) => CardMoveUp(); //Call method when mouse hover the card
    public void OnPointerExit(PointerEventData eventData) => CardMoveDown(); //Call method when mouse exit the card
    public void OnPointerClick(PointerEventData eventData) => CardClick();//Call method when mouse clicks on card

    private void CardClick() //Called when the mouse clicks the card 
    {
        cardActualPos.DOMove(targetGoToMiddle[ParentValue], duration); //Send card to center left row
        cardActualPos.DORotate(cardRotateUp, duration); //Set card rotation to vector3.zero
        cardActualPos.GetComponent<Image>().DOFade(0.0f ,duration).OnComplete(OffMovingCard);// Fade out card and call new method to turn off the card
        clickActive = true;
        
        childCard.gameObject.SetActive(true);
        childTop.GetComponent<RawImage>().DOFade(alphaOne, duration);
        childBottom.GetComponent<RawImage>().DOFade(alphaOne, duration);
        
       
    }

    private void CardMoveUp() //Called when the mouse hovers the card
    {
        cardScale = new Vector3(cardActualPos.localScale.x * scaleMultiplier, cardActualPos.localScale.y * scaleMultiplier, cardActualPos.localScale.z * scaleMultiplier); //Start scale multiplied
        
        if (clickActive) return;
            cardActualPos.DOMove(cardParentPos[ParentValue], duration); //Move the card to tge parent position for better reading
            cardActualPos.DORotate(cardRotateUp, duration); //Set card rotation to vector3.zero
            cardActualPos.DOScale(cardScale, duration); //Set card scale to bigger -> size * scaleMultiplier
            parentPos.transform.SetAsLastSibling();
            Debug.Log("Visualizando posição" + parentPos.transform.GetSiblingIndex());
    }
    private void CardMoveDown() //Called when mouse exits the cards
    {
        cardInitialScale = new Vector3(1.3f, 1.3f, 1.3f); //Original scale
        
        if (clickActive) return;
        cardRotateDown = new Vector3(0, 0, cardIniRotZ[ParentValue]); //Get the Int referent to the card parent and call the right rotation
        cardActualPos.DOMove(cardIniPos[ParentValue], duration); //Move the card to the start position
        cardActualPos.DORotate(cardRotateDown, duration); //Set card rotation to start rotation
        cardActualPos.DOScale(cardInitialScale, duration); //Set card scale to original
        //Debug.Log("Clicou");
    }

    private void OffMovingCard() =>  childCard.gameObject.SetActive(false);




}
