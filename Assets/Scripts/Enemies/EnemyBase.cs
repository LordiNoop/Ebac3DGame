using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Animation;
using Unity.VisualScripting;

namespace Enemy
{
    public class EnemyBase : MonoBehaviour, IDamageable
    {
        public Collider collider;
        public  FlashColor flashColor;
        public ParticleSystem particleSystem;

        public float startLife = 10f;
        public bool LookAtPlayer = false;

        [SerializeField] protected float _currentLife;

        [Header("Animation")]
        [SerializeField] private AnimationBase _animationBase;

        [Header("Start Animation")]
        public float startAnimationDuration = .2f;
        public Ease startAnimationEase = Ease.OutBack;
        public bool startWithBornAnimation = true;

        private Player _player;

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            _player = GameObject.FindObjectOfType<Player>();
        }

        public virtual void Update()
        {
            if (LookAtPlayer)
            {
                transform.LookAt(_player.transform.position);
            }
        }

        protected void ResetLife()
        {
            _currentLife = startLife;
        }

        protected virtual void Init()
        {
            ResetLife();

            if (startWithBornAnimation)
            {
                BornAnimation();
            }
        }

        protected virtual void Kill()
        {
            OnKill();
        }

        protected virtual void OnKill()
        {
            if (collider != null) collider.enabled = false;

            Destroy(gameObject, 3f);
            PlayAnimationByTrigger(AnimationType.DEATH);
        }

        public void OnDamage(float f)
        {
            if (flashColor != null) flashColor.Flash();

            if (particleSystem != null) particleSystem.Emit(15);

            transform.position -= transform.forward;

            _currentLife -= f;

            if (_currentLife <= 0 )
            {
                Kill();
            }
        }

        #region ANIMATION

        private void BornAnimation()
        {
            transform.DOScale(0, startAnimationDuration).SetEase(startAnimationEase).From();
        }

        public void PlayAnimationByTrigger(AnimationType animationType)
        {
            _animationBase.PlayAnimationByTrigger(animationType);
        }

        #endregion

        public void Damage(float damage)
        {
            OnDamage(damage);
        }
        
        public void Damage(float damage, Vector3 dir)
        {
            OnDamage(damage);
            transform.DOMove(transform.position - dir, .1f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Player p = collision.transform.GetComponent<Player>();

            if (p != null)
            {
                p.Damage(1);
            }
        }
    }
}
