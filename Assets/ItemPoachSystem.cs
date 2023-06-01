using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ItemPoachSystem : MonoBehaviour
{
    ChildObjectsManager _objectsGetter;
    [SerializeField] GameObject[] _test = null;
    // Start is called before the first frame update
    void Start()
    {
        _objectsGetter = new ChildObjectsManager();
    }

    // Update is called once per frame
    void Update()
    {
        _test = _objectsGetter.GetChildObjects(gameObject);
        _objectsGetter.MakeChildToParent(_test);
    }
}

/// <summary>
/// �C���x���g���p�̃I�u�W�F�N�g�̔z���ɂ���q�I�u�W�F�N�g�̃}�l�[�W���[�N���X
/// </summary>
public class ChildObjectsManager
{
    /// <summary>
    /// �q�I�u�W�F�N�g�̎擾������GameObject�^�ŕԂ��֐�
    /// </summary>
    /// <param name="parentObject"></param>
    /// <returns></returns>
    public GameObject[] GetChildObjects(GameObject parentObject)
    {
        // �q�I�u�W�F�N�g���i�[����z��쐬
        var _children = new Transform[parentObject.transform.childCount];//�Y�������ϓ�����
        int childIndex = 0;
        GameObject[] _returnObjects = new GameObject[parentObject.transform.childCount];//�Y�������ϓ�����
        int convertingIndex = 0;
        // �q�I�u�W�F�N�g�����Ԃɔz��Ɋi�[
        foreach (Transform child in parentObject.transform)
        {
            if(child != null)
                _children[childIndex++] = child;
            //Debug.Log("child index" + childIndex);
        }
        //�q�I�u�W�F�N�g�����ԂɃg�����X�t�H�[���^����Q�[���I�u�W�F�N�g�^�ɕϊ��A�Ԃ�l�Ɋi�[
        foreach(Transform child in _children)
        {
            if(child != null)
                _returnObjects[convertingIndex++] = child.gameObject;
            //Debug.Log("conv index" + convertingIndex);
        }
        return _returnObjects;
    }

    /// <summary>
    /// �e�q�֌W��؂�֐� ������GameObject�^
    /// </summary>
    /// <param name="childObject"></param>
    public void MakeChildToParent(GameObject childObject)
    {
        if(childObject != null)
        {
            childObject.gameObject.transform.parent = null;
        }
    }

    /// <summary>
    /// �e�q�֌W��؂�֐� ������GameObject[]�^
    /// </summary>
    /// <param name="childObjects"></param>
    public void MakeChildToParent(GameObject[] childObjects)
    {
        if (childObjects != null)
        {
            for(int i = 0; i < childObjects.Length; i++)
            {
                childObjects[i].gameObject.transform.parent = null;
            }
        }
    }
}