using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JsonSaveLoader : MonoBehaviour
{
    public TMP_InputField fileNameInputField; // 파일 이름을 입력받는 InputField
    public Button saveButton; // 저장 버튼
    public Button loadButton; // 로드 버튼
    public string saveDirectory = "C:/Download/TileMap"; // 저장할 디렉터리 경로

    private void Start()
    {
        // Save 및 Load 버튼에 클릭 이벤트 연결
        saveButton.onClick.AddListener(SaveJsonFile);
        loadButton.onClick.AddListener(LoadJsonFile);

        // 디렉터리가 존재하지 않으면 생성
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
    }

    // JSON 파일을 저장하는 함수
    public void SaveJsonFile()
    {
        // InputField에서 입력한 파일 이름을 가져옵니다.
        string fileName = fileNameInputField.text;

        // 파일 이름이 비어있는지 확인합니다.
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.Log("파일 이름이 입력되지 않았습니다.");
            return;
        }

        // 예시로 저장할 데이터 생성
        TestData testData = new TestData
        {
            message = "이것은 테스트 데이터입니다.",
            number = 42
        };

        // JSON으로 변환합니다.
        string json = JsonUtility.ToJson(testData, true);

        // 사용자 지정 경로에 파일 경로를 설정합니다.
        string filePath = Path.Combine(saveDirectory, fileName + ".json");

        // JSON 데이터를 파일로 저장합니다.
        File.WriteAllText(filePath, json);

        // 저장 완료 메시지를 출력합니다.
        Debug.Log("JSON 파일이 저장되었습니다: " + filePath);
    }

    // JSON 파일을 로드하는 함수
    public void LoadJsonFile()
    {
        // InputField에서 입력한 파일 이름을 가져옵니다.
        string fileName = fileNameInputField.text;

        // 파일 이름이 비어있는지 확인합니다.
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.Log("파일 이름이 입력되지 않았습니다.");
            return;
        }

        // 사용자 지정 경로에 파일 경로를 설정합니다.
        string filePath = Path.Combine(saveDirectory, fileName + ".json");

        // 파일이 존재하는지 확인합니다.
        if (File.Exists(filePath))
        {
            // 파일에서 JSON 문자열을 읽어옵니다.
            string json = File.ReadAllText(filePath);

            // JSON 문자열을 TestData 객체로 변환합니다.
            TestData loadedData = JsonUtility.FromJson<TestData>(json);

            // InputField에 로드된 파일 이름을 다시 설정합니다
            fileNameInputField.text = fileName;

            // 로드된 데이터 출력
            Debug.Log("JSON 파일이 로드되었습니다: " + filePath);
            Debug.Log("메시지: " + loadedData.message);
            Debug.Log("숫자: " + loadedData.number);
        }
        else
        {
            Debug.Log("파일을 찾을 수 없습니다: " + filePath);
        }
    }
}

// 테스트용 데이터 클래스
[System.Serializable]
public class TestData
{
    public string message; // 예시 메시지
    public int number; // 예시 숫자
}
