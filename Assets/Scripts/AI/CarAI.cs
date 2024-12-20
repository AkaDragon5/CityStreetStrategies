using System;
using System.Collections;
using System.Collections.Generic;
using SimpleCity.AI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CarAI : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> path = null;
    [SerializeField]
    private float arriveDistance = .3f, lastPointArriveDistance = .1f;
    [SerializeField]
    private float turningAngleOffset = 5;
    [SerializeField]
    private Vector3 currentTargetPosition;

    [SerializeField]
    private GameObject raycastStartingPoint = null;
    [SerializeField]
    private float collisionRaycastLength = 0.1f;

    internal bool IsThisLastPathIndex()
    {
        return index >= path.Count - 1;
    }

    private int index = 0;

    private bool stop;
    private bool collisionStop = false;

    private float timer;


    public bool Stop
    {
        get { return stop || collisionStop; }
        set { stop = value; }
    }

    [field: SerializeField]
    public UnityEvent<Vector2> OnDrive { get; set; }

    public CarAI car;
    public int corsia;
    public GameObject aiDirector;
    public GameObject gameManager;


    private void Start()
    {

        car = GetComponent<CarAI>();
        aiDirector = GameObject.Find("AiDirector");
        gameManager = GameObject.Find("GameManager");

        if (path == null || path.Count == 0)
        {
            Stop = true;
        }
        else
        {
            currentTargetPosition = path[index];
        }
    }

    public void SetPath(List<Vector3> path)
    {
        if (path.Count == 0)
        {
            Destroy(gameObject);
            return;
        }
        this.path = path;
        index = 0;
        currentTargetPosition = this.path[index];

        Vector3 relativepoint = transform.InverseTransformPoint(this.path[index + 1]);

        float angle = Mathf.Atan2(relativepoint.x, relativepoint.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, angle, 0);
        Stop = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (gameManager.GetComponent<GameManager>().end)
        {
            aiDirector.GetComponent<AiDirector>().cars--;
            Destroy(gameObject);
        }
        CheckIfArrived();
        Drive();
        CheckForCollisions();
    }

    private void CheckForCollisions()
    {
        if (Physics.Raycast(raycastStartingPoint.transform.position, transform.forward, collisionRaycastLength, 1 << gameObject.layer))
        {
            collisionStop = true;
        }
        else
        {
            collisionStop = false;
        }


 


    }

    private void Drive()
    {
        if (Stop)
        {
            OnDrive?.Invoke(Vector2.zero);
        }
        else
        {
            Vector3 relativepoint = transform.InverseTransformPoint(currentTargetPosition);
            float angle = Mathf.Atan2(relativepoint.x, relativepoint.z) * Mathf.Rad2Deg;
            var rotateCar = 0;
            if (angle > turningAngleOffset)
            {
                rotateCar = 1;
            }
            else if (angle < -turningAngleOffset)
            {
                rotateCar = -1;
            }
            OnDrive?.Invoke(new Vector2(rotateCar, 1));
        }
    }

    private void CheckIfArrived()
    {
        if (Stop == false)
        {
            var distanceToCheck = arriveDistance;
            if (index == path.Count - 1)
            {
                distanceToCheck = lastPointArriveDistance;
            }
            if (Vector3.Distance(currentTargetPosition, transform.position) < distanceToCheck)
            {
                SetNextTargetIndex();
            }
        }
    }

    private void SetNextTargetIndex()
    {
        index++;
        if (index >= path.Count)
        {
            aiDirector.GetComponent<AiDirector>().cars--;
            gameManager.GetComponent<GameManager>().carsArrived++;
            gameManager.GetComponent<GameManager>().carsTimes += timer;
            Stop = true;
            Destroy(gameObject);

        }
        else
        {
            currentTargetPosition = path[index];
        }
    }


    private void OnTriggerStay(Collider other)
    {



        if (other.CompareTag("LightA"))
        {
            TrafficLightManager trafficlightmanager = other.GetComponentInParent<TrafficLightManager>();


            switch (other.GetComponent<LightControl>().lightColor)
            {
                case LightControl.LightColor.Red:
                    stop = true;
                    break;

                case LightControl.LightColor.Yellow:
                    if (trafficlightmanager.goYellow)
                    {

                        stop = false;

                    }
                    else
                    {
                        stop = true;
                    }

                    break;
                case LightControl.LightColor.Green:

                    stop = false;


                    break;
            }





        }
        if (other.CompareTag("LightB"))
        {

            TrafficLightManager trafficlightmanager = other.GetComponentInParent<TrafficLightManager>();

            switch (other.GetComponent<LightControl>().lightColor)
            {
                case LightControl.LightColor.Red:
                    stop = true;
                    break;

                case LightControl.LightColor.Yellow:
                    if (trafficlightmanager.goYellow)
                    {

                        stop = false;

                    }
                    else
                    {
                        stop = true;
                    }
                    break;
                case LightControl.LightColor.Green:
                    if (!collisionStop)
                    {
                        stop = false;
                    }
                    break;
            }



        }
        if (other.CompareTag("LightC"))
        {
            TrafficLightManager trafficlightmanager = other.GetComponentInParent<TrafficLightManager>();
            switch (other.GetComponent<LightControl>().lightColor)
            {
                case LightControl.LightColor.Red:
                    stop = true;
                    break;

                case LightControl.LightColor.Yellow:
                    if (trafficlightmanager.goYellow)
                    {

                        stop = false;

                    }
                    else
                    {
                        stop = true;
                    }
                    break;
                case LightControl.LightColor.Green:
                    if (!collisionStop)
                    {
                        stop = false;
                    }
                    break;
            }

        }
        if (other.CompareTag("LightD"))
        {
            TrafficLightManager trafficlightmanager = other.GetComponentInParent<TrafficLightManager>();
            switch (other.GetComponent<LightControl>().lightColor)
            {
                case LightControl.LightColor.Red:
                    stop = true;
                    break;

                case LightControl.LightColor.Yellow:
                    if (trafficlightmanager.goYellow)
                    {

                        stop = false;

                    }
                    else
                    {
                        stop = true;
                    }
                    break;
                case LightControl.LightColor.Green:
                    if (!collisionStop)
                    {
                        stop = false;
                    }
                    break;
            }

        }


    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Corsia1"))
        {
            corsia = 1;
            other.GetComponentInParent<SmartRoad>().InQueue1(car);

        }
        else if (other.CompareTag("Corsia2"))
        {
            corsia = 2;
            other.GetComponentInParent<SmartRoad>().InQueue2(car);

        }
    }


}
