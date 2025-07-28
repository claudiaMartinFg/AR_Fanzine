using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class TrackImage_fanzine : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager trackedImage;
    public GameObject JuanImage;
    public GameObject NataliaImage;
    private void OnEnable()
    {
        trackedImage.trackedImagesChanged += OnChanged;
    }

    private void OnDisable()
    {
        trackedImage.trackedImagesChanged -= OnChanged;
    }
    private void OnChanged(ARTrackedImagesChangedEventArgs eventsArgs)
    {
        foreach(var newImage in eventsArgs.added)
        {
            //Cuando detecte una imagen
            Debug.Log(newImage.referenceImage.name);

            if (newImage.referenceImage.name == "JuanImage")
            {
                GameObject instantiatedImage = Instantiate(JuanImage, newImage.transform.position, newImage.transform.rotation, newImage.transform);

                Vector2 size = newImage.size;

                Transform visualTransform = instantiatedImage.transform.Find("JuanImage");
                RectTransform rectTransform = visualTransform.GetComponent<RectTransform>();

                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = size * 200f;
                }
            }
            else
            {
                //natalia image
            }

        }

        foreach (var updatedImage in eventsArgs.updated)
        {
            //Cuando se actualice
            Debug.Log("Actualizo una imagen");
        }

        foreach (var removedImage in eventsArgs.removed)
        {
            //Cuando deje de verla
            Debug.Log("Quito una imagen");
        }
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            ListAllImages();
        }*/
        
    }

    void ListAllImages()
    {
        Debug.Log($"There are {trackedImage.trackables.count} images being tracked.");

        foreach (var trackedImage in trackedImage.trackables)
        {
            Debug.Log($"Image: {trackedImage.referenceImage.name} is at " +
                      $"{trackedImage.transform.position}");
        }
    }
}
