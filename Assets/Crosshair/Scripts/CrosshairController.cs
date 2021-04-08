using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour {

	[Tooltip("RectTransform of the crosshair ui holder")]
	public RectTransform crosshair;		// Crosshair UI Panel holder
	[Tooltip("Power to be added to sizeDelta while making the crosshair bigger")]
	public float spreadPower = 2; 	// Power the crosshair will spread after clicking a mouse button
	[Tooltip("Power to be substracted from sizeDelta while making the crosshair smaller. Must be NEGATIVE value")]
	public float spreadDecreasePower = -1; 	// Power the crosshair will decrease the spread back to normal, in NEGATIVE value
	[Tooltip("Max width/height of sizeDelta to be clamped while spreading the crosshair")]
	public float maxSpread = 50;		// Spread size that crosshair will be clamped to

	[Tooltip("KeyCode that needs to be pressed to add spread to the crosshair")]
	public KeyCode spreadButton;		// KeyCode for adding spread

	float defaultSpread; 		// default sizeDelta of crosshair rect transform

	void Start()
	{
		// Get the default size
		// We need only X as Y will be the same
		defaultSpread = crosshair.sizeDelta.x;
	}

	void Update()
	{
		// Input check
		if(Input.GetKey(spreadButton))
		{
			// Add spread
			SetSpread(spreadPower);
		}
		else
		{
			// Decrease spread
			SetSpread(spreadDecreasePower);
		}

		// Clamp our crosshair spread so it wont be more than maxSpread
		crosshair.sizeDelta = new Vector2(Mathf.Clamp(crosshair.sizeDelta.x, defaultSpread, maxSpread), Mathf.Clamp(crosshair.sizeDelta.y, defaultSpread, maxSpread));
	}

	void SetSpread(float value)
	{
		// Get the current size of crosshair
		Vector2 curSpread = crosshair.sizeDelta;
		// Add spread value to both axis
		curSpread.x += value;
		curSpread.y += value;
		// Set the final spread
		crosshair.sizeDelta = curSpread;
	}
}
