using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _contentsImage;

    private void Start()
    {
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
