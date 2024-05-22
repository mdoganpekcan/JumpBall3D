using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private float CurrentTime;

    private bool Smash, invincible;

    private int currentBrokenStacks, totalStacks;

    public GameObject invincibleObject;
    public UnityEngine.UI.Image invincibleFill;
    public GameObject fireEffect, winEffect, splashEffect;

    public enum BallState
    {
        Prepare,
        Playing,
        Died,
        Finish
    }

    public BallState ballState = BallState.Prepare;

    public AudioClip bounceOffClip, deadClip, winClip, destroyClip, iDestroyClip;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentBrokenStacks = 0;
    }
    void Start()
    {
        totalStacks = FindObjectsOfType<StackController>().Length;
    }
    void Update()
    {
        if (ballState == BallState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
                Smash = true;

            if (Input.GetMouseButtonUp(0))
                Smash = false;

            if (invincible)
            {
                CurrentTime -= Time.deltaTime * 0.35f;
                if (!fireEffect.activeInHierarchy)
                    fireEffect.SetActive(true);
            }
            else
            {
                if (fireEffect.activeInHierarchy)
                    fireEffect.SetActive(false);

                if (Smash)
                    CurrentTime += Time.deltaTime * 0.8f;
                else
                    CurrentTime -= Time.deltaTime * 0.5f;
            }

            if (CurrentTime >= 0.3f || invincibleFill.color == Color.red)
                invincibleObject.SetActive(true);
            else
                invincibleObject.SetActive(false);

            if (CurrentTime >= 1)
            {
                CurrentTime = 1;
                invincible = true;
                invincibleFill.color = Color.red;
            }
            else if (CurrentTime <= 0)
            {
                CurrentTime = 0;
                invincible = false;
                invincibleFill.color = Color.white;
            }

            if (invincibleObject.activeInHierarchy)
                invincibleFill.fillAmount = CurrentTime / 1;
        }

        if (ballState == BallState.Finish)
        {
            if (Input.GetMouseButtonDown(0))
                FindObjectOfType<LevelSpawner>().NextLevel();
        }
    }
    private void FixedUpdate()
    {
        if (ballState == BallState.Playing)
        {
            if (Input.GetMouseButton(0))
            {
                Smash = true;
                rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }
        }

        if (rb.velocity.y > 5)
        {
            rb.velocity = new Vector3(rb.velocity.x, 5, rb.velocity.z);
        }
    }
    public void IncreaseBrokenStacks()
    {
        currentBrokenStacks++;

        if (!invincible)
        {
            ScoreManager.instance.AddScore(1);
            SoundManager.instance.PlaySoundFX(destroyClip, 0.5f);
        }
        else
        {
            ScoreManager.instance.AddScore(2);
            SoundManager.instance.PlaySoundFX(iDestroyClip, 0.5f);
        }
    }
    private void OnCollisionEnter(Collision Target)
    {
        if (!Smash)
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);

            if (Target.gameObject.tag != "Finish")
            {
                GameObject splash = Instantiate(splashEffect);
                splash.transform.SetParent(Target.transform);
                splash.transform.localEulerAngles = new Vector3(90, Random.Range(0, 359), 0);
                float randomScale = Random.Range(0.18f, 0.25f);
                splash.transform.localScale = new Vector3(randomScale, randomScale, 1);
                splash.transform.position = new Vector3(transform.position.x, transform.position.y - 0.22f, transform.position.z);
                splash.GetComponent<SpriteRenderer>().color = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
            }

            SoundManager.instance.PlaySoundFX(bounceOffClip, 0.5f);
        }
        else
        {
            if (invincible)
            {
                if (Target.gameObject.tag == "enemy" || Target.gameObject.tag == "plane")
                {
                    Target.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
            }
            else
            {
                if (Target.gameObject.tag == "enemy")
                {
                    Target.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }

                if (Target.gameObject.tag == "plane")
                {
                    rb.isKinematic = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                    ballState = BallState.Died;
                    SoundManager.instance.PlaySoundFX(deadClip, 0.5f);
                }
            }
        }

        FindObjectOfType<GameUI>().LevelSliderFill(currentBrokenStacks / (float)totalStacks);

        if (Target.gameObject.tag == "Finish" && ballState == BallState.Playing)
        {
            ballState = BallState.Finish;
            SoundManager.instance.PlaySoundFX(winClip, 0.7f);
            GameObject win = Instantiate(winEffect);
            win.transform.SetParent(Camera.main.transform);
            win.transform.localPosition = Vector3.up * 1.5f;
            win.transform.localEulerAngles = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision Target)
    {
        if (!Smash || Target.gameObject.tag == "Finish")
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }
    }
}