using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrains : MonoBehaviour
{
    Vector3 startPosTrain1, endPosTrain1;
    Vector3 startPosTrain2, endPosTrain2;
    Vector3 startPosTrain3, endPosTrain3;
    Vector3 startPosTrain4, endPosTrain4;
    Vector3 startPosTrain5, endPosTrain5;
    public GameObject train1;
    public GameObject train2;
    public GameObject train3;
    public GameObject train4;
    public GameObject train5;
    float speed1 = 40, speed2 = 70, speed3 = 25, speed4 = 30;
    bool returnTrain1 = false, returnTrain2 = false, returnTrain3 = false, returnTrain4 = false, returnTrain5 = false;
    public static MovingTrains instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        TrainLocation();
    }

    // Update is called once per frame
    void Update()
    {
       if (GameManager.instance.gameStarted == true && GameManager.instance.gameEnded == false )
        {
            MovTrains();
        }  
    }

    public void TrainLocation()
    {
        //Tren 1
        startPosTrain1 = new Vector3(-9.3f, 8.75f, -40f);
        endPosTrain1 = new Vector3(-9.3f, 8.75f, 2370);
        //Tren 2
        startPosTrain2 = new Vector3(9.6f, 9.17f, -144f);
        endPosTrain2 = new Vector3(9.6f, 9.17f, 2370f);
        //Tren 3
        startPosTrain3 = new Vector3(7.9f, 7.3f, 2706f);
        endPosTrain3 = new Vector3(7.9f, 7.3f, -130f);
        //Tren 4
        startPosTrain4 = new Vector3(45.3f, 6.4f, 791.7f);
        endPosTrain4 = new Vector3(-60f, 6.4f, 791.7f);
        //Tren 5
        startPosTrain5 = new Vector3(30f, 7.4f, 1977f);
        endPosTrain5 = new Vector3(-70f, 7.4f, 1977f);
    }

    public void MovTrains()
    {
            //Tren 1
            if (CompareTag("train_moving_1") && returnTrain1 == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosTrain1, (speed1 * Time.deltaTime));
                if (transform.position == endPosTrain1)
                {
                    returnTrain1 = true;
                }
            }
            if (CompareTag("train_moving_1") && returnTrain1 == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosTrain1, (speed1 * Time.deltaTime));
                if (transform.position == startPosTrain1)
                {
                    returnTrain1 = false;
                }
            }

            //Tren 2
            if (CompareTag("train_moving_2") && returnTrain2 == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosTrain2, (speed2 * Time.deltaTime));
                if (transform.position == endPosTrain2)
                {
                    returnTrain2 = true;
                }
            }
            if (CompareTag("train_moving_2") && returnTrain2 == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosTrain2, (speed2 * Time.deltaTime));
                if (transform.position == startPosTrain2)
                {
                    returnTrain2 = false;
                }
            }

            //Tren 3
            if (CompareTag("train_moving_3") && returnTrain3 == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosTrain3, (speed3 * Time.deltaTime));
                if (transform.position == endPosTrain3)
                {
                    returnTrain3 = true;
                }
            }
            if (CompareTag("train_moving_3") && returnTrain3 == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosTrain3, (speed3 * Time.deltaTime));
                if (transform.position == startPosTrain3)
                {
                    returnTrain3 = false;
                }
            }

            //Tren 4
            if (CompareTag("train_moving_4") && returnTrain4 == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosTrain4, (speed4 * Time.deltaTime));
                if (transform.position == endPosTrain4)
                {
                    returnTrain4 = true;
                }
            }
            if (CompareTag("train_moving_4") && returnTrain4 == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosTrain4, (speed4 * Time.deltaTime));
                if (transform.position == startPosTrain4)
                {
                    returnTrain4 = false;
                }
            }

            //Tren 5
            if (CompareTag("train_moving_5") && returnTrain5 == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosTrain5, (speed4 * Time.deltaTime));
                if (transform.position == endPosTrain5)
                {
                    returnTrain5 = true;
                }
            }
            if (CompareTag("train_moving_5") && returnTrain5 == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosTrain5, (speed4 * Time.deltaTime));
                if (transform.position == startPosTrain5)
                {
                    returnTrain5 = false;
                }
            }
        }  
    }

