using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OnReceivedList : MonoBehaviour {
    //private float Scale ;
    //private float ToolX ;
    //private float ToolY ;
    //private float ToolZ ; 
    [HideInInspector]
    public int PartsCount1 = 0;
    [HideInInspector]
    public int PartsCount2 = 0;
    [HideInInspector]
    public int PartsCount3 = 0;
    public static OnReceivedList Instance = null;

    [HideInInspector]
    public List<GameObject> PartsList = null;
    /// <summary>
    /// 所有零件
    /// </summary>
    public static List<GameObject> PartsList1 = new List<GameObject>();
    public static List<GameObject> PartsList2 = new List<GameObject>();
    public static List<GameObject> PartsList3 = new List<GameObject>();
    public static List<GameObject> ToolsList1 = new List<GameObject>();
    public static List<GameObject> ToolsList2 = new List<GameObject>();
    public static List<GameObject> ToolsList3 = new List<GameObject>();

    public static List<GameObject> PartsImage1 = new List<GameObject>();
    public static List<GameObject> PartsImage2 = new List<GameObject>();
    public static List<GameObject> PartsImage3 = new List<GameObject>();

    [HideInInspector]
    public static float PartScale = 1;
    public static Vector3 PartLocalScale;

    private static float PartX = 1;
    private static float PartY = 1;
    private static float PartZ = 1;

    private int PartIndex = 0;
    private int PartIndex1 = 0;
    private int PartIndex2 = 0;
    private int PartIndex3 = 0;

    private GameObject PartInfo1;
    private GameObject PartInfo2;
    private GameObject PartInfo3;
    private GameObject PartInfo;

    private List<GameObject> Parts =new List<GameObject>();

    //private GameObject[] Tools1;
    //private GameObject[] Tools2;
    //private GameObject[] Tools3;
    //private GameObject ToolInfo1;
    //private GameObject ToolInfo2;
    //private GameObject ToolInfo3;
    //public float[] Scale_xyz = new float[3];

    void Awake()
    {
        Instance = this;
        
    }
    // Use this for initialization
    void Start()
    {

        GameObject allImage = GameObject.Find("Part_Panel_1/Part_1").gameObject;
        Debug.Log(allImage.transform.childCount);
        foreach (Transform PartTran in allImage.transform)
        {
            Debug.Log(PartTran.gameObject.name);
            PartsImage1.Add(PartTran.gameObject);
            Debug.Log(PartTran.gameObject.name + "_two");
        }

        //GameObject allImage2 = GameObject.Find("Part_Panel_2/Part_2").gameObject;
        //Debug.Log(allImage2.transform.childCount);
        //foreach (Transform PartTran2 in allImage2.transform)
        //{
        //    Debug.Log(PartTran2.gameObject.name);
        //    PartsImage2.Add(PartTran2.gameObject);
        //    Debug.Log(PartTran2.gameObject.name + "_2");
        //}

        //GameObject allImage3 = GameObject.Find("Part_Panel_3/Part_3").gameObject;
        //Debug.Log(allImage3.transform.childCount);
        //foreach (Transform PartTran3 in allImage3.transform)
        //{
        //    Debug.Log(PartTran3.gameObject.name);
        //    PartsImage3.Add(PartTran3.gameObject);
        //    Debug.Log(PartTran3.gameObject.name + "_3");
        //}

        GameObject allMods = GameObject.Find("Models/LCD1").gameObject;
        foreach (Transform curObj in allMods.transform)
        {
            Debug.Log(curObj.gameObject.name);
            Parts.Add(curObj.gameObject);
            Debug.Log(curObj.gameObject.name+"_two");
        }

        if (PartsList.Count == 0)
        {
            for (PartIndex = 0; PartIndex < Parts.Count; PartIndex++)
            {
                PartInfo = Parts[PartIndex].gameObject;
                PartsList.Add(PartInfo);
            }
        }
        

        //OnReceivedToolListUpdate();
        OnReceivedPartListUpdate();
        SetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
    }


    public static List<GameObject> SetChild(int PageIndex)
    {
        int index = 0;
        List<GameObject> curList = new List<GameObject>();
        List<GameObject> curImage = new List<GameObject>();
        switch (PageIndex)
        {
            case 1:
                curList = PartsList1;
                curImage = PartsImage1;
                break;
            case 2:
                curList = PartsList2;
                curImage = PartsImage2;
                break;
            default:
                curList = PartsList3;
                curImage = PartsImage3;
                break;
        }
        foreach (GameObject item in curList)
        {
            PartX = Mathf.Abs(item.GetComponent<MeshFilter>().mesh.bounds.size.x);
            PartY = Mathf.Abs(item.GetComponent<MeshFilter>().mesh.bounds.size.y);
            PartZ = Mathf.Abs(item.GetComponent<MeshFilter>().mesh.bounds.size.z);

            if (PartX >= PartY && PartX >= PartZ)
            {
                PartScale = 0.32f / PartX;
                PartLocalScale = new Vector3(PartScale, PartScale, PartScale);
            }
            else if (PartY >= PartX && PartY >= PartZ)
            {
                PartScale = 0.32f / PartY;
                PartLocalScale = new Vector3(PartScale, PartScale, PartScale);
            }
            else if (PartZ >= PartX && PartZ >= PartY)
            {
                PartScale = 0.32f / PartZ;
                PartLocalScale = new Vector3(PartScale, PartScale, PartScale);
            }
            item.transform.SetParent( curImage[index].transform.Find("cube").transform);
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = PartLocalScale;
            curImage[index].SetActive(true);
            index++;
            if (index > 19) index = 0;
        }
        return curList;
    }

   
    public void OnReceivedPartListUpdate()
    {

        if (PartsList1.Count == 0)
        {
            for (PartIndex1 = 0; PartIndex1 < 65; PartIndex1++)
            {
                PartInfo1 = Parts[PartIndex1];
                PartsList1.Add(PartInfo1);
                PartsCount1 = Mathf.CeilToInt(65 / 20);
            }
        }
        if (PartsList2.Count == 0)
        {
            for (PartIndex2 = 65; PartIndex2 < 135; PartIndex2++)
            {
                PartInfo2 = Parts[PartIndex2];
                PartsList2.Add(PartInfo2);
                PartsCount2 = Mathf.CeilToInt(70 / 20);
            }
        }
        if (PartsList3.Count == 0)
        {
            for (PartIndex3 = 135; PartIndex3 < Parts.Count; PartIndex3++)
            {
                PartInfo3 = Parts[PartIndex3];
                PartsList3.Add(PartInfo3);
                PartsCount3 = Mathf.CeilToInt((Parts.Count-135)/20);
            }
        }
    }


    

    //public void OnReceivedToolListUpdate()
    //{
    //    int ToolIndex1 = 0;
    //    int ToolIndex2 = 0;
    //    int ToolIndex3 = 0;

    //    Tools1 = Resources.LoadAll<GameObject>("ToolsPrefabs/ToolClass_1");
    //    Tools2 = Resources.LoadAll<GameObject>("ToolsPrefabs/ToolClass_2");
    //    Tools3 = Resources.LoadAll<GameObject>("ToolsPrefabs/ToolClass_3");
    //    if (ToolsList1.Count == 0)
    //    {
    //        for (ToolIndex1 = 0; ToolIndex1 < Tools1.Length; ToolIndex1++)
    //        {
    //            ToolsList1.Add(Instantiate(Tools1[ToolIndex1]));
    //            ToolInfo1 = ToolsList1[ToolIndex1] = Tools1[ToolIndex1];
    //        }
                
    //    }

    //    else if (ToolsList1.Count > 0)
    //    {
    //        for (ToolIndex1 = 0; ToolIndex1 < ToolsList1.Count; ToolIndex1++)
    //        {
    //            ToolInfo1 = ToolsList1[ToolIndex1] = Tools1[ToolIndex1];
    //        }
    //    }

    //    if (ToolsList2.Count == 0)
    //    {
    //        for (ToolIndex2 = 0; ToolIndex2 < Tools2.Length; ToolIndex2++)
    //        {
    //            ToolsList2.Add(Instantiate(Tools2[ToolIndex2]));
    //            ToolInfo2 = ToolsList2[ToolIndex2] = Tools2[ToolIndex2];
    //        }

    //    }

    //    else if (ToolsList2.Count > 0)
    //    {
    //        for (ToolIndex2 = 0; ToolIndex2 < ToolsList2.Count; ToolIndex2++)
    //        {
    //            ToolInfo2 = ToolsList2[ToolIndex2] = Tools2[ToolIndex2];
    //        }
    //    }

    //    if (ToolsList2.Count == 0)
    //    {
    //        for (ToolIndex3 = 0; ToolIndex3 < Tools3.Length; ToolIndex3++)
    //        {
    //            ToolsList3.Add(Instantiate(Tools3[ToolIndex3]));
    //            ToolInfo3 = ToolsList3[ToolIndex3] = Tools3[ToolIndex3];
    //        }

    //    }

    //    else if (ToolsList3.Count > 0)
    //    {
    //        for (ToolIndex3 = 0; ToolIndex3 < ToolsList3.Count; ToolIndex3++)
    //        {
    //            ToolInfo3 = ToolsList3[ToolIndex3] = Tools3[ToolIndex3];
    //        }
    //    }
    //    //ToolX = Math.Abs(ToolInfo.GetComponent<MeshFilter>().mesh.bounds.size.x);
    //    //ToolY = Math.Abs(ToolInfo.GetComponent<MeshFilter>().mesh.bounds.size.y);
    //    //ToolZ = Math.Abs(ToolInfo.GetComponent<MeshFilter>().mesh.bounds.size.z);
    //    //if (ToolX >= ToolY && ToolX >= ToolZ)
    //    //{
    //    //    Scale = 0.15f / ToolX;
    //    //}
    //    //else if (ToolY >= ToolX && ToolY >= ToolZ)
    //    //{
    //    //    Scale = 0.15f / ToolY;
    //    //}
    //    //else if (ToolZ >= ToolX && ToolZ >= ToolY)
    //    //{
    //    //    Scale = 0.15f / ToolZ;
    //    //}
    //}


}
