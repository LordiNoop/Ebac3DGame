using Cloth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthBase : MonoBehaviour, IDamageable
{
    public float startLife = 10f;
    public bool destroyOnKill = false;
    [SerializeField] protected float _currentLife;

    public Action<HealthBase> OnDamage;
    public Action<HealthBase> OnKill;

    public List<UIFillUpdater> uiFillUpdaters;

    public float damageMultiply = 1;



    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        LoadPlayerHealth();
        SaveManager.Instance.healthBase = GameObject.FindWithTag("Player").GetComponent<HealthBase>();
    }

    private void LoadPlayerHealth()
    {
        if (gameObject.CompareTag("Player"))
        {
            _currentLife = SaveManager.Instance.Setup.life;
            UpdateUI();
        }
    }

    private void Update()
    {
        if (this.transform.position.y < -100 && _currentLife > 0)
        {
            _currentLife = 0;
            Damage(0);
        }
    }

    public void Init()
    {
        ResetLife();
    }

    public float Life
    {
        get { return _currentLife; }
    }

    public void ResetLife()
    {
        _currentLife = startLife;
    }

    protected virtual void Kill()
    {
        if (destroyOnKill)
            Destroy(gameObject, 3f);

        OnKill?.Invoke(this);
    }

    [NaughtyAttributes.Button]
    public void Damage()
    {
        Damage(5);
    }

    public void Damage(float f)
    {
        _currentLife -= f * damageMultiply;

        if (_currentLife <= 0)
        {
            Kill();
        }

        OnDamage?.Invoke(this);
        UpdateUI();
    }

    public void Damage(float damage, Vector3 dir)
    {
        Damage(damage);
    }

    public void UpdateUI()
    {
        for (int i = 0; i < uiFillUpdaters.Count; i++)
        {
            if (uiFillUpdaters[i] != null)
            {
                uiFillUpdaters[i].UpdateValue((float)_currentLife / startLife);
            }
        }
    }

    public void ChangeDamageMultiply(float damageMultiply, float duration)
    {
        StartCoroutine(ChangeDamageMultiplyCoroutine(damageMultiply, duration));
    }

    IEnumerator ChangeDamageMultiplyCoroutine(float damageMultiply, float duration)
    {
        this.damageMultiply = damageMultiply;
        yield return new WaitForSeconds(duration);
        this.damageMultiply = 1;
    }
}
