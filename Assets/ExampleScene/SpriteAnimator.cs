using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void SpriteAnimatorEvent(SpriteAnimator sender);


public class SpriteAnimator : MonoBehaviour
{
    public MeshRenderer MR;
    public MeshRenderer MR2;
    public int DefaultTextureIndex;

    private SpriteSequence currentSequence;

    public float PPU = 32;

    [SerializeField] [ReadOnly] 
    private string currentSequenceName;

    [SerializeField] [ReadOnly] 
    private float timer = 0;

    [SerializeField] [ReadOnly] 
    private int currentSpriteIndex = -1;

    public List<Texture2D> Textures;
    public List<SpriteSequence> Sequences;

    Camera cam;

    public event SpriteAnimatorEvent SequenceFinished;
    public event SpriteAnimatorEvent TextureChanged;

    public string CurrentSequenceName { get => currentSequenceName; }

    void Start()
    {
        SetTexture(DefaultTextureIndex);
        cam = Camera.main;
    }

    public void Update()
    {
        UpdateSequence();

        // the rotation needs to be set so that the sprite faces the camera.
        // mr2 is backface, it's flipped 180 degrees so shadows look correct.
        transform.rotation = Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.y, Vector3.up);
    }

    public void UpdateSequence()
    {
        if (currentSequence == null)
            return;

        timer += Time.deltaTime;

        if (timer >= currentSequence.Milliseconds / 1000f)
        {
            timer -= currentSequence.Milliseconds / 1000f;
            currentSpriteIndex++;
            if (currentSpriteIndex >= currentSequence.Indices.Count)
            {
                if (currentSequence.Repeats)
                {
                    currentSpriteIndex = 0;
                }
                else
                {
                    SequenceFinished?.Invoke(this);
                    return;
                }
            }

            SetTexture(currentSequence.Indices[currentSpriteIndex]);
        }
    }
    
    void SetTexture(int newIndex)
    {
        float dppu = PPU * 2;
        float flipMulti = 1f; 
        
        if (currentSequence != null)
            flipMulti = currentSequence.Flip ? -1f : +1f;

        var tex = Textures[newIndex];
        MR.material.SetTexture("_BaseMap", tex);
        
        MR.gameObject.transform.localScale = new Vector3(
            flipMulti * tex.width / PPU,
            tex.height / PPU,
            1);

        MR.gameObject.transform.localPosition = new Vector3(
            0,
            tex.height / dppu,
            0);

        MR2.material.SetTexture("_BaseMap", tex);

        MR2.gameObject.transform.localScale = new Vector3(
            -1 * flipMulti * tex.width / PPU,
            tex.height / PPU,
            1);

        MR2.gameObject.transform.localPosition = new Vector3(
            0,
            tex.height / dppu,
            0);

        TextureChanged?.Invoke(this);
    }

    public void PlaySequence(string name)
    {
        var sequence = Sequences.FirstOrDefault(x => x.Name == name);

        currentSequence = sequence;
        currentSequenceName = name;

        timer = 0;
        currentSpriteIndex = 0;

        if (sequence == null)
        {
            Debug.LogError($"Cannot play sequence {name}, no such sequence found in {gameObject.name}");

            return;
        }
        else
        {
            currentSpriteIndex = currentSequence.Indices[0];
            SetTexture(currentSpriteIndex);
        }
    }
}
