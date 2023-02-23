using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    [SerializeField] int total_points = 0;
    [SerializeField] float total_game_duration_in_seconds = 180.0f; // 3 minutes

    [SerializeField] int enemy_count = 0;
    [SerializeField] float spawn_enemy_time_in_seconds = 20.0f;
    [SerializeField] GameObject enemy_prefab;
    [SerializeField] GameObject player_prefab;
    [SerializeField] Transform player_initial_position;

    [SerializeField] Transform map_lower_bounds;
    [SerializeField] Transform map_upper_bounds;

    [SerializeField] Text points_text;
    [SerializeField] Text minutes_text;
    [SerializeField] Text seconds_text;

    [SerializeField] Text initial_countdown_text;

    private float current_game_time = 0.0f;
    private float enemy_spawn_timer = Mathf.Infinity;
    private float enemy_spawn_time = 1.0f;
    private float initial_countdown_timer = 0.0f;

    MenuManager menu_manager;
    PlayerController player_controller;
    GameSettings settings;

    // Start is called before the first frame update
    void Start()
    {
        menu_manager = FindObjectOfType<MenuManager>();
        enemy_count = FindObjectsOfType<AIController>().Length;
        foreach (AIController ai in FindObjectsOfType<AIController>())
        {
            ai.DisableController();
        }
        Instantiate(player_prefab, player_initial_position.position, player_prefab.transform.rotation);
        
        player_controller = FindObjectOfType<PlayerController>();
        player_controller.DisableController();
        
        points_text.text = "0";

        settings = FindObjectOfType<GameSettings>();
        spawn_enemy_time_in_seconds = settings.GetEnemySpawnCooldown();
        total_game_duration_in_seconds = settings.GetMatchDuration();
        UpdateTimersText();
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGameOverByTime();
        CheckSpawnEnemy();
        UpdateTimers();
        UpdateTimersText();
    }

    private void CheckGameOverByTime(){
        if(current_game_time > total_game_duration_in_seconds){
            EndGame();
            return;
        }
    }

    private void CheckSpawnEnemy(){
        if(enemy_spawn_timer >= spawn_enemy_time_in_seconds){
            Vector3 enemy_pos = new Vector3(Random.Range(map_lower_bounds.position.x+1, map_upper_bounds.position.x-1), Random.Range(map_lower_bounds.position.y+1, map_upper_bounds.position.y-1), 0);
            
            if(!CheckIfEmptySpace(enemy_pos)) return;
            
            Instantiate(enemy_prefab, enemy_pos, Quaternion.identity);
            enemy_count++;
            enemy_spawn_timer = 0;
        }
    }

    public bool CheckIfEmptySpace(Vector2 position){
        Collider2D hit;
        hit = Physics2D.OverlapCircle(position, 2.0f);
        
        // Debug.DrawRay(position, Vector3.forward * 20, Color.red);
        return (hit == null);
    }

    private void UpdateTimers(){
        initial_countdown_timer += Time.deltaTime;
        if(initial_countdown_timer < 3) return;
        enemy_spawn_timer += Time.deltaTime;
        current_game_time += Time.deltaTime;
    }

    private void UpdateTimersText(){
        
        minutes_text.text = Mathf.Clamp(Mathf.Floor((total_game_duration_in_seconds-current_game_time)/60.0f), 0, 3).ToString();
        
        if(Mathf.Floor((total_game_duration_in_seconds-current_game_time)%60.0f) >= 10)
            seconds_text.text = Mathf.Clamp(Mathf.Floor((total_game_duration_in_seconds-current_game_time)%60.0f), 0, 59).ToString();
        else 
            seconds_text.text = "0" + Mathf.Clamp(Mathf.Floor((total_game_duration_in_seconds-current_game_time)%60.0f), 0, 59).ToString();

        if(Mathf.Floor(4.0f-initial_countdown_timer) <= 0 && initial_countdown_text.enabled){
            initial_countdown_text.enabled = false;
            foreach (AIController ai in FindObjectsOfType<AIController>()){
                ai.EnableController();
            }
            player_controller.EnableController();
        }
        else
            initial_countdown_text.text = Mathf.Clamp(Mathf.Floor(4.0f-initial_countdown_timer), 0, 4).ToString();
    }

    public void AddPoints(int points){
        total_points += points; 
        points_text.text = total_points.ToString();
    }

    public void ResetGame(){
        total_points = 0;
        current_game_time = 0.0f;
    }

    public void EndGame(){
        if(!menu_manager.IsMenuActive()){
            Time.timeScale = 0.0f;
            player_controller.DisableController();
            menu_manager.SetEndingGamePoints(total_points);
            menu_manager.ActivateGameOverMenu();
        }
            
    }
}
