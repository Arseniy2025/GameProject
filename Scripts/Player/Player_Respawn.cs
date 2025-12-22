using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Respawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;
    private PlayerMove playerMove;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        playerMove = GetComponent<PlayerMove>(); // Получаем ссылку на PlayerMove
        uiManager = FindObjectOfType<UIManager>();
    }

    public void RespawnCheck()
    {
        if (currentCheckpoint == null)
        {
            if (uiManager != null)
            {
                uiManager.GameOver();
            }
            return;
        }

        // Респавним здоровье
        playerHealth.Respawn();

        // Перемещаем персонажа
        transform.position = currentCheckpoint.position;

        // Сбрасываем скорость
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        // Перемещаем камеру (если есть)
        CameraController cameraController = Camera.main?.GetComponent<CameraController>();
        if (cameraController != null)
        {
            cameraController.MoveToNewRoom(currentCheckpoint.parent);
        }

        Debug.Log("Персонаж респавнен на чекпоинте");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;

            // Воспроизводим анимацию активации
            Animator checkpointAnim = collision.GetComponent<Animator>();
            if (checkpointAnim != null)
            {
                checkpointAnim.SetTrigger("activate");
            }

            // Воспроизводим звук
            if (checkpointSound != null)
            {
                AudioSource.PlayClipAtPoint(checkpointSound, transform.position);
            }

            Debug.Log("Чекпоинт активирован!");
        }
    }
}