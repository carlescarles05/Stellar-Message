using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<QuestionsAndAnswers> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public GameObject totalQuizPanel;
    public GameObject quizPanel;
    public GameObject GOPanel;

    public TMP_Text QuestionTxt;
    public TMP_Text ScoreTxt;

    public AudioClip AudioNL; // Audio a reproducir cuando se responden correctamente las tres preguntas
    private AudioSource audioSource; // AudioSource para reproducir el audio

    int totalQuestions = 0;
    public int score;
    public GameObject objectToDisable; // Objeto que se desactivará al responder correctamente las tres preguntas

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Obtener el AudioSource del objeto actual
        totalQuestions = QnA.Count;
        GOPanel.SetActive(false);
        generateQuestion();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void GameOver()
    {
        quizPanel.SetActive(false);
        GOPanel.SetActive(true);
        ScoreTxt.text = score + "/" + totalQuestions;
    }

    public void correct()
    {
        score += 1;
        QnA.RemoveAt(currentQuestion);
        generateQuestion();

        if (score == 3) // Verificar si se han respondido correctamente tres preguntas
        {
            //Reproducir el efecto de sonido 
            audioSource.Play();       
        }
        // Desactivar el objeto deseado
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }
    }

    public void wrong()
    {
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }

    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        if (QnA.Count > 0)
        {
            Debug.Log("QuestionTxt is: " + QuestionTxt);
            currentQuestion = Random.Range(0, QnA.Count);
            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswers();
        }
        else
        {
            Debug.LogWarning("No hay preguntas disponibles en la lista QnA.");
            GameOver();
        }
    }

    public void ResetAnswerColors()
    {
        foreach (GameObject option in options)
        {
            UnityEngine.UI.Image imageComponent = option.GetComponent<UnityEngine.UI.Image>();
            if (imageComponent != null)
            {
                option.GetComponent<UnityEngine.UI.Image>().color = option.GetComponent<AnswerScript>().startColor;
            }
            else
            {
                Debug.LogWarning("El objeto " + option.name + " no tiene un componente UnityEngine.UI.Image.");
            }
        }
    }

    public void ExitGame()
    {
        GOPanel.SetActive(false);
    }
}
