using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction talkAction;
    // The variable type GameObject can contain the data for the Projectile prefab; prefabs are GameObjects saved as reusable assets.
    // This variable is public, so you can assign any GameObject prefab to this variable in the Inspector.
    public AudioSource footstepSource;
    public AudioSource sfxSource;
    public GameObject projectilePrefab;
    public AudioClip throwProjectile;
    /// <summary>
    /// The Animator type variable will store the Animator component reference, just as you did in the EnemyController script.
    // The moveDirection variable is important because the player character can stand still, whereas the enemy is always moving. When the character is still, both Move X and Move Y will be 0, so the State Machine needs to be explicitly provided with a direction, which this variable provides.
    // The keyword new creates a space in memory to store a new variable instance. The new variable instance may or may not have associated parameters — here, moveDirection is a Vector2 type and takes the x and y value of the direction as parameters.
    /// </summary>
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);
    //////////////////////////////////////////
    // public InputAction LeftAction; // The keyword public means that this InputAction will be available to you in the Unity Editor.
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d; // This variable needs to be accessed within this script but not in Unity Editor, so the declaration does not include the public keyword.
    Vector2 move;
    public float speed = 3.0f;
    // Variables related to the health system
    public int maxHealth = 5; // maxHealth has the public keyword, which means that you can adjust it using the Inspector window in the Unity Editor. The variable’s value is set in the declaration as 5.
    // we want the currentHealth to remain protected in this script, so we make a read-only version of it and we named it health. we can even write more than just return currentHealth; , like declaring functions or stuff, but now we just need this
    public int health { get { return currentHealth; } }
    int currentHealth; // currentHealth is not public, so it will not be visible in the Inspector window. This variable’s value has not been set in the declaration.

    // Variables related to temporary invincibility
    // The public float variable timeInvincible controls how long the player character remains invincible after taking damage, measured in seconds. This variable is public, so you can adjust it in the Inspector window for quick iteration.
    public float timeInvincible = 2.0f;
    public bool Invincible { get { return isInvincible; } }
    bool isInvincible;
    // The private float variable damageCooldown stores how much time is remaining until the player character is no longer invincible and can take more damage.
    float damageCooldown;
    // Start is called before the first frame update

    void Start()
    {
        /// These instructions will make Unity render the game at 10 frames per second.
        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = 10;
        ///
        // LeftAction.Enable();
        animator = GetComponent<Animator>();
        MoveAction.Enable();
        talkAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update() // this function is called every time a new frame is displayed.
    {

        // float horizontal = 0.0f;
        // // if (Keyboard.current.leftArrowKey.isPressed)
        // if (LeftAction.IsPressed())
        // {

        //     horizontal = -1.0f;

        // }
        // else if (Keyboard.current.rightArrowKey.isPressed)
        // {

        //     horizontal = 1.0f;

        // }
        // float vertical = 0.0f;
        // if (Keyboard.current.upArrowKey.isPressed)
        // {
        //     vertical = 1.0f;
        // }
        // else if (Keyboard.current.downArrowKey.isPressed)
        // {
        //     vertical = -1.0f;
        // }

        // Debug.Log(horizontal);
        // Vector2 position = transform.position;
        // position.x = position.x + 0.1f * horizontal;
        // position.y = position.y + 0.1f * vertical;
        // transform.position = position;
        // Left arrow key pressed: current position + (0.1 * -1)
        // Right arrow key pressed: current position + (0.1 * 1)
        // No key pressed: current position + (0.1 * 0)
        // Vector2 position = transform.position;
        // position.x = position.x + 0.1f;
        // transform.position = position;
        // Vector2 move = MoveAction.ReadValue<Vector2>(); // move is also 1 or -1 or 0 like when we did it manually
        move = MoveAction.ReadValue<Vector2>();
        if (move.magnitude > 0.1f)
        {
            if (!footstepSource.isPlaying)
                footstepSource.Play();
        }
        else
        {
            if (footstepSource.isPlaying)
                footstepSource.Stop();
        }
        // Debug.Log(move);
        // Vector2 position = (Vector2)transform.position + move * 0.1f;
        // Vector2 position = (Vector2)transform.position + move * 3.0f * Time.deltaTime;
        // transform.position = position;
        // This if statement checks whether move.x or move.y (the current user input movement values, stored in the move variable declared in the Update function) are not equal to 0.
        // The condition uses Mathf.Approximately to make the check instead of the equality operator (==) because the way that computers store float values means that there is a tiny loss in precision. This loss means that you should not test for perfect quality, because an operation that should return 0.0f could end up returning 0.0000000001f instead. The bool Approximately takes the imprecision into account, and returns true if the value can be considered equal minus that imprecision.
        // || is the C# conditional OR operator, which determines the condition to be met if either side of the operator is true (one and/or the other). In this case, the code block executes if either move.x or move.y is not equal to zero — that is, if the player character is moving.
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            // The first instruction sets the moveDirection variable based on the current movement values. When the character is not moving, the moveDirection remains set to the last movement values provided.
            // Vector2 type variables can store direction values as well as positions. The second instruction calls Normalize on moveDirection, which sets its length to 1 but keeps its direction the same.
            // For example, a Vector2 set to (3,0) stores a position one unit to the right of the center of the scene, but it also stores the direction right — if you trace an arrow from (0,0) to (3,0) you get an arrow pointing to the right. Normalizing this vector will set it to (1,0), still pointing to the right but with a length of 1. In general, you normalize vectors that store direction because length is not important.
            // Important: Do not normalize vectors storing positions. Normalizing changes the x and y values, so normalizing position vectors will change the position.
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }
        /////////////////////////////////////////////
        // These instructions pass the direction data to the PlayerCharacter GameObject’s Animator component.
        // The third instruction passes the length of the move vector to the Speed parameter. This length will be 0 if the player character is stationary, or 1 if the character is moving (because the length is normalized).
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        ////////////////////////////////////////////
        if (isInvincible)
        {
            // Time.deltaTime is the interval in seconds from the previous frame to the current one, so the instruction reduces the damageCooldown timer value every frame.
            damageCooldown -= Time.deltaTime; // after one second, one unit is subtracted from damageCooldown
            if (damageCooldown < 0) isInvincible = false;

        }

        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     Launch();
        // }
        if (Keyboard.current.cKey.wasPressedThisFrame) Launch();

        if (Keyboard.current.xKey.wasPressedThisFrame) FindFriend();

    }

    // FixedUpdate has the same call rate as the physics system 
    void FixedUpdate()
    {
        // با ضرب کردن در deltaTime، حرکت شما به زمان واقعی وابسته می‌شود، نه به تعداد فریم.
        //موقعیت جدید = موقعیت فعلی + حرکت در آن فریم.
        // این به شما حرکت پیوسته و صاف می‌دهد.
        Vector2 position = (Vector2)rigidbody2d.position + move * 3.0f * Time.deltaTime;
        rigidbody2d.MovePosition(position);

    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible) return;

            isInvincible = true;
            damageCooldown = timeInvincible;
            // This sets the Hit parameter to True if the player character is taking damage, so the correct animation plays.
            animator.SetTrigger("Hit");
        }
        //The first parameter is the value that needs to be restricted (currentHealth + amount).
        // The second parameter is the minimum allowed value (0).
        // The third parameter is the maximum allowed value (maxHealth).
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        // Debug.Log(currentHealth + "/" + maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        // This instruction calls Instantiate, a Unity function that takes three parameters.
        // Instatiate’s first parameter is a GameObject reference. Calling Instantiate creates a copy of that GameObject in a position defined by the second parameter, with the rotation defined by the third parameter.
        // In this situation, the Projectile prefab is the GameObject. You’ve defined a position at the position of the PlayerCharacter GameObject’s Rigidbody component but a little up, so the Projectile prefab is placed closer to the character sprite’s hands than feet. You’ve also set a rotation of Quaternion.identity.
        // Quaternions are mathematical operators that can express rotation. All you need to know here is that Quaternion.identity means no rotation.
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        // The first instruction gets a reference to the Projectile script on the newly instantiated Projectile GameObject.
        // The second instruction calls the Launch function from the Projectile script on the new Projectile instance, passing it the direction of movement (which you stored in a variable for the player character animations) and a force value of 300.
        // The force value that you have set is high because it is expressed in Newton units. This value is a good fit for a basic 2D adventure game, but if you want to fine-tune the force of the projectile you can create a public float variable and use that as the force instead.
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);
        // This instruction sets a trigger so the Launch character animation plays when the player launches a projectile.
        animator.SetTrigger("Launch");
        PlaySound(throwProjectile);
    }

    void FindFriend()
    {
        // The RaycastHit2D type variable hit stores the result of a raycast, which is obtained by calling Physics2D.Raycast. There are multiple versions of Physics2D.Raycast; the version used here has four arguments (values you can pass to it).
        // The first argument is the starting point for the ray. Here it is an upward offset from the position of the PlayerCharacter GameObject, which will test from the center of the sprite rather than its feet.
        // The second argument is the direction that the player character is looking, using moveDirection.
        // The third argument is the maximum distance for the ray — 1.5 units here keeps the test within a short distance from the start point, because the characters aren’t shouting.
        // The final argument defines a layer mask, which is a way to only check within specified layers. Any layers that are not within the mask are ignored during the intersection test. The NPC layer is the only layer relevant for this test, so it’s the only one in the mask.
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            // Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
            //Note: GetComponent is relatively resource-intensive, so avoid calling it every frame. In this situation, GetComponent is only called when the player provides the correct input.
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character != null) UIHandler.instance.DisplayDialogue();
        }
    }

    public void PlaySound(AudioClip clip)
    {
        // for health collectabel, when got hit, throwing projectile
        sfxSource.PlayOneShot(clip);
    }
}
