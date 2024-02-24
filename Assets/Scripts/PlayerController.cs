using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
 
    public float forwardSpeed = 5f;
    public float laneDistance = 2f; //þeritler arasý
    public float jumpForce = 15f;
    private bool isGrounded ;
    private int currentLane = 1;  //oyuncunun bulunduðu þerit
    public Rigidbody rb;
    private bool isCentering = false;

    public float initialForwardSpeed = 1f; //Ýlerleme hýzý
    public float maxForwardSpeed = 25f; // maksimum hýz
    public float accelerationRate = 0.001f; // hýz artýþ oraný
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
        //oyun baþladýðýnda mevcut can sayýsý maksimum cana eþit 
        currentHealth = maxHealth;
        originalColor = GetComponent<Renderer>().material.color; // oyuncunun orijinal rengi
        playerScore = GetComponent<PlayerScore>();
        //can deðerini en baþta uýda göster
        UpdateHealthBar();


        rb = GetComponent<Rigidbody>();

        SetInitialPositionOnGround();


    }
    private void SetInitialPositionOnGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            // Zeminin tam ortasýna göre baþlangýç pozisyonunu ayarla
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
        //Zýplama kontrolü
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            Jump();
            // Engelin üzerinden zýpladýktan sonrasý için  kontrol
            CheckIfGroundedAfterJump();

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            RotatePlayerDirectly();
        }






        //hýzý arttýr
        IncreaseSpeed();

        // Engelin üzerinden zýpladýktan sonrasý için  kontrol
        //CheckIfGroundedAfterJump();
       

    }
    

    



    void HandleMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

       

        // Saða veya sola kaydýrma iþlemi
        if (Input.GetKeyDown(KeyCode.D) || horizontalInput > 0)  
        {
            MoveLane(1); // Saða kaydýrma

            Slide();
        }
        else if (Input.GetKeyDown(KeyCode.A) || horizontalInput < 0) 
        {
            MoveLane(-1); // Sola kaydýrma

            Slide();
        }
        /*Zýplama kontrolü
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            // Engelin üzerinden zýpladýktan sonrasý için  kontrol
            CheckIfGroundedAfterJump();
        }*/

        // C ye basýnca oyuncuyu merkeze getir
        if (Input.GetKeyDown(KeyCode.C)&& !isCentering)
        {
            isCentering = true;
          
        }






    }
    void MoveLane(int direction)
    {
        int targetLane = currentLane + direction;

        // Geçerli þerit aralýðýný kontrol etme
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
                // Bir engel ile çarpýþtýðýnda canýný düþür
                TakeDamage();
                //Oyuncunun rengini deðiþtir ve hasar almasýný engellemek için;
                StartCoroutine(PlayerChangeColorAndDisableDamage());
            }

            else if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;

                // Yerden zýplamýþ mý kontrol et
                if (hasJumped)
                {
                    hasJumped = false;
                    Debug.Log("Zemine indi!");
                }
            }
        }
    }

        /// <summary>
        /// Zýplama 
        /// </summary>

   private void Jump()
   {
        Debug.Log("zýplama");

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Önce diðer yükseklik bileþenlerini sýfýrla
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Zýplama kuvvetini ekle
        isGrounded = false;
        //rb.AddForce(Vector3.up * jumpForce);
        //isGrounded = false;
        hasJumped = true;


   }

    private void Slide()
    {
         // Kayma animasyonunu baþlat
        isSliding = true;
        // Saða veya sola kayma hýzý
        float slideMovement = slideSpeed * Time.deltaTime;

        // Hedef pozisyonu belirle
        Vector3 targetPosition = new Vector3(currentLane * laneDistance, transform.position.y, transform.position.z);

        // Yeni pozisyonu kayma hýzýyla belirtilen yöne doðru hareket ettir
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
        /// oyuncunun hýzý ile ilgili kýsým
        /// </summary>
        private void IncreaseSpeed()
        {
            //oyuncunun hýzýný arttýrmak için;
            forwardSpeed += accelerationRate * Time.fixedDeltaTime;

            //oyuncunun hýzýný sýnýrlamak için;

            forwardSpeed = Mathf.Clamp( forwardSpeed, initialForwardSpeed, maxForwardSpeed);
        }


        private void CheckIfGroundedAfterJump()
        {
            // Raycast kullanarak karakterin zemine düzgün bir þekilde indiðini kontrol etme
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
        /// player hasar kontrolü için 
        /// </summary>

        void TakeDamage()
        {
            // Can sayýsýný azalt
            currentHealth -= 10;
            UpdateHealthBar();

            Debug.Log("Can kaybý! Mevcut Can: " + currentHealth);

            // Can sýfýr olduðunda ölüm iþlemlerini gerçekleþtir
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
        //hasar almayý engelle:

        yield return new WaitForSeconds(1f);

        //eski renge döndürmek için
        GetComponent<Renderer>().material.color = originalColor;
        playerCanTakeDamage = true;
    }

    private void CenterPlayer()
    {
        // oyuncuyu merkze doðru hareket ettirmek için:
        Vector3 targetPosition = new Vector3(2.3f, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 6f);

       //oyuncu merkeze yaklaþtý mý 
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isCentering = false;
        }
    }


    private void RotatePlayerDirectly()
    {
        // Alt ok tuþuna basýldýðýnda rotasyonu -38.58'e ayarla
        transform.rotation = Quaternion.Euler(-20, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // Belirli bir süre sonra eski rotasyona dönmesi için Invoke fonksiyonunu kullan
        Invoke("ResetRotation", 1.5f);
    }

    private void ResetRotation()
    {
        // 1 saniye sonra eski rotasyona dön ilk önce bunu dene
        //transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // 1 saniye sonra eski rotasyona dön
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        /* Karakterin yüksekliðini sabit tut (groundun altýna düþmesini önle)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float groundHeight = hit.point.y + 0.5f; // Ayarlamak istediðiniz yükseklik
            transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
        }*/

    }

}



