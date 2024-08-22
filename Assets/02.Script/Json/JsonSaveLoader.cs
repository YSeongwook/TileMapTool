using System;
using System.Collections.Generic;
using System.IO;
using EnumTypes;
using EventLibrary;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class JsonSaveLoader : MonoBehaviour
{
    public TMP_InputField fileNameInputField; // 파일 이름을 입력받는 InputField
    public TMP_InputField limitCountInputField; // 제한 횟수를 입력받는 InputField
    public string saveDirectory = "C:/Download/TileMap"; // 저장할 디렉터리 경로

    private List<Tile> _saveTileList;

    private void Awake()
    {
        EventManager<TileEvent>.StartListening<List<Tile>>(TileEvent.JsonSaveData, SaveJsonFile);
        EventManager<DataEvents>.StartListening(DataEvents.TileSave, SaveSuccess);
    }

    private void Start()
    {
        // 디렉터리가 존재하지 않으면 생성
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
    }

    private void OnDestroy()
    {
        EventManager<TileEvent>.StopListening<List<Tile>>(TileEvent.JsonSaveData, SaveJsonFile);
        EventManager<DataEvents>.StopListening(DataEvents.TileSave, SaveSuccess);
    }

    // 타일 맵 JSON 파일을 저장
    private void SaveJsonFile(List<Tile> tileList)
    {
        try
        {
            // 타일 리스트가 비어 있는지 확인
            if (tileList == null || tileList.Count == 0)
            {
                EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "타일 맵이 없습니다. 저장할 수 없습니다.");
                return;
            }

            // InputField에서 입력한 파일 이름을 가져옵니다.
            string fileName = fileNameInputField.text;

            // 파일 이름이 비어있는지 확인합니다.
            if (string.IsNullOrEmpty(fileName))
            {
                // 팝업 발생
                EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "파일 이름이 입력되지 않았습니다.");
                return;
            }

            // 파일 이름이 양식을 지키는지 확인합니다.
            var stringList = fileName.Split('-');
            bool saveAble1 = int.TryParse(stringList[0], out int value1) && value1 < 5;
            bool saveAble2 = int.TryParse(stringList[1], out int value2) && value2 < 21;

            if(!saveAble1 || !saveAble2) 
            {
                // 팝업 발생
                EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "잘못된 파일 이름입니다.");
                return;
            }

            _saveTileList = tileList;

            // 저장하시겠습니까 팝업 발생
            EventManager<UIEvents>.TriggerEvent(UIEvents.SavePopUp);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "JSON 파일 저장 중 오류가 발생했습니다: " + e.Message);
        }
    }

    
    
    private void SaveSuccess()
    {
        // 리스트를 JSON으로 변환합니다.
        string json = JsonConvert.SerializeObject(_saveTileList, Formatting.Indented);

        // 사용자 지정 경로에 파일 경로를 설정합니다.
        string filePath = Path.Combine(saveDirectory, fileNameInputField.text + ".json");

        // JSON 데이터를 파일로 저장합니다.
        File.WriteAllText(filePath, json);

        // 저장 완료 메시지를 출력합니다.
        EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "JSON 파일이 저장되었습니다: " + filePath);
    }

    // JSON 파일을 로드하는 함수
    public List<Tile> LoadJsonFile()
    {
        // InputField에서 입력한 파일 이름을 가져옵니다.
        string fileName = fileNameInputField.text;

        // 파일 이름이 비어있는지 확인합니다.
        if (string.IsNullOrEmpty(fileName))
        {
            EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "파일 이름이 입력되지 않았습니다.");
            return null;
        }

        // 사용자 지정 경로에 파일 경로를 설정합니다.
        string filePath = Path.Combine(saveDirectory, fileName + ".json");

        // 파일이 존재하는지 확인합니다.
        if (File.Exists(filePath))
        {
            // 파일에서 JSON 문자열을 읽어옵니다.
            string json = File.ReadAllText(filePath);

            // JSON 문자열을 List<Tile>로 변환합니다.
            List<Tile> tilesData = JsonConvert.DeserializeObject<List<Tile>>(json);

            // InputField에 로드된 파일 이름을 다시 설정합니다
            fileNameInputField.text = fileName;

            // 로드된 데이터 출력
            EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, $"{fileName}.JSON이 로드되었습니다");

            // 데이터 반환
            return tilesData;   
        }
        else
        {
            EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, $"{fileName}.json을 찾을 수 없습니다");
            return null;
        }
    }
    
    // Todo: 별도의 파일로 제한 횟수를 저장
    // 제한 횟수 파일의 Key는 MapID, FileName, LimitCount
    // 저장 시 새로운 파일이 생성되는 것이 아니라 기존 파일을 수정
    // 파일 없을 시 새 파일 생성
}