using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float maxForce = 20f;
    public float requiredSpeedToScore = 1f;
    public Slider powerSlider;
    public Transform arrow;
    public TextMeshProUGUI contadorText;

    private Rigidbody2D rb;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private bool isDragging;
    private bool isMoving;
    private int intentos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        powerSlider.gameObject.SetActive(false);
        arrow.gameObject.SetActive(false);
        powerSlider.minValue = 0;
        powerSlider.maxValue = maxForce;
        intentos = 0;
        UpdateIntentosText();

        // Ajustar la posición de la meta al comenzar el nivel
        MetaController.Instance.AjustarPosicionMeta();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
            powerSlider.gameObject.SetActive(true);
            arrow.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float launchForce = Mathf.Clamp(Vector2.Distance(startPoint, endPoint), 0, maxForce);
            powerSlider.value = launchForce;

            Vector2 direction = (startPoint - endPoint).normalized;
            arrow.position = (Vector2)transform.position + direction * GetComponent<CircleCollider2D>().radius;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Vector2 launchDirection = (startPoint - endPoint).normalized;
            float launchForce = Mathf.Clamp(Vector2.Distance(startPoint, endPoint), 0, maxForce);

            rb.velocity = Vector2.zero;
            rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);

            isDragging = false;
            isMoving = true;
            powerSlider.gameObject.SetActive(false);
            arrow.gameObject.SetActive(false);

            intentos++;
            GameManager.Instance.AgregarIntentos(intentos);  // Llamamos al GameManager para aumentar el total de intentos
            UpdateIntentosText();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 reflectDirection = Vector2.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
            rb.velocity = reflectDirection * rb.velocity.magnitude;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Meta"))
        {
            MetaController.Instance.CambiarDeNivel(); // Cambia de nivel
        }
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude < 0.1f)
        {
            rb.velocity = Vector2.zero;
            isMoving = false;
        }
    }

    private void UpdateIntentosText()
    {
        contadorText.text = "Intents: " + intentos;
    }
}
