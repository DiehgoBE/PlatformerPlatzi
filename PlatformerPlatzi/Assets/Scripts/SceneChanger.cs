using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Librería necesaria para implementar los cambios de escenas

public class SceneChanger : MonoBehaviour
{
    //Creamos la función que nos permita cargar la escena que le enviemos
    //Puede ser la misma escena o una nueva escena
    public void ChangeSceneTo(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //Creamos una función que nos permita cambiar a la siguiente escena
    //En vez de cambiar a una escena especifica
    //No le ponemos un parametro a la función porque vamos a cambiar a la siguiente escena y no a una especifica
    public void NextScene()
    {
        //GetActiveScene nos devuelve el nombre de la escena en donde nos encontramos actualmente
        //buildIndex: es le numero de la escena que tiene asignado las escenas en Build Settings
        //Al sumar 1 a buildIndex, estamos indicando que queremos ir a la siguiente escena
        //Dentro de Build Settings debemos tener cuidado en el orden en que queremos agregar nuestras escenas
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
