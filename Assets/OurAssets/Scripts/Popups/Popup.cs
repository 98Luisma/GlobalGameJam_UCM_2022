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

    private void Start()
    {
        _mainCamera = GameManager.Instance.MainCamera;
        _canvas.worldCamera = _mainCamera;

        // Start with the window minimized
        // TODO
    }

    public void Initialize(Color color/*, Vector2 size, Vector3 position*/)
    {
        _outerImage.color = color;

        OpenPopup();
    }

    private void OpenPopup()
    {
        StartCoroutine(InnerCoroutine());

        // Definition of the Coroutine called above
        IEnumerator InnerCoroutine()
        {
            yield return null;
        }
    }

    private void ClosePopup()
    {
        StopAllCoroutines();
        StartCoroutine(InnerCoroutine());

        // Definition of the Coroutine called above
        IEnumerator InnerCoroutine()
        {
            yield return null;
        }
    }

    public void Btn_Close() => ClosePopup();
}
