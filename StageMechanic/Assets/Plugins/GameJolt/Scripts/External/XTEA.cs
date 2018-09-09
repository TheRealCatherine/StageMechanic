﻿/* 
 * Helper class for XTEA en/decryption of arbitrary data.
 * 
 * Copyright (c) 2017, Henrik Heine
 */

using System;
using System.IO;
using System.Text;

namespace GameJolt.External {
	/// <summary>
	/// Implementation of the "eXtended Tiny Encryption Algorithm".
	/// XTEA is a block cipher designed to correct weaknesses in TEA.
	/// It is a 64-bit block Feistel cipher with a 128-bit key and suggested 64 rounds.
	/// This algorithm is not as secure as AES or TripleDES, but because of it's small footprint it's a good choise for mobile applications.
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public static class XTEA {
		/// <summary>
		/// The recommended number of rounds is 32 and not 64, because each iteration performs two Feistel-cipher rounds.
		/// </summary>
		private const uint Rounds = 32;

		/// <summary>
		/// Encrypts the given data with the provided key. The result is Base64 encoded.
		/// </summary>
		/// <param name="data">The data to encrypt.</param>
		/// <param name="key">The key used for encryption.</param>
		/// <returns></returns>
		public static string Encrypt(string data, string key) {
			var dataBytes = Encoding.Unicode.GetBytes(data);
			var keyBytes = Encoding.Unicode.GetBytes(key);
			var result = Encrypt(dataBytes, keyBytes);
			return Convert.ToBase64String(result);
		}

		/// <summary>
		/// Decrypts the given data with the provided key.
		/// Throws an exception if the length of the data array is not a multiple of 8.
		/// Throws an exception if the decrypted length is longer than the actual array.
		/// </summary>
		/// <param name="data">The encrypted and Base64 encoded data.</param>
		/// <param name="key">The key used for decryption.</param>
		/// <returns></returns>
		public static string Decrypt(string data, string key) {
			var dataBytes = Convert.FromBase64String(data);
			var keyBytes = Encoding.Unicode.GetBytes(key);
			var result = Decrypt(dataBytes, keyBytes);
			return Encoding.Unicode.GetString(result);
		}

		/// <summary>
		/// Encrypts the given data with the provided key.
		/// </summary>
		/// <param name="data">The data to encrypt.</param>
		/// <param name="key">The key used for encryption.</param>
		/// <returns></returns>
		public static byte[] Encrypt(byte[] data, byte[] key) {
			var keyBuffer = CreateKey(key);
			var blockBuffer = new uint[2];
			var result = new byte[NextMultipleOf8(data.Length + 4)];
			var lengthBuffer = BitConverter.GetBytes(data.Length);
			Array.Copy(lengthBuffer, result, lengthBuffer.Length);
			Array.Copy(data, 0, result, lengthBuffer.Length, data.Length);
			using(var stream = new MemoryStream(result)) {
				using(var writer = new BinaryWriter(stream)) {
					for(int i = 0; i < result.Length; i += 8) {
						blockBuffer[0] = BitConverter.ToUInt32(result, i);
						blockBuffer[1] = BitConverter.ToUInt32(result, i + 4);
						Encrypt(Rounds, blockBuffer, keyBuffer);
						writer.Write(blockBuffer[0]);
						writer.Write(blockBuffer[1]);
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Decrypts the given data with the provided key.
		/// Throws an exception if the length of the data array is not a multiple of 8.
		/// Throws an exception if the decrypted length is longer than the actual array.
		/// </summary>
		/// <param name="data">The encrypted data.</param>
		/// <param name="key">The key used for decryption.</param>
		/// <returns></returns>
		public static byte[] Decrypt(byte[] data, byte[] key) {
			if(data.Length % 8 != 0) throw new ArgumentException("Encrypted data length must be a multiple of 8 bytes.");
			var keyBuffer = CreateKey(key);
			var blockBuffer = new uint[2];
			var buffer = new byte[data.Length];
			Array.Copy(data, buffer, data.Length);
			using(var stream = new MemoryStream(buffer)) {
				using(var writer = new BinaryWriter(stream)) {
					for(int i = 0; i < buffer.Length; i += 8) {
						blockBuffer[0] = BitConverter.ToUInt32(buffer, i);
						blockBuffer[1] = BitConverter.ToUInt32(buffer, i + 4);
						Decrypt(Rounds, blockBuffer, keyBuffer);
						writer.Write(blockBuffer[0]);
						writer.Write(blockBuffer[1]);
					}
				}
			}
			// verify valid length
			var length = BitConverter.ToUInt32(buffer, 0);
			if(length > buffer.Length - 4) throw new ArgumentException("Invalid encrypted data");
			var result = new byte[length];
			Array.Copy(buffer, 4, result, 0, length);
			return result;
		}

		private static int NextMultipleOf8(int length) {
			// XTEA is a 64-bit block chiffre, therefore our data must be a multiple of 64 bit
			return (length + 7) / 8 * 8; // this will give us the next multiple of 8
		}

		/// <summary>
		/// Transforms an key of arbitrary length to a 128 bit key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private static uint[] CreateKey(byte[] key) {
			// It might be a better idea to just calculate the MD5 hash of the key: var hash = MD5.Create().ComputeHash(key);
			// But we don't want to depend on the Cryptography namespace, because it would increase the build size for some Unity3d platforms.
			var hash = new byte[16];
			for(int i = 0; i < key.Length; i++) {
				hash[i % 16] = (byte)((31 * hash[i % 16]) ^ key[i]);
			}
			for(int i = key.Length; i < hash.Length; i++) { // if key was too short
				hash[i] = (byte)(17 * i ^ key[i % key.Length]);
			}
			return new[] {
				BitConverter.ToUInt32(hash, 0), BitConverter.ToUInt32(hash, 4),
				BitConverter.ToUInt32(hash, 8), BitConverter.ToUInt32(hash, 12)
			};
		}

		#region Block Operations
		/// <summary>
		/// Performs an inplace encryption of the provided data array.
		/// </summary>
		/// <param name="rounds">The number of encryption rounds, the recommend value is 32.</param>
		/// <param name="v">Data array containing two values.</param>
		/// <param name="key">Key array containing 4 values.</param>
		private static void Encrypt(uint rounds, uint[] v, uint[] key) {
			uint v0 = v[0], v1 = v[1], sum = 0, delta = 0x9E3779B9;
			for(uint i = 0; i < rounds; i++) {
				v0 += (((v1 << 4) ^ (v1 >> 5)) + v1) ^ (sum + key[sum & 3]);
				sum += delta;
				v1 += (((v0 << 4) ^ (v0 >> 5)) + v0) ^ (sum + key[(sum >> 11) & 3]);
			}
			v[0] = v0;
			v[1] = v1;
		}

		/// <summary>
		/// Performs an inplace decryption of the provided data array.
		/// </summary>
		/// <param name="rounds">The number of encryption rounds, the recommend value is 32.</param>
		/// <param name="v">Data array containing two values.</param>
		/// <param name="key">Key array containing 4 values.</param>
		private static void Decrypt(uint rounds, uint[] v, uint[] key) {
			uint v0 = v[0], v1 = v[1], delta = 0x9E3779B9, sum = delta * rounds;
			for(uint i = 0; i < rounds; i++) {
				v1 -= (((v0 << 4) ^ (v0 >> 5)) + v0) ^ (sum + key[(sum >> 11) & 3]);
				sum -= delta;
				v0 -= (((v1 << 4) ^ (v1 >> 5)) + v1) ^ (sum + key[sum & 3]);
			}
			v[0] = v0;
			v[1] = v1;
		}
		#endregion
	}
}
