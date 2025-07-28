using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

public class MainMenuManager : MonoBehaviour
{

    private void Start()
    {
        AskConsent();
    }
    public void FakeARButton()
    {
        SceneManager.LoadScene("FakeAR");
    }

    public void ImageTrackingButton()
    {
        SceneManager.LoadScene("TrackingImage");
    }

    public void EnvironmentTrackingButton()
    {
        SceneManager.LoadScene("EnvironmentTracking");
    }

    void AskConsent()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Camera) == false)
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
        else
        {
            //ya tiene permiso
        }
    }

}
