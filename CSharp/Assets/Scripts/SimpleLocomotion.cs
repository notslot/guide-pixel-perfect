using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D),
                  typeof(Animator))]
public class SimpleLocomotion : MonoBehaviour
{
  #region Constants

  private static readonly int ANIM_RUN = Animator.StringToHash("Run");

  private static readonly int ANIM_JUMP = Animator.StringToHash("Jump");

  private const float FORCE_RUN = 4;

  private const float FORCE_RUN_SWAP = 3.8f;

  private const float FORCE_JUMP = 6;

  #endregion


  #region Inspector

  [SerializeField]
  private PhysicsMaterial2D materialRun = default;

  [SerializeField]
  private PhysicsMaterial2D materialIdle = default;

  #endregion


  #region Fields

  private SpriteRenderer _renderer;

  private Rigidbody2D _rigidbody;

  private Animator _animator;

  #endregion


  #region MonoBehaviour

  private void Awake ()
  {
    _renderer = GetComponent<SpriteRenderer>();
    _rigidbody = GetComponent<Rigidbody2D>();
    _animator = GetComponent<Animator>();
  }

  private void Update ()
  {
    //
    // Change direction
    //

    if ( Input.GetKeyDown(KeyCode.LeftArrow) )
    {
      _rigidbody.AddForce(new Vector2(-FORCE_RUN_SWAP, 0), ForceMode2D.Impulse);
      _renderer.flipX = true;
    }
    else if ( Input.GetKeyDown(KeyCode.RightArrow) )
    {
      _rigidbody.AddForce(new Vector2(FORCE_RUN_SWAP, 0), ForceMode2D.Impulse);
      _renderer.flipX = false;
    }

    //
    // Change state
    //

    bool hasInput = Input.GetKey(KeyCode.LeftArrow) ||
                    Input.GetKey(KeyCode.RightArrow);

    // Animation
    _animator.SetBool(ANIM_RUN, hasInput);

    // Physics
    _rigidbody.sharedMaterial = hasInput ? materialRun : materialIdle;

    //
    // Jump
    //

    if ( Input.GetButtonDown("Jump") )
    {
      _rigidbody.AddForce(new Vector2(0, FORCE_JUMP), ForceMode2D.Impulse);
      _animator.SetBool(ANIM_JUMP, true);
    }
  }

  private void FixedUpdate ()
  {
    Vector2 force = new(FORCE_RUN * Input.GetAxis("Horizontal"), 0);
    _rigidbody.AddForce(force);
  }

  private void OnTriggerEnter2D (Collider2D col)
  {
    if ( col.CompareTag("Platform") )
      _animator.SetBool(ANIM_JUMP, false);
  }

  #endregion
}