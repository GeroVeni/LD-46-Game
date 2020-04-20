using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    const int MAX_TRIES = 10;

    public int Holes { get; private set; }

    public string soundName;
    public SpriteRenderer holePrefab;

    BoxCollider holeContainer;
    List<SpriteRenderer> holes;

    // Start is called before the first frame update
    void Start()
    {
        Holes = 0;
        holes = new List<SpriteRenderer>();
        holeContainer = GetComponent<BoxCollider>();
        AudioManager.Instance.Play(soundName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPipe()
    {
        Holes = 0;
        holes.ForEach(h => Destroy(h.gameObject));
        holes.Clear();
    }

    Vector3 GetRandomPosition()
    {
        Vector3 res = new Vector3();
        Quaternion inverse = Quaternion.Inverse(transform.rotation);
        Vector3 size = inverse * holeContainer.bounds.extents;
        res.x = Random.Range(-size.x, size.x);
        res.y = Random.Range(-size.y, size.y);
        res.z = 0;
        return res;
    }

    public bool MakeHole()
    {
        for (int tries = 0; tries < MAX_TRIES; ++tries)
        {
            // Generate new hole
            SpriteRenderer hole = Instantiate(holePrefab, transform);
            hole.transform.localPosition = GetRandomPosition();
            hole.transform.localRotation = Quaternion.identity;
            if (holes.Exists(h => h.bounds.Intersects(hole.bounds)))
            {
                Destroy(hole.gameObject);
                continue;
            }
            hole.GetComponent<PipeHole>().HolePatchEvent += OnHolePatched;
            holes.Add(hole);
            Holes++;
            break;
        }
        return false;
    }

    private void OnHolePatched(PipeHole hole)
    {
        holes.Remove(hole.GetComponent<SpriteRenderer>());
        Holes--;
    }
}
