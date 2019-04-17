 using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/PixelBoy")]
public class PixelBoy : MonoBehaviour
{
	public Camera cam;
	public int w = 720;

	private int h;
	private int resolutionLimit;
	private bool isDecreasing;
	private bool isIncreasing;

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
			{
				w -= 2;
			}
			else
			{
				PauseManager.GameIsOver = true;
			}
		}
		if (isIncreasing)
		{
			if (w <= resolutionLimit)
			{
				w += 2;
			}
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

	public void DecreaseResolution(int limit)
	{
		isDecreasing = true;
		isIncreasing = false;
		resolutionLimit = limit;
	}

	public void IncreaseResolution(int limit)
	{
		isIncreasing = true;
		isDecreasing = false;
		resolutionLimit = limit;
	}
}