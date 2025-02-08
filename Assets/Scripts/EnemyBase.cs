using System;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyBase : MonoBehaviour
{
    protected float speed = 1.0f;
    public GameObject arrowImagePrefab; // Prefab chứa UI Image
    public Transform arrowContainer; // Vị trí để chứa các mũi tên
    public Sprite upArrowSprite, downArrowSprite, leftArrowSprite, rightArrowSprite;
    private List<KeyCode> arrowSequence = new List<KeyCode>();
    private int currentIndex = 0;
    Animator animator;
    private List<Image> arrowImages = new List<Image>();

    private readonly KeyCode[] arrowKeys = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };

    protected void Start()
    {
        animator = GetComponent<Animator>();
        GenerateArrowSequence();
    }

    private void Update()
    {
        Move();
    }

    void GenerateArrowSequence()
    {
        int length = Random.Range(3, 5);
        arrowSequence.Clear();
        arrowImages.Clear();

        foreach (Transform child in arrowContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < length; i++)
        {
            KeyCode randomKey = arrowKeys[Random.Range(0, arrowKeys.Length)];
            arrowSequence.Add(randomKey);
            GameObject arrowObj = Instantiate(arrowImagePrefab, arrowContainer);
            arrowObj.transform.localPosition = new Vector3(i * 1, 0, 0);
            Image arrowImage = arrowObj.GetComponent<Image>();
            arrowImage.sprite = GetArrowSprite(randomKey);
            arrowImages.Add(arrowImage);
        }
    }

    public bool CheckInput(KeyCode key)
    {
        Debug.Log(key);
        if (key == arrowSequence[currentIndex])
        {
            arrowImages[currentIndex].color = Color.black;

            currentIndex++;
            if (currentIndex >= arrowSequence.Count)
            {
                TakeDamage();
                return true;
            }
        }
        else
        {
            ResetArrows();
        }
        return false;
    }

    void ResetArrows()
    {
        currentIndex = 0;
        foreach (Image arrowImage in arrowImages)
        {
            arrowImage.color = Color.white;
        }
    }
    void Move()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
    Sprite GetArrowSprite(KeyCode key)
    {
        return key switch
        {
            KeyCode.UpArrow => upArrowSprite,
            KeyCode.DownArrow => downArrowSprite,
            KeyCode.LeftArrow => leftArrowSprite,
            KeyCode.RightArrow => rightArrowSprite,
            _ => null,
        };
    }
    public void TakeDamage()
    {
        arrowContainer.gameObject.SetActive(false);
        animator.SetTrigger("Death");
        StartCoroutine(ReturnToPoolAfterAnimation());
    }

    private IEnumerator ReturnToPoolAfterAnimation()
    {
        yield return new WaitForSeconds(1f);
        EnemyPoolManager.Instance.ReturnToPool(gameObject);
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Base"))
        {
            /*other.GetComponent<BaseHealth>().TakeDamage(damage);*/
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        arrowContainer.gameObject.SetActive(true);
        GenerateArrowSequence();
        currentIndex = 0; // Reset trạng thái
    }

}