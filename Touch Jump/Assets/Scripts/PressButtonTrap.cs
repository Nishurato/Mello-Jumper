using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButtonTrap : MonoBehaviour
{
    [SerializeField] private Rigidbody2D[] press;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Destroy(this.gameObject);
        for (int i = 0; i < press.Length; i++)
        {
            press[i].bodyType = RigidbodyType2D.Dynamic;
		}
	}
}
