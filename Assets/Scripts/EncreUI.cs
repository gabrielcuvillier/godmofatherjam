using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EncreUI : MonoBehaviour
{
    [SerializeField] private Image encreUI;

    private float currentEncre;
    [SerializeField] private float timeToDecrease = 2f;
    private float maxEncre = 255f;
    private float minEncre = 130f;
    private Coroutine coroutine;
    private float encreadded;

    void Start()
    {
        currentEncre = 0f;
        UpdateEncre();
    }

    private void UpdateEncre()
    {
        float alpha = currentEncre / maxEncre;
        Color color = encreUI.color;
        color.a = alpha;
        encreUI.color = color;
    }

    public void AddEncre(float amount)
    {
        encreadded = amount;
        if (currentEncre < minEncre)
            currentEncre = minEncre;
        currentEncre += amount;
        if (currentEncre > maxEncre)
            currentEncre = maxEncre;

        UpdateEncre();
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(WaitForDecrease());
    }

    private IEnumerator WaitForDecrease()
    {
        yield return new WaitForSeconds(timeToDecrease);
        DecreaseEncre();
    }

    private void DecreaseEncre()
    {
        currentEncre -= encreadded;
        if (currentEncre < minEncre)
            currentEncre = 0f;

        UpdateEncre();
        coroutine = StartCoroutine(WaitForDecrease());
    }
}
