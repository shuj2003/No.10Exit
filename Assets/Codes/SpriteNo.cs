//  SpriteNo.cs
//  http://kan-kikuchi.hatenablog.com/entry/SpriteNo
//
//  Created by kan.kikuchi on 2019.09.09.

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Sprite�Ő�����\������N���X
/// </summary>
public abstract class SpriteNo<T> : MonoBehaviour where T : Component
{

    //����(02�݂����ݒ������̂�string�Őݒ�)
    [SerializeField]
    private string _text = "";

    //�����ς݂̃R���|�[�l���g
    [SerializeField, HideInInspector]
    protected List<T> _componentList = new List<T>();
    public int Length => _componentList.Count;

    //�F
    [SerializeField]
    private Color _color = Color.white;

    //�񂹂̐ݒ�
    public enum LayoutType
    {
        Center, Left, Right
    }
    [SerializeField]
    private LayoutType _layoutType = LayoutType.Center;

    //�����̊Ԋu
    [SerializeField]
    private float _textSpan = 1f;

    //�e�����̃X�v���C�g
    [SerializeField]
    private List<Sprite> _spriteList = new List<Sprite>();

    //=================================================================================
    //������
    //=================================================================================
#if UNITY_EDITOR

    //OnValidate�����s���ꂽ��
    private bool _isExecutedOnValidate = false;

    //�X�N���v�g�����[�h���ꂽ����C���X�y�N�^�[�̒l���ύX���ꂽ���i�G�f�B�^�[��̂݁j
    private void OnValidate()
    {
        //Hierarchy��ŃA�N�e�B�u�łȂ���΃X���[(Prefab�̓��e���X�V�������Ɏ��s����Ȃ��悤��)
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        //�N�����Ɏ��s������̓X���[(transform.SetParent�ŃG���[���o��̂�)
        if (EditorApplication.isPlayingOrWillChangePlaymode && !_isExecutedOnValidate)
        {
            _isExecutedOnValidate = true;
            return;
        }

        //�����̃X�v���C�g��10��(0 ~ 9)�ɂȂ�悤�ɒ���
        while (_spriteList.Count != 10)
        {
            if (_spriteList.Count > 10)
            {
                _spriteList.RemoveAt(_spriteList.Count - 1);
            }
            else
            {
                _spriteList.Add(null);
            }
        }

        //�����ς݂̃R���|�[�l���g��S�č폜���A�V���ɍ�蒼��(OnValidate�ł�Destroy�o���Ȃ��̂�1�t���[����)
        EditorApplication.delayCall += () => {
            //�V�[���Đ�����߂��Ă�������null�ɂȂ鎖������̂Ń`�F�b�N
            if (this == null)
            {
                return;
            }

            _componentList.Where(component => component != null).ToList().ForEach(component => DestroyImmediate(component.gameObject));
            _componentList.Clear();
            SetText(_text, true);
        };

    }

#endif

    //�SComponent�̏�����
    protected void InitComponents()
    {
        _componentList.ForEach(component => InitComponent(component));
    }

    //Component�̏�����
    protected abstract void InitComponent(T component);

    //=================================================================================
    //�X�V
    //=================================================================================

    //�SComponent���X�V
    private void UpdateComponents()
    {
        UpdateSprites();
        UpdatePositions();
    }

    //�X�v���C�g�X�V
    private void UpdateSprites()
    {
        for (int i = 0; i < _componentList.Count; i++)
        {
            int no = int.Parse(_text[i].ToString());
            UpdateComponent(_componentList[i], _spriteList[no], _color);
        }
    }

    //�eComponent���X�V
    protected abstract void UpdateComponent(T component, Sprite sprite, Color color);

    //�ʒu�X�V
    private void UpdatePositions()
    {
        int textNum = _text.Length;

        for (int i = 0; i < _componentList.Count; i++)
        {
            Vector3 position = Vector3.zero;

            if (_layoutType == LayoutType.Center)
            {
                position.x = ((float)i - (textNum - 1) / 2f) * _textSpan;
            }
            else if (_layoutType == LayoutType.Left)
            {
                position.x = i * _textSpan;
            }
            else if (_layoutType == LayoutType.Right)
            {
                position.x = -(textNum - 1 - i) * _textSpan;
            }

            _componentList[i].transform.localPosition = position;
        }
    }

    //=================================================================================
    //�ݒ�ύX
    //=================================================================================

    /// <summary>
    /// �񂹂̐ݒ��ύX����
    /// </summary>
    public void ChangeColor(Color color)
    {
        _color = color;
        UpdateSprites();
    }

    /// <summary>
    /// �񂹂̐ݒ��ύX����
    /// </summary>
    public void ChangeLayout(LayoutType layoutType)
    {
        _layoutType = layoutType;
        UpdatePositions();
    }

    /// <summary>
    /// �����̊Ԋu��ύX����
    /// </summary>
    public void ChangeSpan(float textSpan)
    {
        _textSpan = textSpan;
        UpdatePositions();
    }

    //=================================================================================
    //�e�L�X�g�ݒ�
    //=================================================================================

    /// <summary>
    /// �e�L�X�g�𐔎��Őݒ肷��
    /// </summary>
    public void SetNo(int no)
    {
        if (no < 0)
        {
            Debug.LogError($"{no}��0�ȏ�ɂ���K�v������܂�");
            return;
        }
        SetText(no.ToString());
    }

    /// <summary>
    /// �e�L�X�g�𐔎��Őݒ肷��(textFormat���󂩂ǂ����̔�������Ȃ��悤(���דI��)�ɕʃ��\�b�h��)
    /// </summary>
    public void SetNo(int no, string textFormat)
    {
        SetText(no.ToString(textFormat));
    }

    //�e�L�X�g�Ő�����ݒ肷��
    private void SetText(string text, bool isForcibly = false)
    {
        //�e�L�X�g���ς���ĂȂ��ꍇ�̓X���[(isForcibly���L���Ȏ��͏�ɐݒ�)
        if (_text == text && !isForcibly)
        {
            return;
        }
        _text = text;

        //�������ɃR���|�[�l���g������Ȃ���΍쐬�A������΍폜
        while (_componentList.Count != text.Length)
        {
            if (_componentList.Count > text.Length)
            {
                var component = _componentList[0];
                _componentList.Remove(component);

                if (Application.isPlaying)
                {
                    Destroy(component.gameObject);
                }
                else
                {
                    DestroyImmediate(component.gameObject);
                }
            }
            else
            {
                GameObject child = new GameObject(_componentList.Count.ToString());
                child.transform.SetParent(transform, false);

                var newRenderer = child.AddComponent<T>();
                _componentList.Add(newRenderer);

                InitComponent(newRenderer);
            }

        }

        //�SComponent���X�V
        UpdateComponents();
    }

}