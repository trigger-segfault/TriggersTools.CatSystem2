using System;
using System.Text;
using Newtonsoft.Json;
using TriggersTools.SharpUtils.Text;
using TriggersTools.Windows.Resources;
using TriggersTools.CatSystem2.Utils;

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

		/// <summary>
		///  The resource container. Only needed if you plan on overwritting the V_CODEs.
		/// </summary>
		[JsonIgnore]
		private readonly ResourceInfo resInfo;
		/// <summary>
		///  The resource data for KEY_CODE.
		/// </summary>
		[JsonIgnore]
		private readonly GenericResource keyCodeRes;
		/// <summary>
		///  The resource data for V_CODE.
		/// </summary>
		[JsonIgnore]
		private readonly GenericResource vcodeRes;
		/// <summary>
		///  The resource data for V_CODE2.
		/// </summary>
		[JsonIgnore]
		private readonly GenericResource vcode2Res;
		/// <summary>
		///  The blowfish cipher. We keep this around because setting the key is very expensive.
		/// </summary>
		[JsonIgnore]
		private Blowfish blowfish;

		#endregion

		#region Constructors

		public VCodes() {
			resInfo = new ResourceInfo();
			ushort language = CatUtils.LanguageIdentifier;
			resInfo.Add(keyCodeRes = new GenericResource(KeyCodeType, KeyCodeName, language));
			resInfo.Add(vcodeRes = new GenericResource(VCodeType, VCodeName, language));
			resInfo.Add(vcode2Res = new GenericResource(VCode2Type, VCode2Name, language));
			keyCodeRes.Data = Cs2KeyCode;
			vcodeRes.Data = Cs2VCodes;
			vcode2Res.Data = Cs2VCodes;
		}
		private VCodes(string exeFile) {
			// We're using this here so that we can dispose of the loaded module after we're done.
			// We'll still keep the loaded resources.
			using (resInfo = new ResourceInfo(exeFile, false)) {
				IntPtr hModule = resInfo.ModuleHandle;
				ushort language = CatUtils.LanguageIdentifier;
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

				// Update the blowfish cipher with the new key
				InitializeBlowfish();

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

		/// <summary>
		///  Generates the blowfish key from <see cref="KeyCode"/>.
		/// </summary>
		/// <returns>The blowfish key.</returns>
		private byte[] GetKey() {
			byte[] key = new byte[keyCodeRes.Length];
			Array.Copy(keyCodeRes.Data, key, key.Length);
			for (int i = 0; i < key.Length; i++)
				key[i] ^= 0xCD;
			return key;
		}
		/// <summary>
		///  Initializes a new instance of the blowfish encryption algorithm.
		/// </summary>
		private void InitializeBlowfish() {
			blowfish = CatDebug.NewBlowfish(GetKey());
		}
		/// <summary>
		///  Encrypts the V_CODE or V_CODE 2 into binary data.
		/// </summary>
		/// <param name="vcode">The V_CODE to encrypt.</param>
		/// <returns>The encrypted V_CODE data.</returns>
		private byte[] Encrypt(string vcode) {
			// Get the encipher key
			// Get the byte length of the V_CODE string
			int byteLength = CatUtils.ShiftJIS.GetByteCount(vcode);
			// Create a V_CODE buffer with a length that is a multiple of 8
			byte[] vcodeBuffer = new byte[(byteLength + 7) & ~7];
			// Copy the code to the buffer
			CatUtils.ShiftJIS.GetBytes(vcode, 0, vcode.Length, vcodeBuffer, 0);
			// Encrypt the V_CODE buffer
			if (blowfish == null)
				InitializeBlowfish();
			blowfish.Encrypt(vcodeBuffer);
			return vcodeBuffer;
		}
		/// <summary>
		///  Decrypts the V_CODE or V_CODE2 data into a human-readable string.
		/// </summary>
		/// <param name="vcodeData">The V_CODE data to decrypt.</param>
		/// <returns>The human-readable V_CODE string.</returns>
		private string Decrypt(byte[] vcodeData) {
			// Get the decipher key
			byte[] key = GetKey();
			// Create a V_CODE buffer with a length that is a multiple of 8
			byte[] vcodeBuffer = new byte[(vcodeData.Length + 7) & ~7];
			// Copy the V_CODE to the new buffer, so we don't override the original resource
			Array.Copy(vcodeData, vcodeBuffer, vcodeData.Length);
			// Decrypt the V_CODE buffer
			if (blowfish == null)
				InitializeBlowfish();
			blowfish.Decrypt(vcodeBuffer);
			// Null-terminate the V_CODE character string
			return vcodeBuffer.ToNullTerminatedString(CatUtils.ShiftJIS);
		}

		#endregion

		#region ToString Override

		/// <summary>
		///  Gets the string representation of the V_CODEs.
		/// </summary>
		/// <returns>The string representation of the V_CODEs.</returns>
		public override string ToString() => $"V_CODE=\"{VCode}\" V_CODE2={VCode2}";

		#endregion
	}
}
