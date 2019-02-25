using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main SINGLETON;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;

    private BoundsCheck _bndCheck;

    void Awake()
    {
        //calling spawn enemy after rocket is created
        SINGLETON = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1.5f / enemySpawnPerSecond);
    }

    public void SpawnEnemy()
    {
        //instatiate random enemy type
        int _index = Random.Range(0, prefabEnemies.Length);
        GameObject _enemy = Instantiate<GameObject>(prefabEnemies[_index]);

        //bounds check
        float enemyPadding = enemyDefaultPadding;
        if (_enemy.GetComponent<BoundsCheck>() != null)
            enemyPadding = Mathf.Abs(_enemy.GetComponent<BoundsCheck>().radius);

        Vector3 _pos = Vector3.zero;
        float _xMin = -_bndCheck.camWidth + enemyPadding;
        float _xMax = _bndCheck.camWidth - enemyPadding;
       _pos.x = Random.Range(_xMin, _xMax);
        _pos.y = _bndCheck.camHeight + enemyPadding;
        _enemy.transform.position = _pos;

        Invoke("SpawnEnemy", 1.5f / enemySpawnPerSecond);
    }

}