using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    int DNALength = 5;
    public DNA dna;
    public GameObject eyes;
    bool seeDownWall = false;
    bool seeUpWall = false;
    bool seeBottom = false;
    bool seeTop = false;

    public LayerMask walls;

    Vector3 startPosition;
    public float timeAlive = 0;
    public float distanceTravelled = 0;
    public int crash = 0;
    bool alive = true;

    Rigidbody2D rb;

    public void Init()
    {
        //init DNA
        //0 forward
        //1 upwall
        //2 downwall
        //3 normall upward

        dna = new DNA(DNALength, 200);
        this.transform.Translate(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
        startPosition = this.transform.position;
        rb = this.GetComponent<Rigidbody2D>();
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Top" ||
            collision.gameObject.tag == "Bottom" ||
            collision.gameObject.tag == "UpWall" ||
            collision.gameObject.tag == "DownWall")
        {
            crash++;
        }
        else if (collision.gameObject.tag == "Dead")
        {
            alive = false;
        }

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!alive) return;

        seeUpWall = false;
        seeDownWall = false;
        seeTop = false;
        seeBottom = false;

        RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.forward, 1.0f, walls);

        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 1.0f, Color.red);
        Debug.DrawRay(eyes.transform.position, eyes.transform.up * 1.0f, Color.red);
        Debug.DrawRay(eyes.transform.position, -eyes.transform.up * 1.0f, Color.red);

        if (hit.collider != null)
        {
           
            if(hit.collider.gameObject.tag == "UpWall")
            {
                seeUpWall = true;
            }

            else if (hit.collider.gameObject.tag == "DownWall")
            {
                seeDownWall = true;
            }
        }

        hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.up, 1.0f);
        if(hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Top")
            {
                seeTop = true;
            }

        }
        hit = Physics2D.Raycast(eyes.transform.position, -eyes.transform.up, 1.0f);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Bottom")
            {
                seeBottom = true;
            }

        }

        timeAlive = PopulationManager.elapsed;
    }

    private void FixedUpdate()
    {
        if (!alive) return;

        float upForce = 0;
        float forwardForce = 1.0f;

        if (seeUpWall)
        {
            upForce = dna.GetGene(0);

        }

        else if (seeDownWall)
        {
            upForce = dna.GetGene(1);
        }
        else if (seeDownWall)
        {
            upForce = dna.GetGene(1);
        }
        else if (seeTop)
        {
            upForce = dna.GetGene(2);
        }
        else if (seeBottom)
        {
            upForce = dna.GetGene(3);
        }
        else
        {
            upForce = dna.GetGene(4);
        }

        rb.AddForce(this.transform.right * forwardForce);
        rb.AddForce(this.transform.up * upForce * 0.1f);
        distanceTravelled = this.transform.position.x - startPosition.x;
    }


}
