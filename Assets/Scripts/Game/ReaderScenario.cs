using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReaderScenario : MonoBehaviour
{
    #region Singleton
    private static ReaderScenario _instance = null;
    public static ReaderScenario Instance
    {
        get
        {
            if (ReaderScenario._instance == null)
            {
                ReaderScenario._instance = FindObjectOfType<ReaderScenario>();
                if (ReaderScenario._instance == null)
                {
                    GameObject go = new GameObject("ReaderScenario");
                    ReaderScenario._instance = go.AddComponent<ReaderScenario>();
                }
            }
            return ReaderScenario._instance;
        }
    }
    #endregion Singleton

    public ScenarioData _Scenario;
    public PageData _Page;
    public Text _Text;
    public Image _Image;
    public AudioSource _Music;
    public AudioSource _FX;

    public Text _InputUp;
    public Text _InputDown;
    public Text _InputRight;
    public Text _InputLeft;
    public Text _Button;

    void Start ()
    {
        ChangePage(_Scenario._Pages[0]);
    }

    void Update ()
    {
        if (_Page != null)
            _Page.Update();
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    public void ReadAgain()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            if (_Page != null)
                _Page.Read();
        }
#endif
    }

    public void ChangePage(PageData newPage)
    {
        ResetText();
        if (_Page != null)
            _Page.Unload();
        _Page = newPage;
        _Page.Read();
    }

    public void ResetText()
    {
        SetText("", Color.white);
        SetUp("");
        SetDown("");
        SetRight("");
        SetLeft("");
        SetButton("");
    }

    public void SetText(string newText, Color newColor)
    {
        _Text.text = newText;
        _Text.color = newColor;
    }

    public void SetImage(Sprite newSprite)
    {
        _Image.sprite = newSprite;
        Color colorImage = _Image.color;
        colorImage.a = newSprite != null ? 1.0f : 0.0f;
        _Image.color = colorImage;
    }

    public void SetBackground(Color newColor)
    {
        Camera.main.backgroundColor = newColor;
    }

    public void ChangeMusic(AudioClip newMusic)
    {
        StartCoroutine(FadeMusic(newMusic));
    }
    private IEnumerator FadeMusic(AudioClip newMusic)
    {
        float v = _Music.volume;
        float speedFadeOut = 1.5f;
        while (v != 0.0f)
        {
            yield return null;
            v = Mathf.Clamp01(v - Time.deltaTime * speedFadeOut);
            _Music.volume = v;
        }
        _Music.clip = newMusic;
        _Music.Play();
        if (newMusic != null)
        {
            float speedFadeIn = 1.5f;
            while (v != 1.0f)
            {
                yield return null;
                v = Mathf.Clamp01(v + Time.deltaTime * speedFadeIn);
                _Music.volume = v;
            }
        }
    }

    public void ChangeFX(AudioClip newMusic)
    {
        StartCoroutine(FadeFX(newMusic));
    }
    private IEnumerator FadeFX(AudioClip newMusic)
    {
        float v = _FX.volume;
        float speedFadeOut = 1.5f;
        while (v != 0.0f)
        {
            yield return null;
            v = Mathf.Clamp01(v - Time.deltaTime * speedFadeOut);
            _FX.volume = v;
        }
        _FX.clip = newMusic;
        _FX.Play();
        if (newMusic != null)
        {
            float speedFadeIn = 1.5f;
            while (v != 1.0f)
            {
                yield return null;
                v = Mathf.Clamp01(v + Time.deltaTime * speedFadeIn);
                _FX.volume = v;
            }
        }
    }

    public void SetUp(string text)
    {
        _InputUp.text = text;
    }
    public void SetDown(string text)
    {
        _InputDown.text = text;
    }
    public void SetRight(string text)
    {
        _InputRight.text = text;
    }
    public void SetLeft(string text)
    {
        _InputLeft.text = text;
    }
    public void SetButton(string text)
    {
        _Button.text = text;
    }
}
