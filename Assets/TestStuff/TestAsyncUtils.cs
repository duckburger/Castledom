using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.UI;

public static class TestAsyncUtils
{
    public static async Task<Button> GetPressedButton(CancellationToken ct, Button[] buttonsToWatch)
    {
        ct.ThrowIfCancellationRequested();
        CancellationTokenSource cToken = CancellationTokenSource.CreateLinkedTokenSource(ct);
        try
        {
            Task<Button>[] buttonTasks = new Task<Button>[buttonsToWatch.Length];
            for (int i = 0; i < buttonsToWatch.Length; i++)
            {
                buttonTasks[i] = PressedButton(buttonsToWatch[i]);
            }

            Task<Button> pressedButton = await Task.WhenAny(buttonTasks);
            cToken.Cancel();
            return pressedButton.Result;
        }
        finally
        {
            cToken.Dispose();
        }
       
    }

    
    static async Task<Button> PressedButton(Button button)
    {
        bool buttonPressed = false;

        button.onClick.AddListener(() => buttonPressed = true);

        while (!buttonPressed)
        {
            await Task.Yield();
        }

        return button;
    }


}
