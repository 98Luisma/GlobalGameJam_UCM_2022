using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private bool isPaused = false;

    [SerializeField] private List<GameObject> _blockPrefabs = null;
    [SerializeField] private float _blockZSize = 10;
    [SerializeField] private float startZ = 10;
    [SerializeField] private int endAfter = 3;
    [SerializeField] private float depth = 0;
    [SerializeField] private float speed = 4f;

    private List<GameObject> spawnedBlocks = null;

    private Vector3 startPos = new Vector3(0.0f, 0.0f, 10.0f);
    private Vector3 endPos = new Vector3(0.0f, 0.0f, -20.0f);
    private float zSizeBlocks = 30;
    private Vector3 offset = new Vector3(0.0f, 0.0f, 10.0f);

    private static BackgroundManager _instance;
    public static BackgroundManager Instance { get => _instance; }

    private void Awake()
    {
        // Singleton implementation 
        if (_instance && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        // End of singleton implementation
    }

    // Start is called before the first frame update
    void Start()
    {
        zSizeBlocks = endAfter * _blockZSize;
        startPos = new Vector3(0.0f, depth, startZ);
        endPos = new Vector3(0.0f, depth, startPos.z - zSizeBlocks);
        offset = new Vector3(0.0f, 0.0f, -_blockZSize);

        int numPrefabs = _blockPrefabs.Count;
        if (numPrefabs > 0)
        {
            spawnedBlocks = new List<GameObject>();
            for (int i = 0; i < endAfter; i++)
            {
                spawnedBlocks.Add(Object.Instantiate(_blockPrefabs[i%numPrefabs], startPos + (i * offset), Quaternion.identity) as GameObject);
            }  
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused) 
        {
            if (_blockPrefabs.Count > 0)
            {
                Vector3 mov = speed * Vector3.back * Time.deltaTime;
                foreach (GameObject block in spawnedBlocks)
                {
                    block.transform.position += mov;
                    if (block.transform.position.z < endPos.z)
                    {
                        block.transform.position += Vector3.forward * zSizeBlocks;
                    }
                }
                
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }
}
