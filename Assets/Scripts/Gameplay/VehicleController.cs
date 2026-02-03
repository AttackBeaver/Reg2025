using UnityEngine;
using System.Collections.Generic;
using System;
public class VehicleController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private float maxSpeed = 15f;

    [Header("Attachment Settings")]
    [SerializeField] private List<GameObject> attachments = new List<GameObject>();

    private Rigidbody rb;
    private float currentSpeed;
    private bool isGrounded = true;

    // Прикрепленные модули
    private bool hasPropeller = false;
    private bool hasRocket = false;
    private bool hasSpikedWheels = false;
    private bool hasWings = false;

    // Состояния
    private bool propellerActive = false;
    private bool rocketReady = true;
    private bool spikedWheelsActive = false;
    private float wingsCooldown = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = baseSpeed;

        // Анализируем прикрепленные модули
        AnalyzeAttachments();
    }

    public void Initialize(GameObject[] initialAttachments)
    {
        attachments.AddRange(initialAttachments);
    }

    private void AnalyzeAttachments()
    {
        foreach (GameObject attachment in attachments)
        {
            if (attachment.name.Contains("Propeller"))
            {
                hasPropeller = true;
            }
            else if (attachment.name.Contains("Rocket"))
            {
                hasRocket = true;
            }
            else if (attachment.name.Contains("Spiked"))
            {
                hasSpikedWheels = true;
            }
            else if (attachment.name.Contains("Wings"))
            {
                hasWings = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance == null || GameManager.Instance.currentState != GameManager.GameState.Playing)
            return;

        MoveForward();
        CheckGround();
        UpdateCooldowns();
    }

    private void MoveForward()
    {
        // Базовая скорость вперед
        Vector3 forwardForce = transform.forward * currentSpeed;
        rb.AddForce(forwardForce, ForceMode.Acceleration);

        // Ограничиваем максимальную скорость
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // Дополнительная скорость от пропеллера
        if (hasPropeller && propellerActive)
        {
            Vector3 boostForce = transform.forward * (currentSpeed * 0.5f);
            rb.AddForce(boostForce, ForceMode.Acceleration);
        }
    }

    private void CheckGround()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1f);

        if (!isGrounded)
        {
            // Применяем гравитацию
            rb.AddForce(Vector3.down * 9.81f, ForceMode.Acceleration);
        }

        // Проверяем, не упал ли транспорт
        if (transform.position.y < -5f)
        {
            GameManager.Instance.OnVehicleFailed();
        }
    }

    private void UpdateCooldowns()
    {
        if (wingsCooldown > 0)
        {
            wingsCooldown -= Time.fixedDeltaTime;
        }

        // Можно добавить другие кулдауны
    }

    // Методы управления (будут вызываться из UI)
    public void ActivatePropeller(bool activate)
    {
        if (hasPropeller)
        {
            propellerActive = activate;

            // Визуальный эффект пропеллера
            foreach (GameObject attachment in attachments)
            {
                if (attachment.name.Contains("Propeller"))
                {
                    ParticleSystem particles = attachment.GetComponentInChildren<ParticleSystem>();
                    if (particles != null)
                    {
                        if (activate && !particles.isPlaying)
                            particles.Play();
                        else if (!activate && particles.isPlaying)
                            particles.Stop();
                    }
                }
            }
        }
    }

    public void FireRocket()
    {
        if (hasRocket && rocketReady)
        {
            // Поиск цели (препятствие)
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    // Уничтожаем препятствие
                    Destroy(hit.collider.gameObject);
                }
            }

            // Визуальный эффект ракеты
            foreach (GameObject attachment in attachments)
            {
                if (attachment.name.Contains("Rocket"))
                {
                    ParticleSystem particles = attachment.GetComponentInChildren<ParticleSystem>();
                    if (particles != null)
                    {
                        particles.Play();
                    }
                }
            }

            rocketReady = false;
            Invoke(nameof(ResetRocket), 5f); // Перезарядка 5 секунд
        }
    }

    private void ResetRocket()
    {
        rocketReady = true;
    }

    public void ToggleSpikedWheels()
    {
        if (hasSpikedWheels)
        {
            spikedWheelsActive = !spikedWheelsActive;

            // Изменяем физические свойства при езде вверх ногами
            if (spikedWheelsActive)
            {
                // Можно вращаться на 360 градусов
                rb.constraints = RigidbodyConstraints.None;
            }
            else
            {
                // Ограничиваем вращение
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }

    public void UseWings()
    {
        if (hasWings && wingsCooldown <= 0)
        {
            // Прыжок/подлет
            rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            wingsCooldown = 1f; // Кулдаун 1 секунда

            // Визуальный эффект
            foreach (GameObject attachment in attachments)
            {
                if (attachment.name.Contains("Wings"))
                {
                    Animator animator = attachment.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.SetTrigger("Flap");
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Проверка столкновения с препятствием
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (!hasRocket || !rocketReady)
            {
                // Если нет ракеты или она не готова - проигрыш
                GameManager.Instance.OnVehicleFailed();
            }
        }

        // Проверка финиша
        if (collision.gameObject.CompareTag("Finish"))
        {
            GameManager.Instance.OnVehicleReachedFinish();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Сбор монет
        if (other.CompareTag("Coin"))
        {
            CurrencyManager.Instance.AddCoins(10);
            Destroy(other.gameObject);

            // Звук сбора монеты
            AudioManager.Instance.PlayCoinPickup();
        }
    }
}
