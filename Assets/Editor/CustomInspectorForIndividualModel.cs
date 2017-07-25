using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

//[CustomEditor(typeof(IndividualModelLogic))]
public class CustomInspectorForIndividualModel : Editor
{
    //private bool initialized = false;
    //private bool Setting = true;

    //public override void OnInspectorGUI()
    //{
    //    IndividualModelLogic _IndividualModelLogic = (IndividualModelLogic)target;

    //    EditorGUILayout.BeginHorizontal();
    //    GUILayout.Space(20);
    //    EditorGUILayout.BeginVertical();

    //    EditorGUILayout.BeginHorizontal();
    //    Setting = EditorGUILayout.Foldout(Setting, "零件参数设置");
    //    if (GUILayout.Button("所有零件配置面板", GUILayout.Width(100), GUILayout.Height(20)))
    //    {
    //        TestListView.TestListViewButton.SetSelectRow(_IndividualModelLogic.gameObject);
    //        Selection.activeGameObject = GlobalVar._FatherGameObject;
    //    }
    //    EditorGUILayout.EndHorizontal();

    //    if (Setting)
    //    {
    //        if (GUILayout.Button(_IndividualModelLogic._Model.PlayBtnImage, GUILayout.Width(30), GUILayout.Height(20)))
    //        {
    //            if (PlayState.None == _IndividualModelLogic._Model.OnPlayState)
    //            {
    //                _IndividualModelLogic._Model.OnPlayState = PlayState.Playing;
    //            }
    //            else if (PlayState.Stop == _IndividualModelLogic._Model.OnPlayState)
    //            {
    //                _IndividualModelLogic._Model.OnPlayState = PlayState.FallBack;
    //            }
    //        }

    //        if (PlayState.Playing == _IndividualModelLogic._Model.OnPlayState)
    //        {
    //            _IndividualModelLogic._Model.PlayBtnImage = GlobalVar.CustomInspectorButPic2;
    //            GlobalVar._IndividualModelManager.OnPlay(_IndividualModelLogic.gameObject);
    //        }
    //        else if (PlayState.FallBack == _IndividualModelLogic._Model.OnPlayState)
    //        {
    //            _IndividualModelLogic._Model.PlayBtnImage = GlobalVar.CustomInspectorButPic1;
    //            GlobalVar._IndividualModelManager.OnPlay(_IndividualModelLogic.gameObject);
    //        }
    //        else if (PlayState.None == _IndividualModelLogic._Model.OnPlayState)
    //        {
    //            _IndividualModelLogic._Model.PlayBtnImage = GlobalVar.CustomInspectorButPic1;
    //        }
    //        else if (PlayState.Stop == _IndividualModelLogic._Model.OnPlayState)
    //        {
    //            _IndividualModelLogic._Model.PlayBtnImage = GlobalVar.CustomInspectorButPic2;
    //        }

    //        _IndividualModelLogic._Model.EndPos = EditorGUILayout.Vector3Field("目标位置", _IndividualModelLogic._Model.EndPos);
    //        EditorGUI.BeginDisabledGroup(true);
    //        _IndividualModelLogic._Model.StartPos = EditorGUILayout.Vector3Field("源位置", _IndividualModelLogic._Model.StartPos);
    //        EditorGUI.EndDisabledGroup();

    //        _IndividualModelLogic._Model.IsMove = GUILayout.Toggle(_IndividualModelLogic._Model.IsMove, "是否随上一个零件模型一起被拆装", GUILayout.Width(180), GUILayout.Height(20));

    //        EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.SelectableLabel("拆机声音", GUILayout.Width(80));
    //        _IndividualModelLogic._Model.MechanicalSoundID = EditorGUILayout.Popup(_IndividualModelLogic._Model.MechanicalSoundID, _IndividualModelLogic.MechanicalSoundNameList.ToArray(), GUILayout.Width(120));
    //        EditorGUILayout.EndHorizontal();

    //        EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.SelectableLabel("语音介绍", GUILayout.Width(80));
    //        _IndividualModelLogic._Model.IntroductionSpeechID = EditorGUILayout.Popup(_IndividualModelLogic._Model.IntroductionSpeechID, _IndividualModelLogic.IntroductionSpeechNameList.ToArray(), GUILayout.Width(120));
    //        EditorGUILayout.EndHorizontal();
    //    }

    //    EditorGUILayout.EndVertical();
    //    EditorGUILayout.BeginHorizontal();
    //}
}