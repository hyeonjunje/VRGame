using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[System.Serializable]
public class PatrolRoutine
{
    public Transform startPos;
    public Transform endPos;
}


public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private int maxCount;
    [SerializeField] private List<Transform> spawnPoses;
    [SerializeField] private List<PatrolRoutine> spawnPatrolPoses;

    private Queue<Zombie> zombiePool = new Queue<Zombie>();
    
    private void Awake()
    {
        for(int i = 0; i < maxCount; i++)
        {
            zombiePool.Enqueue(CreateObject());
        }
    }


    public void SpawnZombie()
    {
        NavMeshHit hit;

        // 그냥 좀비 생성
        for (int i = 0; i < spawnPoses.Count; i++)
        {
            if (NavMesh.SamplePosition(spawnPoses[i].position, out hit, 5f, NavMesh.AllAreas))
                SpawnZombie(hit.position);
        }

        // 순찰하는 좀비 생성
        for (int i = 0; i < spawnPatrolPoses.Count; i++)
        {
            if (NavMesh.SamplePosition(spawnPatrolPoses[i].startPos.position, out hit, 5f, NavMesh.AllAreas))
                spawnPatrolPoses[i].startPos.position = hit.position;
            if (NavMesh.SamplePosition(spawnPatrolPoses[i].endPos.position, out hit, 5f, NavMesh.AllAreas))
                spawnPatrolPoses[i].endPos.position = hit.position;


            SpawnPatrolZombie(spawnPatrolPoses[i]);
        }
    }


    private Zombie CreateObject()
    {
        Zombie result = Instantiate(zombiePrefab).GetComponent<Zombie>();
        result.zombieSpawner = this;
        result.gameObject.SetActive(false);
        return result;
    }


    private Zombie GetObject()
    {
        if (zombiePool.Count <= 0)
            zombiePool.Enqueue(CreateObject());

        Zombie result = zombiePool.Dequeue();
        result.MyReset();
        result.gameObject.SetActive(true);

        return result;
    }


    private void ReleaseObject(Zombie zombieObject)
    {
        zombieObject.gameObject.SetActive(false);
        zombiePool.Enqueue(zombieObject);

        zombieObject.agent.enabled = false;
    }


    private void SpawnZombie(Vector3 pos)
    {
        Zombie zombie = GetObject();

        zombie.agent.Warp(pos);
        zombie.transform.rotation = Quaternion.Euler(new Vector3(0f, UnityEngine.Random.Range(0, 360), 0f));

        zombie.agent.enabled = true;
    }


    private void SpawnPatrolZombie(PatrolRoutine spawnPatrolPos)
    {
        Zombie zombie = GetObject();
        zombie.SetPatrolInfo(true, spawnPatrolPos);

        zombie.agent.Warp(spawnPatrolPos.startPos.position);

        zombie.agent.enabled = true;
    }
}
