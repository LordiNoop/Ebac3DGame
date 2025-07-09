using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootLimit : GunBase
{
    public List<UIFillUpdater> uiFillUpdaters;

    public float maxShoot = 5f;
    public float timeToRecharge = 1f;

    private float _currentShoots;
    private bool _recharging = false;

    private void Awake()
    {
        GetAllUIs();
        maxShoot = 10f;
    }

    private void Start()
    {
        for (int i = 0; i < uiFillUpdaters.Count; i++)
        {
            if (uiFillUpdaters[i].gameObject.tag != "AmmoUI")
            {
                uiFillUpdaters.Remove(uiFillUpdaters[i]);
            }
        }
    }

    protected override IEnumerator ShootCoroutine()
    {
        if (_recharging) yield break;

        while (true)
        {
            if (_currentShoots < maxShoot)
            {
                Shoot();
                _currentShoots++;
                CheckRecharge();
                UpdateUI();
                yield return new WaitForSeconds(timeBetweenShoot);
            }
        }
    }

    private void CheckRecharge()
    {
        if (_currentShoots >= maxShoot)
        {
            StopShoot();
            StartRecharge();
        }
    }

    private void StartRecharge()
    {
        _recharging = true;
        StartCoroutine(RechargeCoroutine());
    }

    IEnumerator RechargeCoroutine()
    {
        float time = 0;
        while (time < timeToRecharge)
        {
            time += Time.deltaTime;
            uiFillUpdaters.ForEach(i => i.UpdateValue(time/timeToRecharge));
            yield return new WaitForEndOfFrame();
        }
        _currentShoots = 0;
        _recharging = false;
    }

    private void UpdateUI()
    {
        uiFillUpdaters.ForEach(i => i.UpdateValue(maxShoot, _currentShoots));
    }

    private void GetAllUIs()
    {
        uiFillUpdaters = GameObject.FindObjectsOfType<UIFillUpdater>().ToList();

        for (int i = 0; i < uiFillUpdaters.Count; i++)
        {
            if (uiFillUpdaters[i].gameObject.tag != "AmmoUI")
            {
                uiFillUpdaters.Remove(uiFillUpdaters[i]);
            }
        }
    }
}
