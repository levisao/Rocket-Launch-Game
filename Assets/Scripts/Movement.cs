using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // PARAMETERS FIRST -- Things that can be tuned
    [SerializeField] private float rocketMinThrust = 1000f;
    [SerializeField] private float rocketRotationThrust = 200f;
    [SerializeField] private AudioClip mainEngine;

    [SerializeField] private ParticleSystem particleMainThrust;
    [SerializeField] private ParticleSystem leftMainThrust;
    [SerializeField] private ParticleSystem rightMainThrust;

    //THINGS YOU WANT TO CATCH
    private Rigidbody rb;
    private AudioSource audioS;

    //Booleans

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        
       if(Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }

    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * rocketMinThrust * Time.deltaTime); // vai aplicara  força de acordo com as coordenadas do objeto

        if (!audioS.isPlaying)
        {
            audioS.PlayOneShot(mainEngine);
        }
        if (!particleMainThrust.isPlaying)
        {
            particleMainThrust.Play();

        }
    }

    private void StopThrusting()
    {
        particleMainThrust.Stop();
        audioS.Pause();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }


    }

    private void RotateLeft()
    {
        Rotate(rocketRotationThrust);  //(0,0,1)
        if (!rightMainThrust.isPlaying)
        {
            rightMainThrust.Play();
        }
    }
   

    private void RotateRight()
    {
        if (!leftMainThrust.isPlaying)
        {
            leftMainThrust.Play();
        }
        Rotate(-rocketRotationThrust);
        //transform.Rotate(-Vector3.forward * rocketRotationThrust * Time.deltaTime); //(0,0,-1)
    }
    private void StopRotating()
    {
        leftMainThrust.Stop();
        rightMainThrust.Stop();
    }

    void Rotate(float rotationThisFrame) //melhor prática criar um método para não repetir código e dificultar a manutenção
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);

        rb.freezeRotation = false; // unfreezing so the physics system can take over - n entendi pq resolveu o bug, mas basicamente resolve o bug do conflito entre os physics systems
    }
}
