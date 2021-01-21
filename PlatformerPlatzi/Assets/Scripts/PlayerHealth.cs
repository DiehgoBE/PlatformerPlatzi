using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Como vamos a usar elementos de la UI, utilizamos la librería necesaria

public class PlayerHealth : MonoBehaviour
{
    //Creamos una variable numerica que represente nuestra salud
    //La vida es 3 por el número de corazones que colocamos en el Canvas
    int health = 3;

    //Creamos un arreglo de tipo imagenes y que sea público para referenciar a los corazones en Unity
    //El arreglo representa a los corazones dentro del canvas
    public Image[] hearts;

    //Creamos una variable booleana que represente un cooldown de nuestra vida para que no se acabe la vida de inmediato
    bool hasCooldown = false;

    //Creamos nuestra variable publica para referenciar al tipo del Script que creamos para el cambio de escena
    public SceneChanger changeScene;


    //Como nuestra funciones creadas no estan dentro de un Start o un Update no se van a ejecutar
    //Pero queremos que nuestra Función SubstractHealth, la cual engloba todas las funciones, se ejecute cuando nuestro personaje
    //Choque contra un enemigo por lo que usamos OnCollisionEnter2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Validamos que cuando nuestro personaje (Collision) choque con el enemigo, el cual lo identificamos a través del Tag
        //De esta forma evitamos que nuestro personaje muera al chocar con cualquier objeto
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Tambien tenemos que validar que debemos chocar con el enemigo desde los lados y no desde arriba para perder vida
            //Podemos usar el valor en Y para validar su posición o podemos utilizar la variable isgrounded que creamos en el Script PlayerMovement
            //Como tenemos que conseguir la variable de PlayerMovement usamos getComponent ya que los Scripts que creamos tambien son componentes
            //GetComponent<PlayerMovement>().isGrounded --> GetComponent<PlayerMovement>().isGrounded == True
            if (GetComponent<PlayerMovement>().isGrounded)
            {
                SubstractHealth();
            }
        }
    }

    //Creamos una función para realizar la resta de la vida de nuestro personaje
    void SubstractHealth()
    {
        //Primero validamos que nuestro personaje no tenga cooldown (False) para realizar la resta de la vida
        if (!hasCooldown)
        {
            //Validamos si nuestra vida aún no llega a 0 para realizar la resta de vida
            //Ya que cuando llegué a 0 se ejecutará una condición de muerte
            if (health > 0)
            {
                //Mientras nuestra vida sea mayor a 0 se resta 1
                //health = health -1;
                health--;

                //Una vez se resta 1 a nuestra vida, el cooldown pasa a ser verdadero
                hasCooldown = true;

                //Para dar inicio a nuestra corrutina utilizamos StartCoroutine
                StartCoroutine(Cooldown());
            }

            //Validamos si nuestra vida es menor o igual a 0
            //Tambien igualamos a 0 porque puede darse que durante la ejecución del juego se de un poco de lag al chocar con dos enemigos
            //por lo que se puede salta de 1 a -1 directamente
            if (health <= 0)
            {
                //Al cumplirse la condicón queremos que se muestre un cambio de escena, a la escena perder
                changeScene.ChangeSceneTo("LoseScene");
            }

            //Independientemente si mi vida es mayor a 0 o no, revisaremos nuestros corazones en el arreglo
            EmptyHearts();
        }
    }

    //Creamos una función para poder ver en la UI, que vamos perdiendo corazones cada vez que perdamos vida
    void EmptyHearts()
    {
        //Utilizamos el Loop For para poder recorrer el arreglo de imagenes
        for (int i = 0; i<hearts.Length; i++)
        {
            //Validamos la vida del personaje - 1 porque lo estamos comporando con la posición de los corazones 
            //en el arreglo que inicia en 0 mientras que health inica en 1
            if (health - 1 < i)
            {
                //Cuando se cumpla la condición se desactivará un corazón en la posición del arreglo que cumpla la condición
                hearts[i].gameObject.SetActive(false);
            }
        }
    }


    //Para poder reiniciar el Cooldown pasado un determinado tiempo, utilizaremos las corrutinas
    //Con la corrutina podremos hacer un timer de manera más sencilla
    //Las corrutinas la definimos al inicio con IEnumerator
    IEnumerator Cooldown()
    {
        
        //Colocamos un yield return wait for seconds para que pasado cierto tiempo se cambie el valor del cooldown
        //Si queremos que cuando nuestro personaje parpadee cuando tiene Cooldown se puede hacer desde aquí, ya que si se hace directamente
        //desde una función, no se mostrará el proceso sino el resultado total mientras que con la corrutina si muestra el proceso hasta que 
        //se cumpla lo definido en el yield return
        yield return new WaitForSeconds(.5f);

        //Pasado el tiempo definido en yield return, nuestro cooldown volverá a False
        hasCooldown = false;

        //Recodar que cuando usamos corrutinas, tenemos que pararlas, sino siguen eternamente
        StopCoroutine(Cooldown());
    }






}
