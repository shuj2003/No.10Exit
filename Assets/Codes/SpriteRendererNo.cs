//  SpriteRendererNo.cs
//  http://kan-kikuchi.hatenablog.com/entry/SpriteNo
//
//  Created by kan.kikuchi on 2019.09.09.

using UnityEngine;

/// <summary>
/// SpriteRenderer�Ő�����\������N���X
/// </summary>
public class SpriteRendererNo : SpriteNo<SpriteRenderer>
{

    [SerializeField]
    private string _sortingLayerName = "Default";

    [SerializeField]
    private int _sortingOrder = 0;

    //=================================================================================
    //������
    //=================================================================================

    //�V���������SpriteRenderer�̏�����
    protected override void InitComponent(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.sortingLayerName = _sortingLayerName;
        spriteRenderer.sortingOrder = _sortingOrder;
    }

    //=================================================================================
    //�X�V
    //=================================================================================

    //Sprite���X�V
    protected override void UpdateComponent(SpriteRenderer spriteRenderer, Sprite sprite, Color color)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
    }

    //=================================================================================
    //�ݒ�ύX
    //=================================================================================

    /// <summary>
    /// sortingLayerName�̐ݒ��ύX����
    /// </summary>
    public void ChangeSortingLayerName(string sortingLayerName)
    {
        _sortingLayerName = sortingLayerName;
        InitComponents();
    }

    /// <summary>
    /// sortingOrder�̐ݒ��ύX����
    /// </summary>
    public void ChangeSortingOrder(int sortingOrder)
    {
        _sortingOrder = sortingOrder;
        InitComponents();
    }

}