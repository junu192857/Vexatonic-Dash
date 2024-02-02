using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Note : MonoBehaviour
{
    public bool permanent = false;
    public bool activated;

    //개별 노트의 게임오브젝트마다 달려 있는 스크립트.
    public double lifetime; //노트의 생존 시간. 1초부터 시작하며 0초일 때 노트를 처리하는 것이 정확한 타이밍이다.
    public double noteEndTime; // noteEndTime = 다음 노트의 spawnTime
    public NoteType noteType; // Normal, Dash, Jump, Attack, Defend. 프리팹 미리 만들 거라 알아서 0부터 하나씩 들어 있다.
    public NoteSubType noteSubType; // Ground, Air, Wall의 3종류. Wall은 JumpNote에만 쓸 수 있으며, PlatformNote에 쓰면 에러가 나도록 할 생각.
    public CharacterDirection direction; // 노트를 처리했을 시 캐릭터가 이동하는 방향. 중력 방향을 아래라 했을 때 왼쪽, 오른쪽 기준
    public Vector3 spawnPos;
    public Vector3 destPos;

    public Vector3 startPos;
    public Vector3 endPos;


    public void Deactivate() {
        activated = false;
        gameObject.SetActive(false);
    }

    public void Activate() { 
        activated  =true;
        gameObject.SetActive(true);
    }

    public void SetLifetime(double gameTime, double spawnTime) {
        lifetime = spawnTime - gameTime;
    }

    public void FixNote() {
        permanent = true;
        transform.position = destPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!permanent && activated)
        {
            lifetime -= Time.deltaTime;
            transform.position = spawnPos * (float)lifetime + destPos * (float)(1 - lifetime);
        }
    }


}
