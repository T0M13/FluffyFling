using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private Color scoreColor = Color.white;

    public TextMeshProUGUI Score { get => score; set => score = value; }
    public Color ScoreColor { get => scoreColor; set => scoreColor = value; }

    public float moveSpeed = 1f;
    public float fadeSpeed = 1f;
    public float lifeTime = 2f;

    private CanvasGroup canvasGroup;
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void SetScore(int score)
    {
        this.score.text = score.ToString();
    }

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime); // Move the canvas up
        Invoke("FadeOut", lifeTime); // Start fading out after the specified life time
    }

    private void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime); // Continue moving the canvas up
    }

    private void FadeOut()
    {
        StartCoroutine(FadeCanvasOut());
    }

    private IEnumerator FadeCanvasOut()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime; // Reduce the canvas alpha
            yield return null;
        }

        Destroy(gameObject); // Destroy the canvas object after fading out
    }
}
