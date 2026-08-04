using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class TimeManager : MonoBehaviour
{
    [SerializeField] private List<Light> interiorLights;
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private Gradient lightingColor;
    [SerializeField] private Material skybox;
    [SerializeField] private Gradient indoorLight;
    [SerializeField] private Volume volume;
    [SerializeField, Range(0, 24)] private float TimeOfDay;
    public int totalDay;

    // Update is called once per frame
    void Update()
    {
        TimeOfDay += Time.deltaTime / 3;
        TimeOfDay %= 24;
        if(TimeOfDay >= 23.89)
        {
            totalDay++;
            FindAnyObjectByType<Shop_QuestManager>().SpawnQuestNPC();
            TimeOfDay = 0; 
        }
        UpdateLighting(TimeOfDay / 24);
    }

    private void UpdateLighting(float timePercent)
    {
        foreach (Light light in interiorLights)
        {
            light.color = indoorLight.Evaluate(timePercent);
        }

        skybox.SetFloat("_AtmosphereThickness", (-3 * (Mathf.Pow(timePercent, 2)) + (3 * timePercent) + 0.2f));

        DirectionalLight.color = lightingColor.Evaluate(timePercent);
        DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 220f, 0));
    }
}
