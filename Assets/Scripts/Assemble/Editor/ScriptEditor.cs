using UnityEngine;
using UnityEditor;
using System.Collections;

public class ScriptEditor: EditorWindow{


    [MenuItem("Window/ScriptEditor")]
    static void init()
    {
        ScriptEditorDebughelpers.openScriptEditor();
    }
    Rect wr = new Rect(100, 100, 100, 100);
    Rect wr2 = new Rect(300, 100, 100, 100);
    Rect wr3 = new Rect(150, 300, 100, 100);
    Vector2 scrollPos;
    void doWindow(int id)
    {
        GUI.Button(new Rect(0, 30, 100, 50), "Wee!");
        GUI.DragWindow();
    }
    void curveFromTo(Rect wr, Rect wr2, Color color, Color shadow)
    {
        wr.x -= scrollPos.x;
        wr.y -= scrollPos.y;
        wr2.x -= scrollPos.x;
        wr2.y -= scrollPos.y;

        Drawing.DrawBezierLine(
            new Vector2(wr.x + wr.width, wr.y + 3 + wr.height / 2),
            new Vector2(wr.x + wr.width + Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr.y + 3 + wr.height / 2),
            new Vector2(wr2.x, wr2.y + 3 + wr2.height / 2),
            new Vector2(wr2.x - Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr2.y + 3 + wr2.height / 2), shadow, 5, true, 20);
        Drawing.DrawBezierLine(
            new Vector2(wr.x + wr.width, wr.y + wr.height / 2),
            new Vector2(wr.x + wr.width + Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr.y + wr.height / 2),
            new Vector2(wr2.x, wr2.y + wr2.height / 2),
            new Vector2(wr2.x - Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr2.y + wr2.height / 2), color, 2, true, 20);
    }

    void OnGUI()
    {
        Color s = new Color(0.4f, 0.4f, 0.5f);
        curveFromTo(wr, wr2, new Color(0.3f,0.7f,0.4f), s);
        curveFromTo(wr2, wr3, new Color(0.7f,0.2f,0.3f), s);

        scrollPos = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), scrollPos, new Rect(0, 0, 1000, 1000));

        BeginWindows();
        wr = GUI.Window(0, wr, doWindow, "hello");
        wr2 = GUI.Window(1, wr2, doWindow, "world");
        wr3 = GUI.Window(2, wr3, doWindow, "!");
        EndWindows();

        GUI.EndScrollView();
    }
}
