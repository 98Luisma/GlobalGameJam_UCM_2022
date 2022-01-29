using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _outerImage;
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private Transform _canvasParent;
    [Header("Animation")]
    [SerializeField] private float _openTime = 0.2f;
    [SerializeField] private float _closeTime = 0.2f;
    [SerializeField] private float _elasticity = 2f;

    private Camera _mainCamera;

    public event System.Action<Popup> OnPopupDestroyed;

    private void Start()
    {
        _mainCamera = GameManager.Instance.MainCamera;
        _canvas.worldCamera = _mainCamera;

        // Start with the window minimized
        _canvasParent.localScale = new Vector3(1f, 0f, 1f);
    }

    private void OnDestroy()
    {
        OnPopupDestroyed?.Invoke(this);
    }

    public void Initialize(Color color)
    {
        // Set color
        color.a = 1;
        _outerImage.color = color;

        OpenPopup();
    }

    ///<summary>
    /// Plays an open animation.
    ///</summary>
    private void OpenPopup()
    {
        StartCoroutine(InnerCoroutine());

        // Definition of the Coroutine called above
        IEnumerator InnerCoroutine()
        {
            float timer = 0f;
            while (timer <= _openTime)
            {
                float x = timer / _openTime;
                float t = 1f + (_elasticity+1) * Mathf.Pow(x-1f, 3f) + _elasticity * Mathf.Pow(x-1f, 2f);
                _canvasParent.localScale = new Vector3(1f, t, 1f);

                timer += Time.deltaTime;
                yield return null;
            }
            _canvasParent.localScale = Vector3.one;
        }
    }

    ///<summary>
    /// Plays a close animation and destroys the Object.
    ///</summary>
    private void ClosePopup()
    {
        StopAllCoroutines();
        StartCoroutine(InnerCoroutine());

        // Definition of the Coroutine called above
        IEnumerator InnerCoroutine()
        {
            // TODO
            yield return null;

            // When finished
            Destroy(gameObject);
        }
    }

    public void Btn_Close() => ClosePopup();

    public float GetWidth() => _collider.size.x;

    public float GetHeight() => _collider.size.y;
}
