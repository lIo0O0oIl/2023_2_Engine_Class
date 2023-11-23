using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using DG.Tweening;
using System;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private SpriteLibraryAsset[] _spriteAssets;
    [SerializeField] private SpriteRenderer _screenWave;

    private readonly int _hasWaveDistance = Shader.PropertyToID("_WaveDistance");
    private Material _material;

    private SpriteLibrary _spriteLibrary;

    private int _currentSpriteIndex = 0;
    private void Awake()
    {
        _spriteLibrary = GetComponent<SpriteLibrary>();
        _material = _screenWave.GetComponent<SpriteRenderer>().material;
    }

    private void SetNextSprite()
    {
        _currentSpriteIndex = (_currentSpriteIndex + 1) % _spriteAssets.Length;
        _spriteLibrary.spriteLibraryAsset = _spriteAssets[_currentSpriteIndex];

        ShockWaveEffect();
    }

    private Tween _shockTween = null;
    private void ShockWaveEffect()
    {
        if (_shockTween != null && _shockTween.IsActive())
        {
            _shockTween.Kill();
        }

        Material mat = _screenWave.material;
        _screenWave.gameObject.SetActive(true);

        mat.SetFloat(_hasWaveDistance, -0.1f);
        //_shockTween = DOTween.To(() => mat.GetFloat(_hasWaveDistance), value => mat.SetFloat(_hasWaveDistance, value), 1f, 0.3f).Complete(() => _screenWave.gameObject.SetActive(false));
        _shockTween = DOTween.To(() => mat.GetFloat(_hasWaveDistance), value => mat.SetFloat(_hasWaveDistance, value), 1f, 0.3f).OnComplete(() => _screenWave.gameObject.SetActive(false));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetNextSprite();
        }
    }
}
