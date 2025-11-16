using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[System.Serializable]
public class ImagePrefabPair
{
    // El nombre exacto de la imagen en la Reference Library.
    [Tooltip("El nombre exacto de la imagen en la Reference Library")]
    public string imageName;
    // El Prefab que instanciar para esa imagen.
    [Tooltip("El Prefab que instanciar para esa imagen")]
    public GameObject prefab;
}

public class TrackImage_fanzine : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager trackedImage;

    [SerializeField] private List<ImagePrefabPair> imagePrefabPairs; //Lista para hacer puente, se edita en el inspector.

    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>(); //Diccionario para saber que prefab le pertenece a cada imagen.

    //private Dictionary<string, GameObject> instantiatedObjects = new Dictionary<string, GameObject>(); //Diccionario para saber que hay instanciado

    private Dictionary<TrackableId, GameObject> instantiatedObjects = new Dictionary<TrackableId, GameObject>();
    void Awake()
    {
        //Para llenar la info del diccionario
        foreach (var pair in imagePrefabPairs)
        {
            prefabDictionary.Add(pair.imageName, pair.prefab);
        }
    }

    private void OnEnable()
    {
        trackedImage.trackedImagesChanged += OnChanged;
    }

    private void OnDisable()
    {
        trackedImage.trackedImagesChanged -= OnChanged;
    }
    private void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            //Busca el nombre de la imagen dentro del diccionario.
            if (prefabDictionary.TryGetValue(newImage.referenceImage.name, out GameObject prefabToInstantiate))
            {
                //Si encuentra el nombre instancia el prefab y lo guarda en la lista de objetos instanciados.
                GameObject instantiatedObject = Instantiate(prefabToInstantiate, newImage.transform.position, newImage.transform.rotation, newImage.transform);
               // instantiatedObjects[newImage.referenceImage.name] = instantiatedObject;
                instantiatedObjects[newImage.trackableId] = instantiatedObject;

                Debug.Log($"Instanciado y guardado: {instantiatedObject.name}");

                var videoPlayer = instantiatedObject.GetComponentInChildren<VideoPlayer>();
                if (videoPlayer != null)
                {
                    //Ajusta la escala del Quad para que coincida con el tamaño de la imagen física.
                    float aspectRatio = (float)videoPlayer.width / videoPlayer.height;
                    instantiatedObject.transform.localScale = new Vector3(newImage.size.x, newImage.size.x / aspectRatio, 1f);
                }
                

                Debug.Log($"Hecho: Instanciado {prefabToInstantiate.name} sobre {newImage.referenceImage.name}");
                
            }
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            //Buscamos el objeto correspondiente en nuestro diccionario de instanciados.
            /*if (instantiatedObjects.TryGetValue(updatedImage.referenceImage.name, out GameObject instance))
            {
                //Comprobamos si se sigue trackeando bien o ha perdido el rastro.
                bool isTracking = updatedImage.trackingState == TrackingState.Tracking;

                //Activamos o desactivamos el objeto según el estado del tracking.
                instance.SetActive(isTracking);

                //Si la estamos trackeando, actualizamos también su posición por si se ha movido.
                if (isTracking)
                {
                    instance.transform.SetPositionAndRotation(updatedImage.transform.position, updatedImage.transform.rotation);
                }
            }*/

            if (instantiatedObjects.TryGetValue(updatedImage.trackableId, out GameObject instance))
            {
                bool isTracking = updatedImage.trackingState == TrackingState.Tracking;
                instance.SetActive(isTracking);

                if (isTracking)
                {
                    instance.transform.SetPositionAndRotation(
                        updatedImage.transform.position,
                        updatedImage.transform.rotation
                    );
                }
            }


        }

        foreach (var removedImage in eventArgs.removed)
        {
            //Busca su objeto, lo destruye y lo quita del diccionario para limpiar la memoria.
            if (instantiatedObjects.TryGetValue(removedImage.trackableId, out GameObject instance))
            {
                Destroy(instance);
                // instantiatedObjects.Remove(removedImage.referenceImage.name);
                instantiatedObjects.Remove(removedImage.trackableId);

                Debug.Log($"Destruido y eliminado: {instance.name}");
            }
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
