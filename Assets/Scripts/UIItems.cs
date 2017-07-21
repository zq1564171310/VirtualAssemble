using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HoloToolkit.Examples.SpatialUnderstandingFeatureOverview;

/// <summary>
/// 每个UI上都会挂载这个脚本来控制鼠标进入状态
/// </summary>
public class UIItems : StateMechinePro, IFocusable, IInputClickHandler
{
    #region 字段

    State onfocus = new State();

    State ido = new State();

    State onselect = new State();

    Vector3 originepos;
    Vector3 originrot;

    SpatialUnderstandingCursor cursor;
    ModelsUI mu;

	//用于focus时播放声音计时
	float timer = 1;
	bool isplayedfocus = false;


	//focus时实际转动的物体
	private Transform modelobj;
	[SerializeField]
	private Transform endpos;


    #endregion

      #region 接口实现
    public void OnFocusEnter()
    {
        STATE = onfocus;
    }

    public void OnFocusExit()
    {
        STATE = ido;
     //   Debug.Log("<color=yellow>OnFocusExit</color>");
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
		//Sound.Instance.PlayerEffect ("DropDown");
        GameObject go = ObjectPool.Instance.Spawn(gameObject.name);
    //    Debug.Log("name========" + gameObject.name);
    //    go.GetComponentInChildren<MeshRenderer>().material.color = new Color(UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1), UnityEngine.Random.Range(0, 1));
        ItemPro pro = go.GetComponent<ItemPro>();
		if (pro == null) 
		{
			pro = go.AddComponent<ItemPro> ();
		}
        pro.cursor = cursor;
        pro.mu = this.mu;
        pro.Init();
        pro.ChangeState(ClickState.Move);
        go.transform.position = Camera.main.transform.forward * 2;
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

		endpos = transform.Find ("EndPos");
		modelobj = transform.Find ("Model");

        if (GetComponent<BoxCollider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

		originepos = modelobj.transform.localPosition;
		originrot = modelobj.transform.localEulerAngles;



    }

    void Update()
    {
        OnUpdater(Time.deltaTime);
		timer += Time.deltaTime;
    }
    #endregion

    #region 状态机方法
    void FocusEnter()
    {
 //       InputManager.Instance.OverrideFocusedObject = gameObject;
        //Sound.Instance.PlayerEffect("Focus");
    }
    void FocusUpdater(float timer)
    {
		if (endpos != null) {
			modelobj.transform.position = Vector3.Lerp(modelobj.transform.position, endpos.position, timer * 2);	//new Vector3(originepos.x, originepos.y + 1, originepos.z)
		} else {
			modelobj.transform.localPosition = Vector3.Lerp(modelobj.transform.localPosition, new Vector3(originepos.x, originepos.y + 1, originepos.z), timer * 2);	//new Vector3(originepos.x, originepos.y + 1, originepos.z)
		}
		modelobj.transform.Rotate(Vector3.back, timer * 90);

		if (timer > 1f && !isplayedfocus)
		{
//			isplayedfocus = true;
//			Sound.Instance.PlayerEffect("Focus");
			//timer = 0;
		}

    }
    void FocuseLeave()
    {
 //       InputManager.Instance.OverrideFocusedObject = null;
    }

    void SelectEnter() { }
    void SelectUpdater(float timer) { }
    void SelectLeave() { }

    void IdoEnter() { }
    void IdoUpdater(float timer)
    {
        if (statetimer < 2)
        {
			modelobj.transform.localPosition = Vector3.Lerp(modelobj.transform.localPosition, originepos, timer * 2);
			modelobj.transform.localEulerAngles = Vector3.Lerp(modelobj.transform.localEulerAngles, originrot, timer * 2);
        }
    }
    #endregion

    #region 帮助方法
    public void Init(SpatialUnderstandingCursor cursor,ModelsUI mu)
    {
        this.cursor = cursor;
        this.mu = mu;
    }
    #endregion
}
