using UnityEngine;
using System.Collections;

public class PlayerTankController : MonoBehaviour
{
    public GameObject Bullet;
    public float stoppingDistance = 30f;

    private Transform Turret;
    private Transform bulletSpawnPoint;
    private float curSpeed, targetSpeed, rotSpeed;
    private float turretRotSpeed = 10.0f;
    private float maxForwardSpeed = 300.0f;
    private float maxBackwardSpeed = -300.0f;
    private int playerHealth = 100;


    protected float shootRate;
    protected float elapsedTime;

    private Vector3 targetPoint;

    void Start()
    {
        rotSpeed = 150.0f;
        Turret = gameObject.transform.GetChild(0).transform;
        bulletSpawnPoint = Turret.GetChild(0).transform;
    }

    void OnEndGame()
    {
        this.enabled = false;
    }

    void Update()
    {
        UpdateControl();
        UpdateWeapon();
    }

    void UpdateControl()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position + new Vector3(0, 0, 0));

        Ray RayCast = Camera.main.ScreenPointToRay(Input.mousePosition);

        float HitDist = 0;

        if (playerPlane.Raycast(RayCast, out HitDist))
        {
            Vector3 RayHitPoint = RayCast.GetPoint(HitDist);

            Quaternion targetRotation = Quaternion.LookRotation(RayHitPoint - transform.position);
            Turret.transform.rotation = Quaternion.Slerp(Turret.transform.rotation, targetRotation, Time.deltaTime * turretRotSpeed);
        }

        if (Input.GetKey(KeyCode.W))
        {
            targetSpeed = maxForwardSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetSpeed = maxBackwardSpeed;
        }
        else
        {
            targetSpeed = 0;
        }


        //Vehicle move by mouse click
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(1) && Physics.Raycast(ray, out hit, 10000.0f))
        {
            targetPoint = hit.point;
        }

        //Directional vector to the target position
        Vector3 dir = (targetPoint - transform.position);
        dir.Normalize();
        targetPoint.y = 5;

        //Don't move the vehicle when the target point is reached
        if (Vector3.Distance(targetPoint, transform.position) < stoppingDistance)
            return;

        //Assign the speed with delta time
        curSpeed = 240.0f * Time.deltaTime;

        //Rotate the vehicle to its target directional vector
        var rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5.0f * Time.deltaTime);

        //Move the vehicle towards
        transform.position += transform.forward * curSpeed;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -rotSpeed * Time.deltaTime, 0.0f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, rotSpeed * Time.deltaTime, 0.0f);
        }

        curSpeed = Mathf.Lerp(curSpeed, targetSpeed, 7.0f * Time.deltaTime);
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }


    protected void Explode()
    {

        Destroy(gameObject, 1.5f);
    }

    void UpdateWeapon()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (elapsedTime >= shootRate)
            {
                elapsedTime = 0.0f;

                Instantiate(Bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            }
        }
    }
}