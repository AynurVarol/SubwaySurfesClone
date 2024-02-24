using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
 
    public float forwardSpeed = 5f;
    public float laneDistance = 2f; //�eritler aras�
    public float jumpForce = 15f;
    private bool isGrounded ;
    private int currentLane = 1;  //oyuncunun bulundu�u �erit
    public Rigidbody rb;
    private bool isCentering = false;

    public float initialForwardSpeed = 1f; //�lerleme h�z�
    public float maxForwardSpeed = 25f; // maksimum h�z
    public float accelerationRate = 0.001f; // h�z art�� oran�
    public float slideSpeed = 10f;
    public float raycastDistance = 1.1f; // Raycast mesafesi

    // oyuncunun rotasyonu
    private bool isRotating = false;





    private bool hasJumped = false;
    private bool isSliding = false;

    public int maxHealth = 30; // maksimum can
    private int currentHealth; // mevcut can
    //public Slider healthBar;
    public Image[] healthImages;


    private PlayerScore playerScore;

    private bool isGameOver = false;

    private bool playerCanTakeDamage = true;
    private Color originalColor;



    private void Start()
    {
        //oyun ba�lad���nda mevcut can say�s� maksimum cana e�it 
        currentHealth = maxHealth;
        originalColor = GetComponent<Renderer>().material.color; // oyuncunun orijinal rengi
        playerScore = GetComponent<PlayerScore>();
        //can de�erini en ba�ta u�da g�ster
        UpdateHealthBar();


        rb = GetComponent<Rigidbody>();

        SetInitialPositionOnGround();


    }
    private void SetInitialPositionOnGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            // Zeminin tam ortas�na g�re ba�lang�� pozisyonunu ayarla
            transform.position = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
        }
    }


    public void SetRunning(bool running)
    {
        if (playerScore != null)
        {
            playerScore.SetRunningState(running);
        }

    }
    private void FixedUpdate()
    {
        MoveForward(); //ileri hareket
        HandleMovementInput(); //yatay hareket
        if (isCentering)
        {
            CenterPlayer();
        }
        //Z�plama kontrol�
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            Jump();
            // Engelin �zerinden z�plad�ktan sonras� i�in  kontrol
            CheckIfGroundedAfterJump();

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            RotatePlayerDirectly();
        }






        //h�z� artt�r
        IncreaseSpeed();

        // Engelin �zerinden z�plad�ktan sonras� i�in  kontrol
        //CheckIfGroundedAfterJump();
       

    }
    

    



    void HandleMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

       

        // Sa�a veya sola kayd�rma i�lemi
        if (Input.GetKeyDown(KeyCode.D) || horizontalInput > 0)  
        {
            MoveLane(1); // Sa�a kayd�rma

            Slide();
        }
        else if (Input.GetKeyDown(KeyCode.A) || horizontalInput < 0) 
        {
            MoveLane(-1); // Sola kayd�rma

            Slide();
        }
        /*Z�plama kontrol�
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            // Engelin �zerinden z�plad�ktan sonras� i�in  kontrol
            CheckIfGroundedAfterJump();
        }*/

        // C ye bas�nca oyuncuyu merkeze getir
        if (Input.GetKeyDown(KeyCode.C)&& !isCentering)
        {
            isCentering = true;
          
        }






    }
    void MoveLane(int direction)
    {
        int targetLane = currentLane + direction;

        // Ge�erli �erit aral���n� kontrol etme
        if (targetLane < -4 || targetLane > 4)
        {
            return;
        }

        currentLane = targetLane;
        Vector3 newPosition = transform.position;
        newPosition.x = currentLane * laneDistance;
        rb.MovePosition(newPosition);
    }







    /// <summary>
    /// nesnenin ve player temas kontolleri
    /// </summary>
    /// <param name="collision"></param>

    void OnCollisionEnter(Collision collision)
    {
        if (playerCanTakeDamage)
        {
            if (collision.gameObject.CompareTag("Barrier"))
            {
                // Bir engel ile �arp��t���nda can�n� d���r
                TakeDamage();
                //Oyuncunun rengini de�i�tir ve hasar almas�n� engellemek i�in;
                StartCoroutine(PlayerChangeColorAndDisableDamage());
            }

            else if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;

                // Yerden z�plam�� m� kontrol et
                if (hasJumped)
                {
                    hasJumped = false;
                    Debug.Log("Zemine indi!");
                }
            }
        }
    }

        /// <summary>
        /// Z�plama 
        /// </summary>

   private void Jump()
   {
        Debug.Log("z�plama");

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // �nce di�er y�kseklik bile�enlerini s�f�rla
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Z�plama kuvvetini ekle
        isGrounded = false;
        //rb.AddForce(Vector3.up * jumpForce);
        //isGrounded = false;
        hasJumped = true;


   }

    private void Slide()
    {
         // Kayma animasyonunu ba�lat
        isSliding = true;
        // Sa�a veya sola kayma h�z�
        float slideMovement = slideSpeed * Time.deltaTime;

        // Hedef pozisyonu belirle
        Vector3 targetPosition = new Vector3(currentLane * laneDistance, transform.position.y, transform.position.z);

        // Yeni pozisyonu kayma h�z�yla belirtilen y�ne do�ru hareket ettir
        transform.position = Vector3.Lerp(transform.position, targetPosition, slideMovement);

    }

        private void StopSliding()
        {
            // Kayma animasyonunu durdur
            isSliding = false;
           

        }



    /* private void MoveSideways(float input)
     {
         float newPosition = transform.position.x + input * slideSpeed;
       transform.position = new Vector3(newPosition, transform.position.y, transform.position.z);
     }*/

    private void MoveForward()
        {
         Vector3 forwardMovement = transform.forward *forwardSpeed* Time.deltaTime;
         rb.MovePosition(rb.position + forwardMovement);


        }

        /// <summary>
        /// oyuncunun h�z� ile ilgili k�s�m
        /// </summary>
        private void IncreaseSpeed()
        {
            //oyuncunun h�z�n� artt�rmak i�in;
            forwardSpeed += accelerationRate * Time.fixedDeltaTime;

            //oyuncunun h�z�n� s�n�rlamak i�in;

            forwardSpeed = Mathf.Clamp( forwardSpeed, initialForwardSpeed, maxForwardSpeed);
        }


        private void CheckIfGroundedAfterJump()
        {
            // Raycast kullanarak karakterin zemine d�zg�n bir �ekilde indi�ini kontrol etme
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
            {
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (slopeAngle < 45f && hit.collider.CompareTag("Ground"))
                {
                    isGrounded = true;
                }
            }
        }


        /// <summary>
        /// player hasar kontrol� i�in 
        /// </summary>

        void TakeDamage()
        {
            // Can say�s�n� azalt
            currentHealth -= 10;
            UpdateHealthBar();

            Debug.Log("Can kayb�! Mevcut Can: " + currentHealth);

            // Can s�f�r oldu�unda �l�m i�lemlerini ger�ekle�tir
            if (currentHealth <= 0)
            {
                GameManager.gameOver = true;
                playerScore.SetRunningState(false);
                Debug.Log("isruning false");


            }


        }

        void UpdateHealthBar()
        {
            float healthPercentage = (float)currentHealth / maxHealth;
          int imagesToHide = Mathf.RoundToInt((1 - healthPercentage) * healthImages.Length);

           for (int i = 0; i < healthImages.Length; i++)
          {
            healthImages[i].enabled = (i >= imagesToHide);
           }


         }
    IEnumerator PlayerChangeColorAndDisableDamage()
    {
        playerCanTakeDamage = false;

        GetComponent<Renderer>().material.color = Color.red;
        //hasar almay� engelle:

        yield return new WaitForSeconds(1f);

        //eski renge d�nd�rmek i�in
        GetComponent<Renderer>().material.color = originalColor;
        playerCanTakeDamage = true;
    }

    private void CenterPlayer()
    {
        // oyuncuyu merkze do�ru hareket ettirmek i�in:
        Vector3 targetPosition = new Vector3(2.3f, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 6f);

       //oyuncu merkeze yakla�t� m� 
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isCentering = false;
        }
    }


    private void RotatePlayerDirectly()
    {
        // Alt ok tu�una bas�ld���nda rotasyonu -38.58'e ayarla
        transform.rotation = Quaternion.Euler(-20, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // Belirli bir s�re sonra eski rotasyona d�nmesi i�in Invoke fonksiyonunu kullan
        Invoke("ResetRotation", 1.5f);
    }

    private void ResetRotation()
    {
        // 1 saniye sonra eski rotasyona d�n ilk �nce bunu dene
        //transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // 1 saniye sonra eski rotasyona d�n
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        /* Karakterin y�ksekli�ini sabit tut (groundun alt�na d��mesini �nle)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float groundHeight = hit.point.y + 0.5f; // Ayarlamak istedi�iniz y�kseklik
            transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
        }*/

    }

}



