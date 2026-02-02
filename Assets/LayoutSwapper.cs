using UnityEngine;

public class LayoutSwapper : MonoBehaviour
{
    [Header("Room Lists")] 
    [Tooltip("Rooms on the upper floor that can be swapped among themselves.")]
    [SerializeField] private Transform[] upstairsRooms;

    [Tooltip("Rooms on the lower floor that can be swapped among themselves.")]
    [SerializeField] private Transform[] downstairsRooms;

    [Header("Player reference")]
    [Tooltip("Transform representing the player height. If null, Camera.main.transform will be used if available.")]
    [SerializeField] private Transform playerTransform;

    [Header("Swapping behavior")] 
    [Tooltip("Seconds to wait before allowing another swap.")]
    [SerializeField] private float swapCooldownSeconds = 5f;

    [Tooltip("Y-axis height threshold. If player is below this, they are downstairs (upstairs rooms swap). Otherwise, they are upstairs (downstairs rooms swap).")]
    [SerializeField] private float heightThreshold = 5f;

    [Tooltip("If true, a room may end up at the same position it previously had after a shuffle.")]
    [SerializeField] private bool allowSamePositionOnSwap = false;

    private float _lastSwapTime;
    private bool _wasDownstairsLastFrame;
    private bool _isFirstFrame = true;
    private float _floorChangeTime = -999f;
    private bool _pendingSwapForUpstairs = false;

    private void Awake()
    {
        if ((upstairsRooms == null || upstairsRooms.Length == 0) && 
            (downstairsRooms == null || downstairsRooms.Length == 0))
        {
            Debug.LogWarning("LayoutSwapper: Please assign at least one room list (upstairs or downstairs) in the inspector.", this);
            enabled = false;
            return;
        }

        if (playerTransform == null && Camera.main != null)
        {
            playerTransform = Camera.main.transform;
        }

        if (playerTransform == null)
        {
            Debug.LogWarning("LayoutSwapper: playerTransform is not assigned and Camera.main is null. Swaps will be disabled.", this);
            enabled = false;
            return;
        }

        _lastSwapTime = Time.time;
    }

    private void Update()
    {
        float playerY = playerTransform.position.y;
        bool isDownstairsNow = playerY < heightThreshold;

        // Initialize on first frame
        if (_isFirstFrame)
        {
            _wasDownstairsLastFrame = isDownstairsNow;
            _isFirstFrame = false;
            return;
        }

        // Check if floor state has changed
        bool floorStateChanged = _wasDownstairsLastFrame != isDownstairsNow;
        
        if (floorStateChanged)
        {
            // Floor state just changed - start the cooldown timer
            _floorChangeTime = Time.time;
            _pendingSwapForUpstairs = isDownstairsNow; // true if we should swap upstairs rooms
            _wasDownstairsLastFrame = isDownstairsNow;
            return; // Don't swap yet, wait for cooldown
        }

        // Check if we have a pending swap and cooldown has expired
        if (_floorChangeTime > 0 && Time.time - _floorChangeTime >= swapCooldownSeconds)
        {
            // Cooldown expired - perform the swap
            if (_pendingSwapForUpstairs)
            {
                // Swap upstairs rooms (player is downstairs)
                if (upstairsRooms != null && upstairsRooms.Length > 1)
                {
                    PerformRandomSwap(upstairsRooms);
                }
            }
            else
            {
                // Swap downstairs rooms (player is upstairs)
                if (downstairsRooms != null && downstairsRooms.Length > 1)
                {
                    PerformRandomSwap(downstairsRooms);
                }
            }
            
            _floorChangeTime = -999f; // Reset so we don't swap again until next floor change
        }
    }

    /// <summary>
    /// Swap rooms among themselves by shuffling their positions and rotations.
    /// </summary>
    private void PerformRandomSwap(Transform[] roomsToSwap)
    {
        if (roomsToSwap == null || roomsToSwap.Length <= 1) return;

        int n = roomsToSwap.Length;

        // Capture current poses of all rooms in the list
        Vector3[] currPositions = new Vector3[n];
        Quaternion[] currRotations = new Quaternion[n];
        for (int i = 0; i < n; i++)
        {
            if (roomsToSwap[i] == null) continue;
            currPositions[i] = roomsToSwap[i].position;
            currRotations[i] = roomsToSwap[i].rotation;
        }

        // Create a permutation for the n rooms
        int[] permutation = new int[n];
        for (int i = 0; i < n; i++) permutation[i] = i;
        ShuffleArray(permutation);

        if (!allowSamePositionOnSwap)
        {
            // Avoid fixed points if possible (simple retry loop)
            bool hasFixed = true;
            int attempts = 0;
            while (hasFixed && attempts < 20)
            {
                hasFixed = false;
                for (int i = 0; i < n; i++)
                {
                    if (permutation[i] == i)
                    {
                        hasFixed = true;
                        break;
                    }
                }

                if (hasFixed)
                {
                    ShuffleArray(permutation);
                }

                attempts++;
            }
        }

        // Apply the permutation: each room gets the position/rotation from the permuted index
        for (int i = 0; i < n; i++)
        {
            if (roomsToSwap[i] == null) continue;
            int sourceIndex = permutation[i];
            roomsToSwap[i].SetPositionAndRotation(currPositions[sourceIndex], currRotations[sourceIndex]);
        }
    }

    private void ShuffleArray(int[] array)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            int j = Random.Range(i, array.Length);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}
