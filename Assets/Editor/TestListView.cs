using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using WH.Editor;
using System;

public class TestListView : EditorWindow
{
    #region ListView模板
    //定义每一行的宽度和信息，如果是定义多个lable new[] { 100, 100, 100 }，同时m_MsgList定义多个list
    public static List<string> m_MsgList = new List<string>();
    public static int[] m_ColWidths = new[] { 510 };

    public static ListViewState m_ListView = new ListViewState();
    public static bool m_Focus;

    public class Styles
    {
        public readonly GUIStyle listItem = new GUIStyle("PR Label");
        public readonly GUIStyle listItemBackground = new GUIStyle("CN EntryBackOdd");
        public readonly GUIStyle listItemBackground2 = new GUIStyle("CN EntryBackEven");
        public readonly GUIStyle listBackgroundStyle = new GUIStyle("CN Box");
        public Styles()
        {
            Texture2D background = this.listItem.hover.background;
            //开启即失去焦点时，也显示蓝色
            //this.listItem.onNormal.background = background;
            this.listItem.onActive.background = background;
            this.listItem.onFocused.background = background;
        }
    }
    public static Styles s_Styles;

    static void Init()
    {
        GetWindow(typeof(TestListView));
    }

    private void OnGUI()
    {

    }

    protected static string GetRowText(ListViewElement el)
    {
        return m_MsgList[el.row];
    }

    private void OnFocus()
    {
        m_Focus = true;
    }

    private void OnLostFocus()
    {
        m_Focus = false;
    }
    #endregion

    /// <summary>
    /// 需要继承Editor类，重写OnInspectorGUI函数，也需要使用TestListView类中的相关变量和方法（因为TestListView继承自EditorWindow，需要继承EditorWindow），故使用内部类，达到同时使用或者重写Editor类和EditorWindow方法的目的
    /// </summary>
   // [CustomEditor(typeof(AllModelsLogic))]
    public class TestListViewButton : Editor
    {
        //    private bool IsDataInitFlag;                                //数据初始化标记

        //    private static List<ListViewState> Engine_ListView = new List<ListViewState>();

        //    private bool RootFoldOutFlag = true;                        //根的折叠标记   
        //    private string RootName = "root";                                    //根名称

        //    private List<bool> GroupFoldOutFlag = new List<bool>();                             //组的折叠标记
        //    private bool GroupCountChangeFlag = true;   //组的个数长度，一旦组的个数发生变化，UI需要重新添加或者删除新的组，ListView需要重新绘制

        //    private static GameObject InspectorSelectGameobject;                           //被选中的物体

        //    static void Init()
        //    {
        //        GetWindow(typeof(TestListViewButton));
        //        GetWindow(typeof(TestListViewReordering));
        //    }
        //    public override void OnInspectorGUI()
        //    {
        //        if (s_Styles == null)
        //        {
        //            s_Styles = new Styles();
        //        }

        //        //定义每一行的高度
        //        s_Styles.listItem.fixedHeight = 22f;

        //        if (false == IsDataInitFlag)
        //        {
        //            AllModelsLogic.GroupName.Clear();
        //            GroupFoldOutFlag.Clear();

        //            AllModelsLogic.GroupName.Add("Group0");
        //            GroupFoldOutFlag.Add(true);

        //            int GroupMaxNum = 0;
        //            for (int j = 0; j < GlobalVar._AllModelsLogic.GetModelList().Count; j++)
        //            {
        //                if (GlobalVar._AllModelsLogic.GetModelList()[j].GroupNum > GroupMaxNum)
        //                {
        //                    GroupMaxNum = GlobalVar._AllModelsLogic.GetModelList()[j].GroupNum;
        //                }
        //            }
        //            for (int j = 1; j < GroupMaxNum + 1; j++)
        //            {
        //                AllModelsLogic.GroupName.Add("Group" + j);
        //                GroupFoldOutFlag.Add(false);
        //            }
        //            GroupCountChangeFlag = true;

        //            IsDataInitFlag = true;

        //            AllModelsLogic.NameDisplayModelList.Add("方式1");

        //            AllModelsLogic.MoveModelList.Add("直接飞走");                     //默认渐入渐出方式
        //        }
        //        Event current = Event.current;
        //        EditorGUILayout.BeginVertical();
        //        //根
        //        EditorGUILayout.BeginHorizontal();
        //        RootFoldOutFlag = EditorGUILayout.Foldout(RootFoldOutFlag, RootName);

        //        if (GUILayout.Button("上一步"))                   //点击上一步
        //        {
        //            GlobalVar._PlayerManager.OnPlayLastManager();
        //            System.Threading.Thread.Sleep(50);
        //        }
        //        if (GUILayout.Button("下一步"))                    //点击下一步
        //        {
        //            GlobalVar._PlayerManager.OnPlayNextManager();
        //            System.Threading.Thread.Sleep(50);
        //        }

        //        if (GUILayout.Button("添加组"))
        //        {
        //            AllModelsLogic.GroupName.Add("Group" + (AllModelsLogic.GroupName.Count));
        //            GroupFoldOutFlag.Add(true);
        //            GroupCountChangeFlag = true;
        //        }

        //        EditorGUILayout.EndHorizontal();
        //        if (RootFoldOutFlag)
        //        {
        //            if (GroupCountChangeFlag)
        //            {
        //                Engine_ListView.Clear();
        //                for (int i = 0; i < AllModelsLogic.GroupName.Count; i++)
        //                {
        //                    ListViewState EngineView = new ListViewState();
        //                    Engine_ListView.Add(EngineView);
        //                }
        //                GroupCountChangeFlag = false;
        //            }

        //            GlobalVar._AllModelsLogic.GetModelList().Sort();            //排序，先按组，后按ID

        //            int rowNum = -1;
        //            rowNum = GetSelectRow();

        //            for (int i = 0; i < AllModelsLogic.GroupName.Count; i++)
        //            {
        //                //相同的组，放入相同的List中，便于ListView显示
        //                List<ModelClass> listTemp = new List<ModelClass>();
        //                for (int j = 0; j < GlobalVar._AllModelsLogic.GetModelList().Count; j++)
        //                {
        //                    if (AllModelsLogic.GroupName[i] == "Group" + GlobalVar._AllModelsLogic.GetModelList()[j].GroupNum)
        //                    {
        //                        listTemp.Add(GlobalVar._AllModelsLogic.GetModelList()[j]);
        //                        GlobalVar._AllModelsLogic.GetModelList()[j].PlayOrderId = listTemp.Count;
        //                    }
        //                }

        //                Engine_ListView[i].rowHeight = 22;
        //                Engine_ListView[i].totalRows = listTemp.Count;
        //                if (rowNum >= 0 && (rowNum + 1 <= Engine_ListView[i].totalRows))
        //                {
        //                    Engine_ListView[i].row = rowNum;
        //                    rowNum = -1;
        //                }
        //                else
        //                {
        //                    rowNum = rowNum - Engine_ListView[i].totalRows;
        //                }
        //                EditorGUILayout.BeginVertical();

        //                EditorGUILayout.BeginHorizontal();
        //                GUILayout.Space(10);

        //                EditorGUILayout.BeginVertical();

        //                //每个Group内容
        //                GroupFoldOutFlag[i] = EditorGUILayout.Foldout(GroupFoldOutFlag[i], AllModelsLogic.GroupName[i] + "    (共有" + Engine_ListView[i].totalRows + "个零件模型)");
        //                if (GroupFoldOutFlag[i])
        //                {
        //                    EditorGUILayout.BeginHorizontal();
        //                    EditorGUILayout.SelectableLabel("", GUILayout.Width(20), GUILayout.Height(25));
        //                    EditorGUILayout.SelectableLabel("零件名称", GUILayout.Width(80), GUILayout.Height(25));
        //                    EditorGUILayout.SelectableLabel("渐入渐出方式", GUILayout.Width(100), GUILayout.Height(25));
        //                    EditorGUILayout.SelectableLabel("名称显示方式", GUILayout.Width(100), GUILayout.Height(25));
        //                    EditorGUILayout.SelectableLabel("所在的组", GUILayout.Width(100), GUILayout.Height(25));
        //                    EditorGUILayout.SelectableLabel("修改播放次序", GUILayout.Width(100), GUILayout.Height(25));
        //                    EditorGUILayout.EndHorizontal();

        //                    Rect playNumRect = new Rect();
        //                    GUIContent textContent = new GUIContent();
        //                    Rect moveModelRect = new Rect();
        //                    Rect nameDisplayModelRect = new Rect();
        //                    Rect groupNameRect = new Rect();
        //                    Rect upNameRect = new Rect();
        //                    Rect downNameRect = new Rect();
        //                    foreach (ListViewElement el in ListViewGUI.ListView(Engine_ListView[i], ListViewOptions.wantsReordering, m_ColWidths, "", s_Styles.listBackgroundStyle, GUILayout.Height((Engine_ListView[i].totalRows > 10) ? (200) : (Engine_ListView[i].totalRows * 22))))
        //                    {
        //                        playNumRect = el.position;
        //                        playNumRect.x += playNumRect.width - 480;
        //                        playNumRect.y += 2;
        //                        playNumRect.width = 70;
        //                        playNumRect.height = 18;

        //                        moveModelRect = el.position;
        //                        moveModelRect.x += moveModelRect.width - 400;
        //                        moveModelRect.y += 2;
        //                        moveModelRect.width = 60;
        //                        moveModelRect.height = 18;

        //                        nameDisplayModelRect = el.position;
        //                        nameDisplayModelRect.x += nameDisplayModelRect.width - 290;
        //                        nameDisplayModelRect.y += 2;
        //                        nameDisplayModelRect.width = 50;
        //                        nameDisplayModelRect.height = 18;

        //                        groupNameRect = el.position;
        //                        groupNameRect.x += groupNameRect.width - 195;
        //                        groupNameRect.y += 2;
        //                        groupNameRect.width = 60;
        //                        groupNameRect.height = 18;

        //                        upNameRect = el.position;
        //                        upNameRect.x += upNameRect.width - 90;
        //                        upNameRect.y += 2;
        //                        upNameRect.width = 30;
        //                        upNameRect.height = 18;

        //                        downNameRect = el.position;
        //                        downNameRect.x += downNameRect.width - 50;
        //                        downNameRect.y += 2;
        //                        downNameRect.width = 30;
        //                        downNameRect.height = 18;

        //                        if (current.type == EventType.mouseDown && current.button == 0 && el.position.Contains(current.mousePosition))
        //                        {
        //                            if (current.clickCount == 2)
        //                            {
        //                                //双击跳转到相应的物体上
        //                                Selection.activeGameObject = GameObject.Find(GetRowText(listTemp, el));
        //                            }
        //                            else if (current.clickCount == 1)
        //                            {
        //                                EditorGUIUtility.PingObject(GameObject.Find(GetRowText(listTemp, el)));
        //                                InspectorSelectGameobject = GameObject.Find(GetRowText(listTemp, el));
        //                                SetSelectRow(GameObject.Find(GetRowText(listTemp, el)));
        //                            }
        //                        }


        //                        if (current.type == EventType.Repaint)
        //                        {
        //                            textContent.text = GetRowText(listTemp, el);
        //                            // 交替显示不同背景色
        //                            GUIStyle style = (el.row % 2 != 0) ? s_Styles.listItemBackground2 : s_Styles.listItemBackground;
        //                            style.Draw(el.position, false, true, Engine_ListView[i].row == el.row, false);
        //                        }

        //                        EditorGUI.LabelField(el.position, el.row.ToString());

        //                        EditorGUI.LabelField(playNumRect, textContent);

        //                        listTemp[el.row].MoveModel = EditorGUI.Popup(moveModelRect, listTemp[el.row].MoveModel, AllModelsLogic.MoveModelList.ToArray());

        //                        listTemp[el.row].NameDisplay = EditorGUI.Popup(nameDisplayModelRect, listTemp[el.row].NameDisplay, AllModelsLogic.NameDisplayModelList.ToArray());

        //                        listTemp[el.row].GroupNum = EditorGUI.Popup(groupNameRect, listTemp[el.row].GroupNum, AllModelsLogic.GroupName.ToArray());

        //                        if (GUI.Button(upNameRect, "↑") && Engine_ListView[i].totalRows > 0 && el.row > 0)
        //                        {
        //                            var tmpName = listTemp[el.row - 1].Name;
        //                            listTemp[el.row - 1].Name = listTemp[el.row].Name;
        //                            listTemp[el.row].Name = tmpName;

        //                            var tmpPopueInt2 = listTemp[el.row - 1].MoveModel;
        //                            listTemp[el.row - 1].MoveModel = listTemp[el.row].MoveModel;
        //                            listTemp[el.row].MoveModel = tmpPopueInt2;

        //                            var tmpPopueInt3 = listTemp[el.row - 1].NameDisplay;
        //                            listTemp[el.row - 1].NameDisplay = listTemp[el.row].NameDisplay;
        //                            listTemp[el.row].NameDisplay = tmpPopueInt3;

        //                            var tmpPopueInt5 = listTemp[el.row - 1].GroupNum;
        //                            listTemp[el.row - 1].GroupNum = listTemp[el.row].GroupNum;
        //                            listTemp[el.row].GroupNum = tmpPopueInt5;

        //                            Engine_ListView[i].row = el.row - 1;
        //                        }

        //                        if (GUI.Button(downNameRect, "↓") && Engine_ListView[i].totalRows > 0)
        //                        {
        //                            var tmpName = listTemp[el.row + 1].Name;
        //                            listTemp[el.row + 1].Name = listTemp[el.row].Name;
        //                            listTemp[el.row].Name = tmpName;

        //                            var tmpPopueInt2 = listTemp[el.row + 1].MoveModel;
        //                            listTemp[el.row + 1].MoveModel = listTemp[el.row].MoveModel;
        //                            listTemp[el.row].MoveModel = tmpPopueInt2;

        //                            var tmpPopueInt3 = listTemp[el.row + 1].NameDisplay;
        //                            listTemp[el.row + 1].NameDisplay = listTemp[el.row].NameDisplay;
        //                            listTemp[el.row].NameDisplay = tmpPopueInt3;

        //                            var tmpPopueInt5 = listTemp[el.row + 1].GroupNum;
        //                            listTemp[el.row + 1].GroupNum = listTemp[el.row].GroupNum;
        //                            listTemp[el.row].GroupNum = tmpPopueInt5;

        //                            Engine_ListView[i].row = el.row + 1;
        //                        }
        //                    }

        //                    if (Engine_ListView[i].totalRows > 0 && Engine_ListView[i].selectionChanged)
        //                    {
        //                        // 拖动更新
        //                        if (Engine_ListView[i].draggedFrom != -1 && Engine_ListView[i].draggedTo != -1)
        //                        {
        //                            var tmpName = listTemp[Engine_ListView[i].draggedFrom].Name;
        //                            listTemp[Engine_ListView[i].draggedFrom].Name = listTemp[Engine_ListView[i].row].Name;
        //                            listTemp[Engine_ListView[i].row].Name = tmpName;

        //                            var tmpPopueInt2 = listTemp[Engine_ListView[i].draggedFrom].MoveModel;
        //                            listTemp[Engine_ListView[i].draggedFrom].MoveModel = listTemp[Engine_ListView[i].row].MoveModel;
        //                            listTemp[Engine_ListView[i].row].MoveModel = tmpPopueInt2;

        //                            var tmpPopueInt3 = listTemp[Engine_ListView[i].draggedFrom].NameDisplay;
        //                            listTemp[Engine_ListView[i].draggedFrom].NameDisplay = listTemp[Engine_ListView[i].row].NameDisplay;
        //                            listTemp[Engine_ListView[i].row].NameDisplay = tmpPopueInt3;

        //                            var tmpPopueInt5 = listTemp[Engine_ListView[i].draggedFrom].GroupNum;
        //                            listTemp[Engine_ListView[i].draggedFrom].GroupNum = listTemp[Engine_ListView[i].row].GroupNum;
        //                            listTemp[Engine_ListView[i].row].GroupNum = tmpPopueInt5;
        //                        }
        //                    }
        //                }
        //                EditorGUILayout.EndVertical();

        //                //第一组永远存在,不能被删除
        //                if ("Group0" != AllModelsLogic.GroupName[i] && GUILayout.Button("删除组"))
        //                {
        //                    for (int j = 0; j < GlobalVar._AllModelsLogic.GetModelList().Count; j++)
        //                    {
        //                        if (i <= GlobalVar._AllModelsLogic.GetModelList()[j].GroupNum)
        //                        {
        //                            GlobalVar._AllModelsLogic.GetModelList()[j].GroupNum -= 1;
        //                        }
        //                    }
        //                    AllModelsLogic.GroupName.RemoveAt(i);
        //                    GroupFoldOutFlag.RemoveAt(i);
        //                    GroupCountChangeFlag = true;
        //                    IsDataInitFlag = false;
        //                }
        //                EditorGUILayout.EndHorizontal();

        //                EditorGUILayout.EndVertical();
        //            }
        //        }
        //        EditorGUILayout.EndVertical();
        //    }

        //    public static void SetSelectRow(GameObject gameObj)
        //    {
        //        InspectorSelectGameobject = gameObj;
        //    }

        //    private int GetSelectRow()
        //    {
        //        int row = -1;
        //        for (int i = 0; i < GlobalVar._AllModelsLogic.GetModelList().Count; i++)
        //        {
        //            if (InspectorSelectGameobject == GlobalVar._AllModelsLogic.GetModelList()[i].GameObj)
        //            {
        //                row = i;
        //                break;
        //            }
        //        }
        //        return row;
        //    }

        //    private static string GetRowText(List<ModelClass> list, ListViewElement el)
        //    {
        //        return list[el.row].Name;
        //    }

    }
}



