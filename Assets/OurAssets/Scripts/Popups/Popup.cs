using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _outerImage;
    [SerializeField] private Image _contentsImage;

    private Camera _mainCamera;

    public event System.Action<Popup> OnPopupDestroyed;

    private void Start()
    {
        _mainCamera = GameManager.Instance.MainCamera;
        _canvas.worldCamera = _mainCamera;

        // Start with the window minimized
        // TODO
    }

    private void OnDestroy()
    {
        OnPopupDestroyed?.Invoke(this);
    }

    public void Initialize(Color color/*, Vector2 size, Vector3 position*/)
    {
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
            // TODO
            yield return null;
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
}
