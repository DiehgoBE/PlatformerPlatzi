using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    //Creamos una variable para referenciar al Object SceneChanger
    //Scenechanger no es un Object al cual vamos acceder a su componente 
    //sino un Object para acceder a las funciones que definimos en el código por lo cual podemos referenciarlo desde el Prefab Scene
    public SceneChanger changeScene;

    //Creamos la función OnTrigger para que cuando el Object Player choque con la colisión de las particulas
    //Se cambia de escena ya sea a un nuevo nivel o se reinicie el nivel
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            changeScene.NextScene();
        }
    }
}
