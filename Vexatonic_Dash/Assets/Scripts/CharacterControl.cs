using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private IEnumerator characterCoroutine;

    private float g => GameManager.g;
    private int gravityAngle;

    public void MoveCharacter(Note note, double gameTime) {
        if (characterCoroutine != null) StopCoroutine(characterCoroutine);
        switch (note.noteType) {
            case NoteType.Normal:
            case NoteType.Dash:
                if (note is PlatformNote)
                {
                    characterCoroutine = MoveCharacterCoroutine(note, gameTime);
                    StartCoroutine(characterCoroutine);
                }
                else Debug.LogError("note type wrong");
                break;
            case NoteType.Jump:
                if (note is JumpNote)
                {
                    characterCoroutine = JumpCharacterCoroutine(note, gameTime);
                    StartCoroutine(characterCoroutine);
                }
                else Debug.LogError("note type wrong");
                break;
            default:
                break;
        }
    }

    private IEnumerator MoveCharacterCoroutine(Note note, double gameTime) {
        PlatformNote platformNote = note as PlatformNote;
        gameObject.transform.position = platformNote.startPos;

        float playerMovingTime = (float)(platformNote.noteEndTime - gameTime);
        float time = 0;
        Debug.Log("Time that managing character position: " + Time.time);
        while (time < playerMovingTime + 0.166f) {
            Vector3 targetPosition = platformNote.startPos * (playerMovingTime - time) / playerMovingTime + platformNote.endPos * time / playerMovingTime;
            gameObject.transform.position = targetPosition;
            time += Time.deltaTime;
            yield return null;
        }
        characterCoroutine = null;
    }

    private IEnumerator JumpCharacterCoroutine(Note note, double gameTime) {
        JumpNote jumpNote = note as JumpNote;


        gameObject.transform.position = jumpNote.startPos;
        

        float playerMovingTime = (float)(jumpNote.noteEndTime - gameTime);
        float time = 0;

        float v_x = (note.endPos.x - note.startPos.x) / playerMovingTime;
        float v_y = (note.endPos.y - note.startPos.y) / playerMovingTime - 0.5f * g * playerMovingTime;
        
        // Vector3 vDelta = (note.endPos - note.startPos) / playerMovingTime;
        // Vector3 vVertical = -0.5f * g * playerMovingTime;
        // Vector3 v = vDelta + vVertical

        while (time < playerMovingTime + 0.166f) {
            Vector3 targetPosition = CalculateJumpPosition(v_x, v_y, time, note.startPos);
            if (time > playerMovingTime) targetPosition.y = note.endPos.y;
            // Vector3 targetPosition = CalculateJumpPosition(v, time, note.startPos);
            // if (time > playerMovingTime) targetPosition = note.endPos;
            gameObject.transform.position = targetPosition;
            time += Time.deltaTime;
            yield return null;
        }
        characterCoroutine = null;
    }

    [Obsolete("Please provide vector directly instead its x and y coordination")]
    private Vector3 CalculateJumpPosition(float v_x, float v_y, float time, Vector3 startPos) {
        float x = startPos.x + v_x * time;
        float y = startPos.y + v_y * time + 0.5f * g * time * time;

        return new Vector3(x, y, 0);
    }

    private Vector3 CalculateJumpPosition(Vector3 v, float time, Vector3 startPos)
    {
        return startPos + (v * time) + (0.5f * GameManager.myManager.GravityAsVector * time * time);
    }

    private void Start()
    {
        UpdateGravity();
    }

    private void Update()
    {
        UpdateGravity();
    }
    
    private void UpdateGravity()
    {
        gravityAngle = GameManager.myManager.gravity;
        transform.rotation = Quaternion.Euler(0f, 0f, gravityAngle);
    }
}
