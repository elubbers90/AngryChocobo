using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScalingButton : Button, IPointerEnterHandler, IPointerExitHandler {
    private Coroutine currentRoutine;

    public override void OnPointerEnter(PointerEventData eventData) {
        if (interactable) {
            if (currentRoutine != null) {
                StopCoroutine(currentRoutine);
            }
            currentRoutine = StartCoroutine(ScaleObject(true));
        } else {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //Do this when the cursor exits the rect area of this selectable UI object.
    public override void OnPointerExit(PointerEventData eventData) {
        if (interactable) {
            if (currentRoutine != null) {
                StopCoroutine(currentRoutine);
            }
            currentRoutine = StartCoroutine(ScaleObject(false));
        } else {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    IEnumerator ScaleObject(bool smaller) {
        float lastTime = Time.realtimeSinceStartup;
        if (smaller) {
            for (float i = 1f; i >= 0.9f; i -= ((Time.realtimeSinceStartup - lastTime) * 5)) {
                lastTime = Time.realtimeSinceStartup;
                transform.localScale = new Vector3(i, i, i);
                yield return null;
            }
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        } else {
            for (float i = 0.9f; i <= 1; i += ((Time.realtimeSinceStartup - lastTime) * 5)) {
                lastTime = Time.realtimeSinceStartup;
                transform.localScale = new Vector3(i, i, i);
                yield return null;
            }
            transform.localScale = new Vector3(1, 1, 1);
        }

        currentRoutine = null;
    }
}
