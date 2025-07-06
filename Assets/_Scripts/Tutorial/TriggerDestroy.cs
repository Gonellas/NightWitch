using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDestroy : MonoBehaviour
{
    [Tooltip("Número del trigger (1, 2, 3 o 4)")]
    public int triggerIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TutorialManager tm = FindObjectOfType<TutorialManager>();
            if (tm != null)
            {
                switch (triggerIndex)
                {
                    case 1:
                        tm.OnTrigger1Hit();
                        break;
                    case 2:
                        tm.OnTrigger2Hit();
                        break;
                    case 3:
                        tm.OnTrigger3Hit();
                        break;
                    case 4:
                        tm.OnTrigger4Hit();
                        break;
                    case 5:
                        tm.OnTrigger5Hit();
                        break;
                    default:
                        Debug.LogWarning("Trigger index inválido en " + gameObject.name);
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}
