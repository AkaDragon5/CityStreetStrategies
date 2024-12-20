using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TrafficLightManager : MonoBehaviour
{

    public GameObject lightA, lightB, lightC, lightD;
    public Material redOn, greenOn, yellowOn, redOf, greenOf, yellowOf;
    public int greenTime;
    public int redTime;
    public int yellowTime;

    public bool goYellow;

    void Start()
    {


        Material[] materialsA = new Material[lightA.GetComponent<MeshRenderer>().materials.Length];
        materialsA = lightA.GetComponent<MeshRenderer>().materials;
        materialsA[0] = greenOn;
        lightA.GetComponent<MeshRenderer>().materials = materialsA;

        Material[] materialsB = new Material[lightB.GetComponent<MeshRenderer>().materials.Length];
        materialsB = lightB.GetComponent<MeshRenderer>().materials;
        materialsB[0] = greenOn;
        lightB.GetComponent<MeshRenderer>().materials = materialsB;

        Material[] materialsC = new Material[lightC.GetComponent<MeshRenderer>().materials.Length];
        materialsC = lightC.GetComponent<MeshRenderer>().materials;
        materialsC[3] = redOn;
        lightC.GetComponent<MeshRenderer>().materials = materialsC;

        Material[] materialsD = new Material[lightD.GetComponent<MeshRenderer>().materials.Length];
        materialsD = lightD.GetComponent<MeshRenderer>().materials;
        materialsD[3] = redOn;
        lightD.GetComponent<MeshRenderer>().materials = materialsD;

        CountLightsAB(lightA);
        CountLightsAB(lightB);
        CountLightsCD(lightC);
        CountLightsCD(lightD);

    }

    IEnumerator GreenLight(GameObject light)
    {
        light.GetComponentInChildren<LightControl>().lightColor = LightControl.LightColor.Green;
        yield return new WaitForSeconds(greenTime);
        Material[] materials = new Material[light.GetComponent<MeshRenderer>().materials.Length];
        materials = light.GetComponent<MeshRenderer>().materials;
        materials[0] = greenOf;
        materials[2] = yellowOn;
        light.GetComponent<MeshRenderer>().materials = materials;
        StartCoroutine(YellowLight(light));


    }

    IEnumerator YellowLight(GameObject light)
    {
        light.GetComponentInChildren<LightControl>().lightColor = LightControl.LightColor.Yellow;
        goYellow = true;
        yield return new WaitForSeconds(yellowTime - 3);
        goYellow = false;
        yield return new WaitForSeconds(yellowTime - 2);
        Material[] materials = new Material[light.GetComponent<MeshRenderer>().materials.Length];
        materials = light.GetComponent<MeshRenderer>().materials;
        materials[3] = redOn;
        materials[2] = yellowOf;
        light.GetComponent<MeshRenderer>().materials = materials;
        StartCoroutine(RedLight(light));


    }

    IEnumerator RedLight(GameObject light)
    {
        light.GetComponentInChildren<LightControl>().lightColor = LightControl.LightColor.Red;
        yield return new WaitForSeconds(redTime);
        Material[] materials = new Material[light.GetComponent<MeshRenderer>().materials.Length];
        materials = light.GetComponent<MeshRenderer>().materials;
        materials[3] = redOf;
        materials[0] = greenOn;
        light.GetComponent<MeshRenderer>().materials = materials;
        StartCoroutine(GreenLight(light));


    }

    public void CountLightsAB(GameObject light)
    {
        StartCoroutine(GreenLight(light));

    }

    public void CountLightsCD(GameObject light)
    {
        StartCoroutine(RedLight(light));

    }







}
