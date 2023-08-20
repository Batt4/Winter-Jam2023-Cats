using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class stack : MonoBehaviour
{

    [SerializeField]
    GameObject moneda;

    public float StackSpacing = 0.09f;

    [SerializeField]
    int Coins;

    int coinsLastFrame;
    int deltaCoins = 0;
    int loopcount = 0;

    List<GameObject> stackMonedas = new List<GameObject>();
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GameObject.Find("Player").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        deltaCoins = 0;
        Coins = animator.GetInteger("coins");
        if(Coins != coinsLastFrame) 
        {
            deltaCoins = Coins - coinsLastFrame;
            while(deltaCoins > 0) 
            {
                loopcount++;
                stackMonedas.Add(Instantiate(moneda,
                    new Vector3(transform.position.x, transform.position.y + ((coinsLastFrame + loopcount) * StackSpacing), transform.position.z), Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z), gameObject.transform) as GameObject);
                stackMonedas[stackMonedas.Count - 1].transform.Rotate(0,Random.Range(-359,359),0);
                deltaCoins = deltaCoins - 1;
            }
            
            loopcount = 0;
            
            while (deltaCoins < 0)
            {
                Destroy(stackMonedas[stackMonedas.Count - 1]);
                stackMonedas.Remove(stackMonedas[stackMonedas.Count - 1]);
                deltaCoins = deltaCoins + 1;
            }

        }

        coinsLastFrame = Coins;
    }
}
