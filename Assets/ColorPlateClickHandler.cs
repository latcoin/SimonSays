using UnityEngine;
using System.Collections;

public class ColorPlateClickHandler : MonoBehaviour 
{
	SimonLightPlate.eType GetColorPlate(string clickPlaneStr)
	{
		if (this.name == "BlueClickPlane")
		{
			return SimonLightPlate.eType.BLUE;
		}
		else if (this.name == "GreenClickPlane")
		{
			return SimonLightPlate.eType.GREEN;
		}
		else if (this.name == "RedClickPlane")
		{
			return SimonLightPlate.eType.RED;
		}
		else if (this.name == "YellowClickPlane")
		{
			return SimonLightPlate.eType.YELLOW;
		}
		return SimonLightPlate.eType.INVALID_TYPE;
	}
	
	void OnMouseOver()
	{
		// Handle left mouse click.
		if (Input.GetMouseButtonDown(0))
		{		
			this.SendMessageUpwards("OnLeftClickDown", GetColorPlate(this.name), SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnMouseUp()
	{
		if (Input.GetMouseButtonUp(0))
		{
			this.SendMessageUpwards("OnLeftClickUp", GetColorPlate(this.name), SendMessageOptions.DontRequireReceiver);
		}
	}
}

