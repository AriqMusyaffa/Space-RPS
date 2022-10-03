using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private GameManager GM;
    public GameObject bgPrefab;

    void Start()
    {
        GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        transform.position -= new Vector3(0, 20, 0) * Time.deltaTime;

        if (transform.localPosition.y <= GM.bgDeletePos.y)
        {
            transform.localPosition = GM.bgSpawnPos;
        }
    }
}
