using UnityEngine;

namespace BallCrush
{
    public class SpecialBlock : BaseBlock
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            Health = 1;
        }


        public override void TakeDamage(int damage = 1)
        {
            Health -= damage;
            if (Health < 1)
            {
                BallSpawner.Instance.AddBall();
                Destroy(this.gameObject);
            }
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((ballLayer.value & (1 << collision.gameObject.layer)) != 0)
            {
                TakeDamage();
            }
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}

