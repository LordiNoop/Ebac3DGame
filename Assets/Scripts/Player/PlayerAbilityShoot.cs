using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityShoot : PlayerAbilityBase
{
    public List<GunBase> gunBase;
    public Transform gunPosition;

    private GunBase _currentGun;

    public int index { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && index != 0)
        {
            index = 0;
            CreateGun();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && index != 1)
        {
            index = 1;
            CreateGun();
        }
    }

    protected override void Init()
    {
        index = 0;

        base.Init();

        CreateGun();

        inputs.Gameplay.Shoot.performed += ctx => StartShoot();
        inputs.Gameplay.Shoot.canceled += ctx => CancelShoot();
    }

    private void CreateGun()
    {
        if (_currentGun != null) Destroy(_currentGun);

        _currentGun = Instantiate(gunBase[index], gunPosition);

        _currentGun.transform.localPosition = _currentGun.transform.localEulerAngles = Vector3.zero;
    }

    private void StartShoot()
    {
        _currentGun.StartShoot();
        //Debug.Log("Start Shoot");
    }

    private void CancelShoot()
    {
        _currentGun.StopShoot();
        //Debug.Log("Cancel Shoot");
    }
}
