using UnityEngine;
using System.Collections.Generic;

namespace BallCrush
{
    public class InputHanlder : MonoBehaviour
    {
        public static InputHanlder Instance { get; private set; }
        public static event System.Action<Vector2> OnShoot;

        [SerializeField] private GameObject _ballGhost;


        [field: SerializeField] public bool IsDragging { get; set; }
        private Transform _shootPointPosition;
      


        private LineRenderer _lr;
        [Min(0)]
        private int _bounces = 0;

        private List<Ray2D> rays;
        private List<RaycastHit2D> hits;
        private List<Vector2> normals;
        private List<Vector2> dirs;

        private Ray2D ray;
        private Vector2 dir;
        private Vector2 normal;
        private RaycastHit2D hit;
        private float _maxLength = 40f;

        private void Awake()
        {
            Instance = this;
        }


        private void Start()
        {          
            _lr = GetComponent<LineRenderer>();
            _shootPointPosition = BallSpawner.Instance.ShootPoint;
            _ballGhost.transform.position = _shootPointPosition.position;
            rays = new List<Ray2D>();
            hits = new List<RaycastHit2D>();
            normals = new List<Vector2>();
            dirs = new List<Vector2>();


            for (int a = 0; a < _bounces + 1; a++)
            {
                rays.Add(ray);
                hits.Add(hit);
                normals.Add(normal);
                dirs.Add(dir);
            }
        }

        private void Update()
        {
            if (GameplayManager.Instance.CurrentState != GameplayManager.GameState.PLAYING) return;
           
            if (Input.GetMouseButtonDown(0))
            {            
                IsDragging = true;
            }
            else if(Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (mousePosition.y < _shootPointPosition.position.y)
                {
                    IsDragging = true;
                }
                else
                {
                    IsDragging = false;
                }

            }
            else if (Input.GetMouseButtonUp(0))
            {
                if(IsDragging)
                {
                    IsDragging = false;
                    _ballGhost.SetActive(false);

                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    OnShoot?.Invoke(mousePosition);
                    GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.WAITING);
                }              
            }
        }


       

        private void FixedUpdate()
        {
            if (IsDragging)
            {
                Aiming(); // aiming
                float totalDistance = 0f;

                for (int i = 1; i < _bounces + 2; i++)
                {
                    rays.Add(ray);
                    hits.Add(hit);
                    normals.Add(normal);
                    dirs.Add(dir);

                    switch (i)
                    {
                        case 1:
                            rays[i - 1] = new Ray2D(_shootPointPosition.position, dirs[i - 1]);
                            hits[i - 1] = Physics2D.Raycast(rays[i - 1].origin, rays[i - 1].direction, Mathf.Infinity);
                            _lr.positionCount = _bounces + 2;
                            _lr.SetPosition(i - 1, rays[i - 1].origin);
                            _lr.SetPosition(i, hits[i - 1].point);
                            totalDistance += Vector2.Distance(rays[i - 1].origin, hits[i - 1].point);
                            break;

                        default:
                            normals[i - 2] = hits[i - 2].normal;
                            dirs[i - 1] = Vector2.Reflect(rays[i - 2].direction, normals[i - 2].normalized);
                            rays[i - 1] = new Ray2D(hits[i - 2].point + dirs[i - 1] * 0.01f, dirs[i - 1]);
                            hits[i - 1] = Physics2D.Raycast(rays[i - 1].origin, rays[i - 1].direction, Mathf.Infinity);
                            _lr.positionCount = _bounces + 2;

                            float maxSegmentLength = _maxLength / (_bounces + 1);
                            float currentSegmentLength = Vector2.Distance(hits[i - 2].point, hits[i - 1].point);
                            float interpolationFactor = maxSegmentLength / currentSegmentLength;

                            Vector2 interpolatedPoint = Vector2.Lerp(hits[i - 2].point, hits[i - 1].point, interpolationFactor);

                            _lr.SetPosition(i, interpolatedPoint);
                            totalDistance += currentSegmentLength;
                            break;
                    }

                    if (totalDistance > _maxLength)
                    {
                        // Shorten the line if it exceeds the maximum length
                        float excess = totalDistance - _maxLength;
                        int startIndex = i - 1;

                        while (startIndex >= 0 && excess > 0)
                        {
                            float segmentLength = Vector2.Distance(_lr.GetPosition(startIndex), _lr.GetPosition(startIndex + 1));
                            float clampedFactor = Mathf.Clamp01(1f - excess / segmentLength);
                            Vector2 shortenedPoint = Vector2.Lerp(_lr.GetPosition(startIndex), _lr.GetPosition(startIndex + 1), clampedFactor);

                            _lr.SetPosition(startIndex + 1, shortenedPoint);
                            excess -= segmentLength;

                            startIndex--;
                        }
                    }
                }
            }
            else
            {
                _lr.positionCount = 0;
            }

        }

        private void Aiming()
        {
            dirs[0] = Input.mousePosition - Camera.main.WorldToScreenPoint(_shootPointPosition.position);
            float angle = Mathf.Atan2(dirs[0].y, dirs[0].x) * Mathf.Rad2Deg;
            _shootPointPosition.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public void ShowGhost()
        {
            _ballGhost.SetActive(true);
        }
    }

}

