using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayColor : MonoBehaviour
{
    private Material _material;
    [SerializeField] private Shader _shader;

    // Start is called before the first frame update
    void Start()
    {
        _material = new Material(_shader);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, _material);
	}
}
