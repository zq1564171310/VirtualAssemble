using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartPanel : MonoBehaviour {

    [SerializeField]
    private GameObject Part_Panel_1, Part_Panel_2, Part_Panel_3;
    [SerializeField]
    private Toggle PartClass_1, PartClass_2, PartClass_3;

    // Use this for initialization
    void Start()
    {
        PartClass_1.onValueChanged.AddListener(OnValChanged1);
        PartClass_2.onValueChanged.AddListener(OnValChanged2);
        PartClass_3.onValueChanged.AddListener(OnValChanged3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnValChanged1(bool check)
    {
        Part_Panel_1.SetActive(check);
        Part_Panel_2.SetActive(!check);
        Part_Panel_3.SetActive(!check);
    }
    void OnValChanged2(bool check)
    {
        Part_Panel_1.SetActive(!check);
        Part_Panel_2.SetActive(check);
        Part_Panel_3.SetActive(!check);
    }
    void OnValChanged3(bool check)
    {
        Part_Panel_1.SetActive(!check);
        Part_Panel_2.SetActive(!check);
        Part_Panel_3.SetActive(check);
    }

}
