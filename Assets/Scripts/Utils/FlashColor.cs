using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class FlashColor : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    [Header("Setup")]
    public Color color = Color.red;
    public float duration = .1f;

    private Color defaultColor;

    private Tween _currTween;

    public string colorParameter = "_EmissionColor";

    private void OnValidate()
    {
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        if (skinnedMeshRenderer == null) skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        if (meshRenderer != null) defaultColor = meshRenderer.material.GetColor("_EmissionColor");
    }

    [NaughtyAttributes.Button]
    public void Flash()
    {
        if (meshRenderer != null && !_currTween.IsActive())
        {
            _currTween = meshRenderer.material.DOColor(color, colorParameter, duration).SetLoops(2, LoopType.Yoyo);
        }

        if (skinnedMeshRenderer != null && !_currTween.IsActive())
        {
            _currTween = skinnedMeshRenderer.material.DOColor(color, colorParameter, duration).SetLoops(2, LoopType.Yoyo);
        }
    }
}
