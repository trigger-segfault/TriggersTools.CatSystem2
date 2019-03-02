﻿using System;
using System.Text;
using Newtonsoft.Json;
using TriggersTools.CatSystem2.Native;
using TriggersTools.Resources;
using TriggersTools.Resources.Enumerations;
using TriggersTools.SharpUtils.Text;

namespace TriggersTools.CatSystem2 {
	/// <summary>
	///  A set of an executable file's KEY_CODE, V_CODE, and V_CODE2 resources.
	/// </summary>
	public sealed partial class VCodes {
		#region Constants

		public const string KeyCodeType = "KEY_CODE";
		public const string KeyCodeName = "KEY";
		public const string VCodeType = "V_CODE";
		public const string VCodeName = "DATA";
		public const string VCode2Type = "V_CODE2";
		public const string VCode2Name = "DATA";

		private static readonly byte[] Cs2KeyCode = { 0xBA, 0xA4, 0xA3, 0xA9, 0xA0, 0xA4, 0xA1, 0xA1 };
		private static readonly byte[] Cs2VCodes = {
			0x85, 0x83, 0xD7, 0x8A, 0x8B, 0x88, 0x04, 0xCB, 0xC3, 0x78, 0xCF, 0xD0, 0xB1, 0xA4, 0xE5, 0x9A
		};

		#endregion

		#region Fields

		[JsonIgnore]
		private readonly ResourceInfo resInfo;
		[JsonIgnore]
		private readonly GenericResource keyCodeRes;
		[JsonIgnore]
		private readonly GenericResource vcodeRes;
		[JsonIgnore]
		private readonly GenericResource vcode2Res;

		#endregion

		#region Constructors

		public VCodes() {
			resInfo = new ResourceInfo();
			ushort language = (ushort) LanguageIdentifier.Japanese;
			resInfo.Add(keyCodeRes = new GenericResource(KeyCodeType, KeyCodeName, language));
			resInfo.Add(vcodeRes = new GenericResource(VCodeType, VCodeName, language));
			resInfo.Add(vcode2Res = new GenericResource(VCode2Type, VCode2Name, language));
			keyCodeRes.Data = Cs2KeyCode;
			vcodeRes.Data = Cs2VCodes;
			vcode2Res.Data = Cs2VCodes;
		}
		private VCodes(string exeFile) {
			using (resInfo = new ResourceInfo(exeFile, false)) {
				IntPtr hModule = resInfo.ModuleHandle;
				ushort language = (ushort) LanguageIdentifier.Japanese;
				resInfo.Add(keyCodeRes = new GenericResource(hModule, KeyCodeType, KeyCodeName, language));
				resInfo.Add(vcodeRes = new GenericResource(hModule, VCodeType, VCodeName, language));
				resInfo.Add(vcode2Res = new GenericResource(hModule, VCode2Type, VCode2Name, language));
			}
		}

		#endregion

		#region Properties

		/// <summary>
		///  Gets or sets the key code used to encrypt the V_CODEs.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		///  value is null.
		/// </exception>
		[JsonProperty("key_code")]
		public byte[] KeyCode {
			get {
				byte[] key = new byte[keyCodeRes.Length];
				Array.Copy(keyCodeRes.Data, key, keyCodeRes.Length);
				return key;
			}
			set {
				if (value == null)
					throw new ArgumentNullException(nameof(value));
				// Make sure we have backups of the V_CODEs to re-encode afterwards
				string vcodeBackup = VCode;
				string vcode2Backup = VCode2;
				byte[] newKey = new byte[value.Length];
				Array.Copy(value, newKey, value.Length);
				keyCodeRes.Data = newKey;
				// Update VCodes to match new key
				VCode = vcodeBackup;
				VCode2 = vcode2Backup;
			}
		}
		/// <summary>
		///  Gets or sets the V_CODE.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		///  value is null.
		/// </exception>
		[JsonProperty("v_code")]
		public string VCode {
			get => Decrypt(vcodeRes.Data);
			set {
				if (value == null)
					throw new ArgumentNullException(nameof(value));
				vcodeRes.Data = Encrypt(value);
			}
		}
		/// <summary>
		///  Gets or sets the V_CODE2.
		/// </summary>
		/// 
		/// <exception cref="ArgumentNullException">
		///  value is null.
		/// </exception>
		[JsonProperty("v_code2")]
		public string VCode2 {
			get => Decrypt(vcode2Res.Data);
			set {
				if (value == null)
					throw new ArgumentNullException(nameof(value));
				vcode2Res.Data = Encrypt(value);
			}
		}

		#endregion

		#region Load/Save

		public static VCodes Load(string exeFile) {
			if (exeFile == null)
				throw new ArgumentNullException(nameof(exeFile));
			if (string.IsNullOrWhiteSpace(exeFile))
				throw new ArgumentException($"{nameof(exeFile)} is empty or whitespace!", nameof(exeFile));
			return new VCodes(exeFile);
		}
		public void Save(string exeFile) {
			if (exeFile == null)
				throw new ArgumentNullException(nameof(exeFile));
			if (string.IsNullOrWhiteSpace(exeFile))
				throw new ArgumentException($"{nameof(exeFile)} is empty or whitespace!", nameof(exeFile));
			resInfo.Save(exeFile);
		}

		#endregion

		#region Encrypt/Decrypt

		private byte[] GetKey() {
			byte[] key = new byte[keyCodeRes.Length];
			Array.Copy(keyCodeRes.Data, key, key.Length);
			for (int i = 0; i < key.Length; i++)
				key[i] ^= 0xCD;
			return key;
		}
		private byte[] Encrypt(string vcode) {
			// Get the encipher key
			byte[] key = GetKey();
			// Get the byte length of the V_CODE string
			int byteLength = CatUtils.ShiftJIS.GetByteCount(vcode);
			// Create a V_CODE buffer with a length that is a multiple of 8
			byte[] vcodeData = new byte[(byteLength + 7) & ~7];
			// Copy the code to the buffer
			CatUtils.ShiftJIS.GetBytes(vcode, 0, vcode.Length, vcodeData, 0);
			// Encrypt the V_CODE buffer
			Asmodean.EncryptVCode(key, key.Length, vcodeData, vcodeData.Length);
			return vcodeData;

			//byte[] vcodeBytes = CatUtils.ShiftJIS.GetBytes(vcode.Code);
			//if (vcodeBytes.Length >= vcode.Length)
			//	throw new InvalidOperationException($"VCode is too long, must fit within {nameof(VCode)}." +
			//										$"{nameof(VCode.Length)} {vcode.Length} bytes!");
			// Create a vcode buffer with a length that is a multiple of 8
			//byte[] vcodeBuffer = new byte[(vcode.Code.Length + 7) & ~7];
			//Array.Copy(vcodeBytes, vcodeBuffer, vcodeBytes.Length);
			//byte[] vcodeData = new byte[(vcode.Code.Length + 7) & ~7];
			//Array.Copy(vcodeBytes, vcodeBuffer, vcodeBytes.Length);

			// Create a vcode buffer with a length that is the destination length
			//byte[] vcodeData2 = new byte[vcode.Length];
			//Array.Copy(vcodeBuffer, vcodeData2, vcodeData2.Length);
		}
		private string Decrypt(byte[] vcodeData) {
			// Get the decipher key
			byte[] key = GetKey();
			// Create a V_CODE buffer with a length that is a multiple of 8
			byte[] vcodeBuffer = new byte[(vcodeData.Length + 7) & ~7];
			// Copy the V_CODE to the new buffer, so we don't override the original resource
			Array.Copy(vcodeData, vcodeBuffer, vcodeData.Length);
			// Decrypt the V_CODE buffer
			Asmodean.DecryptVCode(key, key.Length, vcodeBuffer, vcodeData.Length);
			// Null-terminate the V_CODE character string
			return vcodeBuffer.ToNullTerminatedString(CatUtils.ShiftJIS);
		}

		#endregion
	}
}