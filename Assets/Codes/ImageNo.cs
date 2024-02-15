//  ImageNo.cs
//  http://kan-kikuchi.hatenablog.com/entry/SpriteNo
//
//  Created by kan.kikuchi on 2019.09.09.

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// uGUI��Image�Ő�����\������N���X
/// </summary>
public class ImageNo : SpriteNo<Image>
{

    [SerializeField]
    private bool _raycastTarget = false;

    //=================================================================================
    //������
    //=================================================================================

    //�V���������Image�̏�����
    protected override void InitComponent(Image image)
    {
        image.raycastTarget = _raycastTarget;
    }

    //=================================================================================
    //�X�V
    //=================================================================================

    //Sprite���X�V
    protected override void UpdateComponent(Image image, Sprite sprite, Color color)
    {
        image.sprite = sprite;
        image.color = color;
        image.SetNativeSize();
    }

    //=================================================================================
    //�ݒ�ύX
    //=================================================================================

    /// <summary>
    /// RaycastTarget�̐ݒ��ύX����
    /// </summary>
    public void ChangeRaycastTarget(bool raycastTarget)
    {
        _raycastTarget = raycastTarget;
        InitComponents();
    }

}