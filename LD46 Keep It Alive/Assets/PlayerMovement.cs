using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    public float Speed;
    public float Jump;
    public GameObject TerrainTilemap;
    public AudioClip DeadSound;
    public AudioClip FoodSound;
    public AudioClip WinSound;

    private Tilemap _tilemap;
    private float _moveVelocity;
    private bool _isGrounded = true;
    
    private GUIStyle currentStyle = null;
    private float _health = 0;
    private Vector2 pos = new Vector2(200,40);
    private Vector2 size = new Vector2(300,40);
    private Texture2D progressBarEmpty;
    private Texture2D progressBarFull;

    private bool _gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        pos = new Vector2((Screen.width / 2) - 150, 40);
        
        _health = 1f;
        InvokeRepeating("DecreaseHealth", 1.0f, 1.0f);

        if (TerrainTilemap != null)
        {
            _tilemap = TerrainTilemap.GetComponent<Tilemap>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameOver) return;
        
        if(_health <= 0.0f) OnPlayerDead();
        
        //Jumping
        if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.W)) 
        {
            if(_isGrounded)
            {
                GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, Jump);
                _isGrounded = false;
            }
        }

        _moveVelocity = 0;

        //Left Right Movement
        if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) 
        {
            _moveVelocity = -Speed;
        }
        if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) 
        {
            _moveVelocity = Speed;
        }

        GetComponent<Rigidbody2D> ().velocity = new Vector2 (_moveVelocity, GetComponent<Rigidbody2D> ().velocity.y);

    }

    private void OnGUI()
    {
        if (_gameOver)
        {
            GUI.color = Color.Lerp(Color.black, Color.white, Time.time);
            GUI.Button(new Rect(0, 0, Screen.width, Screen.height),
                "## Congratulations! You WON!  ##" + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                "(game made during LUDUM DARE #46 Keep It Alive)");

            GUI.color = Color.white;
        }
        else
        {
            InitStyles();

            // draw the background:
            GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
            GUI.Box(new Rect(0, 0, size.x, size.y), progressBarEmpty);


            // draw the filled-in part:
            GUI.BeginGroup(new Rect(0, 0, size.x * _health, size.y));
            GUI.Box(new Rect(0, 0, size.x, size.y), " F O O D", currentStyle);

            GUI.EndGroup();

            GUI.EndGroup();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _isGrounded = true;
        
        //check if woden thorns below
        Vector3Int cellPosition = _tilemap.layoutGrid.WorldToCell(transform.position);
        var tile = _tilemap.GetTile(new Vector3Int(cellPosition.x, cellPosition.y - 1, 0));
        
        if (tile != null && tile.name.Contains("woodenThorn"))
        {
            OnPlayerDead();
        }
        
        //check if touched monster
        if (other.gameObject.tag.Contains("Monster"))
        {
            OnPlayerDead();
        }
        
        //check if food eaten
        if (other.gameObject.tag.Contains("Food"))
        {
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);

            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(FoodSound);
            
            _health += 0.1f;
            if (_health > 1.0f)
                _health = 1.0f;
        }
        
        //check if win
        if (other.gameObject.tag.Contains("Unicorn"))
        {
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(WinSound);

            _gameOver = true;
        }
    }

    private void OnPlayerDead()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(DeadSound);
        
        this.transform.Rotate(Vector3.back, 90);
        Destroy(this);
    }
    
    private void InitStyles()
    {
        if( currentStyle == null )
        {
            currentStyle = new GUIStyle( GUI.skin.box );
            currentStyle.normal.background = MakeTex( 20, 20, new Color( 1f, 0f, 0f, 0.5f ) );
        }
    }
 
    private Texture2D MakeTex( int width, int height, Color col )
    {
        Color[] pix = new Color[width * height];
        for( int i = 0; i < pix.Length; ++i )
        {
            pix[ i ] = col;
        }
        Texture2D result = new Texture2D( width, height );
        result.SetPixels( pix );
        result.Apply();
        return result;
    }

    void DecreaseHealth()
    {
        _health -= 0.03f;
    }

}
