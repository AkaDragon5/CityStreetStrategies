using SimpleCity.AI;
using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraMovement cameraMovement;
    public RoadManager roadManager;
    public InputManager inputManager;

    public UIController uiController;

    public StructureManager structureManager;

    public ObjectDetector objectDetector;

    public PathVisualizer pathVisualizer;

    public bool end;
    public float carsArrived;
    public float carsTimes;
    public float agentsArrived;
    public float agentsTimes;
    private float carsAverangeTime = 0f;
    private float agentsAverangeTime = 0f;




    void Start()
    {
        end = false;
        uiController.OnRoadPlacement += RoadPlacementHandler;
        uiController.OnHousePlacement += HousePlacementHandler;
        uiController.OnSpecialPlacement += SpecialPlacementHandler;
        uiController.OnBigStructurePlacement += BigStructurePlacement;
        inputManager.OnEscape += HandleEscape;
    }

    private void HandleEscape()
    {
        ClearInputActions();
        uiController.ResetButtonColor();
        pathVisualizer.ResetPath();
        inputManager.OnMouseClick += TrySelectingAgent;

    }



    private void TrySelectingAgent(Ray ray)
    {
        GameObject hitObject = objectDetector.RaycastAll(ray);
        if (hitObject != null)
        {
            var agentScript = hitObject.GetComponent<AiAgent>();
            agentScript?.ShowPath();
        }
    }

    private void BigStructurePlacement()
    {
        ClearInputActions();

        inputManager.OnMouseClick += (pos) =>
        {
            ProcessInputAndCall(structureManager.PlaceBigStructure, pos);
        };
        inputManager.OnEscape += HandleEscape;
    }

    private void SpecialPlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseClick += (pos) =>
        {
            ProcessInputAndCall(structureManager.PlaceSpecial, pos);
        };
        inputManager.OnEscape += HandleEscape;
    }

    private void HousePlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseClick += (pos) =>
        {
            ProcessInputAndCall(structureManager.PlaceHouse, pos);
        };
        inputManager.OnEscape += HandleEscape;
    }

    private void RoadPlacementHandler()
    {
        ClearInputActions();

        inputManager.OnMouseClick += (pos) =>
        {
            ProcessInputAndCall(roadManager.PlaceRoad, pos);
        };
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
        inputManager.OnMouseHold += (pos) =>
        {
            ProcessInputAndCall(roadManager.PlaceRoad, pos);
        };
        inputManager.OnEscape += HandleEscape;
    }

    private void ClearInputActions()
    {
        inputManager.ClearEvents();
    }

    private void ProcessInputAndCall(Action<Vector3Int> callback, Ray ray)
    {
        Vector3Int? result = objectDetector.RaycastGround(ray);
        if (result.HasValue)
            callback.Invoke(result.Value);
    }



    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x, 0, inputManager.CameraMovementVector.y));
    }

    public void StartSimulation()
    {
        carsAverangeTime = 0f;
        agentsAverangeTime = 0f;
        agentsArrived = 0;
        agentsTimes = 0;
        carsArrived = 0;
        carsTimes = 0;
        uiController.carsResult.text = "";
        uiController.agentsResult.text = "";
        end = false;
        uiController.GetComponent<Timer>().start = true;

        ClearInputActions();
        uiController.ResetButtonColor();

        uiController.panel.SetActive(false);
        uiController.startSimulation.SetActive(false);
        uiController.exitButton.SetActive(false);
        uiController.endSimulation.SetActive(true);
        uiController.panelData.SetActive(true);
        uiController.spawnAgents.SetActive(true);
        uiController.spawnCar.SetActive(true);


    }

    public void EndSimulation()
    {
        end = true;
        uiController.GetComponent<Timer>().start = false;
        uiController.GetComponent<Timer>().ResetTimer();
        uiController.endSimulation.SetActive(false);
        uiController.panelResult.SetActive(true);
        uiController.panelData.SetActive(false);
        uiController.spawnAgents.SetActive(false);
        uiController.spawnCar.SetActive(false);

        carsAverangeTime = carsTimes / carsArrived;
        carsAverangeTime = (float)Math.Round(carsAverangeTime, 2);
        if (!carsAverangeTime.Equals(float.NaN))
        {
            uiController.carsResult.text = "Average arrival time of cars:  " + carsAverangeTime + " s";

        }
        else
        {
            uiController.carsResult.text = "Average arrival time of cars: no result";
        }


        agentsAverangeTime = agentsTimes / agentsArrived;
        agentsAverangeTime = (float)Math.Round(agentsAverangeTime, 2);
        if (!agentsAverangeTime.Equals(float.NaN))
        {
            uiController.agentsResult.text = "Average arrival time of pedestrian: " + agentsAverangeTime + " s";

        }
        else
        {
            uiController.agentsResult.text = "Average arrival time of pedestrian: no result";
        }






    }

    public void ExitGame()
    {
        Application.Quit();
    }


    public void CloseResult()
    {
        uiController.panelResult.SetActive(false);
        uiController.panel.SetActive(true);
        uiController.startSimulation.SetActive(true);
        uiController.exitButton.SetActive(true);

    }
}
