using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicGenerator : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject canvas;
    public GameObject startPointPrefab;
    public GameObject endPointPrefab;
    public List<GameObject> LogicGatePrefabs;
    private List<int> steps = new List<int> {1, 2, 4, 8, 16};
    private List<float> yStart = new List<float> { 0.5f, 1, 2, 4, 8 };
    public GameObject endPoint;

    public void CreateLogic(int level)
    {
        if (canvas.transform.childCount != 0)
            Destroy(canvas.transform.GetChild(0).gameObject);
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(1904, 12000);
        endPoint = Instantiate(endPointPrefab, canvas.transform);
        LogicEndpoint endPointScript = endPoint.GetComponent<LogicEndpoint>();
        endPointScript.parentNode = null;
        endPointScript.currentToggle = endPoint.GetComponent<Toggle>();
        endPointScript.SetLogicGenerator(this);
        endPointScript.transform.localPosition = new Vector3((level * 250) / 2, 0, 0);

        //Call CreateRecursiveLogic Method with endPoint, level, and height = -1
        CreateRecursiveLogic(endPointScript, level, -1);
    }

    public void CreateRecursiveLogic(LogicNode parentNode, int level, int height)
    {
        //If level = 1 then
        //Create startPoint at position height and return
        Toggle toggle;

        if (level == 1)
        {
            GameObject startPoint = Instantiate(startPointPrefab, parentNode.gameObject.transform);
            LogicStartpoint startPointScript = startPoint.GetComponent<LogicStartpoint>();
            startPointScript.parentNode = parentNode;
            toggle = startPoint.GetComponent<Toggle>();
            startPointScript.SetRandomToggle(toggle);
            parentNode.AddInput(toggle);

            //Set Position to the left if height = -1 (first node)
            if (height == -1)
                startPoint.transform.localPosition = new Vector3(-250, 0, 0);
            else
            {
                Debug.Log("Level = " + level + ", y = " + (yStart[level - 1] - steps[level - 1]) + "\nyStart = " + (yStart[level - 1]) + ", step = " + (steps[level - 1])); startPoint.transform.localPosition = new Vector3(-250, (yStart[level - 1] - (steps[level - 1] * height)) * 100, 0);
            }
            return;
        }

        //Create gate node at position height, then set parent as parentNode.
        GameObject currentPrefab;
        if (height == -1)
            currentPrefab = LogicGatePrefabs[Random.Range(0, 2)];
        else
            currentPrefab = LogicGatePrefabs[Random.Range(0, 2)];
        GameObject gateNode = Instantiate(currentPrefab, parentNode.gameObject.transform);
      

        Debug.Log(currentPrefab.name);
        LogicNode gateNodeScript = gateNode.GetComponent(currentPrefab.name) as LogicNode;
        Debug.Log(gateNodeScript);
        
        gateNodeScript.parentNode = parentNode;
        toggle = gateNode.GetComponent<Toggle>();
        gateNodeScript.currentToggle = toggle;
        parentNode.AddInput(toggle);

        //Set Position to the left if height = -1 (first node)
        if (height == -1)
            gateNode.transform.localPosition = new Vector3(-250, 0, 0);
        else
        {
            Debug.Log("Level = " + level + ", y = " + (yStart[level - 1] - steps[level - 1]) + "\nyStart = " + (yStart[level - 1]) + ", step = " + (steps[level - 1]));
            gateNode.transform.localPosition = new Vector3(-250, (yStart[level - 1] - (steps[level - 1] * height)) * 100, 0);
        }

        //For every i in level
        //Call CreateRecursiveLogic Method with new gate node, level - 1, and height = i.

        for (int i = 0; i < 2; i++)
            CreateRecursiveLogic(gateNodeScript, level - 1, i);
    }

    public void LogicComplete()
    {
        Debug.Log("Logic Complete");
        Destroy(endPoint);
        playerController.LogicComplete();
    }    
}
