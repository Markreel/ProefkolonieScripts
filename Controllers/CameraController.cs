using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Material mapMaterial;
    [SerializeField] Material mapMaterial2;

    [Header("Map Settings")]
    [SerializeField] float minMapAlpha = 0;
    [SerializeField] float maxMapAlpha = 20;

    [Header("Position")]
    [SerializeField] float defaultSpeed;
    [SerializeField] float fastSpeed;
    private float movementSpeed;
    [SerializeField] float movementTime;
    [SerializeField] Vector2 minPosition;
    [SerializeField] Vector2 maxPosition;

    [Header("Rotation")]
    [SerializeField] float rotationAmount;

    [Header("Zoom")]
    [SerializeField] Vector3 zoomAmount;
    [SerializeField] Vector3 zoomOnFocus;
    [SerializeField] Vector3 minZoom;
    [SerializeField] Vector3 maxZoom;
    public float NormalizedZoomValue { get; private set; }

    private Vector3 newPosition;
    private Quaternion newRotation;
    private Vector3 newZoom;

    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;

    public IFocusable CurrentFocusable;
    public IHoverable CurrentHoverable;

    public bool IsEnabled = false;

    private void Awake()
    {
        IsEnabled = false;
        if (cameraTransform == null) { cameraTransform = GetComponentInChildren<Transform>(); }

        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    private void Update()
    {
        if (!IsEnabled) { return; }

        HandleMouseInput();
        HandleKeyboardInput();

        HandleMapTransparency();
    }

    private void HandleMouseInput()
    {
        CheckForIHoverable();

        //Zoom (scroll)
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        //Movement (drag mouse)
        if (Input.GetMouseButtonDown(0))
        {
            CheckForIFocusable();

            Plane _plane = new Plane(Vector3.up, Vector3.zero);
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float _entry;
            if (_plane.Raycast(_ray, out _entry))
            {
                dragStartPosition = _ray.GetPoint(_entry);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Plane _plane = new Plane(Vector3.up, Vector3.zero);

            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float _entry;

            if (_plane.Raycast(_ray, out _entry))
            {
                dragCurrentPosition = _ray.GetPoint(_entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }

        //Rotate
        if (Input.GetMouseButtonDown(1))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            rotateCurrentPosition = Input.mousePosition;
            Vector3 _difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-_difference.x / 5f));
        }
    }

    private void HandleKeyboardInput()
    {
        //Speed
        movementSpeed = Input.GetKey(KeyCode.LeftShift) ? fastSpeed : defaultSpeed;

        //Movement
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            followTransform = null;
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            followTransform = null;
            newPosition += (-transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            followTransform = null;
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            followTransform = null;
            newPosition += (-transform.right * movementSpeed);
        }

        //Rotation
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(-Vector3.up * rotationAmount);
        }


        //Zoom
        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }

        //Clamp Values
        newZoom = MathM.ClampVector3(newZoom, minZoom, maxZoom);
        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.z = Mathf.Clamp(newPosition.z, minPosition.y, maxPosition.y);

        //Get normalized zoom float
        NormalizedZoomValue = 1f / (maxZoom.y - minZoom.y) * (newZoom.y -minZoom.y);     

        //Set values
        if (followTransform != null) { transform.position = Vector3.Lerp(transform.position, followTransform.position, Time.deltaTime * movementTime); }
        else { transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime); }

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    private void CheckForIFocusable()
    {
        RaycastHit _hit;
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            if(EventSystem.current.IsPointerOverGameObject() == true) { return; }

            if (_hit.transform.GetComponent<IFocusable>() != null)
            {
                Focus(_hit.transform, _hit.transform.GetComponent<IFocusable>());
                return;
            }
            else if (_hit.transform.GetComponentInParent<IFocusable>() != null)
            {
                Focus((_hit.transform.GetComponentInParent<IFocusable>() as MonoBehaviour).transform, _hit.transform.GetComponentInParent<IFocusable>());
                return;
            }
        }

        if(followTransform != null) { Unfocus(); }
    }

    private void CheckForIHoverable()
    {
        RaycastHit _hit;
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            IHoverable _hoverable = _hit.transform.GetComponentInParent<IHoverable>();
            if (_hoverable != null && _hoverable != CurrentFocusable)
            {
                Hover(_hoverable);
                return;
            }
        }

        if (CurrentHoverable != null) { Unhover(); }
    }

    private void HandleMapTransparency()
    {
        float _t = 1f / maxZoom.y * newZoom.y;

        Color _col = mapMaterial.color;
        _col.a = Mathf.Clamp(Mathf.Lerp(minMapAlpha, maxMapAlpha, _t), 0f, 1f);

        mapMaterial.SetColor("_BaseColor", _col);
        mapMaterial2.SetColor("_BaseColor", _col);
    }

    private void Focus(Transform _focusObject, IFocusable _focusable)
    {
        if (CurrentFocusable != null) { CurrentFocusable.OnUnfocus(); } //unfocus previous focus object

        followTransform = _focusObject.transform;

        CurrentFocusable = _focusable;
        CurrentFocusable.OnFocus();

        newZoom = zoomOnFocus * _focusable.ZoomAmountOnFocus;
    }

    public void Unfocus()
    {
        CurrentFocusable?.OnUnfocus();
        CurrentFocusable = null;
        followTransform = null;
    }

    private void Hover(IHoverable _hoverable)
    {
        if(CurrentHoverable != null) { Unhover(); }

        CurrentHoverable = _hoverable;
        CurrentHoverable.OnHover();
    }

    private void Unhover()
    {
        CurrentHoverable.OnUnhover();
        CurrentHoverable = null;
    }

}
