using System;
using UnityEngine;

public class StateMechinePro : MonoBehaviour
{
	protected float statetimer;
	[SerializeField]
	private State _state;

	public State STATE {
		get 
		{
			return _state;
		}
		set 
		{
			if (_state != null && _state.OnLeave != null) 
			{
				_state.OnLeave ();
			}
			_state = value;
			statetimer = 0;
			if (_state != null && _state.OnEnter != null)
			{
				_state.OnEnter ();
			}
		}
	}

	public void OnUpdater(float timer)
	{
		statetimer += timer;
		if (_state != null && _state.OnUpdate != null)
		{
			_state.OnUpdate (timer);
		}
	}

}


public class State
{
	public Action OnEnter;
	public Action<float> OnUpdate;
	public Action OnLeave;
}
