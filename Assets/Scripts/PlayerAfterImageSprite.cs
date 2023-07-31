using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField]
    private float _activeTime = 0.1f;
    private float _timeActivated;
    private float _alpha;
    private float _alphaSet = 0.8f;
    private float _alphaMultiplier = 0.85f;

    private Transform _player;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _playerSpriteRenderer;
    private Color _color;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();

        _alpha = _alphaSet;
        _spriteRenderer.sprite = _playerSpriteRenderer.sprite;
        transform.position = _player.position;
        transform.rotation = _player.rotation;
        _timeActivated = Time.time;
    }

    private void Update()
    {
        _alpha *= _alphaMultiplier;
        _color = new Color(1, 1, 1, _alpha);
        _spriteRenderer.color = _color;

        if(Time.time >= (_timeActivated + _activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}