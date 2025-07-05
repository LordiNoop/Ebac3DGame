using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyShoot : EnemyBase
    {
        public GunBase gunBase;

        public float dist = 8f;
        private bool _isShooting = false;

        protected override void Init()
        {
            base.Init();
        }

        public override void Update()
        {
            base.Update();

            if (_currentLife <= 0)
            {
                gunBase.StopShoot();
            }

            if (Vector3.Distance(transform.position, Player.Instance.transform.position) < dist && _isShooting == false)
            {
                gunBase.Invoke("StartShoot", 1f);
                _isShooting = true;
            }

            if (Vector3.Distance(transform.position, Player.Instance.transform.position) > dist)
            {
                gunBase.StopShoot();
                _isShooting = false;
            }
        }
    }
}
