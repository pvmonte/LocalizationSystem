using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationController : MonoBehaviour
{
    public static LocalizationController instance;

    public CsvLoader csvLoader { get; private set; }
    public SystemLanguage systemLanguage;
    public string language;

    List<LocalizedUIText> uiTexts = new List<LocalizedUIText>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        csvLoader = new CsvLoader();

        StartLanguage();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            StartLanguage();

            foreach (var item in uiTexts)
            {
                item.LoadText(language);
            }
        }
    }

    public void StartLanguage()
    {
        switch (systemLanguage)
        {
            case SystemLanguage.Portuguese:
                language = "PT";
                break;
            default:
                language = "EN";
                break;
        }
    }

    public void RegisterText(LocalizedUIText uiText)
    {
        if (uiTexts.Contains(uiText))
            return;

        uiTexts.Add(uiText);
    }

    public void DeregisterText(LocalizedUIText uiText)
    {
        if (uiTexts.Contains(uiText))
            uiTexts.Remove(uiText);
    }
}
