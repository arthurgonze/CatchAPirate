using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] int match_duration_in_seconds = 60;
    [SerializeField] int enemy_spawn_cooldown_in_seconds = 60;

    public static GameSettings singleton_instance { get; private set; }

    private void Awake()
    {
        if (singleton_instance != null && singleton_instance != this)
            Destroy(gameObject);
        else
            singleton_instance = this;
        
        DontDestroyOnLoad(gameObject);
    }
    
    public int GetMatchDuration(){
        return match_duration_in_seconds;
    }

    public int GetEnemySpawnCooldown(){
        return enemy_spawn_cooldown_in_seconds;
    }

    public void SetEnemySpawnCooldown(int cooldown){
        enemy_spawn_cooldown_in_seconds = cooldown;
    }

    public void SetMatchDuration(int duration){
        match_duration_in_seconds = duration;
    }

    public void SetEnemySpawnCooldownFromMenu(){
        enemy_spawn_cooldown_in_seconds = FindObjectOfType<MenuManager>().GetEnemySpawnCooldown();
    }

    public void SetMatchDurationFromMenu(){
        match_duration_in_seconds = FindObjectOfType<MenuManager>().GetMatchDuration();
    }
}
