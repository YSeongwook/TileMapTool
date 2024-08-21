using TMPro;
using UnityEngine;

public class VersionDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text versionText;

    private void Start()
    {
        // Application.version을 통해 현재 빌드 버전을 가져옴
        versionText.text = $"Version: {Application.version}";
    }
}