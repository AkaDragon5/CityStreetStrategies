using System;
using System.Collections;
using System.Collections.Generic;
using SimpleCity.AI;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Action OnRoadPlacement, OnHousePlacement, OnSpecialPlacement, OnBigStructurePlacement;
    public Button placeRoadButton, placeHouseButton, placeSpecialButton, placeBigStructureButton;
    public GameObject panel, startSimulation, endSimulation, panelData, spawnAgents, spawnCar, panelResult, exitButton;
    public Color outlineColor;

    public Text countAgent, countCar, carsResult, agentsResult;
    List<Button> buttonList;

    public GameObject aiDirector;

    private void Start()
    {
        aiDirector = GameObject.Find("AiDirector");
        buttonList = new List<Button> { placeHouseButton, placeRoadButton, placeSpecialButton, placeBigStructureButton };

        placeRoadButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeRoadButton);
            OnRoadPlacement?.Invoke();

        });
        placeHouseButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeHouseButton);
            OnHousePlacement?.Invoke();

        });
        placeSpecialButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeSpecialButton);
            OnSpecialPlacement?.Invoke();

        });
        placeBigStructureButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeBigStructureButton);
            OnBigStructurePlacement?.Invoke();

        });
    }

    private void Update()
    {
        countCar.text = aiDirector.GetComponent<AiDirector>().cars.ToString();
        countAgent.text = aiDirector.GetComponent<AiDirector>().agents.ToString();

    }

    private void ModifyOutline(Button button)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    public void ResetButtonColor()
    {
        foreach (Button button in buttonList)
        {
            button.GetComponent<Outline>().enabled = false;
        }
    }
}
