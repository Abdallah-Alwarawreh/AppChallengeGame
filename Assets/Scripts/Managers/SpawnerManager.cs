using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class SpawnerManager : MonoBehaviour{
    public static SpawnerManager instance;
    [Header("Spawner")]
    [SerializeField] private List<Transform> SpawnersPoints = new List<Transform>();
    [SerializeField] private List<SpawnableVars> Spawnables = new List<SpawnableVars>();
    [SerializeField] private List<SpawnableWithTimerVars> SpawnablesWithTimers = new List<SpawnableWithTimerVars>();
    [SerializeField, Tooltip("The spawning interval.")] private float SpawnInterval = 1f;
    [SerializeField, Tooltip("The amount of time to remove from spawnInterval each x seconds.")] private float DecreaseSpawnIntervalBy = 0.1f;
    [SerializeField, Tooltip("The amount of time needed to decrease the spawnInterval by \"decreaseSpawnIntervalBy\"")] private float AmountOfTimeToDecreaseSpawnInterval = 10f;
    [SerializeField, Tooltip("The minimum value of \"spawnInterval\"")] private float MinSpawnInterval = 0.1f;
    [SerializeField, Tooltip("the amount of time to increase the Time.timeScale by each x seconds.")] private float IncreaseTimeScaleBy = 0.1f;
    [SerializeField, Tooltip("the amount of time needed to increase the Time.timeScale by \"increaseTimeScaleBy\"")] private float AmountOfTimeToIncreaseTimeScale = 10f;
    [SerializeField, Tooltip("the maximum value of Time.timeScale")] private float MaxTimeScale = 2f;
    [Header("Question Block")]
    [SerializeField] private GameObject QuestionBlockPrefab;
    [SerializeField, Range(0f, 100f)] private float QuestionBlockSpawnChance;
    [SerializeField] private float StopSpawningFor;
    [SerializeField] private float RestQuestionBlockEach;
    
    private bool HasSpawnedQuestionBlock = false;
    [HideInInspector]public bool StopSpawning = false;
    private double AccumulatedWeights;
    private System.Random rand = new System.Random();
    private float SpawnTimerMax;
    private float SpawnTimer;
    private float DecreaseTimer;
    private float TimeScaleTimer;
    private float RestQuestionBlockEachTimer;
    private void Awake(){
        
        instance = this;
        SpawnTimerMax = SpawnInterval;
        CalculateWeights();
    }

    void Update(){
        if (StopSpawning){
            StopSpawningFor -= Time.deltaTime;
            if (StopSpawningFor <= 0){
                StopSpawning = false;
                GameManager.instance.ChangeDecreaseFuel(true);
            }
        }
        if(GameManager.instance.IsGameOver || StopSpawning || GameManager.instance.IsFightingAliens) return;
        TimeScaleTimer += Time.deltaTime;
        DecreaseTimer += Time.deltaTime;
        SpawnTimer += Time.deltaTime;
        if(GameManager.instance.IsEndlessMode && HasSpawnedQuestionBlock){
            RestQuestionBlockEachTimer += Time.deltaTime;
            if(RestQuestionBlockEachTimer >= RestQuestionBlockEach){
                RestQuestionBlockEachTimer = 0;
                HasSpawnedQuestionBlock = false;
            }
        }
        if(SpawnTimer >= SpawnTimerMax){
            if (!HasSpawnedQuestionBlock){
                float QuestionBlockSpawnChance = UnityEngine.Random.Range(0f, 100f);
                if(QuestionBlockSpawnChance <= this.QuestionBlockSpawnChance){
                    StopSpawning = true;
                    SpawnQuestionBlock();
                } else{
                    SpawnObstacle();
                }
            } else{
                SpawnObstacle();
            }
            SpawnTimer = 0;
        }

        if (DecreaseTimer >= AmountOfTimeToDecreaseSpawnInterval){
            DecreaseSpawnInterval();
            DecreaseTimer = 0;
        }
        
        if (TimeScaleTimer >= AmountOfTimeToIncreaseTimeScale){
            Time.timeScale += IncreaseTimeScaleBy;
            if(Time.timeScale > MaxTimeScale){
                Time.timeScale = MaxTimeScale;
            }

            TimeScaleTimer = 0;
        }

        foreach (var obj in SpawnablesWithTimers){
            if (obj.OnUpdate()){
                Instantiate(obj.prefab, GetRandomSpawnPoint(), obj.prefab.transform.rotation);
            }
        }
        
        
    }

    /// <summary>
    /// Spawns the question block.
    /// </summary>
    private void SpawnQuestionBlock(){
        HasSpawnedQuestionBlock = true;
        Instantiate(QuestionBlockPrefab, GetRandomSpawnPoint(), QuestionBlockPrefab.transform.rotation);
        GameManager.instance.ChangeDecreaseFuel(false);
    }

    /// <summary>
    /// Spawns an obstacle at a random spawner point.
    /// </summary>
    void SpawnObstacle(){
        SpawnableVars Spawn = Spawnables[GetRandomSpawnableIndex()];
        if(Spawn == null){
            return;
        }
        
        GameObject Obstacle = Instantiate(Spawn.prefab, GetRandomSpawnPoint(), Spawn.prefab.transform.rotation);
        
    }

    /// <summary>
    /// Gets a random spawnable index based on the spawnChance of each spawnable.
    /// </summary>
    /// <returns>The random spawnable.</returns>
    private int GetRandomSpawnableIndex() {
        double r = rand.NextDouble () * AccumulatedWeights ;

        for (int i = 0; i < Spawnables.Count; i++){
            if (Spawnables[i].weight >= r && GameManager.instance.Score >= Spawnables[i].MinScoreToSpawn){
                return i ;
            }
        }

        return 0;
    }
    
    #if UNITY_EDITOR
    /// <summary>
    /// Gets a random spawnable index based on the spawnChance of each spawnable.
    /// </summary>
    /// <returns>The random spawnable.</returns>
    private int GetRandomSpawnableIndexEditor() {
        double r = rand.NextDouble () * AccumulatedWeights ;

        for (int i = 0; i < Spawnables.Count; i++)
            if (Spawnables[i].weight >= r)
                return i ;

        return 0 ;
    }
    #endif
    
    /// <summary>
    /// Calculates the weights of each spawnable.
    /// </summary>
    void CalculateWeights(){
        AccumulatedWeights = 0f ;
        foreach (SpawnableVars spawnable in Spawnables) {
            AccumulatedWeights += spawnable.spawnChance ;
            spawnable.weight = AccumulatedWeights ;
        }
    }


    /// <summary>
    /// return a random Vector3 between the two SpawnerPoints;
    /// </summary>
    /// <returns>Vector 3 pos.</returns>
    public Vector3 GetRandomSpawnPoint(){
        float X1 = SpawnersPoints[0].position.x;
        float Z1 = SpawnersPoints[0].position.z;
        
        float X2 = SpawnersPoints[1].position.x;
        float Z2 = SpawnersPoints[1].position.z;
        
        Vector3 Pos = new Vector3(UnityEngine.Random.Range(X1, X2), SpawnersPoints[0].position.y, UnityEngine.Random.Range(Z1, Z2));
        return Pos;
    }
    

    /// <summary>
    /// Decrease the spawn interval by the "amount".
    /// </summary>
    /// <param name="amount"></param>
    public void DecreaseSpawnInterval(){
        SpawnTimerMax -= DecreaseSpawnIntervalBy;
        SpawnTimerMax = Mathf.Clamp(SpawnTimerMax, MinSpawnInterval, SpawnInterval);
    }

    /// <summary>
    /// Spawns an Object.
    /// </summary>
    public GameObject SpawnObject(GameObject obj){
        return Instantiate(obj, GetRandomSpawnPoint(), obj.transform.rotation);
        
    }

    #if UNITY_EDITOR
    public string TestSpawning(){
        CalculateWeights();
        int[] test1 = new int[Spawnables.Count];
        for (int i = 0; i < 200; i++){
            int index = GetRandomSpawnableIndexEditor();
            test1[index]++;
        }
        
        int[] test2 = new int[Spawnables.Count];
        for (int i = 0; i < 200; i++){
            int index = GetRandomSpawnableIndexEditor();
            test2[index]++;
        }

        int[] test3 = new int[Spawnables.Count];
        for (int i = 0; i < 200; i++){
            int index = GetRandomSpawnableIndexEditor();
            test3[index]++;
        }

        int[] TestAverage = new int[Spawnables.Count];
        for (int i = 0; i < Spawnables.Count; i++){
            TestAverage[i] = (test1[i] + test2[i] + test3[i]) / 3;
        }


        string result = "";
        for (int i = 0; i < TestAverage.Length; i++){
            result += Spawnables[i].name + " has spawned " + TestAverage[i] + "times.\n";
        }
        foreach (SpawnableVars spawnable in Spawnables) {
            spawnable.weight = 0 ;
        }
        return result;
    }
    #endif
    
    /// <summary>
    /// Adds amount to the speed (Time.timeScale)
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeSpeed(float amount){
        Time.timeScale += amount;
        if(Time.timeScale > MaxTimeScale){
            Time.timeScale = MaxTimeScale;
        }else if (Time.timeScale < 1){
            Time.timeScale = 1;
        }
    }
}

[Serializable]
public class SpawnableVars{
    public string name;
    public GameObject prefab;
    [Range(0f, 100f)] public float spawnChance;
    public float MinScoreToSpawn;

    [HideInInspector] public double weight;
}


[Serializable]
public class SpawnableWithTimerVars{
    public string name;
    public GameObject prefab;
    public float SpawnInterval;
    private float Timer;

    public bool OnUpdate(){
        Timer += Time.deltaTime;
        if(Timer >= SpawnInterval){
            Timer = 0;
            return true;
        }
        
        return false;
    }
}
