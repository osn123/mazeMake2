using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgaguriController : MonoBehaviour
{

    public void Shoot(Vector3 dir)
    {
        //GetComponent<Rigidbody>().AddForce(dir);
        GetComponent<Rigidbody>().velocity=dir/50;
    }

    void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<ParticleSystem>().Play();
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        //Shoot(new Vector3(0, 200, 2000));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
