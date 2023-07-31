using UnityEngine;

namespace Utils
{
    public class SpawnUtils : MonoBehaviour
    {
        private const string ContainerName = "===[ Spawned ]===";

        public static GameObject Spawn(GameObject prefab, Vector3 position)
        {
            var contaner = GameObject.Find(ContainerName);
            if (contaner == null)
            {
                contaner = new GameObject(ContainerName);
            }
            
            return Instantiate(prefab, position, Quaternion.identity, contaner.transform);
        }
    }
}