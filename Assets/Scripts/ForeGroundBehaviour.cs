using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeGroundBehaviour : MonoBehaviour
{
  public float transparencyVal;
  public float lerpDuration = 3f;
  private bool fade = false;
  private bool unfade = false;
  private bool currentlyFading = false;

  private void OnTriggerEnter2D(Collider2D other) {
    PlayerController controller = other.GetComponent<PlayerController>();
    if(controller != null) {
      fade = true;
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    PlayerController controller = other.GetComponent<PlayerController>();
    if(controller != null) {
      unfade = true;
    }
  }

  IEnumerator FadeSprite(SpriteRenderer sr, float endValue, float duration) {
    float time = 0f;
    float currentAlpha = 0f;

    currentlyFading = true;
    while(time < duration) {
      currentAlpha = sr.material.GetFloat("_Transparency");
      sr.material.SetFloat("_Transparency", Mathf.Lerp(currentAlpha, endValue, time / duration));
      time += Time.deltaTime;
      yield return null;
    }
    currentlyFading = false;

    sr.material.SetFloat("_Transparency", endValue);
  }

  void Update() {
    if(fade && !currentlyFading) {
      for(int i = 0; i < gameObject.transform.childCount; i++)
      {
        GameObject child = gameObject.transform.GetChild(i).gameObject;
        StartCoroutine(FadeSprite(child.GetComponent<SpriteRenderer>(), transparencyVal, lerpDuration));
      }      
      fade = false;
    } 

    if (unfade && !currentlyFading) {
      for(int i = 0; i < gameObject.transform.childCount; i++)
      {
        GameObject child = gameObject.transform.GetChild(i).gameObject;
        StartCoroutine(FadeSprite(child.GetComponent<SpriteRenderer>(), 0f, lerpDuration));
      }
      unfade = false;
    }

  }

}

