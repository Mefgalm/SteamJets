using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Controller : NetworkBehaviour
{
    private const int CircleDeegres = 360;
    private float _currentAngle;

    private float _nextFire = 0.0F;

    public float fireRate = 0.5F;

    public float angleSpeed;
    public float _shipSpeed;
    public float maxShipSpeed;
    public float shipAcceleration;
    public float backShipAcceleration;

    public float breakPowerCoeff;

    public GameObject bulletManager;
    public Transform bulletSpawn;

    public Camera camera;


	// Use this for initialization
	void Start () {
        _currentAngle = 0;
        _shipSpeed = 0;
    }
	
    private float GetAngle(float currentAngle, float directAngle) 
    {
        float angleDelta = directAngle - currentAngle;

        float angleAbs = Mathf.Abs(angleDelta);

        if(angleAbs < 0.001f)
        {
            return 0;
        }

        int sign = (int)Mathf.Sign(angleDelta);
        
        if(CircleDeegres - angleAbs < angleAbs)
        {
            sign *= -1;
        }

        return sign * Mathf.Min(angleSpeed, angleAbs);
    }

    private void Rotate(bool canRotate)
    {
        if (!canRotate) return;

        var mouseVectro = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 worldMousePostion = camera.ScreenToWorldPoint(mouseVectro);

        float angle = Mathf.Atan2((worldMousePostion.y - transform.position.y), (worldMousePostion.x - transform.position.x)) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += CircleDeegres;
        }

        transform.Rotate(0, 0, GetAngle(_currentAngle, angle));

        _currentAngle = transform.rotation.eulerAngles.z;
    }

    private void DisableRigbodyVelocity()
    {
        Rigidbody2D rigbody2D = GetComponent<Rigidbody2D>();
        
        rigbody2D.velocity = Vector3.zero;
    }

    private void Move(bool canMove)
    {
        if (!canMove) return;
        bool isAnyControlKeyPressed = false;

        if (Input.GetKey(KeyCode.W))
        {
            _shipSpeed += shipAcceleration;
            if (_shipSpeed > maxShipSpeed)
            {
                _shipSpeed = maxShipSpeed;
            }
            isAnyControlKeyPressed = true;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            _shipSpeed -= shipAcceleration * backShipAcceleration;
            if (Mathf.Abs(_shipSpeed) > maxShipSpeed)
            {
                _shipSpeed = -maxShipSpeed;
            }
            isAnyControlKeyPressed = true;
        }

        if (!isAnyControlKeyPressed)
        {
            if (_shipSpeed != 0)
            {
                if (_shipSpeed > 0)
                {
                    _shipSpeed -= shipAcceleration * backShipAcceleration * breakPowerCoeff;
                    if (_shipSpeed < 0)
                    {
                        _shipSpeed = 0;
                    }
                }
                else if (_shipSpeed < 0)
                {
                    _shipSpeed += shipAcceleration * breakPowerCoeff;
                    if (_shipSpeed > 0)
                    {
                        _shipSpeed = 0;
                    }
                }
            }
        }

        Vector3 position = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z) * (Vector3.right * _shipSpeed);

        transform.position += position;
    }

    public override void OnStartClient()
    {
        camera.enabled = false;
    }

    public override void OnStartLocalPlayer()
    {
        camera.enabled = true;
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    [Command]
    private void CmdFire(bool canFire)
    {
        if (!canFire) return;
        
        var bullet = Instantiate(
            bulletManager,
            bulletSpawn.position,
            bulletSpawn.rotation);

        bullet.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z) * Vector3.right * Constans.BulletSpeed;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0f);
    }

    // Update is called once per frame
    void Update () {

        if (!isLocalPlayer)
        {
            return;
        }

        if(Input.GetKey(KeyCode.Space) && Time.time > _nextFire)
        {
            _nextFire = Time.time + fireRate;
            CmdFire(true);
        }

        Rotate(true);
        Move(true);
    }
}
