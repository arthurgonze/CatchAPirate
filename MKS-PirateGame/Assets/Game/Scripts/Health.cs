using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float max_health = 100;
    [SerializeField] float current_health;

    [SerializeField] bool is_dead = false;

    [SerializeField] Sprite[] damage_level_sprites;
    [SerializeField] Image healthbar;

    [SerializeField] GameObject death_vfx;

    SpriteRenderer spriteRenderer;

    float health_percentage;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        current_health = max_health;
        is_dead = false;
    }

    public void TakeDamage(float damage){
        current_health = Mathf.Clamp(current_health-damage, 0.0f, max_health);
        
        UpdateHealthUI();
        CheckDeath();
    }

    private void CheckDeath(){
        if(current_health <= 0 && !is_dead){
            is_dead = true;

            if(this.gameObject.GetComponent<IController>() != null)
                this.gameObject.GetComponent<IController>()?.DisableController();
            
            spriteRenderer.sprite = damage_level_sprites[2];

            GameObject death_vfx_object = Instantiate(death_vfx, this.gameObject.transform.position, this.gameObject.transform.rotation);
            Destroy(death_vfx_object, 0.75f);

            if(gameObject.tag == "Player")
                FindObjectOfType<GameMode>().EndGame();
            else{
                FindObjectOfType<GameMode>().AddPoints(GetComponent<PointGiver>().GetPointsToGive());
                Destroy(this.gameObject, 1.0f);
            }  
        }    
    }

    private void UpdateHealthUI(){
        health_percentage = current_health/max_health;
        healthbar.fillAmount = health_percentage;

        if(health_percentage >= 0.75){
            healthbar.color = Color.green;
        }
        if((health_percentage < 0.75) && (health_percentage > 0.25)){
            healthbar.color = Color.yellow;
            spriteRenderer.sprite = damage_level_sprites[0];
        }
        if(health_percentage <= 0.25){
            healthbar.color = Color.red;
            spriteRenderer.sprite = damage_level_sprites[1];
        }
    }

    public void SetBoatSprites(Sprite[] new_sprites){
        damage_level_sprites = new_sprites;
        spriteRenderer.sprite = new_sprites[new_sprites.Length-1];
    }


    public bool IsDead(){
        return is_dead;
    }

    private void ComputePercentage(){
        health_percentage = current_health/max_health;
    }

    public void Explode(){
        TakeDamage(max_health);
    }
}
