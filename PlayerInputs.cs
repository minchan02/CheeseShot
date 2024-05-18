// GENERATED AUTOMATICALLY FROM 'Assets/PlayerInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""PlayerInputAction"",
            ""id"": ""574ffd62-9a0c-4887-bf20-f26b81a262ae"",
            ""actions"": [
                {
                    ""name"": ""Direction"",
                    ""type"": ""Value"",
                    ""id"": ""08152188-fd0c-4b80-8745-1c23352c28b5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""98964a36-d098-4a76-8664-f7cd0e031459"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Direction"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d63ee3cf-7d8c-4a70-a1c7-d32af2ca65b4"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardControl"",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""85fbbfea-b12f-4230-89ca-ff5f653cd81b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardControl"",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7d9aa3a6-25a4-4a99-b628-aaced89d902f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardControl"",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0b2a1476-6cc4-4d3c-a538-95d476292874"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardControl"",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""1a52fa4b-568b-4476-b69e-02ca982420f3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Direction"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9a89f29d-d0a7-435d-88b6-e7d49e44ee94"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardControl"",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f26e31ae-217c-4c2d-9d2e-122ce890ef22"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardControl"",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7c87045c-fc8f-459f-8fb6-736700f59ee3"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardControl"",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0ce26917-f12e-476c-912e-2fef24558552"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardControl"",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5481516f-43e2-41da-b0a5-6855d78d928f"",
                    ""path"": ""<Joystick>/stick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""JoystickControl"",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardControl"",
            ""bindingGroup"": ""KeyboardControl"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""JoystickControl"",
            ""bindingGroup"": ""JoystickControl"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerInputAction
        m_PlayerInputAction = asset.FindActionMap("PlayerInputAction", throwIfNotFound: true);
        m_PlayerInputAction_Direction = m_PlayerInputAction.FindAction("Direction", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // PlayerInputAction
    private readonly InputActionMap m_PlayerInputAction;
    private IPlayerInputActionActions m_PlayerInputActionActionsCallbackInterface;
    private readonly InputAction m_PlayerInputAction_Direction;
    public struct PlayerInputActionActions
    {
        private @PlayerInputs m_Wrapper;
        public PlayerInputActionActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Direction => m_Wrapper.m_PlayerInputAction_Direction;
        public InputActionMap Get() { return m_Wrapper.m_PlayerInputAction; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerInputActionActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerInputActionActions instance)
        {
            if (m_Wrapper.m_PlayerInputActionActionsCallbackInterface != null)
            {
                @Direction.started -= m_Wrapper.m_PlayerInputActionActionsCallbackInterface.OnDirection;
                @Direction.performed -= m_Wrapper.m_PlayerInputActionActionsCallbackInterface.OnDirection;
                @Direction.canceled -= m_Wrapper.m_PlayerInputActionActionsCallbackInterface.OnDirection;
            }
            m_Wrapper.m_PlayerInputActionActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Direction.started += instance.OnDirection;
                @Direction.performed += instance.OnDirection;
                @Direction.canceled += instance.OnDirection;
            }
        }
    }
    public PlayerInputActionActions @PlayerInputAction => new PlayerInputActionActions(this);
    private int m_KeyboardControlSchemeIndex = -1;
    public InputControlScheme KeyboardControlScheme
    {
        get
        {
            if (m_KeyboardControlSchemeIndex == -1) m_KeyboardControlSchemeIndex = asset.FindControlSchemeIndex("KeyboardControl");
            return asset.controlSchemes[m_KeyboardControlSchemeIndex];
        }
    }
    private int m_JoystickControlSchemeIndex = -1;
    public InputControlScheme JoystickControlScheme
    {
        get
        {
            if (m_JoystickControlSchemeIndex == -1) m_JoystickControlSchemeIndex = asset.FindControlSchemeIndex("JoystickControl");
            return asset.controlSchemes[m_JoystickControlSchemeIndex];
        }
    }
    public interface IPlayerInputActionActions
    {
        void OnDirection(InputAction.CallbackContext context);
    }
}
