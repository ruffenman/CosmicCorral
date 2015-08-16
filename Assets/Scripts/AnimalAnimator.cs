using UnityEngine;
using System.Collections;

public class AnimalAnimator : MonoBehaviour {
	private int NUM_ANIMAL_TYPES = 3;
	private void Awake()
	{
		m_animator = GetComponent<Animator>();
		m_animal = GetComponent<AnimalController>();
	}
	// Use this for initialization
	void Start () {
		int layerIndex = m_animator.GetLayerIndex(name);
		m_animator.SetLayerWeight(layerIndex, 1);
	}
	
	// Update is called once per frame
	void Update () {
		m_animator.SetInteger("facingDirection", (int)m_animal.direction);
	}

	private Animator m_animator;
	private AnimalController m_animal;
}
