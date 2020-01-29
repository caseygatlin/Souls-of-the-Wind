using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeMine : MonoBehaviour {

    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float xPos = Random.Range(-1f, 1f) * magnitude;
            float yPos = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(xPos, yPos, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }


}
