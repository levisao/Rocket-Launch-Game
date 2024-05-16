using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] float movementFactor; // [Range(0,1)]  range transforma em um slider
    [SerializeField] float period = 2f;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Oscillate();
    }

    private void Oscillate()
    {
        
        if (period <= Mathf.Epsilon) { return;}
        float cycles = Time.time / period; //quanto tempo queremos que um ciclo seja. Continually growing over time

        const float tau = Mathf.PI * 2; //tau é o valor da circunferencia do circulo. "Deveríamos usar tau ao inves de pi". 6.283
        float rawSinWave = Mathf.Sin(cycles * tau); //essa formula vai retornar um valor entre 1 e -1

        movementFactor = (rawSinWave + 1f) / 2f; //rawSinWave + 1 significa q o valor será entre 0 e 2... dividindo por 2, agora será entre 0 e 1

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
