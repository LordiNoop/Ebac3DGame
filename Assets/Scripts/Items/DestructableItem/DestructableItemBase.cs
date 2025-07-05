using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableItemBase : MonoBehaviour
{
    public HealthBase healthBase;

    public float shakeDuration = .1f;
    public float shakeForce = 1;

    public int dropCoinsAmount = 10;
    public GameObject coinPrefab;
    public Transform dropPosition;

    private void OnValidate()
    {
        if (healthBase == null) healthBase = GetComponent<HealthBase>();
    }

    private void Awake()
    {
        OnValidate();
        healthBase.OnDamage += OnDamage;
    }

    private void OnDamage(HealthBase h)
    {
        transform.DOShakePosition(shakeDuration, new Vector3(shakeForce, shakeForce, shakeForce)).SetLoops(3, LoopType.Yoyo);
        DropGroupOfCoins();
    }

    [NaughtyAttributes.Button]
    private void DropCoins()
    {
        var i = Instantiate(coinPrefab);
        i.transform.position = dropPosition.transform.position;
        i.transform.DOScale(0, .7f).SetEase(Ease.OutBack).From();
    }

    private void DropGroupOfCoins()
    {
        StartCoroutine(DropGroupOfCoinsCoroutine());
    }

    IEnumerator DropGroupOfCoinsCoroutine()
    {
        for (int i = 0; i < dropCoinsAmount; i++)
        {
            DropCoins();
            yield return new WaitForSeconds(.1f);
        }
    }
}
