using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmartRoad : MonoBehaviour
{
    Queue<CarAI> trafficQueue1 = new Queue<CarAI>();
    Queue<CarAI> trafficQueue2 = new Queue<CarAI>();
    Queue<CarAI> trafficQueue = new Queue<CarAI>();
    public CarAI currentCar1;
    public CarAI currentCar2;
    public CarAI currentCar;




    [SerializeField]
    private bool pedestrianWaiting = false, pedestrianWalking = false;

    [field: SerializeField]
    public UnityEvent OnPedestrianCanWalk { get; set; }
    [SerializeField] public bool isFourWay;

    private void OnTriggerEnter(Collider other)
    {
        if (!isFourWay)
        {
            if (other.CompareTag("Car"))
            {
                var car = other.GetComponent<CarAI>();

                if (car != null && car != currentCar && car.IsThisLastPathIndex() == false)
                {
                    trafficQueue.Enqueue(car);
                    car.Stop = true;
                }

            }
        }


    }

    public void InQueue1(CarAI car)
    {
        if (currentCar1 == null)
        {
            currentCar1 = car;
        }
        if (car != null && car != currentCar1 && car.IsThisLastPathIndex() == false)
        {
            trafficQueue1.Enqueue(car);
            car.Stop = true;
        }
    }

    public void InQueue2(CarAI car)
    {
        if (currentCar2 == null)
        {
            currentCar2 = car;
        }
        if (car != null && car != currentCar2 && car.IsThisLastPathIndex() == false)
        {
            trafficQueue2.Enqueue(car);
            car.Stop = true;
        }
    }

    private void Update()
    {
        if (!isFourWay)
        {
            if (currentCar == null)
            {

                if (trafficQueue.Count > 0 && pedestrianWaiting == false && pedestrianWalking == false)
                {
                    currentCar = trafficQueue.Dequeue();
                    currentCar.Stop = false;
                }
                else if (pedestrianWalking || pedestrianWaiting)
                {
                    OnPedestrianCanWalk?.Invoke();
                    pedestrianWalking = true;
                }
            }
        }
        else
        {
            if (currentCar1 == null)
            {

                if (trafficQueue1.Count > 0 && pedestrianWaiting == false && pedestrianWalking == false)
                {
                    currentCar1 = trafficQueue1.Dequeue();
                    currentCar1.Stop = false;
                }
                else if (pedestrianWalking || pedestrianWaiting)
                {
                    OnPedestrianCanWalk?.Invoke();
                    pedestrianWalking = true;
                }
            }

            if (currentCar2 == null)
            {

                if (trafficQueue2.Count > 0 && pedestrianWaiting == false && pedestrianWalking == false)
                {
                    currentCar2 = trafficQueue2.Dequeue();
                    currentCar2.Stop = false;
                }
                else if (pedestrianWalking || pedestrianWaiting)
                {
                    OnPedestrianCanWalk?.Invoke();
                    pedestrianWalking = true;
                }
            }

        }







    }

    private void OnTriggerExit(Collider other)
    {


        if (other.CompareTag("Car"))
        {

            var car = other.GetComponent<CarAI>();

            if (car != null)
            {
                if (!isFourWay)
                {
                    RemoveCar(car);
                }
                else
                {
                    if (car.corsia == 1)
                    {
                        RemoveCar1(car);
                    }

                    if (car.corsia == 2)
                    {
                        RemoveCar2(car);
                    }
                }

            }
        }


    }

    private void RemoveCar(CarAI car)
    {
        if (car == currentCar)
        {
            currentCar = null;
        }

    }

    private void RemoveCar1(CarAI car)
    {
        if (car == currentCar1)
        {
            currentCar1 = null;
        }

    }


    private void RemoveCar2(CarAI car)
    {
        if (car == currentCar2)
        {
            currentCar2 = null;
        }

    }


    public void SetPedestrianFlag(bool val)
    {
        if (val)
        {
            pedestrianWaiting = true;
        }
        else
        {
            pedestrianWaiting = false;
            pedestrianWalking = false;
        }
    }
}
