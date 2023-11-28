using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public TopDownPlayerMovement player;
    public void disableInput()
    {
        player.disableInput = true;
    }

    public void enableInput()
    {
        player.disableInput = false;
    }

    public void enableInputAfterTime(float time)
    {
        StartCoroutine(WaitForEnableInput(time));
    }

    IEnumerator WaitForEnableInput(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        player.disableInput = false;
    }
}
