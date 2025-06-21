using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToggleObject : MonoBehaviour
{
    public GameObject[] targetObjects;     // Array of objects to toggle
    public float toggleInterval = 1.0f;    // Seconds between on/off
    public float dissolveDuration = 0.5f;  // Duration of dissolve effect

    private List<Material> objectMaterials = new List<Material>();
    private List<bool> isDissolving = new List<bool>();
    private List<float> dissolveTimers = new List<float>();
    private List<bool> targetStates = new List<bool>();

    void Start()
    {
        foreach (var obj in targetObjects)
        {
            var renderer = obj.GetComponent<Renderer>();
            objectMaterials.Add(renderer != null ? renderer.material : null);
            isDissolving.Add(false);
            dissolveTimers.Add(0f);
            targetStates.Add(true); // Start as active
        }

        StartCoroutine(ToggleRoutine());
    }

    void Update()
    {
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (isDissolving[i])
            {
                dissolveTimers[i] += Time.deltaTime;
                float progress = Mathf.Clamp01(dissolveTimers[i] / dissolveDuration);

                if (objectMaterials[i] != null)
                {
                    float dissolveAmount = targetStates[i] ? 1 - progress : progress;
                    objectMaterials[i].SetFloat("_DissolveAmount", dissolveAmount);
                }

                if (dissolveTimers[i] >= dissolveDuration)
                {
                    isDissolving[i] = false;
                    targetObjects[i].SetActive(targetStates[i]);

                    if (targetStates[i] && objectMaterials[i] != null)
                    {
                        objectMaterials[i].SetFloat("_DissolveAmount", 0f);
                    }
                }
            }
        }
    }

    IEnumerator ToggleRoutine()
    {
        while (true)
        {
            for (int i = 0; i < targetObjects.Length; i++)
            {
                if (!isDissolving[i])
                {
                    targetStates[i] = !targetObjects[i].activeSelf;

                    if (objectMaterials[i] != null)
                    {
                        targetObjects[i].SetActive(true); // Ensure visible during dissolve
                        isDissolving[i] = true;
                        dissolveTimers[i] = 0f;

                        if (targetStates[i])
                            objectMaterials[i].SetFloat("_DissolveAmount", 1f); // Start fully dissolved
                    }
                    else
                    {
                        targetObjects[i].SetActive(targetStates[i]); // Toggle directly
                    }
                }
            }

            yield return new WaitForSeconds(toggleInterval);
        }
    }
}
