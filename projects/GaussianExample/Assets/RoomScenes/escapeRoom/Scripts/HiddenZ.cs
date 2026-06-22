using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenZ : MonoBehaviour
{
    private int requirements = 2;

    public void TriggerRequirement()
    {
        requirements--;
        if (requirements <= 0) gameObject.SetActive(true);
    }

}
