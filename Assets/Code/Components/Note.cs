
using Assets.Code.Singletons;
using System;
using UnityEngine;

namespace Assets.Code.Components
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class Note : MonoBehaviour
    {

        public float initialVelocity;
        public bool correct;

        private Rigidbody2D _rigidBody2D;
        private SpriteRenderer _spriteRenderer;
        private float _currentVelocity;

        public void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }

        public void Miss()
        {
            Color color = _spriteRenderer.color;
            color.a = 50f;
            _spriteRenderer.color = color;
        }

        public void Kill()
        {
            Destroy(this.gameObject);
        }

        private void Awake()
        {
            _rigidBody2D = this.GetComponent<Rigidbody2D>();
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
            _currentVelocity = 0;
        }
        private void Start()
        {
            EventSingleton.Events.OnStartGame.AddListener(() =>
            {
                _currentVelocity = initialVelocity;
            });
        }

        private void Update()
        {
            _rigidBody2D.velocity = new Vector2(0, -_currentVelocity);
        }

    }


}
