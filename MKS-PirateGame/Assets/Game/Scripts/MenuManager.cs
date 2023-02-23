using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    bool is_menu_active = false;
    [SerializeField] Canvas game_over_canvas;
    [SerializeField] Canvas main_menu_canvas;
    [SerializeField] Canvas settings_canvas;
    [SerializeField] Slider match_duration_slider;
    [SerializeField] Text match_duration_text_value;
    [SerializeField] Slider enemy_spawn_cooldown_slider;
    [SerializeField] Text enemy_spawn_cooldown_text_value;

    [SerializeField] Canvas HUD_canvas;
    [SerializeField] Text points_ending_text;

    [SerializeField] AudioClip[] theme_songs;

    AudioSource audio_source;
    

    // Start is called before the first frame update
    void Start()
    {
        audio_source = GetComponent<AudioSource>();
        if(game_over_canvas != null){
            game_over_canvas.enabled = false;
            audio_source.clip = theme_songs[1];
            audio_source.Play();
        }
            

        if(settings_canvas != null){
            settings_canvas.enabled = false;
            audio_source.clip = theme_songs[0];
            audio_source.Play();
        }
            

        UpdateEnemySpawnValueText();
        UpdateMatchDurationValueText();
    }

    public void ActivateGameOverMenu(){
        is_menu_active = true;
        game_over_canvas.enabled = true;
        HUD_canvas.enabled = false;
        // Time.timeScale = 0.0f;
    }

    public void DeactivateGameOverMenu(){
        is_menu_active = false;
        game_over_canvas.enabled = false;
        HUD_canvas.enabled = true;
        // Time.timeScale = 1.0f;
    }

    public void ActivateSettingsMenu(){
        is_menu_active = true;
        settings_canvas.enabled = true;

        enemy_spawn_cooldown_slider.value = FindObjectOfType<GameSettings>().GetEnemySpawnCooldown();
        match_duration_slider.value = FindObjectOfType<GameSettings>().GetMatchDuration();
    }

    public void DeactivateSettingsMenu(){
        is_menu_active = false;
        settings_canvas.enabled = false;
    }

    public void ActivateMainMenu(){
        is_menu_active = true;
        main_menu_canvas.enabled = true;
    }

    public void DeactivateMainMenu(){
        is_menu_active = false;
        main_menu_canvas.enabled = false;
    }

    public bool IsMenuActive(){
        return is_menu_active;
    }

    public void UpdateEnemySpawnValueText(){
        if(!enemy_spawn_cooldown_text_value) return;
        enemy_spawn_cooldown_text_value.text = enemy_spawn_cooldown_slider.value.ToString();
        FindObjectOfType<GameSettings>()?.SetEnemySpawnCooldown((int)enemy_spawn_cooldown_slider.value);
    }

    public void UpdateMatchDurationValueText(){
        if(!match_duration_text_value) return;
        match_duration_text_value.text = match_duration_slider.value.ToString();
        FindObjectOfType<GameSettings>()?.SetMatchDuration((int)match_duration_slider.value);
    }

    public int GetEnemySpawnCooldown(){
        return (int)enemy_spawn_cooldown_slider.value;
    }

    public int GetMatchDuration(){
        return (int)match_duration_slider.value;
    }

    public void SetEndingGamePoints(int points){
        points_ending_text.text = points.ToString();
    }
}
