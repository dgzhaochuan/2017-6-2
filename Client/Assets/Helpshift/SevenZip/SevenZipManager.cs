using UnityEngine;
using System.Collections;
using SevenZip.Compression.LZMA;
using System.IO;
using System;

public class SevenZipManager
{
    //压缩文件
	public static void CompressFileLZMA(string inFile, string outFile, Action callBack)
	{
		Encoder coder = new SevenZip.Compression.LZMA.Encoder();
		DGFileUtil.CreateDirectoryWhenNotExists(outFile);

		FileStream input = new FileStream(inFile, FileMode.Open);
		FileStream output = new FileStream(outFile, FileMode.Create);
		
		// Write the encoder properties
		coder.WriteCoderProperties(output);
		
		// Write the decompressed file size.
		output.Write(BitConverter.GetBytes(input.Length), 0, 8);
		
		// Encode the file.
		coder.Code(input, output, input.Length, -1, null);
		output.Flush();
		output.Close();
        input.Close();
        output.Dispose();
        input.Dispose();

        if(callBack != null)
        {
            callBack();
        }
    }
    //解压缩文件
    public static void DecompressFileLZMA(string inFile, string outFile, Action callBack=null)
	{
		SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
        DGFileUtil.CreateDirectoryWhenNotExists(outFile);

        FileStream input = new FileStream(inFile, FileMode.Open);
		FileStream output = new FileStream(outFile, FileMode.Create);
		
		// Read the decoder properties
		byte[] properties = new byte[5];
		input.Read(properties, 0, 5);
		
		// Read in the decompress file size.
		byte [] fileLengthBytes = new byte[8];
		input.Read(fileLengthBytes, 0, 8);
		long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);
		
		// Decompress the file.
		coder.SetDecoderProperties(properties);
		coder.Code(input, output, input.Length, fileLength, null);
		output.Flush();
		output.Close();
		input.Close();
        output.Dispose();
        input.Dispose();
        if (callBack != null) callBack();
    }
    //解压缩文件
    public static void DecompressFileLZMA(byte[] inFile, string outFile, Action callBack)
	{
		DGFileUtil.CreateDirectoryWhenNotExists(outFile);

		SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
		MemoryStream input = new MemoryStream(inFile);
		FileStream output = new FileStream(outFile, FileMode.Create);
		
		// Read the decoder properties
		byte[] properties = new byte[5];
		input.Read(properties, 0, 5);
		
		// Read in the decompress file size.
		byte [] fileLengthBytes = new byte[8];
		input.Read(fileLengthBytes, 0, 8);
		long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

		// Decompress the file.
		coder.SetDecoderProperties(properties);
		coder.Code(input, output, input.Length, fileLength, null);
		output.Flush();
		output.Close();
		input.Close();
        output.Dispose();
        input.Dispose();
        if (callBack != null) callBack();
    }
}
