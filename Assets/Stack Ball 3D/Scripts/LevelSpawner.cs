using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSpawner : MonoBehaviour
{
    public GameObject[] Models;
    [HideInInspector]
    public GameObject[] ModelPrefab= new GameObject[4];
    public GameObject WinPrefab;

    private GameObject Temp1, Temp2;

    public int Level = 1, AddOn = 7;
    float i = 0;

    public Material plateMat, baseMat;
    public MeshRenderer ballMesh;

    void Awake()
    {
        plateMat.color = Random.ColorHSV(0, 1, 0.5f, 1, 1, 1);
        baseMat.color = plateMat.color + Color.gray;
        ballMesh.material.color = plateMat.color;

        Level = PlayerPrefs.GetInt("Level", 1);

        if (Level > 9)
            AddOn = 0;

        ModelSelection();
        float random = Random.value;
        for (i=0; i > -Level - AddOn; i -= 0.5f)
        {
            if (Level <= 20)
                Temp1 = Instantiate(ModelPrefab[Random.Range(0, 2)]);

            if (Level > 20 && Level <= 50)
                Temp1 = Instantiate(ModelPrefab[Random.Range(1, 3)]);

            if (Level > 50 && Level <= 100)
                Temp1 = Instantiate(ModelPrefab[Random.Range(2, 4)]);

            if (Level > 100)
                Temp1 = Instantiate(ModelPrefab[Random.Range(3, 4)]);

            Temp1.transform.position = new Vector3(0, i - 0.01f, 0);
            Temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);

            if (Mathf.Abs(i) >=Level * 0.3f  && Mathf.Abs(i) <= Level * 0.6f)
            {
                Temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);
                Temp1.transform.eulerAngles += Vector3.up * 180;
            }
            else if (Mathf.Abs(i) >= Level * 0.8f)
            {
                Temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);

                if (random > 0.75f)
                    Temp1.transform.eulerAngles += Vector3.up * 180;
            }

            Temp1.transform.parent = FindObjectOfType<Rotator>().transform;
        }

        Temp2 = Instantiate(WinPrefab);
        Temp2.transform.position = new Vector3(0, i - 0.01f, 0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            plateMat.color = Random.ColorHSV(0, 1, 0.5f, 1, 1, 1);
            baseMat.color = plateMat.color + Color.gray;
            ballMesh.material.color = plateMat.color;
        }
    }

    void ModelSelection()
    {
        int randomModel = Random.Range(0, 5);

        switch (randomModel)
        {
            case 0:
                for (int i = 0; i < 4; i++)
                {
                    ModelPrefab[i] = Models[i];
                }
                break;
            case 1:
                for (int i = 0; i < 4; i++)
                {
                    ModelPrefab[i] = Models[i + 4];
                }
                break;
            case 2:
                for (int i = 0; i < 4; i++)
                {
                    ModelPrefab[i] = Models[i + 8];
                }
                break;
            case 3:
                for (int i = 0; i < 4; i++)
                {
                    ModelPrefab[i] = Models[i + 12];
                }
                break;
            case 4:
                for (int i = 0; i < 4; i++)
                {
                    ModelPrefab[i] = Models[i + 16];
                }
                break;
        }
    }
    public void NextLevel()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        SceneManager.LoadScene(0);
    }
}
