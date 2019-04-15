 using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/PixelBoy")]
public class PixelBoy : MonoBehaviour
{
	public Camera cam;
	public int w = 720;

	private int h;

	protected void Start()
	{
		cam = GetComponent<Camera>();

		if (!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return;
		}
	}

	void Update()
	{
		float ratio = ((float)cam.pixelHeight / (float)cam.pixelWidth);
		h = Mathf.RoundToInt(w * ratio);
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		source.filterMode = FilterMode.Point;
		RenderTexture buffer = RenderTexture.GetTemporary(w, h, -1);
		buffer.filterMode = FilterMode.Point;
		Graphics.Blit(source, buffer);
		Graphics.Blit(buffer, destination);
		RenderTexture.ReleaseTemporary(buffer);
	}

	public void DecreaseResolution(float limit)
	{

	}

	public void IncreaseResolution(float limit)
	{
		while (w <= limit)
			w++;
	}
}