using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class LockedInspector : MonoBehaviour
{
    // MenuItemのためのアトリビュート　"%l" の部分はホットキー（ショートカット）　Ctrl + l で開く（MacはCmd + l)
    [MenuItem("Window/Open Locked Inspector %l")]
    static void LockInspectorAndOpenAnother()
    {
        // InspectorWindowタイプの取得
        var inspectorType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");

        // InspectorWindowのインスタンスを作製し、表示する
        var inspectorInstance = ScriptableObject.CreateInstance(inspectorType) as EditorWindow;
        inspectorInstance.Show();

        // "isLockedプロパティがロックされているか（ウィンドウの鍵マークに対応している）
        var isLocked = inspectorType.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.Public);
        // 作製したインスペクタウィンドウのisLockedをtrueに設定する
        isLocked.GetSetMethod().Invoke(inspectorInstance, new object[] { true });
    }
}