using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 头部选择按钮所挂
/// </summary>
public class MainChoiseBut :StateMechinePro , IFocusable, IInputClickHandler
{


    #region 字段

    State onfocus = new State();

    State ido = new State();

    State onselect = new State();

    Vector3 originepos;
    Vector3 originrot;

    Color forwardBG_old;
    Color backgrund_old;

    public Material backgurund;
    public Transform Icorn;

    #endregion


    #region 接口实现
    public void OnFocusEnter()
    {
        //Sound.Instance.PlayerEffect("Focus");
        STATE = onfocus;
     //   Debug.Log("OnFocusEnter");
    }

    public void OnFocusExit()
    {
        STATE = ido;
    //    Debug.Log("<color=yellow>OnFocusExit</color>");
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        //Sound.Instance.PlayerEffect("MainChoise");
    }
    #endregion


    #region Unity回调
    void Start()
    {
        onfocus.OnEnter = FocusEnter;
        onfocus.OnUpdate = FocusUpdater;
        onfocus.OnLeave = FocuseLeave;

        onselect.OnEnter = SelectEnter;
        onselect.OnUpdate = SelectUpdater;
        onselect.OnLeave = SelectLeave;

        ido.OnEnter = IdoEnter;
        ido.OnUpdate = IdoUpdater;


//        if (GetComponent<BoxCollider>() == null)
//        {
//            gameObject.AddComponent<BoxCollider>();
//        }
       // forwardBG = transform.Find("ListButton").GetComponent<SpriteRenderer>();
        backgurund = transform.Find("ButtonPlate").GetComponent<SpriteRenderer>().material;
        Icorn = transform.Find("GameObject");

        originepos = Icorn.transform.localPosition;
        originrot = Icorn.transform.localEulerAngles;

        backgrund_old = backgurund.color;

    }

    void Update()
    {
        OnUpdater(Time.deltaTime);
    }
    #endregion

    #region 状态机方法
    void FocusEnter()
    {
    //    InputManager.Instance.OverrideFocusedObject = gameObject;
    }
    void FocusUpdater(float timer)
    {
        Icorn.transform.localPosition = Vector3.Lerp(Icorn.transform.localPosition, new Vector3(originepos.x, originepos.y, originepos.z - 0.04f), timer * 2);
        Icorn.transform.Rotate(transform.up, timer * 90);
        backgurund.color = Color.Lerp(backgurund.color, Color.green, timer * 2);
    }
    void FocuseLeave()
    {
    //    InputManager.Instance.OverrideFocusedObject = null;
    }

    void SelectEnter() { }
    void SelectUpdater(float timer) { }
    void SelectLeave() { }

    void IdoEnter() { }
    void IdoUpdater(float timer)
    {
        if (statetimer < 2)
        {
            Icorn.transform.localPosition = Vector3.Lerp(Icorn.transform.localPosition, originepos, timer * 2);
            Icorn.transform.localEulerAngles = Vector3.Lerp(Icorn.transform.localEulerAngles,new Vector3(Icorn.transform.localEulerAngles.x ,originrot.y,Icorn.transform.localEulerAngles.z), timer * 2);

            backgurund.color = Color.Lerp(backgurund.color, backgrund_old, timer * 2);
        }
    }
        #endregion


}
