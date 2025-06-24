using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyShoot : EnemyBase
    {
        public GunBase gunBase;

        protected override void Init()
        {
            base.Init();

            gunBase.Invoke("StartShoot", 1f);
        }

        public override void Update()
        {
            base.Update();

            if (_currentLife <= 0)
            {
                gunBase.StopShoot();
            }
        }
    }
}
