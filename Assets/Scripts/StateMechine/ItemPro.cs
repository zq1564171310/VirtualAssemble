using System;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Examples.SpatialUnderstandingFeatureOverview;
using HoloToolkit.Unity;
using UnityEngine.VR.WSA.Input;

public enum ClickState
{
	Ido,Move,Rotate,Scale,Delet,OpenUI,CloseUI
}


///点击按钮实例出来的obj所挂脚本
public class ItemPro : StateMechinePro,IFocusable,IInputClickHandler,IManipulationHandler
{

    #region ImanipulationHandler实现(旋转和缩放)
    public void OnManipulationStarted(ManipulationEventData eventData)
    {
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        Vector3 temp = eventData.CumulativeDelta;
        float xdistance = temp.x;
		float ydistance = temp.y;
		Vector3 localeulerangles = transform.localEulerAngles;
		Vector3 localscale = transform.localScale;
		switch (_cS) 
		{
		case ClickState.Rotate:
			transform.localEulerAngles = new Vector3(localeulerangles.x, localeulerangles.y - temp.x * 4, localeulerangles.z);
			break;
		case ClickState.Scale:
			if (localscale.x < scaler_max && localscale.x > scaler_min) 
			{
				if (ScaleIcon.GetComponent<SpriteRenderer>().color != Color.green)
				{
					ScaleIcon.GetComponent<SpriteRenderer> ().color = Color.green;
				}
				transform.localScale = new Vector3 (localscale.x + ydistance, localscale.y + ydistance, localscale.z + ydistance) ;
			}
			else if(localscale.x > scaler_max)
			{
				if (temp.y < 0) {
					transform.localScale = new Vector3 (localscale.x + ydistance, localscale.y + ydistance, localscale.z + ydistance);
				} else {
					LerpColor (Time.time);
				}
			}else if(localscale.x < scaler_min)
			{
				if (temp.y > 0) {
					transform.localScale = new Vector3 (localscale.x + ydistance, localscale.y + ydistance, localscale.z + ydistance) ;
				} else {
					LerpColor (Time.time);
				}
			}
			break;
		default:
			break;
		}
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
    }

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
    }

    #endregion

    #region IInputClickHandler 实现

    public void OnInputClicked (InputClickedEventData eventData)
	{
        Debug.Log("点击到么了，，，，，上个状态为：：：：：：：" + _cS.ToString() );
      
        switch (_cS)
        {
            case ClickState.Ido:
            case ClickState.CloseUI:
                ChangeState(ClickState.OpenUI);
                break;
            case ClickState.Scale:
            case ClickState.Rotate:
                ChangeState(ClickState.Ido);
                break;
		//	case ClickState.Move:
			case ClickState.OpenUI:
                Debug.Log("子物体执行：：OnInputClicked：：：" + gameObject.name);
                ChangeState (ClickState.CloseUI);
				box.enabled = true;
                break;             
            default:
                break;
        }


	}

	#endregion

	
	#region IFocusable 实现

	public void OnFocusEnter ()
	{
    //    InputManager.Instance.OverrideFocusedObject = gameObject;
	}

	public void OnFocusExit ()
	{
     //   InputManager.Instance.OverrideFocusedObject = null;
	}

    #endregion


    #region 字段

    //自身放大缩小极限
    private float scaler_max;
    private float scaler_min;
    

    //放UI的地方
    private Transform UIpos;

    //操作自身的UI
	private GameObject UI;

    //旋转图标
    private GameObject RotateIcon;

    //缩放图标
	private GameObject ScaleIcon;

    //是否允许AirTap
    private bool canair = false;

    //点击button改变状态
    [SerializeField]
	private ClickState _cS;

    //自身带boxcollider
    private BoxCollider box;

    //Foucus光标
	public SpatialUnderstandingCursor cursor;
    public ModelsUI mu;

    

	#endregion


	#region 状态

	//空闲
	private State _ido = new State();

	//focused
	private State _focused = new State();
	//打开UI
	private State _openUI = new State();
	//关闭UI
	private State _closeUI = new State();
	//移动
	private State _move = new State();
	//旋转
	private State _rotate = new State();
	//缩放
	private State _Scaler = new State();
	//删除
	private State _delete = new State();

    #endregion


    #region Unity回调

    void Start()
	{
      
        RotateIcon = transform.Find("RoateIcon").gameObject;
		ScaleIcon = transform.Find ("ScaleIcon").gameObject;

        scaler_max = transform.localScale.x * 2;
        scaler_min = transform.localScale.y / 2;

        ScaleIcon.AddComponent<Billboard>();


		
    }

	void Update()
	{
		OnUpdater (Time.deltaTime);

        //测试
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && _cS == ClickState.Move)
        {
            Debug.Log("CloseUI");
            ChangeState(ClickState.CloseUI);
            Sound.Instance.PlayerEffect("DropDown");
        }
#endif

    }

	void OnDisable()
	{
        mu.airtapE -= OnAirTap;

    }

	#endregion


	#region 状态机
	//被注视
	void FocusedEnter()
	{
		
	}
	void FocusedUpdater(float timer)
	{
       
	}

	//打开UI
	void OpenUIEnter()
	{
        if (UI == null)
        {
            UI = ObjectPool.Instance.Spawn("ItemUI");
        }

        box.enabled = false;

        Transform pos = transform.Find ("UIPos");
		UI.transform.SetParent (pos.transform);
		UI.transform.position = pos.position;
		UI.transform.localEulerAngles = Vector3.zero;
		UI.GetComponent<UIManager> ().Itemobj = this;
        UI.transform.SetParent(null);

	}
	void OpenUIUpdater(float timer)
	{
		float temp = statetimer;
		if (temp <= 1) 
		{
			UI.transform.localScale = new Vector3 (temp, temp, temp);
		}else if (UI.transform.localScale != Vector3.one)
		{
			UI.transform.localScale = Vector3.one;
		}
	}
    void OpenUILeave()
    {
        box.enabled = true;
    }


	//关闭UI
	void CloseUIEnter()
	{
    }
	void CloseUIUpdater(float timer)
	{
        if (UI != null)
        {
            float temp = statetimer;
            if (temp <= 1)
            {
                UI.transform.localScale = new Vector3(1 - temp, 1 - temp, 1 - temp);
            }
            else if (UI.transform.localScale != Vector3.zero)
            {
                UI.transform.localScale = Vector3.zero;
            }
        }
    }

	//移动
	void MoveEnter()
	{
		cursor.Target = gameObject;
		box.enabled = false;
		//cursor.canserchself = false;
		//gameObject.layer = LayerMask.NameToLayer ("Default");
	}
	void MoveUpdater(float timer)
	{
        if (statetimer > 1 && !canair)
        {
            canair = true;
        }
    }
	void MoveLeave()
	{
        canair = false;
		box.enabled = true;
		cursor.Target = null;
		Sound.Instance.PlayerEffect("DropDown");
		//cursor.canserchself = true;
		//gameObject.layer = LayerMask.NameToLayer ("UI");
	}

    //旋转
    void RoateOnEnter()
    {
        RotateIcon.SetActive(true);
    }
	void RotateUpdater(float timer)
	{
		RotateIcon.transform.Rotate(Vector3.back, timer * 5);
	}
    void RoateLeave()
    {
        RotateIcon.SetActive(false);
    }


	//缩放
	void ScaleEnter()
	{
		ScaleIcon.SetActive (true);
	}
	void ScaleUpdater(float timer)
	{
		
	}
	void ScaleLeave()
	{
		ScaleIcon.SetActive (false);
	}

    //空闲状态
    void IdoEnter() { }
    void IdoUpdater(float timer)
    {
        if (UI != null)
        {
            float temp = statetimer;
            if (temp <= 1)
            {
                UI.transform.localScale = new Vector3(1 - temp, 1 - temp, 1 - temp);
            }
            else if (UI.transform.localScale != Vector3.zero)
            {
                UI.transform.localScale = Vector3.zero;
            }
        }
        
    }
    void IdoLeave() { }


	//删除
	void DeleteEnter()
	{
        ObjectPool.Instance.UnSpawn(UI);
        ObjectPool.Instance.UnSpawn (gameObject);
	} 
	#endregion



	#region 帮助方法
	public void Init()
	{
		_focused.OnEnter = FocusedEnter;
		_focused.OnUpdate = FocusedUpdater;

		_openUI.OnEnter = OpenUIEnter;
		_openUI.OnUpdate = OpenUIUpdater;
        _openUI.OnLeave = OpenUILeave;

		_closeUI.OnEnter = CloseUIEnter;
		_closeUI.OnUpdate = CloseUIUpdater;

		_move.OnEnter = MoveEnter;
		_move.OnUpdate = MoveUpdater;
		_move.OnLeave = MoveLeave;

        _rotate.OnEnter = RoateOnEnter;
        _rotate.OnUpdate = RotateUpdater;
		_rotate.OnLeave = RoateLeave;

		_Scaler.OnEnter = ScaleEnter;
		_Scaler.OnUpdate = ScaleUpdater;
		_Scaler.OnLeave = ScaleLeave;

        _ido.OnEnter = IdoEnter;
        _ido.OnUpdate = IdoUpdater;
        _ido.OnLeave = IdoLeave;

		_delete.OnEnter = DeleteEnter;

        canair = false;

        box = GetComponent<BoxCollider> ();
		//DestroyImmediate (box);
		//box = gameObject.AddComponent<BoxCollider> ();
		cursor.canserchself = false;
		box.enabled = false;

        mu.airtapE += OnAirTap;

       // InputManager.Instance.
    }

   

    public void ChangeState(ClickState cs)
	{
        Debug.Log(cs.ToString());
		switch (cs)
		{
		case ClickState.Ido:
			STATE = _ido;
			break;
		case ClickState.Move:
			STATE = _move;
			break;
		case ClickState.Rotate:
			STATE = _rotate;
			break;
		case ClickState.Scale:
			STATE = _Scaler;
			break;
		case ClickState.Delet:
			STATE = _delete;
			break;
        case ClickState.CloseUI:
            STATE = _closeUI;
            break;
        case ClickState.OpenUI:
            STATE = _openUI;
            break;
            default:
			break;
		}
		_cS = cs;
	}

	private void LerpColor(float t)
	{
		float d = t % 1;
		Color c = new Color (1 - d, d, 0);
		ScaleIcon.GetComponent<SpriteRenderer> ().color = c;
	}


    private void OnAirTap(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (this._cS == ClickState.Move && canair)
        {
            //InputManager.Instance.OverrideFocusedObject = gameObject;
            ChangeState(ClickState.CloseUI);
            Sound.Instance.PlayerEffect("DropDown");
            Debug.Log("CloseUI");
        }
        
    }

    #endregion

}
