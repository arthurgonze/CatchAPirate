using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Canonball : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private Vector3 position_update = Vector3.zero;
    

    private float lifetime_timer;
    [SerializeField] private Vector3 dir = Vector3.zero;
    private int ignored_actor_id;
    // Start is called before the first frame update
    void Start()
    {
        lifetime_timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        position_update = dir * speed * Time.deltaTime;
        this.transform.position += position_update;

        if(lifetime_timer > lifetime)
            Destroy(this.gameObject);
        
        lifetime_timer += Time.deltaTime;
    }

    public float GetDamage(){
        return damage;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.GetComponent<Ship>() && ignored_actor_id != other.gameObject.GetInstanceID())
            Destroy(this.gameObject);
    }

    public void SetDir(Vector3 new_dir){
        dir = new_dir;
    }
    public void SetIgnoredActorID(int id){
        ignored_actor_id = id;
    }
}
