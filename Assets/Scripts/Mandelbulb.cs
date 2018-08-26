using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Mandelbulb : MonoBehaviour
{
    public ComputeShader mandelbulbCS;
    private ComputeBuffer buffer;
    private static readonly int GROUP_SIZE = 16;
    private static readonly int THREAD_SIZE = 8;
    private static readonly int SIZE = GROUP_SIZE * THREAD_SIZE;
    public Mesh mesh;
    public Material material;
    public Vector3 objectScale = new Vector3(0.1f, 0.2f, 0.5f);
    // GPUインスタンシングのための引数（ComputeBufferへの転送用）
    // インスタンスあたりのインデックス数, インスタンス数, 
    // 開始インデックス位置, ベース頂点位置, インスタンスの開始位置
    uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
    // GPUインスタンシングのための引数バッファ
    ComputeBuffer argsBuffer;

    // Use this for initialization
    void Start()
    {
        buffer = new ComputeBuffer(SIZE * SIZE * SIZE, Marshal.SizeOf(typeof(uint)));
        var data = new uint[SIZE * SIZE * SIZE];
        buffer.SetData(data);

        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
    }

    private void OnDestroy()
    {
        if (buffer != null)
        {
            buffer.Release();
            buffer = null;
        }

        if (argsBuffer != null)
        {
            argsBuffer.Release();
            argsBuffer = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var cs = mandelbulbCS;
        var id = cs.FindKernel("CSMain");
        cs.SetBuffer(id, "_CountMap", buffer);
        cs.SetFloat("_Time", Time.time);
        cs.Dispatch(id, GROUP_SIZE, GROUP_SIZE, GROUP_SIZE);

        uint numIndices = (mesh != null) ? (uint)mesh.GetIndexCount(0) : 0;
        //Debug.Log(numIndices);
        args[0] = numIndices; // メッシュのインデックス数をセット
        args[1] = (uint)(SIZE * SIZE * SIZE); // インスタンス数をセット
        argsBuffer.SetData(args);

        material.SetBuffer("_CountMap", buffer);
        material.SetVector("_ObjectScale", objectScale);

        var bounds = new Bounds(Vector3.zero, new Vector3(32, 32, 32));
        Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer);
        Debug.Log(Time.time);
    }
}
