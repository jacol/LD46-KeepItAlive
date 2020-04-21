using UnityEngine;

public class MonsterPlay : MonoBehaviour
{
    public float Speed;
    public int Steps;
    public GameObject Food;
    
    private float _moveVelocity;
    private int _stepCount;
    private bool _moveRight;
    
    // Start is called before the first frame update
    void Start()
    {
        _stepCount = 0;
        _moveRight = true;
        
        InvokeRepeating("DeployFood", 1.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        _moveVelocity = 0;

        if (_stepCount < Steps)
        {
            _stepCount++;
            
            if (_moveRight)
            {
                _moveVelocity = Speed; //move right
            }
            else
            {
                _moveVelocity = -Speed;//move left
            }
        }
        else
        {
            _stepCount = 0;
            _moveRight = !_moveRight;
        }
        

        GetComponent<Rigidbody2D> ().velocity = new Vector2 (_moveVelocity, GetComponent<Rigidbody2D> ().velocity.y);
    }

    void DeployFood()
    {
        var newFood = Instantiate(Food);
        var position = this.gameObject.transform.position;
        newFood.transform.position = new Vector3(position.x, position.y + 2, position.z);
        var rigid2d = newFood.GetComponent<Rigidbody2D>();
        
        rigid2d.AddForce(new Vector2(5 * Random.Range(-1.0f, 1.0f),  Random.Range(5.0f, 10.0f)), ForceMode2D.Impulse);
        
    }
}

