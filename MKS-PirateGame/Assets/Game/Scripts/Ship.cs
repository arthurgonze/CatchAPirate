using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(BoxCollider2D))]
public class Ship : MonoBehaviour
{
    [SerializeField] int collision_damage = 5;
    [SerializeField] float shoot_cooldown = 1.0f;
    [SerializeField] Transform[] left_lateral_shot_spawnpoint = new Transform[3];
    [SerializeField] Transform[] right_lateral_shot_spawnpoint = new Transform[3];
    [SerializeField] Transform front_shot_spawnpoint;
    [SerializeField] Canonball canonball_prefab;

    [SerializeField] Transform left_shot_vfx_spawnpoint;
    [SerializeField] Transform right_shot_vfx_spawnpoint;
    [SerializeField] GameObject shot_vfx;

    [SerializeField] AudioClip[] cannon_shot_sfx;

    AudioSource audio_source;

    Health health;
    // RectTransform canvas_rect;
    float cooldown_timer = Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        audio_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown_timer += Time.deltaTime;
    }

    public bool FrontShot(){
        if(cooldown_timer < shoot_cooldown) return false;
        Canonball bullet = Instantiate(canonball_prefab, front_shot_spawnpoint.position, this.transform.rotation);
        
        if(shot_vfx != null && left_shot_vfx_spawnpoint != null && right_shot_vfx_spawnpoint != null){
            GameObject shot_vfx_object;
            int random_side = Random.Range(1, 100);
            if(random_side%2==0)
                shot_vfx_object = Instantiate(shot_vfx, left_shot_vfx_spawnpoint);
            else
                shot_vfx_object = Instantiate(shot_vfx, right_shot_vfx_spawnpoint);
            Destroy(shot_vfx_object, 0.75f);
        }

        audio_source.PlayOneShot(cannon_shot_sfx[Random.Range(0,cannon_shot_sfx.Length-1)]);

        bullet.SetDir(-bullet.transform.up);
        bullet.SetIgnoredActorID(this.gameObject.GetInstanceID());
        cooldown_timer = 0.0f;
        return true;
    }

    public bool LateralShot(float boat_side){
        if(cooldown_timer < shoot_cooldown) return false;

        Canonball bullet_left, bullet_mid, bullet_right;
        GameObject shot_vfx_object = new GameObject();
        if(boat_side < 0){
            bullet_left = Instantiate(canonball_prefab, right_lateral_shot_spawnpoint[0].position, this.transform.rotation);
            bullet_mid = Instantiate(canonball_prefab, right_lateral_shot_spawnpoint[1].position, this.transform.rotation);
            bullet_right = Instantiate(canonball_prefab, right_lateral_shot_spawnpoint[2].position, this.transform.rotation);
            
            if(shot_vfx != null && left_shot_vfx_spawnpoint != null)
                shot_vfx_object = Instantiate(shot_vfx, left_shot_vfx_spawnpoint);
        }else{
            bullet_left = Instantiate(canonball_prefab, left_lateral_shot_spawnpoint[0].position, this.transform.rotation);
            bullet_mid = Instantiate(canonball_prefab, left_lateral_shot_spawnpoint[1].position, this.transform.rotation);
            bullet_right = Instantiate(canonball_prefab, left_lateral_shot_spawnpoint[2].position, this.transform.rotation);
            
            if(shot_vfx != null && right_shot_vfx_spawnpoint != null)
                shot_vfx_object = Instantiate(shot_vfx, right_shot_vfx_spawnpoint);
        }
        Destroy(shot_vfx_object, 0.75f);

        bullet_left.SetDir(-boat_side*bullet_left.transform.right);
        bullet_mid.SetDir(-boat_side*bullet_mid.transform.right);
        bullet_right.SetDir(-boat_side*bullet_right.transform.right);

        audio_source.PlayOneShot(cannon_shot_sfx[Random.Range(0,cannon_shot_sfx.Length-1)]);

        bullet_left.SetIgnoredActorID(this.gameObject.GetInstanceID());
        bullet_mid.SetIgnoredActorID(this.gameObject.GetInstanceID());
        bullet_right.SetIgnoredActorID(this.gameObject.GetInstanceID());

        cooldown_timer = 0.0f;
        return true;
    }

    public int GetDamage(){
        return collision_damage;
    }

    public void Explode(){
         health.Explode();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Canonball canonball = other.gameObject.GetComponent<Canonball>();
        if(canonball){
            health.TakeDamage(canonball.GetDamage());
        }

        Ship kamikaze = other.gameObject.GetComponent<Ship>();
        if(kamikaze != null && kamikaze.tag != "Player" && !kamikaze.GetComponent<Health>().IsDead()){
            health.TakeDamage(kamikaze.GetDamage());
            if(other.gameObject.GetComponent<AIController>())
                kamikaze.Explode();  
        }
    }
}
