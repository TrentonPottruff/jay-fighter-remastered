using UnityEngine;
using System.Collections;

/// <summary>
/// AUTHOR: Taliyah Pottruff
/// CONTRIBUTOR: Garrett Nicholas
/// </summary>
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerCombat : MonoBehaviour {
    public bool firing = false;

    public AudioClip gunSound;
    public AudioClip coinpickup;

    public GameObject topDown;
    public GameObject topLeft;
    public GameObject topRight;
    public GameObject topUp;

    private Vector2 fireVector = Vector2.zero;
    private Rigidbody2D rb;
    private AudioSource aSource;

    private GameObject bulletPrefab;
    private GameObject muzzleflashPrefab;
    private Vector2 playerPositon = Vector2.zero;

    public bool _topDown = true;
    public bool _topLeft = false;
    public bool _topRight = false;
    public bool _topUp = false;
    private Transform shooter0;
    private Transform shooter1;
    private bool shooter = false;

    private Player player;

    private void Start() {
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        muzzleflashPrefab = Resources.Load<GameObject>("Prefabs/Muzzleflash");

        rb = GetComponent<Rigidbody2D>();
        aSource = GetComponent<AudioSource>();
        player = GetComponent<Player>();

        StartCoroutine(FireBullet());
    }

    private void Update() {
        if (!Game.PAUSED)
            DoUpdate();
    }

    public void DoUpdate() {
        //Set sfx audio volume
        aSource.volume = Game.SFX_VOLUME;

        playerPositon = (Vector2)this.transform.position;

        if (player.currentScheme == ControlScheme.Keyboard)
            fireVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        else
            fireVector = new Vector2(Input.GetAxis("FireHorizontal"), Input.GetAxis("FireVertical"));

        if (Input.GetMouseButton(0) || Input.GetButton("Fire1"))
            firing = true;
        else
            firing = false;

        //If the player is firing slow them down
        if (firing)
            player.movemement.SetSpeed(0.5f);
        else
            player.movemement.SetSpeed(1f);

        float horizontal = Mathf.Abs(fireVector.x);
        float vertical = Mathf.Abs(fireVector.y);

        //Set directional data
        if (player.isLocalPlayer) {
            if (horizontal != 0 || vertical != 0) {
                if (horizontal > vertical) {
                    if (fireVector.x > 0) {
                        CmdSetTop(1);
                        shooter0 = topRight.transform.Find("Right Shooter");
                        shooter1 = topRight.transform.Find("Right Shooter");
                    }
                    else {
                        CmdSetTop(2);
                        shooter0 = topLeft.transform.Find("Left Shooter");
                        shooter1 = topLeft.transform.Find("Left Shooter");
                    }
                }
                else {
                    if (fireVector.y > 0) {
                        CmdSetTop(3);
                        shooter0 = topUp.transform.Find("Left Shooter");
                        shooter1 = topUp.transform.Find("Right Shooter");
                    }
                    else {
                        CmdSetTop(4);
                        shooter0 = topDown.transform.Find("Left Shooter");
                        shooter1 = topDown.transform.Find("Right Shooter");
                    }
                }
            }
        }

        topDown.SetActive(_topDown);
        topLeft.SetActive(_topLeft);
        topRight.SetActive(_topRight);
        topUp.SetActive(_topUp);
    }

    public void CmdSetTop(int _case) {
        switch (_case) {
            case 1:
                //Right
                _topDown = false;
                _topLeft = false;
                _topRight = true;
                _topUp = false;
                
                break;
            case 2:
                //Left
                _topDown = false;
                _topLeft = true;
                _topRight = false;
                _topUp = false;
                
                break;
            case 3:
                //Up
                _topDown = false;
                _topLeft = false;
                _topRight = false;
                _topUp = true;
                
                break;
            case 4:
                //Down
                _topDown = true;
                _topLeft = false;
                _topRight = false;
                _topUp = false;
                
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Health health = other.GetComponent<Health>();
        CoinScript Coin = other.GetComponent<CoinScript>();
        CPU cpu = other.GetComponent<CPU>();
        if(Coin != null) {
            aSource.PlayOneShot(coinpickup);
        }
    }
    public virtual IEnumerator FireBullet() {
        do {
            if (firing && player.isLocalPlayer && !Game.PAUSED) {
                Vector2 direction = fireVector.normalized;

                Vector3 pos = Vector3.zero;

                if (shooter) {
                    pos = shooter0.position;
                    shooter = false;
                }
                else {
                    pos = shooter1.position;
                    shooter = true;
                }
                
                FireBullet(pos, direction, player.id);
                aSource.PlayOneShot(gunSound);
            }
            yield return new WaitForSeconds(0.1f);
        } while (true);
    }

    private void FireBullet(Vector2 position, Vector2 direction, uint _owner) {
        GameObject bulletObj = Instantiate(bulletPrefab, position, Quaternion.FromToRotation(Vector2.up, direction)) as GameObject;
        GameObject flashObj = Instantiate(muzzleflashPrefab, position, Quaternion.FromToRotation(Vector2.up, direction)) as GameObject;
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.playerBullet = true;
        bullet.owner = _owner;
        bullet.SetVelocity(direction * 10);
        //NetworkServer.Spawn(bulletObj);
    }
}
