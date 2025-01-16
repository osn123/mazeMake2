using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;


public class FindSceneObjectsWithTag : EditorWindow
{
    private string oldTagToSearchFor = "";
    private string tagToSearchFor = "";

    private Vector2 scrollPos;
    private Vector2 resultScrollPos;
    private Vector2 objectsScrollPos;

    private Dictionary<string, Pair<bool, List<GameObject>>> sceneObjectsWithTag;
    private List<GameObject> results;

    private bool searchFlag = false;
    private bool listFlag = true;

    enum ActiveOption
    {
        ALL,
        ACTIVE_ONLY,
        INACTIVE_ONLY
    };
    private ActiveOption oldActiveOption = ActiveOption.ALL;
    private ActiveOption activeOption = ActiveOption.ALL;


    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Find GameObjects In Scene With Tag", EditorStyles.boldLabel);

            oldActiveOption = activeOption;
            activeOption = (ActiveOption)EditorGUILayout.EnumPopup("Select Active or Inactive", (System.Enum)activeOption);

            if (GUILayout.Button("Reload") || activeOption != oldActiveOption)
            {
                LoadSceneObjectsWithTag();
                Search();
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);


            //�^�O�����w�肷�邱�Ƃōi�荞��////////////////////////////////////////////////////////////
            if (searchFlag = EditorGUILayout.Foldout(searchFlag, "Search By Tag"))
            {
                EditorGUI.indentLevel++;

                oldTagToSearchFor = tagToSearchFor;
                tagToSearchFor = EditorGUILayout.TagField("Tag To Search:", tagToSearchFor);

                if (tagToSearchFor != oldTagToSearchFor)
                    Search();

                if (results != null)
                {
                    EditorGUILayout.LabelField("Scene Objects Found:", results.Count.ToString(), EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;

                    foreach (GameObject go in results)
                    {
                        EditorGUILayout.ObjectField(go, typeof(GameObject), false);
                    }

                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
            }
            ////////////////////////////////////////////////////////////////////////////////////////////

            EditorGUILayout.Space();

            //���ׂẴV�[�����I�u�W�F�N�g���^�O�ʂɃ��X�g�A�b�v////////////////////////////////////////
            if (listFlag = EditorGUILayout.Foldout(listFlag, "List By Tags"))
            {
                if (sceneObjectsWithTag != null)
                {
                    EditorGUI.indentLevel++;

                    foreach (var item in sceneObjectsWithTag)
                    {

                        if (item.Value.First = EditorGUILayout.Foldout(item.Value.First, item.Key + ":" + item.Value.Second.Count.ToString()))
                        {
                            EditorGUI.indentLevel++;
                            foreach (GameObject go in item.Value.Second)
                            {
                                EditorGUILayout.ObjectField(go, typeof(GameObject), false);
                            }
                            EditorGUI.indentLevel--;
                        }
                    }

                    EditorGUI.indentLevel--;
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////

            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }


    // ���ׂẴI�u�W�F�N�g���擾���^�O�ʂɃ��X�g�Ɋi�[����////////////////////////////////////////////////////////////
    void LoadSceneObjectsWithTag()
    {

        // Dictionary��Key�̓^�O�AValue��Pair�N���X.Pair�̓c���[�\����\���t���O��GameObject���X�g.
        if (sceneObjectsWithTag == null)
            sceneObjectsWithTag = new Dictionary<string, Pair<bool, List<GameObject>>>();

        // �I�u�W�F�N�gDictionaly�̏��������̂P�F���o�^�̃^�O������΂��̃��X�g���쐬
        foreach (string tag in InternalEditorUtility.tags)
        {
            if (!sceneObjectsWithTag.ContainsKey(tag))
            {
                sceneObjectsWithTag.Add(tag, new Pair<bool, List<GameObject>>());
            }
            sceneObjectsWithTag[tag].Second = new List<GameObject>();
        }

        // �I�u�W�F�N�gDictionaly�̏��������̂Q�F�^�O����������Ă���΂��̃��X�g���폜        
        bool existTag = false;
        foreach (string key in sceneObjectsWithTag.Keys)
        {
            foreach (string tag in InternalEditorUtility.tags)
            {
                if (key == tag)
                {
                    existTag = true;
                    break;
                }
                else
                {
                    existTag = false;
                }
            }

            if (!existTag)
            {
                sceneObjectsWithTag.Remove(key);
            }
        }

        // �S�ẴI�u�W�F�N�g��z��Ŏ擾�����ɏ�������
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {

            // �A�Z�b�g����p�X���擾.�V�[����ɑ��݂���I�u�W�F�N�g�̏ꍇ,�V�[���t�@�C���i.unity�j�̃p�X���擾
            string path = AssetDatabase.GetAssetOrScenePath(obj);

            // �V�[����ɑ��݂���I�u�W�F�N�g���ǂ����g���q�Ŕ���
            bool isScene = path.Contains(".unity");
            if (isScene && sceneObjectsWithTag.ContainsKey(obj.tag))
            {
                if (activeOption == ActiveOption.ALL || obj.activeInHierarchy == (activeOption == ActiveOption.ACTIVE_ONLY))
                {  // Active�Ɋւ���I�v�V�����Ɉ�v���邩�`�F�b�N
                    sceneObjectsWithTag[obj.tag].Second.Add(obj);      // �^�O�Ɉ�v�����I�u�W�F�N�g�̓��X�g�ɒǉ�
                }
            }
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Search()
    {

        if (tagToSearchFor != null && tagToSearchFor != "")
        {
            results = sceneObjectsWithTag[tagToSearchFor].Second;
        }

    }


    // �G�f�B�^�[�̃��j���[�o�[�ɒǉ�
    [MenuItem("Tool/Find GameObjects In Scene With Tag...")]
    static void Init()
    {
        FindSceneObjectsWithTag window = EditorWindow.GetWindow<FindSceneObjectsWithTag>("Find With Tag");
        window.LoadSceneObjectsWithTag();
        window.ShowPopup();
    }


    //����Pair�N���X
    private class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    };
}
