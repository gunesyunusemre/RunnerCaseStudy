using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private Image loadingSlide;

    private void Awake()
    {
        loadingSlide.fillAmount = 0f;
    }

    private void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOVirtual.Float(0f, .5f, .5f, (value) => { loadingSlide.fillAmount = value; }));
        DOVirtual.Float(0f, .5f, .5f, (value) => { loadingSlide.fillAmount = value; }).OnKill(() =>
        {
            DOVirtual.Float(.5f, 1f, .5f, (value) => { loadingSlide.fillAmount = value; })
                .SetDelay(.25f)
                .OnKill(() =>
                {
                    var activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
                    SceneManager.LoadScene(activeSceneIndex + 1);
                });
        });
    }
}