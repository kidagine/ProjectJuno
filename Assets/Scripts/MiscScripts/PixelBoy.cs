 using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/PixelBoy")]
public class PixelBoy : MonoBehaviour
{
	public Camera cam;
	public GameObject gameOver;
	public int w = 720;

	private int h;
	private int resolutionLimit;
	private bool isDecreasing;

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
		if (isDecreasing)
		{
			if (w >= resolutionLimit)
				w -= 2;
			else
				gameOver.SetActive(true);
		}
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

	public void SetResolutionZero()
	{
	}

	public void DecreaseResolution(int limit)
	{
		isDecreasing = true;
		resolutionLimit = limit;
	}

	public void IncreaseResolution(float limit)
	{
		//TODO
	}
}