using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Extensión para poder usar las características de la UI

public class Collectable : MonoBehaviour
{
    //Variable para ir acumulando puntos al pasar por el Collectable
    //int collectableQuantity = 0;

    //Si queremos que nuestra variable que acumula puntos se la misma para cada uno de los gameObject
    //Agregamos Public y Static para que de esta forma la variable sea la misma para cada una de las instancias de Unity (GameObjects) y no independiente
    //De lo contrario como en Unity cada Game Object es una instancia independiente, el contador de puntos no acumularia
    //Debido a que cada instancia inicia como su fuera uno nuevo, lo cual siempre mostraría el valor de uno en el contador
    public static int collectableQuantity = 0;

    //Creamos una publica variable para referenciar un Object de la UI
    //public Text collectableText;

    //Como al collectable hemos convertido en un Prefabs, no podemos asignarle una variable de referencia tipo texto
    //Ya que los prefabs pueden ser usados en varias escenas o niveles por lo que no podemos ligarlo a un objeto especifico de una escena
    //En las versiones más recientes, Unity permite asignarlos manualmente las variables de referencia a cada Prefabs en la herarchy
    //Pero no es recomendable ya que pueden exitir más de 100 prefabs en un juego.
    //Por lo tanto cuando usamos un Prefabs, es mejor no usar variables por referencia
    Text collectableText;


    //Creamos una variable del tipo ParticleSystem para dar inicio a nuestra particula
    ParticleSystem collectablePart;

    //Creamos una variable de tipo Audio Source para acceder al componente y se ejecute cuando interactue
    AudioSource collectableAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Como definimos a nuestra variable que acumula puntos como estatica, se mantiene la cantidad entre escenas
        //Si queremos que se reinicie el contador en cada escena, tenemos que definirla en el Start
        //Ya que en el Start se ejecuta una sola vez al inicio del juego
        collectableQuantity = 0;

        //Inicializamos la variable Text en el Start porque ya no es una variable por referencia
        //Utilizamos Find ya que el componente Text esta en el Object CollectableQuantityText y no en el Object Collectable
        collectableText = GameObject.Find("CollectableQuantityText").GetComponent<Text>();

        //Existe otro find que puede devolver directamente el tipo de dato
        //El inconveniente es que si tenemos más de un objeto con el tipo de dato de queremos, puede devolver cualquiera al azar
        //Por lo que si tenemos más de un objeto con el mismo tipo de dato es mejor usar Find para encontrar directamente el objeto que queremos
        //collectableText = GameObject.FindObjectOfType


        //Para ParticleSystem no podemos usar getComponent directamente ya que es un gameObject
        //Y no un componente del gameObject que representa al coleccionable
        //Usaremos la opción Find(): busca y regresa un objeto de tipo GameObject
        //Recuerda que cuando usamos el nombre de un objeto debe ser igual al que esta en Unity
        //Como collectablePart es un Particle System y Find devuelve un gameObject, entonces recién
        //agregamos el getComponent para acceder al componente de Particle System
        collectablePart = GameObject.Find("CollectableParticle").GetComponent<ParticleSystem>();

        //Podemos inicalizar la variable Audio Source con Find ya que el componente no esta en el Object collectable
        //Sino en el Object Padre de collectable
        //Otra forma de inicializarlo al tener un componente en el Object Padre es con getComponentInParent
        collectableAudio = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Vamos a utilizar Trigger para cuando el Player atraviese el objeto (Collectable) desaparezca
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Como queremos que el Object Player sea que recolecte los collectables
        //Usamos Tag para poder identificar al objeto que vaya a atravesar la colisión
        //Por qué en la función OnCollisionEnter usamos collision.gameObject.tag y aquí no?
        //Porque la función OnCollisionEnter es un Collision2D mientras que Trigger es un Collider2D
        //Por lo tanto desde el Trigger podemos acceder directamente al Tag mientras que OnCollisionEnter debe consultar al gameObject primero
        if (collision.tag == "Player")
        {
            //Para que la particula no se ejecute en una sola posición sino en la posición de cada coleccionable
            //Tenemos que acceder a su posición para que se ejecute en la posición del coleccionable respectivo
            //De esta forma no tenemos que hacer un Particle por cada coleccionable
            collectablePart.transform.position = transform.position;

            //Ejecutamos el Particle cuando interactue con el coleccionable
            collectablePart.Play();

            //Ejecutamos el sonido cuando choquemos con el coleccionable
            collectableAudio.Play();

            //Cuando se cumpla la condición el Objecto Collectable se desactiva
            gameObject.SetActive(false);

            //Forma abreviada de ir sumando más 1 cada vez que se pase por el Collectable
            collectableQuantity++;

            //Una vez tengamos la cantidad de collectables debemos mostrarlos en el Object de la UI
            //collectableQuantity al ser una variable numerica tenemos que convertirla a texto (ToString)
            collectableText.text = collectableQuantity.ToString();

            //Padleft es una función Donde el primer parámetro es el limite de caracteres mínimo, 
            //y el segundo parámetro es el caracter que se colocara en caso de no cumplirse el mínimo de caracteres
            //collectableText.text = collectableQuantity.ToString().PadLeft(2, '0');
        }
    }
}
