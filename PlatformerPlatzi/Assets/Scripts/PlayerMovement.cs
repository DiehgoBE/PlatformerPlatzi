using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Creamos una variable publica para referenciar el RigiBody
    public Rigidbody2D playerRb;
    //Creamos una variable publica para modificar la velocidad
    public float speed = .5f;

    //Creamos una variable publica para la velocidad del salto
    //Si usamos la misma variable de la velocidad que se uso para velocity
    //el Player no ejecutará el salto por que AddForce requiere una velocidad mayor ya que
    //se esta agregando una fuerza directamente al RigidBody 
    public float jumpSpeed = 300f;

    //Creamos una variable para validar que nuestro personaje esta en el suelo
    //De esta forma el personaje solo salte cuando esta en el suelo
    //bool isGrounded = true;

    //Como queremos usar la variable isGrounded desde otro Script la declaramos publica
    public bool isGrounded = true;

    //Para poder acceder a los parámetros del Animator creamos una variable que lo referencie
    //De esta forma poder cambiar entre las transiciones de las animaciones
    public Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vamos a modificar la velocidad de nuestro personaje para que se mueva
        //Lo vamos hacer a través de RigidBody
        //Podemos acceder al Rigidbody por una variable referenciada o por getComponent
        //Para este caso no vamos usar el movimiento de mouse para mover el personaje
        //sino vamos a usar las flechas del teclado GetAxis, tambien se puede modificar para usar el joystick
        //vamos a mantener la velocidad en Y, al trabajar con velocidad se usa un Vector2
        playerRb.velocity = new Vector2(Input.GetAxis("Horizontal")*speed,playerRb.velocity.y);

        //Agregamos una condicional para validar si estamos yendo a la izquierda o derecha
        //Y que nuestra animación de caminado se active al caminar
        //RECUERDA: GetAxis tiene 3 valores → -1: Izquierda     0:Quieto    1:Derecha
        //De esta forma podemos saber hacia donde nos dirigimos y que acción tomar cuando suceda cada una de ellas
        if (Input.GetAxis("Horizontal") == 0) // Quieto
        {
            //Con el Animator referenciado podemos acceder a los parámetros y modificar su valores
            //RECUERDA: al llamar a los parámetros del Animator en el código, debemos respetar el uso de minusculas y mayusculas
            //De esta forma el personaje no ejecuta la animacion de Walk sino la de Idle
            playerAnim.SetBool("isWalking", false);
        } else if (Input.GetAxis("Horizontal") < 0) //El personaje va a la izquierda
        {
            //Colocamos las acciones que va a realizar el personaje cuando se cumpla la condición de ir a la izquierda
            //Con el isWalking en true la animación de caminar se activa
            playerAnim.SetBool("isWalking", true);

            //En este caso usamos GetComponent en vez de referenciar con una variable (Es igual)
            //Como queremos que nuestro persona gire cuando va a la izquierda usaremos Flip para girar a nuestro personaje en el eje X
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetAxis("Horizontal") > 0) //El personaje va a la derecha
        {
            //Colocamos las acciones que va a realizar el personaje cuando se cumpla la condición de ir a la izquierda
            //Con el isWalking en true la animación de caminar se activa
            playerAnim.SetBool("isWalking", true);

            //En este caso usamos GetComponent en vez de referenciar con una variable (Es igual)
            //Como queremos que nuestro persona vaya a la derecha usaremos Flip para girar a nuestro personaje en el eje X
            GetComponent<SpriteRenderer>().flipX = false;
        }

                          
        //Antes de saltar tenemos que validar que personaje esta en el suelo
        //De lo contrario podrá hacer varios saltos en el aire
        if (isGrounded)
        {
            //Creamos una condicional para validar cuando se precione una tecla para el salto
            //para este caso se usará la barra espaciadora pero puede ser cualquier tecla
            //KeyCode permite identificar que tecla queremos usar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Podemos buscarlo directamente desde getComponent porque esta dentro del mismo Object
                //Podemos colocar directamente el getComponent sin inicializar con una variable solo cuando
                //Se va ejecutar una vez y no se va usar constantemente
                //Si lo vamos a usar más de una vez, lo colocamos en el Start
                GetComponent<AudioSource>().Play();

                //Cuando se cumpla la condición al player se le agregará una fuerza
                //AddForce es una forma distinta de mover a nuestro personaje
                //Como queremos que nuestro personaje salte, agregamos un vector2 Up
                //y lo multiplicamos por una velocidad para que se produzca el salto
                playerRb.AddForce(Vector2.up * jumpSpeed);
                isGrounded = false;

                //Agregamos la animación de salto cuando se de GetKeyDown Space
                //En este caso el parametro de salto es un trigger por lo que no hay necesidad de cambiar su valor
                //Se cambia automaticamente su valor ya que solo ocurre una vez
                playerAnim.SetTrigger("Jump");


            }

        }
        
    }

    //Creamos un CollisionEnter para cambiar el estado de nuestra variable isGrounded a verdadero
    //Cuando choque contra el piso
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Mediante la comparación con el Tag podemos validar que cuando se choque con el suelo se cambie la variable
        //De esta forma nuestro personaje pueda volver a saltar
        //Usamos Tags desde Unity para asegurarnos que se reinicie la variable cuando choque contra el suelo y no contra
        //cualquier collider
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

}
