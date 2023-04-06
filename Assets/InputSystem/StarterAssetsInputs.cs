using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aim;
		public bool weaponState = false;
		
		public int weaponIdx;


		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		private Animator _animator;
		private int _animIDCarry;
		private int _animIDUnCarry;
		private int _animIDPickUp;

		private WeaponManager _weaponManager;


#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED

		private void Awake()
        {
			_animator = GetComponent<Animator>();
			_weaponManager = GetComponent<WeaponManager>();
			_animIDCarry = Animator.StringToHash("Carry");
			_animIDUnCarry = Animator.StringToHash("Uncarry");
			_animIDPickUp = Animator.StringToHash("PickItem");
			weaponIdx = 0;

		}
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnAim(InputValue value)
        {
			AimInput(value.isPressed);
        }

		public void OnWeapon(InputValue value)
        {
            if(value.isPressed){
				WeaponInput();
            }
        }

		public void OnUsePrevWeapon(InputValue value)
        {
			
            if (value.isPressed)
            {
				_weaponManager.UsePrevWeapon();
            }
        }

        public void OnNextWeapon(InputValue value)
		{
			
			if (value.isPressed)
			{
				_weaponManager.UseNextWeapon();

			}
		}

		public void OnPickUp(InputValue value)
        {
            if (value.isPressed)
            {
				processPickUp();
			}
        }

		public void OnFire(InputValue value)
        {
			if (value.isPressed)
			{
				FireWeapon();
			}
		}

		public void OnGrenade(InputValue value)
        {
			if (value.isPressed)
			{
				ThrowGrenade();
			}
		}
		public void OnThrowWeapon(InputValue value)
        {
			if (value.isPressed)
			{
				ThrowWeapon();
			}
		}

        private void ThrowWeapon()
        {
            if (weaponState)
            {
				_weaponManager.ThrowWeapon();
            }
            else
            {
				print("can not throw weapon");
            }
        }

        private void ThrowGrenade()
        {
			_weaponManager.ThrowGrenade();

		}

        private void FireWeapon()
        {
			if (!weaponState)
			{
				WeaponInput();
				//_animator.SetTrigger(_animIDCarry);
				//weaponState = true;
				return;
			}
			_weaponManager.FireWeapon();

		}

        private void processPickUp()
        {
			if(TPSShootController.toBePickedUp != null)
            {
				_animator.SetTrigger(_animIDPickUp);
			}
        }

		private void WeaponInput()
        {
			weaponState = !weaponState;
            if (weaponState)
            {
				if (!_weaponManager.hasWeapon())
				{
					weaponState = false;
					return;
				}
				_animator.SetTrigger(_animIDCarry); //动画事件为PickUpCurrentObject()，在TPSShootController脚本中
			}
            else
            {
				_animator.SetTrigger(_animIDUnCarry);
			}
		}

		private void SwitchWeapon(int currentweaponIdx)
		{
			
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void AimInput(bool newAimState)
        {
			aim = newAimState;
		}


		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}