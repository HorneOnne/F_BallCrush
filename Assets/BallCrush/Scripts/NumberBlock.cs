using TMPro;
using UnityEngine;


namespace BallCrush
{
    public class NumberBlock : BaseBlock
    {
        [SerializeField] private TextMeshPro _healthText;


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            UpdateHealthText();
        }


        public override void SetHealth(int health)
        {
            base.SetHealth(health);
            UpdateHealthText();
        }

        public override void TakeDamage(int damage = 1)
        {
            Health -= damage;
            if (Health < 1)
            {
                Destroy(this.gameObject);
            }
            UpdateHealthText();
        }

        private void UpdateHealthText()
        {
            _healthText.text = $"{Health}";
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

