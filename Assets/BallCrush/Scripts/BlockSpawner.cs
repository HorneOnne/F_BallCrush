using System.Collections.Generic;
using UnityEngine;


namespace BallCrush
{

    public class BlockSpawner : MonoBehaviour
    {
        public static BlockSpawner Instance { get; private set; }

        [Header("Prefabs")]
        [SerializeField] private List<BaseBlock> _blocks;
        [SerializeField] private SpecialBlock _specialBlock;

        [Header("Others")]
        [SerializeField] private Transform _centerPointTransform;
        private float _distanceEachStage = 1.6f;
        private float _gridSize = 1.6f;
        private bool _isEvenSpawn = false;


        [Header("Probability")]
        [Range(0f, 1f)]
        [SerializeField] private float _fillBlockRate;
        [Range(0f, 1f)]
        [SerializeField] private float _specialBlockRate;
        [Range(0f, 1f)]
        [SerializeField] private float _strongBlockRate;



        #region Properties
        public int Round { get; private set; }
        #endregion

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            GameplayManager.OnRoundFinished += GenerateNextRow;
            GameplayManager.OnRoundFinished += ()=>
            {
                Round++;
            };
        }

        private void OnDisable()
        {
            GameplayManager.OnRoundFinished -= GenerateNextRow;
            GameplayManager.OnRoundFinished -= () =>
            {
                Round++;
            };
        }

        private void Start()
        {
            Round = 1;

            for (int i = 0; i < 4; i++)
            {
                Vector2 position = new Vector2(_centerPointTransform.position.x, _centerPointTransform.position.y + _distanceEachStage * i);
                int spawnCount = _isEvenSpawn == true ? 3 : 4;
                GenerateRowBlocks(position, spawnCount);
                _isEvenSpawn = !_isEvenSpawn;
            }

        }


        private void GenerateNextRow()
        {
            Vector2 position = new Vector2(_centerPointTransform.position.x, _centerPointTransform.position.y - _distanceEachStage);
            int spawnCount = _isEvenSpawn == true ? 3 : 4;
            GenerateRowBlocks(position, spawnCount, true);
            _isEvenSpawn = !_isEvenSpawn;
        }

        private void GenerateRowBlocks(Vector3 centerPoint, int count, bool moveUp = false)
        {
            var points = GeneratePoints(centerPoint, count, default(Vector2), _gridSize);

            for (int i = 0; i < points.Count; i++)
            {
                if(Random.Range(0f, 1f) < _fillBlockRate)
                {
                    if (Random.Range(0f, 1f) < _specialBlockRate)
                    {
                        BaseBlock block = Instantiate(_specialBlock, points[i], Quaternion.identity);
                        if (moveUp)
                        {
                            block.MoveUp();
                        }
                    }
                    else
                    {
                        Vector3 eulerAngle = GetRandomBlockRotation(-25f, 25f);
                        BaseBlock block = Instantiate(_blocks[Random.Range(0, _blocks.Count)], points[i], Quaternion.Euler(eulerAngle));

                        if (moveUp)
                        {
                            block.MoveUp();
                        }
                        if (Random.Range(0f, 1f) < _strongBlockRate)
                            block.SetHealth(Round + Random.Range(5,15));
                        else
                            block.SetHealth(Round);


                    }
                    
                }
            }
        }

        private List<Vector2> GeneratePoints(Vector2 centerPoint, int numberOfPoints, Vector2 vectorOffset = default(Vector2), float spacing = 1.45f)
        {
            Vector2 dirVector = Vector2.right;
            List<Vector2> points = new List<Vector2>();

            if (numberOfPoints % 2 == 0)
            {
                // Generate an even number of points
                for (int i = 0; i < numberOfPoints; i++)
                {
                    float offset = (i - numberOfPoints / 2 + 0.5f) * spacing;
                    Vector2 newPoint = centerPoint + dirVector * offset;
                    points.Add(newPoint);
                }
            }
            else
            {
                // Generate an odd number of points
                int middleIndex = numberOfPoints / 2;
                points.Add(centerPoint);

                for (int i = 1; i <= middleIndex; i++)
                {
                    float offset = i * spacing;
                    Vector2 leftPoint = centerPoint - dirVector * offset;
                    Vector2 rightPoint = centerPoint + dirVector * offset;

                    points.Add(leftPoint);
                    points.Add(rightPoint);
                }
            }
            return points;
        }


        private Vector3 GetRandomBlockRotation(float minZ, float maxZ)
        {
            return new Vector3(0f, 0f, Random.Range(minZ, maxZ));
        }
    }
}

