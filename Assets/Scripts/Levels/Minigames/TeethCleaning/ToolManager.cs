using UnityEngine;
using System.Collections.Generic;

public class ToolManager : MonoBehaviour
{
    
    [Header("Tool GameObjects")]
    
    public GameObject toothbrushToolObject;
    public GameObject pickToolObject;
    public GameObject drillToolObject;

    
    public static string ActiveToolName { get; private set; } 
    private const string ToothbrushName = "Toothbrush";
    private const string PickName = "Pick";
    private const string DrillName = "Drill";

     
    private Dictionary<string, GameObject> toolObjects;


    void Awake()
    {
   
        toolObjects = new Dictionary<string, GameObject>
        {
            { ToothbrushName, toothbrushToolObject },
            { PickName, pickToolObject },
            { DrillName, drillToolObject }
        };

        
        ActiveToolName = ToothbrushName;
    
        SetActiveTool(ActiveToolName); 
    }

  
    public static void SetActiveTool(string toolName)
    {
    
        ToolManager instance = FindObjectOfType<ToolManager>();
        if (instance == null) return; 

        if (ActiveToolName != toolName)
        {
            
            if (instance.toolObjects.ContainsKey(ActiveToolName))
            {
                instance.toolObjects[ActiveToolName].SetActive(false);
            }

            
            ActiveToolName = toolName;

            
            if (instance.toolObjects.ContainsKey(ActiveToolName))
            {
                instance.toolObjects[ActiveToolName].SetActive(true);
            }
            
            Debug.Log("Tool switched to: " + ActiveToolName);
        } 
        else if (!instance.toolObjects[toolName].activeInHierarchy)
        {
            
             instance.toolObjects[toolName].SetActive(true);
        }
    }

    
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            SetActiveTool(ToothbrushName); 
        }

         
        else if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            SetActiveTool(PickName); 
        }

        
        else if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            SetActiveTool(DrillName); 
        }
    }
}