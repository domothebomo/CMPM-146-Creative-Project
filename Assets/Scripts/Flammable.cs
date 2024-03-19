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

    float fireTick = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (fireEffect != null)
        {
            fireInstance = Instantiate(fireEffect, transform.position += new Vector3(0, 0.1f, 0), fireEffect.transform.rotation);
            fireInstance.transform.parent = transform;
            fireInstance.Stop(true);
        }
        

        if (startOnFire)
        {
            SetOnFire();
        }
    }

    // Update is called once per frame
    void Update()
    {
        fireTick += Time.deltaTime;
        if (IsOnFire() && fireTick >= 5.0f)
        {
            SpreadFire();
            fireTick = 0.0f;
        }
    }

    private void SpreadFire()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2.5f);
        foreach (var hitCollider in hitColliders)
        {
            if ((hitCollider.CompareTag("Object") || hitCollider.CompareTag("Character")) && !hitCollider.gameObject.GetComponent<Flammable>().IsOnFire())
            {
                hitCollider.gameObject.GetComponent<Flammable>().SetOnFire();
            }
        }
    }

    public void SetOnFire() 
    {
        onFire = true;

        if (fireEffect != null)
        {
            fireInstance.Play();
        }
    }
    public void PutOutFire() 
    {
        onFire = false;

        if (fireEffect != null)
        {
            fireInstance.Stop(true);
        }
    }

    public bool IsOnFire() 
    {
        return onFire; 
    }
}
