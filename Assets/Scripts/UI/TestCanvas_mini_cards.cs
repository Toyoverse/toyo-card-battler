using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TestCanvas_mini_cards : MonoBehaviour
{
    //public references of pivots
    [Header("TARGET TO GO")]
    public RectTransform targetLeft;
    public RectTransform targetRight;
    [Tooltip("Time that the cards take to reach the target position")]
    public float fMoveTime;
    
    
    //public references of mini cards
    [Header("CARDS PIVOTS")]
    public RectTransform[] miniCardLeft;
    public RectTransform[] miniCardRight;
    
    //Public reference to turn the image off without modifying the line
    //public Image[] cardVisibility;
    
    //Top and Bottom pieces of the card
    [Header("TOP/BOTTOM SEARCH FOR")]
    //private RectTransform cardPivot;
    public RectTransform cardTop;
    public RectTransform cardBottom;
    public string cardTopName;
    public string cardBottomName;
    //public string pivotName;
    
    
    //public path 
    [Header("PATH SETTINGS")]
    public PathType pathType;
    public PathMode pathMode;
    public RotateMode rotateMode;
    [Tooltip("Time to walk the path and all other animations")]
    public float duration;
    [Tooltip("Time used only for the grow scale")]
    public float smallDuration;
    
    
    //Left
    [Header("LEFT PATH SETTINGS")]
    public Vector3[] pathLeftTop;
    public Vector3[] pathLeftBottom;
    public Vector3 rotateLeftTop;
    public Vector3 rotateLeftBottom;
    [Tooltip("Decides the scale grow of the Top piece")]
    public float scaleLeftTop;
    [Tooltip("Decides the scale grow of the Bottom piece")]
    public float scaleLeftBottom;
    
    //Right
    [Header("RIGHT PATH SETTINGS")]
    public Vector3[] pathRightTop;
    public Vector3[] pathRightBottom;
    public Vector3 rotateRightTop;
    public Vector3 rotateRightBottom;
    [Tooltip("Decides the scale grow of the Top piece")]
    public float scaleRightTop;
    [Tooltip("Decides the scale grow of the Bottom piece")]
    public float scaleRightBottom;
    
    
    //Values
    [Header("WIN PUNCH SETTINGS")]
    [Tooltip("Left winning punch direction and distance")]
    public Vector3 v3LeftPunch;
    [Tooltip("Right winning punch direction and distance")]
    public Vector3 v3RightPunch;
    [Tooltip("Duration of the animation")]
    public float fWinFeedback;
    [Tooltip("How much the punch vibrates")]
    public int iVib;
    [Tooltip("Represents how much the vector will go beyond the starting position 0(Off) - 1(On)")]
    public float fElastic;


    //Win Lose bools
    [Header("WIN/LOSE SETTINGS")]
    public bool bOnLeftWin;
    public bool bOnRightWin;

    [Header("VALIDATION STATUS")] 
    [Tooltip("Link to left status dot")]
    public RectTransform leftStatus;
    [Tooltip("Link to right status dot")]
    public RectTransform rightStatus;
    [Tooltip("Time to change color")]
    public float timeToCheck;
    //[Tooltip("Time between color change")]
    private float timeInterval = 0.0f;

    //private float setTimer = 1.0f;
    
    

    //private Values
    private int cardNumLeft; //Changes inside the SWITCH, is used to identify the cards in the array
    private int cardNumRight; //Changes inside the SWITCH, is used to identify the cards in the array
    private int iCountValueLeft; //Used to change SWITCH CASES when left mouse click
    private int iCountValueRight; //Used to change SWITCH CASES when right mouse click

    private bool callTimer = false; //Time to the next status color

    private void Start()
    {
        leftStatus.gameObject.SetActive(false); //Disable left status dot
        rightStatus.gameObject.SetActive(false); //Disable right status dot
    }

    // Update is called onc1e per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) //Only left wins
        {
            iCountValueLeft += 1;
            Debug.Log("Left value :" + iCountValueLeft);
            
           AllToCenter();
           /* if (bOnLeftWin)
            {
                leftToCenter();
            }
            else if (bOnRightWin)
            {
                RightToCenter();
            } */
            
            callTimer = true;

        }
        
        if (callTimer)
        {
            timeInterval += Time.deltaTime;
            //Debug.Log("Timer called" + timeInterval);
        }
        
        OnLeftStatusCheck();

       /* if (Input.GetMouseButtonDown(1)) //Only right wins
        {
            iCountValueRight += 1;
            Debug.Log("Right value: " + iCountValueRight);
            
            RightToCenter();
        }*/

        /*if () //use method disableUi boolean to deactivate the gameObject
        {
            //make a variable that updates when the switch changes
            //disable boolean after passing through
        }*/

    }

    public void Validation()
    {

        if (bOnLeftWin)
        {
            //Left winning animation
            miniCardLeft[cardNumLeft].DOPunchPosition(v3LeftPunch, fWinFeedback, iVib, fElastic).OnComplete(disableUi);

            //Right lose animation
            cardTop = miniCardRight[cardNumRight].Find(cardTopName).GetComponent<RectTransform>();
            cardBottom = miniCardRight[cardNumRight].Find(cardBottomName).GetComponent<RectTransform>();

            cardTop.DOPath(pathRightTop, duration, pathType, pathMode, 10, Color.red);
            cardTop.DORotate(rotateRightTop, duration, rotateMode);
            cardTop.DOScale(scaleRightTop, smallDuration).OnComplete(OnScaleTop); //Grow scale - makes the part get bigger before "fading" in size
            cardBottom.DOPath(pathRightBottom, duration, pathType, pathMode, 10, Color.blue);
            cardBottom.DORotate(rotateRightBottom, duration, rotateMode);
            cardBottom.DOScale(scaleRightBottom, smallDuration).OnComplete(OnScaleBottom);//Grow scale - makes the part get bigger before "fading" in size
            
        } else if (bOnRightWin)
        {
            //Right winning animation
            miniCardRight[cardNumRight].DOPunchPosition(v3RightPunch, fWinFeedback, iVib, fElastic).OnComplete(disableUi);
            
            //Left lose animation
            cardTop = miniCardLeft[cardNumLeft].Find(cardTopName).GetComponent<RectTransform>();
            cardBottom = miniCardLeft[cardNumLeft].Find(cardBottomName).GetComponent<RectTransform>();

            cardTop.DOPath(pathLeftTop, duration, pathType, pathMode, 10, Color.red);
            cardTop.DORotate(rotateLeftTop, duration, rotateMode);
            cardTop.DOScale(scaleLeftTop, smallDuration).OnComplete(OnScaleTop); //Grow scale - makes the part get bigger before "fading" in size
            cardBottom.DOPath(pathLeftBottom, duration, pathType, pathMode, 10, Color.blue);
            cardBottom.DORotate(rotateLeftBottom, duration, rotateMode);
            cardBottom.DOScale(scaleLeftBottom, smallDuration).OnComplete(OnScaleBottom);//Grow scale - makes the part get bigger before "fading" in size
            
        }
    }
    
    

    private void OnLeftStatusCheck() //Change status dot colors to simulate validation process
    {
        
        
        if (timeInterval > 0.3f && timeInterval < 0.4)
        {
            leftStatus.gameObject.SetActive(true); //Activate status dot everytime a card goes to center
            leftStatus.GetComponent<RawImage>().DOColor(Color.gray, 0.0f);
            
        }else if (timeInterval > 0.5f && timeInterval < 1)
        {
            leftStatus.GetComponent<RawImage>().DOColor(Color.blue, timeToCheck);
            //Debug.Log("Mudou cor azul");
            
        }else if (timeInterval > 2 && timeInterval < 2.2f)
        {
            leftStatus.GetComponent<RawImage>().DOColor(Color.red, timeToCheck);
            //Debug.Log("Mudou cor vermelho");
            
        }else if (timeInterval > 2.2f)
        {
            Validation();
            callTimer = false;
            timeInterval = 0;
        }

        /* Sequence seq = DOTween.Sequence(); 
         seq
             
             .Append(leftStatus.GetComponent<RawImage>().DOColor(Color.gray, timeToCheck)) 
             //.AppendInterval(timeInterval * 0.5f) //Don't work in the middle of the sequence, only send time to star or end 
             .Append(leftStatus.GetComponent<RawImage>().DOColor(Color.blue, timeToCheck))
             //.AppendInterval(timeInterval) //Don't work in the middle of the sequence, only send time to star or end 
              .Append(leftStatus.GetComponent<RawImage>().DOColor(Color.red, timeToCheck))
             .AppendCallback(Validation); //Calls the validation method
             */
        
        //leftStatus.gameObject.SetActive(true);
        //leftStatus.GetComponent<Image>().DOColor(Color.gray, timeToCheck);
    }

   /* public void winningCard()
    {

        //Make Left and Right Ifs - Activate the Ifs when press the mouse button
        Debug.Log("Ganhou");
        miniCardLeft[cardNumLeft].DOPunchPosition(v3LeftPunch, fWinFeedback, iVib, fElastic);
        //After comparing both cards, calls the winning animation, call DoPunch, DoScale and disable card
    }

    public void loseCard()
    {
        //Make Left and Right Ifs - Activate the Ifs when press the mouse buttom
        Debug.Log("Perdeu");
        
        cardTop = miniCardLeft[cardNumLeft].Find(cardTopName).GetComponent<RectTransform>();
            cardBottom = miniCardLeft[cardNumLeft].Find(cardBottomName).GetComponent<RectTransform>();

            cardTop.DOPath(pathLeftTop, duration, pathType, pathMode, 10, Color.red);
            cardTop.DORotate(rotateLeftTop, duration, rotateMode);
            cardTop.DOScale(scaleLeftTop, smallDuration).OnComplete(OnScaleTop); //Grow scale - makes the part get bigger before "fading" in size
            cardBottom.DOPath(pathLeftBottom, duration, pathType, pathMode, 10, Color.blue);
            cardBottom.DORotate(rotateLeftBottom, duration, rotateMode);
            cardBottom.DOScale(scaleLeftBottom, smallDuration).OnComplete(OnScaleBottom); //Grow scale - makes the part get bigger before "fading" in size
            //After comparing both cards, calls the lose animation(breaking the card), DoScale and disable card
        }*/

    public void OnScaleTop()
    {
        
        cardTop.DOScale(Vector3.zero, duration);
    }
    
    public void OnScaleBottom()
    {
        
        cardBottom.DOScale(Vector3.zero, duration);
       
    }

    public void disableUi()
    {

        //Debug.Log("Desligou carta");
        if (bOnLeftWin)
        {
            //miniCardLeft[cardNumLeft].GetComponent<Image>().DOFade(0f, duration); //Fade card and .Oncomplete() call the disable
            miniCardLeft[cardNumLeft].gameObject.SetActive(false); //Disable left winning card
            leftStatus.gameObject.SetActive(false); //Disable left status dot
        }
            

        else if (bOnRightWin)
        {
            miniCardRight[cardNumRight].gameObject.SetActive(false);
            rightStatus.gameObject.SetActive(false); //Disable right status dot
        }

        //Use boolean to deactivate card
    }

   /* void leftToCenter() //Callback method must contain the win and lose in conditions, they can be differed by a bool 
    {
        switch (iCountValueLeft)
        {
            case 1:
                miniCardLeft[0].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck); //Card moves to center
                cardNumLeft = 0;
                //Verify who wins - Check both cards script to see who wins, change OnComplete method depending on the result
               //Check if it's done and call a method - call in the end to disable the card

                break;
            case 2:
                miniCardLeft[1].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck);
                cardNumLeft = 1;
                break;
            case 3:
                miniCardLeft[2].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck);
                cardNumLeft = 2;
                break;
            case 4:
                miniCardLeft[3].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck);
                cardNumLeft = 3;
                break;
            case 5:
                miniCardLeft[4].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck);
                cardNumLeft = 4;
                break;
        }
        
    }
    void RightToCenter() //Callback method must contain the win and lose in conditions, they can be differed by a bool 
    {
        switch (iCountValueLeft)
        {
            case 1:
                miniCardRight[0].DOMove(targetRight.position, fMoveTime, false).OnComplete(Validation); //Card moves to center
                cardNumRight = 0;
                miniCardLeft[0].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck); //Card moves to center
                cardNumLeft = 0;
                //Verify who wins - Check both cards script to see who wins, change OnComplete method depending on the result
               //Check if it's done and call a method - call in the end to disable the card

                break;
            case 2:
                miniCardRight[1].DOMove(targetRight.position, fMoveTime, false).OnComplete(Validation);
                cardNumRight = 1;
                miniCardLeft[1].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck); //Card moves to center
                cardNumLeft = 1;
                break;
            case 3:
                miniCardRight[2].DOMove(targetRight.position, fMoveTime, false).OnComplete(Validation);
                cardNumRight = 2;
                miniCardLeft[2].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck); //Card moves to center
                cardNumLeft = 2;
                break;
            case 4:
                miniCardRight[3].DOMove(targetRight.position, fMoveTime, false).OnComplete(Validation);
                cardNumRight = 3;
                miniCardLeft[3].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck); //Card moves to center
                cardNumLeft = 3;
                break;
            case 5:
                miniCardRight[4].DOMove(targetRight.position, fMoveTime, false).OnComplete(Validation);
                cardNumRight = 4;
                miniCardLeft[4].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck); //Card moves to center
                cardNumLeft = 4;
                break;
        }
        
    } */ 
    void AllToCenter() //Callback method must contain the win and lose in conditions, they can be differed by a bool 
    {
        switch (iCountValueLeft)
        {
            case 1:
                miniCardRight[0].DOMove(targetRight.position, fMoveTime, false);//.OnComplete(Validation); //Card moves to center
                cardNumRight = 0;
                miniCardLeft[0].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck); //Card moves to center
                cardNumLeft = 0;
                //Verify who wins - Check both cards script to see who wins, change OnComplete method depending on the result
               //Check if it's done and call a method - call in the end to disable the card
                break;
            case 2:
                miniCardRight[1].DOMove(targetRight.position, fMoveTime, false);//.OnComplete(Validation);
                cardNumRight = 1;
                miniCardLeft[1].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck); //Card moves to center
                cardNumLeft = 1;
                break;
            case 3:
                miniCardRight[2].DOMove(targetRight.position, fMoveTime, false);//.OnComplete(Validation);
                cardNumRight = 2;
                miniCardLeft[2].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck); //Card moves to center
                cardNumLeft = 2;
                break;
            case 4:
                miniCardRight[3].DOMove(targetRight.position, fMoveTime, false);//.OnComplete(Validation);
                cardNumRight = 3;
                miniCardLeft[3].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck); //Card moves to center
                cardNumLeft = 3;
                break;
            case 5:
                miniCardRight[4].DOMove(targetRight.position, fMoveTime, false);//.OnComplete(Validation);
                cardNumRight = 4;
                miniCardLeft[4].DOMove(targetLeft.position, fMoveTime, false).OnComplete(OnLeftStatusCheck); //Card moves to center
                cardNumLeft = 4;
                break;
        }
        
    } 
}
