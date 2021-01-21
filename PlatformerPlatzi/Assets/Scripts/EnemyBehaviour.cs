using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    //Vamos a acceder a los componentes de nuestro objeto de una manera distinta
    //Esta vez no usaremos una variable pública que referencie al componente.
    Rigidbody2D enemyRb;

    //Variables que usaremos para el Timer del personaje
    float timeBeforeChange;
    public float delay = .5f; //El tiempo que demora para iniciar el movimiento según el timer

    //Variable para la velocidad del personaje
    public float speed = .3f;

    //Variable para poder acceder a las caracteristicas del componente SpriteRenderer
    SpriteRenderer enemySp;

    //Como creamos un parametro Bool para activar o no la animación de muerte
    //Creamos una variable Animator para poder acceder a sus caracteristicas
    Animator animEnemy;

    //Creamos una variable del tipo Particle System
    ParticleSystem enemyParticle;

    AudioSource enemyAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Dentro del Start hacemos que acceda una vez al componente que difinimos
        //De esta forma podemos seguir usando la variable cada vez que querramos acceder al componente RigidBody
        enemyRb = GetComponent<Rigidbody2D>();

        //Inicializamos la variable dentro del Start para seguir usando la variable cada vez que querramos acceder al componente
        enemySp = GetComponent<SpriteRenderer>();

        //Dentro del Star inicializamos la variable Animator
        animEnemy = GetComponent<Animator>();

        //Inicializamos la variable enemyParticle con Find y getComponent
        enemyParticle = GameObject.Find("EnemyParticle").GetComponent<ParticleSystem>();

        enemyAudio = GetComponentInParent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //Colocamos el movimiento del personaje fuera del condicional porque queremos que siempre se mueva
        //El condidional lo que va hacer es decir cuando debe moverse hacia otra dirección
        //Vector2.right es una forma más sencilla de hacer new Vector2(X,Y) porque no tendriamos que consumir espacio en memoria
        //Para Unity Vector2.right es Vector2(1,0) para cual lo tiene definido en sus parámetros
        enemyRb.velocity = Vector2.right * speed;

        //Realizamos una validación a la variable speed ya que se esta multiplicando por -1
        //De esta forma el Obj Enemy cambia de dirección, si es positivo va a la derecha y si es negativo va a la izquierda
        //Poque no usamos corchetes en el if? porque si solo hay una línea dentro de la condicional, no es necesario usar los corchetes
        //Si hay más de una línea, se usa los corchetes para poder englobar los resultados de la condicional
        if (speed > 0)
            //De esta forma accedemos a las caracteristicas de SpriteRenderer para cambiar la orientación del Sprite
            enemySp.flipX = false;
        else if (speed < 0)
            //De esta forma accedemos a las caracteristicas de SpriteRenderer para cambiar la orientación del Sprite
            enemySp.flipX = true;


        //Vamos a mover a nuestro personaje Enemy a través de su velocidad
        //Como no vamos a realizar el movimiento del Enemy a través de un Input
        //Utilizaremos un timer para que cada x tiempo el personaje se mueva de izquierda a derecha
        //Usaremos una condicional para validar el Time para saber cuando debe moverse el personaje Enemy
        //La condicional se va ejecutar varias veces y entrará cada vez que se cumpla la condicional del tiempo
        //Time.time es el tiempo desde que nuestro juego inició
        if (timeBeforeChange < Time.time)
        {
            //Para cambiar la dirección del personaje
            //speed *= -1 ------→ speed = speed * -1;
            speed *= -1;
            
            //De esta forma nuestro timeBeforeChange para aumentar para saber cuando debe cambiar de dirección al moverse
            timeBeforeChange = Time.time + delay;
        }

    }

    //Para la animación de muerte del enemigo usaremos OnCollisionEnter ya que cuando el Player entre en contacto con Enemy se muera
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Para que solo el Game Object Player sea el que genere la animación de muerte
        //Usaremos el Tag Player, el cual ya esta predefinido por Unity
        if (collision.gameObject.tag == "Player")
        {
            //Como no queremos que Enemy muera cuando choque en cualquier parte de su collider
            //Usaremos una condicional para que solo lo mate cuando el Player choque desde arriba 
            //Por lo que realizamos una validacion entre los eje Y de cada game Object
            //Para que no sea mucho la diferencia entre los valores Y podemos sumarte un poco al Y del Enemy
            if (transform.position.y + .03f < collision.transform.position.y)
            {
                //Como va existir más enemigos y queremos que la particula se ejecute en la posición de cada enemigo respectivamente
                enemyParticle.transform.position = transform.position;

                //Ejecutamos la particula cuando choque con el enemigo desde arriba
                enemyParticle.Play();

                enemyAudio.Play();

                //Declaramos si la animación de muerte se activa o no
                //Al declarar el parametro Bool desde Unity usamos SetBool
                animEnemy.SetBool("isDead", true);
            }
        }
    }

    //Creamos una función pública para desactivar a nuestro Enemigo cuando se de la condición de muerte
    //La creamos como una función publica porque lo vamos agregar directamente en el animator como un evento
    //Para que suceda cuando se acabe la animación
    public void DisableEnemy()
    {
        //Usamos gameObject que hace referencia al Object(Enemy) que tiene este Script
        //SetActive permite mediante codigo desactivar (quitar el Check) el game object en Unity
        //De esta forma el game object desaparece de la pantalla
        gameObject.SetActive(false);

        //Desaparece el gameObject de la pantalla
        //Pero el gameObject desaparece de la jerarquia
        //Dependiendo que busquemos en nuestro juego, podemos usar el Destroy o no
        //Si nuestro juego esta constantemente creando y destruyendo Object, nuestro juego se puede alentar ya que esta 
        //consumiendo memoria
        //Destroy(gameObject);
    }

}
