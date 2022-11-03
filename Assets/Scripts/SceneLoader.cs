using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Wrapper for loading a scene so it can be targeted by a Unity event
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public string Scene;

    public void LoadScene()
    {
        SceneManager.LoadScene(Scene);
    }
}
