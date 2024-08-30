using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DG.Tweening.Core.Easing;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySQLDBManager : MonoBehaviour
{
    private static MySQLDBManager _instance;
    public static MySQLDBManager Instance
    {
        get
        {
            // 만약 인스턴스가 null이면, 새로운 GameManager 오브젝트를 생성
            if (_instance == null)
            {
                _instance = FindObjectOfType<MySQLDBManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<MySQLDBManager>();
                    singletonObject.name = typeof(MySQLDBManager).ToString() + " (Singleton)";

                    // GameManager 오브젝트가 씬 전환 시 파괴되지 않도록 설정
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    //private AndroidJavaObject _androidJavaObject;
    private string connectionString;
    [SerializeField] private Text textCheck;
    //[SerializeField] private GameObject DBDataManager_;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //_androidJavaObject = new AndroidJavaObject("com.unity3d.player.KakaoLogin");
        // 데이터베이스 연결 문자열 설정
        connectionString = "Server=3.38.178.218;Database=ProjectP;User ID=ubuntu;Password=P@ssw0rd!;Pooling=false;SslMode=None;AllowPublicKeyRetrieval=true;";
    }

    // type = Table 이름 str = 넘겨받은 || 로 구분된 정보
    public void InsertData(string type, string str)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                // check용
                //textCheck.text += $"{str}\n";

                conn.Open();

                string[] strArr = str.Split("||");

                string query = $"INSERT INTO {type} (";

                string values = "VALUES (";

                switch (type)
                {
                    case "MemberInfo": // Kakao = 회원번호||이메일||닉네임||프로필사진URL
                        if (!string.IsNullOrEmpty(strArr[0]))
                        {
                            query += "MemberID";
                            values += "@MemberID";
                        }
                        if (!string.IsNullOrEmpty(strArr[1]))
                        {
                            query += ", Email";
                            values += ", @Email";
                        }
                        if (!string.IsNullOrEmpty(strArr[2]))
                        {
                            query += ", Nickname";
                            values += ", @Nickname";
                        }
                        if (!string.IsNullOrEmpty(strArr[3]))
                        {
                            query += ", ProfileURL";
                            values += ", @ProfileURL";
                        }
                        //if (!string.IsNullOrEmpty(strArr[4]))
                        //{
                        //    query += ", GuestPassword";
                        //    values += ", @GuestPassword";
                        //}
                        break;
                    case "Assets": // Kakao = 회원번호||이메일||닉네임||프로필사진URL
                        if (!string.IsNullOrEmpty(strArr[0]))
                        {
                            query += "MemberID";
                            values += "@MemberID";
                        }
                        if (!string.IsNullOrEmpty(strArr[1]))
                        {
                            query += ", Gold";
                            values += ", @Gold";
                        }
                        if (!string.IsNullOrEmpty(strArr[2]))
                        {
                            query += ", HeartTime";
                            values += ", @HeartTime";
                        }
                        if (!string.IsNullOrEmpty(strArr[3]))
                        {
                            query += ", ItemCount";
                            values += ", @ItemCount";
                        }
                        //if (!string.IsNullOrEmpty(strArr[4]))
                        //{
                        //    query += ", GuestPassword";
                        //    values += ", @GuestPassword";
                        //}
                        break;
                    case "MapData": // 새로운 MapData 케이스 추가
                        if (!string.IsNullOrEmpty(strArr[0]))
                        {
                            query += "Chapter";
                            values += "@Chapter";
                        }
                        if (!string.IsNullOrEmpty(strArr[1]))
                        {
                            query += ", Stage";
                            values += ", @Stage";
                        }
                        if (!string.IsNullOrEmpty(strArr[2]))
                        {
                            query += ", MapID";
                            values += ", @MapID";
                        }
                        if (!string.IsNullOrEmpty(strArr[3]))
                        {
                            query += ", TileValue";
                            values += ", @TileValue";
                        }
                        if (!string.IsNullOrEmpty(strArr[4]))
                        {
                            query += ", LimitCount";
                            values += ", @LimitCount";
                        }
                        break;
                    default:
                        Debug.LogError("Invalid query type provided.");
                        return;
                }

                query += ") ";
                values += ")";

                query += values;

                // check용
                //textCheck.text += $"{query}\n";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                switch (type)
                {
                    case "MemberInfo":
                        if (!string.IsNullOrEmpty(strArr[0])) cmd.Parameters.AddWithValue("@MemberID", Int64.Parse(strArr[0]));
                        if (!string.IsNullOrEmpty(strArr[1])) cmd.Parameters.AddWithValue("@Email", strArr[1]);
                        if (!string.IsNullOrEmpty(strArr[2])) cmd.Parameters.AddWithValue("@Nickname", strArr[2]);
                        if (!string.IsNullOrEmpty(strArr[3])) cmd.Parameters.AddWithValue("@ProfileURL", strArr[3]);
                        //if (!string.IsNullOrEmpty(strArr[4])) cmd.Parameters.AddWithValue("@GuestPassword", "");
                        break;
                    case "Assets":
                        if (!string.IsNullOrEmpty(strArr[0])) cmd.Parameters.AddWithValue("@MemberID", Int64.Parse(strArr[0]));
                        if (!string.IsNullOrEmpty(strArr[1])) cmd.Parameters.AddWithValue("@Gold", strArr[1]);
                        if (!string.IsNullOrEmpty(strArr[2])) cmd.Parameters.AddWithValue("@HeartTime", strArr[2]);
                        if (!string.IsNullOrEmpty(strArr[3])) cmd.Parameters.AddWithValue("@ItemCount", strArr[3]);
                        //if (!string.IsNullOrEmpty(strArr[4])) cmd.Parameters.AddWithValue("@GuestPassword", "");
                        break;
                    case "MapData": // 새로운 MapData 케이스 추가
                        if (!string.IsNullOrEmpty(strArr[0])) cmd.Parameters.AddWithValue("@Chapter", Int32.Parse(strArr[0]));
                        if (!string.IsNullOrEmpty(strArr[1])) cmd.Parameters.AddWithValue("@Stage", Int32.Parse(strArr[1]));
                        if (!string.IsNullOrEmpty(strArr[2])) cmd.Parameters.AddWithValue("@MapID", strArr[2]);
                        if (!string.IsNullOrEmpty(strArr[3])) cmd.Parameters.AddWithValue("@TileValue", strArr[3]);
                        if (!string.IsNullOrEmpty(strArr[4])) cmd.Parameters.AddWithValue("@LimitCount", Int32.Parse(strArr[4]));
                        break;
                    default:
                        Debug.LogError("Invalid query type provided.");
                        return;
                }

                cmd.ExecuteNonQuery();

                // 최초 로그인시 DBDataManager에 Dictionary 데이터 저장
                InputDataAtDictionary(type, str);

                Debug.Log("Data Inserted Successfully");
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to insert data: " + ex.Message);
            }
        }
    }

    public void ReadData(string str)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                //str = "MemberInfo||3666640951";

                // 체크용
                //textCheck.text += $"{str}\n";

                // strArr[0]은 테이블명 strArr[1]은 회원번호 고정
                string[] strArr = str.Split("||");

                string query = string.Empty;

                conn.Open();

                switch (strArr[0])
                {
                    case "MemberInfo": // Kakao = 회원번호||이메일||닉네임||프로필사진URL
                        query = $"SELECT * FROM {strArr[0]} WHERE MemberID = {strArr[1]}";
                        // 체크용
                        // textCheck.text += $"{query}\n";
                        break;
                    case "Assets": // Kakao = 회원번호||이메일||닉네임||프로필사진URL
                        query = $"SELECT * FROM {strArr[0]} WHERE MemberID = {strArr[1]}";
                        break;
                    default:
                        Debug.LogError("Invalid query type provided.");
                        return;
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    switch (strArr[0])
                    {
                        case "MemberInfo": // Kakao = 회원번호||이메일||닉네임||프로필사진URL
                            string MemberID = reader["MemberID"].ToString();
                            string Email = reader["Email"].ToString();
                            string Nickname = reader["Nickname"].ToString();
                            string ProfileURL = reader["ProfileURL"].ToString();

                            // 체크용
                            //textCheck.text += $"{MemberID}\n{Email}\n{Nickname}\n{ProfileURL}";

                            InputDataAtDictionary("MemberInfo", $"{MemberID}||{Email}||{Nickname}||{ProfileURL}");
                            break;
                        case "Assets": // Kakao = 회원번호||이메일||닉네임||프로필사진URL
                            string ID = reader["MemberID"].ToString();
                            string Gold = reader["Gold"].ToString();
                            string HeartTime = reader["HeartTime"].ToString();
                            string ItemCount = reader["ItemCount"].ToString();

                            // 체크용
                            //textCheck.text += $"{MemberID}\n{Email}\n{Nickname}\n{ProfileURL}";

                            InputDataAtDictionary("Assets", $"{ID}||{Gold}||{HeartTime}||{ItemCount}");
                            break;
                        default:
                            Debug.LogError("Invalid query type provided.");
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to read data: " + ex.Message);
            }
        }
    }

    // Kotlin에서 넘겨받은 카카오톡 유저 데이터
    public void GetAndSetUserData(string userData)
    {
        //userData = "3666640951||dls625@hanmail.net||.||https://img1.kakaocdn.net/thumb/R110x110.q70/?fname=https://t1.kakaocdn.net/account_images/default_profile.jpeg";

        InsertData("MemberInfo", userData);

        // 만들어야 할거!
        //InsertData("Assets", userData);
    }

    public void InputDataAtDictionary(string type, string str)
    {
        string[] strArr = str.Split("||");

        switch (type)
        {
            case "MemberInfo":
                //DBDataManager.Instance.UserData.Add("MemberID", strArr[0]);
                //DBDataManager.Instance.UserData.Add("Email", strArr[1]);
                //DBDataManager.Instance.UserData.Add("Nickname", strArr[2]);
                //DBDataManager.Instance.UserData.Add("ProfileURL", strArr[3]);
                //DBDataManager.Instance.ShowDicDataCheck("UserData");
                // GuestLogin에서 안불러와져서, 여기서 Assets 데이터 Call.
                ReadData($"Assets||{strArr[0]}");
                break;
            case "Assets":
                //DBDataManager.Instance.UserAssetsData.Add("MemberID", strArr[0]);
                //DBDataManager.Instance.UserAssetsData.Add("Gold", strArr[1]);
                //DBDataManager.Instance.UserAssetsData.Add("HeartTime", strArr[2]);
                //DBDataManager.Instance.UserAssetsData.Add("ItemCount", strArr[3]);
                //DBDataManager.Instance.ShowDicDataCheck("Assets");
                // 다 불러오면 Scene Change
                SceneManager.LoadScene("Jinmyung");
                break;
            default:
                Debug.LogError("Invalid query type provided.");
                return;
        }

    }

    public void ParsingListDataToDB(List<Tile> tileList, string MapID, int LimitCount)
    {
        // StringBuilder 초기화
        StringBuilder sb = new StringBuilder();

        string[] mapID = MapID.Split("-");
        // Chapter
        sb.Append(mapID[0]);
        sb.Append("||");
        // Stage
        sb.Append(mapID[1]);
        sb.Append("||");
        // MapID
        sb.Append(MapID);
        sb.Append("||");

        int count = 0;  // 인덱스 역할을 하는 변수
        int totalCount = tileList.Count;  // 전체 리스트의 길이

        foreach (Tile tile in tileList) 
        {
            //public TileType Type; // 빈 타일, 길 타일, 기믹 타일
            sb.Append(((int)tile.Type).ToString());
            sb.Append("/");
            //public RoadShape RoadShape;
            sb.Append(((int)tile.RoadShape).ToString());
            sb.Append("/");
            //public GimmickShape GimmickShape;
            sb.Append(((int)tile.GimmickShape).ToString());
            sb.Append("/");
            //public int RotateValue;
            sb.Append(tile.RotateValue.ToString());

            count++;  // 카운트를 증가시킴

            if (count < totalCount)  // 마지막 인덱스가 아닌 경우에만 # 추가
            {
                sb.Append("#");
            }
        }
        sb.Append("||");
        // LimitCount
        sb.Append(LimitCount.ToString());

        // DB로 데이터 넘김.
        InsertData("MapData", sb.ToString());
        //Debug.Log(sb.ToString());
    }
}
