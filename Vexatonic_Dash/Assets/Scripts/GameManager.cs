using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RankType
{
    V,
    S,
    A,
    B,
    C,
    D,
}

public class GameManager : MonoBehaviour
{
    public static GameManager myManager;

    public float scrollSpeed; // 카메라 이동 속도를 의미한다.
    public float noteSpeed; // 노트의 속력을 의미한다.
    public float notePosition; // 판정선의 위치를 의미한다.
    public float volume; // 소리 크기를 의미한다.

    public string filepath; // 레벨의 맵 파일 위치를 의미한다.
    // 맵 하나 추가할 때마다 업데이트는 불가능 --> filepath도 외부에 있어야 한다.

    public RhythmManager rm;
    public UIManager um;
    public InputManager im;

    public void Awake()
    {
        if (myManager == null)
        {
            myManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
        if (scrollSpeed == 0f) scrollSpeed = 1f;
    }

    public void Start()
    {
        MetaReader.GetSongMeta();
    }

    public static RankType GetRank(int score) => score switch
    {
        1010000                  => RankType.V,
        >= 1000000 and < 1010000 => RankType.S,
        >= 950000  and < 1000000 => RankType.A,
        >= 900000  and < 950000  => RankType.B,
        >= 800000  and < 900000  => RankType.C,
        >= 0       and < 800000  => RankType.D,
        _ => throw new ArgumentOutOfRangeException()
    };

    public float CalculateInputWidthFromTime(double time) =>  scrollSpeed * 2 * (float)time;
    public double CalculateTimeFromInputWidth(float width) => width / (2 * scrollSpeed);

    public const float g = -9.8f;
}
