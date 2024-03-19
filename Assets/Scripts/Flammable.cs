using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Flammable : MonoBehaviour
{
    [SerializeField] ParticleSystem fireEffect;
    ParticleSystem fireInstance;


    [SerializeField] bool startOnFire = false;
    bool onFire = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (startOnFire)
        {
            SetOnFire();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetOnFire() 
    {
        onFire = true;

        if (fireEffect != null)
        {
            fireInstance = Instantiate(fireEffect, transform.position += new Vector3(0, 0.1f, 0), fireEffect.transform.rotation);
        }
    }
    public void PutOutFire() 
    { 
        onFire = false;

        if (fireEffect != null)
        {
            Destroy(fireInstance);
        }
    }

    public bool IsOnFire() { return onFire; }
}
