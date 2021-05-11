using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;       // Одиночка

    

    [Header("Set in Inspector")]
    // Поля, управляющие движением корабля
    public float speed = 30f;
    public float rollMult = -45f;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;

    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    [Header("Set dinamically")]
    [SerializeField] private float _shieldLevel = 1;

    // Эта переменная хранит ссылку на последний столкнувшийся объект
    private GameObject lastTriggerGo = null;

    private void Awake()
    {
        if (S == null)
        {
            S = this;           // Сохранить ссылку на одиночку
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S");
        }
    }
    private void Update()
    {
        // Извлечь информацию из класс Input
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        // Изменить трансформ.позишн, опираясь на информацию по осям
        Vector3 pos = transform.position; 
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        // Повернуть корабль, чтобы придать ощущение динамизма
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        // Позволить кораблю выстрелить
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TempFire();
        }
    }

    private void TempFire()
    {
        GameObject projGo = Instantiate<GameObject>(projectilePrefab);
        projGo.transform.position = transform.position;
        Rigidbody rigidB = projGo.GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.up * projectileSpeed;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        // print("triggered: " + go.name);

        // Гаранитровать невозможность повторного столкновения с тем же объектом
        if (go == lastTriggerGo)
        {
            return;
        }

        lastTriggerGo = go;

        if (go.tag == "Enemy")      // Если щит столкунлся с врагом
        {
            shieldLevel--;          // уменшить уровень защиты на 1
            Destroy(go);            // уничтожить врага
        }
        else
        {
            print("Triggered: " + go.name);
        }
    }

    public float shieldLevel
    {
        get { return (_shieldLevel); }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);     // Mathf.MIn гарантирует что щит не получит значение выше 4
            // Если уровень упал до нуля или ниже
            if (value < 0)
            {
                Destroy(this.gameObject);
                // Сообщить объекту MainSpawner.S о необходимости перезапустить игру
                MainSpawner.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
