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
    private Dictionary<string, Dictionary<string, object>> _limitCountTable; // 전체 제한 횟수 테이블

    private void Awake()
    {
        EventManager<JsonEvent>.StartListening<List<Tile>>(JsonEvent.JsonSaveData, SaveJsonFile);
        EventManager<DataEvents>.StartListening(DataEvents.TileSave, SaveSuccess);
    }

    private void Start()
    {
        // 디렉터리가 존재하지 않으면 생성
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        // LimitCountTable.json 파일을 로드
        LoadLimitCountTable();
    }

    private void OnDestroy()
    {
        EventManager<JsonEvent>.StopListening<List<Tile>>(JsonEvent.JsonSaveData, SaveJsonFile);
        EventManager<DataEvents>.StopListening(DataEvents.TileSave, SaveSuccess);
    }

    private void LoadLimitCountTable()
    {
        string filePath = Path.Combine(saveDirectory, "LimitCountTable.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _limitCountTable = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);
        }
        else
        {
            _limitCountTable = new Dictionary<string, Dictionary<string, object>>();
        }
    }

    private void SaveLimitCountTable()
    {
        string filePath = Path.Combine(saveDirectory, "LimitCountTable.json");
        string json = JsonConvert.SerializeObject(_limitCountTable, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    // 타일 맵 JSON 파일 저장
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

            // 파일 이름과 제한 횟수 입력 확인
            string fileName = fileNameInputField.text;
            if (string.IsNullOrEmpty(fileName))
            {
                EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "파일 이름이 입력되지 않았습니다.");
                return;
            }

            if (string.IsNullOrEmpty(limitCountInputField.text) ||
                !int.TryParse(limitCountInputField.text, out int limitCount))
            {
                EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "제한 횟수가 입력되지 않았거나 잘못된 값입니다. 저장할 수 없습니다.");
                return;
            }

            if (limitCount < 1)
            {
                EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "제한 횟수는 1보다 작을 수 없습니다.");
                return;
            }

            _saveTileList = tileList;

            // 저장하시겠습니까 팝업 발생
            EventManager<UIEvents>.TriggerEvent(UIEvents.SavePopUp);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "JSON 파일 저장 중 오류가 발생했습니다: " + e.Message);
        }
    }

    private void SaveLimitCountJsonFile()
    {
        try
        {
            // 제한 횟수 입력 확인
            if (string.IsNullOrEmpty(limitCountInputField.text) ||
                !int.TryParse(limitCountInputField.text, out int limitCount))
            {
                EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "제한 횟수가 입력되지 않았거나 잘못된 값입니다.");
                return;
            }

            // MapID 생성 (파일 이름의 앞부분을 기반으로 함)
            string[] fileNameParts = fileNameInputField.text.Split('-');
            string mapID = $"M{fileNameParts[0]}{fileNameParts[1].PadLeft(2, '0')}";

            // 제한 횟수를 저장할 데이터 구조
            var limitData = new Dictionary<string, object>
            {
                { "MapID", mapID },
                { "FileName", fileNameInputField.text },
                { "LimitCount", limitCount }
            };

            // 기존 테이블에 데이터 추가 또는 수정
            if (_limitCountTable.ContainsKey(mapID))
            {
                _limitCountTable[mapID] = limitData;
            }
            else
            {
                _limitCountTable.Add(mapID, limitData);
            }

            // LimitCountTable.json 파일 저장
            SaveLimitCountTable();

            // 저장 완료 메시지를 출력합니다.
            EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "제한 횟수 파일이 저장되었습니다.");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "제한 횟수 파일 저장 중 오류가 발생했습니다: " + e.Message);
        }
    }

    private void SaveSuccess()
    {
        // 타일 맵 JSON 저장
        string tileMapJson = JsonConvert.SerializeObject(_saveTileList, Formatting.Indented);
        string tileMapFilePath = Path.Combine(saveDirectory, fileNameInputField.text + ".json");
        File.WriteAllText(tileMapFilePath, tileMapJson);

        // 제한 횟수 JSON 저장
        SaveLimitCountJsonFile();

        // 저장 완료 메시지를 출력합니다.
        EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "JSON 파일과 제한 횟수 파일이 저장되었습니다.");
    }

    // 타일 맵 JSON 파일 로드
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

        // 사용자 지정 경로에 파일 경로를 설정
        string filePath = Path.Combine(saveDirectory, fileName + ".json");

        // 파일이 존재하는지 확인
        if (File.Exists(filePath))
        {
            // 파일에서 JSON 문자열을 읽기
            string json = File.ReadAllText(filePath);

            // JSON 문자열을 List<Tile>로 변환
            List<Tile> tilesData = JsonConvert.DeserializeObject<List<Tile>>(json);

            // InputField에 로드된 파일 이름을 다시 설정
            fileNameInputField.text = fileName;

            // 로드된 데이터 출력
            EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, $"{fileName}.json이 로드되었습니다");

            // 제한 횟수 로드
            LoadLimitCount(fileName);

            // 데이터 반환
            return tilesData;
        }
        else
        {
            EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, $"{fileName}.json을 찾을 수 없습니다");
            return null;
        }
    }

// 제한 횟수 불러오기 기능
    private void LoadLimitCount(string fileName)
    {
        string limitCountFilePath = Path.Combine(saveDirectory, "LimitCountTable.json");

        if (File.Exists(limitCountFilePath))
        {
            // LimitCountTable.json 파일에서 JSON 문자열 읽기
            string json = File.ReadAllText(limitCountFilePath);

            // JSON 문자열을 Dictionary로 변환
            var limitCountTable = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);

            // MapID 생성 (파일 이름의 앞부분을 기반으로 함)
            string[] fileNameParts = fileName.Split('-');
            string mapID = $"M{fileNameParts[0]}{fileNameParts[1].PadLeft(2, '0')}";

            // 해당 MapID가 LimitCountTable에 존재하는지 확인하고, 존재하면 제한 횟수 값을 가져오기
            if (limitCountTable.TryGetValue(mapID, out var limitData) &&
                limitData is Dictionary<string, object> limitDict)
            {
                if (limitDict.TryGetValue("LimitCount", out var limitCount))
                {
                    limitCountInputField.text = limitCount.ToString();
                }
                else
                {
                    limitCountInputField.text = "";
                    EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "제한 횟수를 찾을 수 없습니다.");
                }
            }
            else
            {
                // 제한 횟수를 찾을 수 없는 경우, 초기화
                limitCountInputField.text = "";
                EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "해당 MapID를 찾을 수 없습니다.");
            }
        }
        else
        {
            EventManager<UIEvents>.TriggerEvent(UIEvents.ErrorPopUP, "LimitCountTable.json 파일을 찾을 수 없습니다.");
        }
    }
}