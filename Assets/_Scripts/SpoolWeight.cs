using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpoolWeight : MonoBehaviour
{

    public Animator animator;
    public int currentWeight;

    public float multiplier;

    public List<GameObject> effects;

    [Header("UI")] 
    public GameObject uiButtons;
    public GameObject wonScreen;
    public DamageNumber numberPrefab;
    public Transform location;

    public TextMeshProUGUI moneyText;

    private bool endCalled;

    private float tick;

    private int money = 100;

    void Start()
    {
        moneyText.text = "$" + money;
    }

    private void GameEnded()
    {
        endCalled = true;
        uiButtons.SetActive(false);
        foreach (var effect in effects)
        {
            effect.SetActive(true);
        }

        StartCoroutine(ShowScreen());
    }

    IEnumerator ShowScreen()
    {
        yield return new WaitForSeconds(2f);
        wonScreen.SetActive(true);
    }

    void Update()
    {
        CheckIfDone();
        PrintMoney();
    }

    private void CheckIfDone()
    {
        var animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        var ntTime = animStateInfo.normalizedTime;

        if (ntTime > 1.0f & !endCalled)
        {
            GameEnded();
        }
        
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void PrintMoney()
    {
        if (currentWeight <= 0) return;

        tick += Time.deltaTime;

        if (!(tick >= 1f)) return;
        
        tick = 0;
        money += currentWeight;
        moneyText.text = "$" + money;
        numberPrefab.Spawn(location.transform.position, currentWeight);


    }

    public void AddAmt()
    {
        money += currentWeight;
        moneyText.text = "$" + money;
        numberPrefab.Spawn(location.transform.position, 1);
    }

    public void DecreaseValue()
    {
        money -= 20;

        if (money <= 0)
        {
            moneyText.text = "$" + 0;
        }
        else
        {
            moneyText.text = "$" + money;
        }
        
    }


    public void AddWeight(int value)
    {
        if (value == 0) value = 1;
        currentWeight += value;
        animator.SetFloat("speed", currentWeight * multiplier);
    }

    public void ReduceWeight(int value)
    {
        if (value == 0) value = 1;
        currentWeight -= value;
        animator.SetFloat("speed", currentWeight * multiplier);
    }
}
