using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SandwichCustomers : MonoBehaviour
{

    public TextMeshProUGUI moneyText;

    private static SandwichCustomers _instance;

    public static SandwichCustomers Instance { get { return _instance; } }

    public GameObject customer;

    public List<Transform> customerLocations;
    public Queue<Transform> locQueue = new Queue<Transform>();
    public Queue<Customer> customers = new Queue<Customer>();

    private Customer firstCustomer;

    private Customer prevCustomer;
    
    public DamageNumber numberPrefab;

    public int currentAmt;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void Start()
    {
        foreach (var location in customerLocations)
        {
            locQueue.Enqueue(location);
        }

        currentAmt = 100;
    }

    public void Update()
    {
        SpawnCustomers();
    }

    private void SpawnCustomers()
    {
        if (locQueue.Count <= 0) return;
        
        SpawnCustomer();
    }

    public void GiveSandwichToCustomer(GameObject sandwich)
    {
        sandwich.transform.DOMove(firstCustomer.transform.position + new Vector3(0, 2, .2f), .8f).onComplete += PrintMoney;

        if (!firstCustomer.GiveSandwich(sandwich))
        {
            prevCustomer = firstCustomer;
            firstCustomer = customers.Dequeue();
        }
        
    }

    public void PrintMoney()
    {
        currentAmt += 20;
        moneyText.text = "$" + currentAmt;
        
        firstCustomer.PlaySmileEffect();
        
        
        firstCustomer.CheckIfDone();

        if (prevCustomer != null)
        {
            prevCustomer.CheckIfDone();
        }
        
        numberPrefab.Spawn(firstCustomer.transform.position + new Vector3(0,4.5f,0), 20);
    }

    public void ReduceMoney()
    {
        currentAmt -= 30;

        if (currentAmt <= 0)
            currentAmt = 0;
        
        moneyText.text = "$" + currentAmt;
        
    }

    public void AddCustomerToQueue(Customer customer)
    {
        if (firstCustomer == null)
        {
            firstCustomer = customer;
        }
        else
        {
            customers.Enqueue(customer);
        }
    }


    public void SpawnCustomer()
    {
        var spawnLoc = locQueue.Dequeue();
        var newCustomer = Instantiate(customer, spawnLoc.transform.position, Quaternion.identity).GetComponent<Customer>();
        
        newCustomer.MoveToLocation();
        newCustomer.sandwichCustomers = this;
        newCustomer.startLoc = spawnLoc;


    }

    public void SpawnBread()
    {
        SpoolHolder.SpawnBreadClicked();
    }
    
    
}
