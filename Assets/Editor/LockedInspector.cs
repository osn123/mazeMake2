using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class LockedInspector : MonoBehaviour
{
    // MenuItem�̂��߂̃A�g���r���[�g�@"%l" �̕����̓z�b�g�L�[�i�V���[�g�J�b�g�j�@Ctrl + l �ŊJ���iMac��Cmd + l)
    [MenuItem("Window/Open Locked Inspector %l")]
    static void LockInspectorAndOpenAnother()
    {
        // InspectorWindow�^�C�v�̎擾
        var inspectorType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");

        // InspectorWindow�̃C���X�^���X���쐻���A�\������
        var inspectorInstance = ScriptableObject.CreateInstance(inspectorType) as EditorWindow;
        inspectorInstance.Show();

        // "isLocked�v���p�e�B�����b�N����Ă��邩�i�E�B���h�E�̌��}�[�N�ɑΉ����Ă���j
        var isLocked = inspectorType.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.Public);
        // �쐻�����C���X�y�N�^�E�B���h�E��isLocked��true�ɐݒ肷��
        isLocked.GetSetMethod().Invoke(inspectorInstance, new object[] { true });
    }
}