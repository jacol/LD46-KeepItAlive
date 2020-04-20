using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehave : MonoBehaviour
{
    public AudioClip FoodEndSound;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("AutoDestroy", 10.0f, 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AutoDestroy()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(FoodEndSound);

        
        StartCoroutine(waiter());
        
        this.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(1);
    }
}
