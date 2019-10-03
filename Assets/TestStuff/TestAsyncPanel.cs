using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Threading;

public class TestAsyncPanel : MonoBehaviour
{
    [SerializeField] Button[] panelButtons;

    Button pressedButton;
    CancellationTokenSource newCancellationSource;

    private async void Start()
    {
        try
        {
            newCancellationSource = new CancellationTokenSource();
            CancellationToken token = newCancellationSource.Token;
            pressedButton = await TestAsyncUtils.GetPressedButton(token, panelButtons);
        }
        finally
        {
            Debug.Log($"Pressed button is {pressedButton.name}");
        }
    }

    public void CloseButton()
    {
        Destroy(gameObject);
    }

    public void NoButton()
    {
        Destroy(gameObject);
    }


    private void OnDestroy()
    {
        newCancellationSource.Cancel();
    }

    private void OnDisable()
    {
        newCancellationSource.Cancel();
    }

}
