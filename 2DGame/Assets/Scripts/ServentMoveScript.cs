﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ServentMoveScript : MonoBehaviour
{
    Rigidbody2D rigidBody2D;

    //Text Variebles
    public GameObject textBox;
    private TextScript textObject;

    //Movement Sequence
    List<Vector2> servantCallMovement = new List<Vector2>{
        new Vector2(0f, -5f),
        new Vector2(8f, 0f),
        new Vector2(0f, -0.8f)
    };

    List<Vector2> servantToDoorMovement = new List<Vector2>{
        new Vector2(0f, 0.8f),
        new Vector2(-8f, 0f),
        new Vector2(0f, 5f)
    };
    //previosPosition is use to calculate the distance traveled
    Vector2 previosPosition;
    int currentMoveSequence = 0;

    //Moveing speed variable
    float speed = 3f;
    //Moving variable
    bool moving = false;
    //Initialize variable
    bool initialize = true;
    //the position it is aiming to get
    Vector2 currentTargetPoint;
    //a rounded float position of this rigidbody
    Vector2 RigidbodyPosition;
    //the moving direction that changes depending on the current moveSequence
    Vector2 currentMoveDirection;

    //Current Text Displayed
    string currentText;
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        textObject = GetComponent<TextScript>();
        //initialize for the first movement
        previosPosition = rigidBody2D.position;
    }

    // Update is called once per frame
    void Update()
    {
        //update currentText
        currentText = textBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;

        //Conditions for moving
        if (currentText == "Servant!" || moving)
        {
            Debug.Log("ONE" + moving + currentText);
            NPCMovement(servantCallMovement);
        }

        else if (currentText == "KING: What is going on? I wonder..." || moving)
        {
            Debug.Log("TWO");
            NPCMovement(servantToDoorMovement);
        }
    }

    //Moving function
    public void NPCMovement(List<Vector2> moveArray)
    {
        currentTargetPoint = new Vector2(Mathf.Round((previosPosition.x + moveArray[currentMoveSequence].x) * 10f) / 10f, Mathf.Round((previosPosition.y + moveArray[currentMoveSequence].y) * 10f) / 10f);
        RigidbodyPosition = new Vector2(Mathf.Round(rigidBody2D.position.x * 10f) / 10f, Mathf.Round(rigidBody2D.position.y * 10f) / 10f);

        if (initialize) 
        {
            moving = true;
            if (RigidbodyPosition.x - currentTargetPoint.x < 0 && currentTargetPoint.x != 0f)
            {
                currentMoveDirection += new Vector2(1f, 0f);
            }
            else if (RigidbodyPosition.x - currentTargetPoint.x != 0)
            {
                currentMoveDirection += new Vector2(-1f, 0f);
            }

            if (RigidbodyPosition.y - currentTargetPoint.y < 0 && currentTargetPoint.y != 0f)
            {
                currentMoveDirection += new Vector2(0f, 1f);
            }
            else if (RigidbodyPosition.y - currentTargetPoint.y != 0)
            {
                currentMoveDirection += new Vector2(0f, -1f);
            }
            initialize = false;
        }

        //Check if equals CURRENT target point
        if (RigidbodyPosition == currentTargetPoint)
        {
            //Check if it's NOT last movement
            if (currentMoveSequence != moveArray.Count - 1)
            {
                currentMoveSequence++;
                previosPosition = rigidBody2D.position;
                currentMoveDirection = new Vector2(0f, 0f);
                currentTargetPoint = new Vector2(Mathf.Round((previosPosition.x + moveArray[currentMoveSequence].x) * 10f) / 10f, Mathf.Round((previosPosition.y + moveArray[currentMoveSequence].y) * 10f) / 10f);
                RigidbodyPosition = new Vector2(Mathf.Round(rigidBody2D.position.x * 10f) / 10f, Mathf.Round(rigidBody2D.position.y * 10f) / 10f);
                if (RigidbodyPosition.x - currentTargetPoint.x < 0 && currentTargetPoint.x != 0f)
                {
                    currentMoveDirection += new Vector2(1f, 0f);
                }
                else if (RigidbodyPosition.x - currentTargetPoint.x != 0)
                {
                    currentMoveDirection += new Vector2(-1f, 0f);
                }

                if (RigidbodyPosition.y - currentTargetPoint.y < 0 && currentTargetPoint.y != 0f)
                {
                    currentMoveDirection += new Vector2(0f, 1f);
                }
                else if (RigidbodyPosition.y - currentTargetPoint.y != 0)
                {
                    currentMoveDirection += new Vector2(0f, -1f);
                }
            }
            //If reached final destination
            else
            {
                rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                previosPosition = rigidBody2D.position;
                moving = false;
            }
        }

        //Moving!
        else
        {
            rigidBody2D.MovePosition(rigidBody2D.position + currentMoveDirection * speed * Time.deltaTime);
        }
    }
}
