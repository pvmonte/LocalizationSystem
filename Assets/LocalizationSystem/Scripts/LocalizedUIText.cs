using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedUIText : MonoBehaviour
{
    Text uiText;
    [HideInInspector] public int keyIndex;
    [HideInInspector] public string key;

    // Start is called before the first frame update
    void Start()
    {        
        uiText = GetComponent<Text>();        

        LocalizationController.instance.RegisterText(this);
        LoadText(LocalizationController.instance.language);
    }

    public void LoadText(string language)
    {
        var csv = LocalizationController.instance.csvLoader;
        uiText.text = csv.GetLanguageDictionaryPairValue(language, key);
    }

    private void OnDestroy()
    {
        var controller = LocalizationController.instance;

        if (controller.gameObject.activeInHierarchy)
            controller.DeregisterText(this);
    }
}
