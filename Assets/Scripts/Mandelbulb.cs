using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Mandelbulb : MonoBehaviour
{
    public ComputeShader mandelbulbCS;
    private ComputeBuffer buffer;
    private readonly int SIZE = 64;

    // Use this for initialization
    void Start()
    {
        buffer = new ComputeBuffer(SIZE * SIZE * SIZE, Marshal.SizeOf(typeof(uint)));
        var data = new uint[SIZE * SIZE * SIZE];
        buffer.SetData(data);
    }

    private void OnDestroy()
    {
        if (buffer != null)
        {
            buffer.Release();
            buffer = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var cs = mandelbulbCS;
        var id = cs.FindKernel("CSMain");
        //Texture3D tex = new Texture3D(SIZE, SIZE, SIZE, TextureFormat.ARGB32, true);
        //cs.SetTexture(id, "Result", tex);
        cs.SetBuffer(id, "countMap", buffer);
        cs.Dispatch(id, 1, 1, 1);
    }
}
