using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    private const string FADING_BOOL = "IsFade";

    [SerializeField] private GameObject _tutorList;
    [SerializeField] private Button _buttonOfTutor;
    [SerializeField] private Animator _animator;

    private float _timeToLoadScene = 0.5f;
    public void StartOfGame()
    {
        StartCoroutine(Loading());
    }

    public void ShowTutor()
    {
        _tutorList.SetActive(true);
        _buttonOfTutor.gameObject.SetActive(false);
    }

    public void CloseTutor()
    {
        _tutorList.SetActive(false);
        _buttonOfTutor.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator Loading()
    {
        _animator.SetBool(FADING_BOOL, true);
        yield return new WaitForSeconds(_timeToLoadScene);
        SceneManager.LoadScene(1);
    }
}
