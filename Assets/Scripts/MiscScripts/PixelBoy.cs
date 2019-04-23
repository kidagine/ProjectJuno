 using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/PixelBoy")]
public class PixelBoy : MonoBehaviour
{
	public Camera cam;
	public LayerMask excludeLayers = 0;
	public int w = 720;

	private GameObject tmpCam = null;
	private Camera _camera;
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

		Camera cam = null;
		if (excludeLayers.value != 0) cam = GetTmpCam();

		if (cam && excludeLayers.value != 0)
		{
			cam.targetTexture = destination;
			cam.cullingMask = excludeLayers;
			cam.Render();
		}
	}

	Camera GetTmpCam()
	{
		if (tmpCam == null)
		{
			if (_camera == null) _camera = GetComponent<Camera>();

			string name = "_" + _camera.name + "_GrayScaleTmpCam";
			GameObject go = GameObject.Find(name);

			if (null == go) // couldn't find, recreate
			{
				tmpCam = new GameObject(name, typeof(Camera));
			}
			else
			{
				tmpCam = go;
			}
		}

		tmpCam.hideFlags = HideFlags.DontSave;
		tmpCam.transform.position = _camera.transform.position;
		tmpCam.transform.rotation = _camera.transform.rotation;
		tmpCam.transform.localScale = _camera.transform.localScale;
		tmpCam.GetComponent<Camera>().CopyFrom(_camera);

		tmpCam.GetComponent<Camera>().enabled = false;
		tmpCam.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
		tmpCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;

		return tmpCam.GetComponent<Camera>();
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