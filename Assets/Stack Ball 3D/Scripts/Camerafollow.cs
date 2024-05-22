using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    private Vector3 CamFollow;
    [SerializeField] private Transform ball, Win;
    private void Awake()
    {
        ball = FindObjectOfType<Ball>().transform;
    }
    void Update()
    {
        if (Win == null)
            Win = GameObject.Find("Win(Clone)").GetComponent<Transform>();

        if (transform.position.y > ball.transform.position.y && transform.position.y > Win.position.y + 4f)
            CamFollow = new Vector3(transform.position.x, ball.position.y, transform.position.z);

        transform.position =new Vector3(transform.position.x, CamFollow.y, -5);
    }
}
