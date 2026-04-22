using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class FullScreen : MonoBehaviour
{
    public Toggle toggle;

    public TMP_Dropdown resolutionsDropDown;
    Resolution[] resolutions;    

    //Detecta si el toggle esta activado o no para el modo pantalla completa
    void Start()
    {
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        CheckResolution();
    }
    void Update()
    {
        
    }

     //Segun el estado del toggle activa o desactiva la pantalla completa
    public void ActiveFullScreen(bool isfullScreen)
    {
        Screen.fullScreen = isfullScreen;
    }

    public void CheckResolution()
{
    // Obtener todas las resoluciones disponibles
    Resolution[] allResolutions = Screen.resolutions;
    resolutionsDropDown.ClearOptions();
    
    // Primera lista para las opciones de texto y segunda lista para resoluciones filtradas
    List<string> options = new List<string>();
    List<Resolution> filteredResolutions = new List<Resolution>();

    int actualResolutionIndex = 0;

    for (int i = 0; i < allResolutions.Length; i++)
    {
        // Creamos el string del formato actual
        string option = allResolutions[i].width + " x " + allResolutions[i].height;

        // Agrega a la lista las opciones que no esten
        // Tambien filtra las tasas de refresco repetidas(Hz)
        if (!options.Contains(option))
        {
            options.Add(option);
            filteredResolutions.Add(allResolutions[i]);

            // Verifica si es la resolucion actual y la marca en el DropDown
            if (allResolutions[i].width == Screen.currentResolution.width && 
                allResolutions[i].height == Screen.currentResolution.height)
            {
                actualResolutionIndex = options.Count - 1;
            }
        }
    }

    // Actualiza nuestra variable global de resoluciones con la lista filtrada
    resolutions = filteredResolutions.ToArray();
    
    resolutionsDropDown.AddOptions(options);
    resolutionsDropDown.value = actualResolutionIndex;
    resolutionsDropDown.RefreshShownValue();
}

    public void ChangeResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.FullScreenWindow);

        Debug.Log("Cambiando resolución a: " + resolution.width + "x" + resolution.height);
    }
}
