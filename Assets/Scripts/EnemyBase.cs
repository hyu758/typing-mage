using System.Collections;
using UnityEngine;
using TMPro;

public class EnemyBase : MonoBehaviour
{
    public float speed = 2f;
    public int damage = 1;
    public string enemyWord;
    public TextMeshProUGUI wordText;
    Animator animator;
    public Transform target;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        enemyWord = GetRandomWord();
        wordText.text = enemyWord;
    }

    protected virtual void Update()
    {
        MoveTowardsBase();
    }

    protected void MoveTowardsBase()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    protected string GetRandomWord()
    {
        string[] words = { "fire", "storm", "magic", "blast", "curse" };
        return words[Random.Range(0, words.Length)];
    }

    public virtual void TakeDamage()
    {
        StartCoroutine(EnemyDeath());
        EnemyPoolManager.Instance.ReturnToPool(gameObject);
    }

    IEnumerator EnemyDeath()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(0.5f);
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Base"))
        {
            /*other.GetComponent<BaseHealth>().TakeDamage(damage);*/
            Destroy(gameObject);
        }
    }
}