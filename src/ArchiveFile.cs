using System;
using System.IO;
using System.Security.Cryptography;

namespace Yukihota
{
	// Token: 0x02000029 RID: 41
	public class ArchiveFile
	{
		// Token: 0x06001F46 RID: 8006 RVA: 0x00018E5C File Offset: 0x00017E5C
		public ArchiveFile(string name, ArchiveMode mode)
		{
			this.openMode = mode;
			if (this.openMode == ArchiveMode.Write)
			{
				this.stream = new FileStream(name + ".dat", (FileMode)2, (FileAccess)2);
				return;
			}
			if (this.openMode == ArchiveMode.Read)
			{
				this.stream = new FileStream(name + ".dat", (FileMode)3, (FileAccess)1);
				this.index = new ArchiveIndex();
				this.index.Load(this.stream);
				this.index.Lock();
				return;
			}
			throw new ArgumentException("Invalid CodeType");
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x00018EF0 File Offset: 0x00017EF0
		~ArchiveFile()
		{
			if (this.stream != null)
			{
				this.stream.Close();
			}
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x00018F38 File Offset: 0x00017F38
		public static void VerifyKey(string key)
		{
			ArchiveFile.keyVerified = key == ArchiveFile.keyMaterialA.Substring(0, 6);
			if (!ArchiveFile.keyVerified)
			{
				throw new ArchiveException();
			}
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x00018F60 File Offset: 0x00017F60
		public ArchiveIndex GetIndex()
		{
			if (!ArchiveFile.keyVerified)
			{
				throw new ArchiveException();
			}
			return this.index;
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x00018F78 File Offset: 0x00017F78
		public void SaveIndex()
		{
			if (!ArchiveFile.keyVerified)
			{
				throw new ArchiveException();
			}
			this.index.Save(this.stream);
			this.index.Lock();
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x00018FA4 File Offset: 0x00017FA4
		public void Extract(string name, Stream dataStream)
		{
			if (!ArchiveFile.keyVerified)
			{
				throw new ArchiveException();
			}
			if (dataStream == null)
			{
				throw new ArgumentException("Null Stream");
			}
			if (!dataStream.CanWrite)
			{
				throw new ArgumentException("Stream cannot Write");
			}
			byte[] array = new byte[1024];
			int num;
			int num2;
			this.index.FindEntry(name, out num, out num2);
			if (num == -1)
			{
				throw new FileNotFoundException(name + " is not in the Package");
			}
			MemoryStream memoryStream;
			try
			{
				memoryStream = new MemoryStream();
				this.stream.Position = (long)num;
				int num3 = 0;
				while (num3 + 1024 < num2)
				{
					this.stream.Read(array, 0, 1024);
					memoryStream.Write(array, 0, 1024);
					num3 += 1024;
				}
				this.stream.Read(array, 0, num2 - num3);
				memoryStream.Write(array, 0, num2 - num3);
				memoryStream.Flush();
			}
			catch (Exception)
			{
				throw new IOException("Error Decode " + name);
			}
			try
			{
				byte[] array2 = new byte[24];
				byte[] array3 = new byte[8];
				for (int i = 0; i < 24; i++)
				{
					array2[i] = (byte)ArchiveFile.keyMaterialA[i];
				}
				for (int j = 0; j < 8; j++)
				{
					array3[j] = (byte)ArchiveFile.keyMaterialB[j];
				}
				TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
				memoryStream.Position = 0L;
				dataStream.Position = 0L;
				CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDESCryptoServiceProvider.CreateDecryptor(array2, array3), (CryptoStreamMode)0);
				int num3;
				do
				{
					num3 = cryptoStream.Read(array, 0, 1024);
					dataStream.Write(array, 0, num3);
				}
				while (num3 == 1024);
				dataStream.Flush();
				memoryStream.Close();
				cryptoStream.Close();
			}
			catch (Exception)
			{
				throw new IOException("Error Decode " + name);
			}
			memoryStream = null;
			GC.Collect();
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x000191BC File Offset: 0x000181BC
		public void Extract(string name, string directory)
		{
			if (!ArchiveFile.keyVerified)
			{
				throw new ArchiveException();
			}
			FileStream fileStream = new FileStream(string.Concat(new string[]
			{
				directory,
				"\\",
				name,
				".",
				this.index.GetExtension()
			}), (FileMode)2, (FileAccess)2);
			this.Extract(name, fileStream);
			fileStream.Close();
			GC.Collect();
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x0001922C File Offset: 0x0001822C
		public void ExtractAll(string directory)
		{
			if (!ArchiveFile.keyVerified)
			{
				throw new ArchiveException();
			}
			int entryCount = this.index.GetCount();
			for (int i = 0; i < entryCount; i++)
			{
				string text;
				this.index.FindEntry(i, out text);
				this.Extract(text, directory);
			}
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x0001927C File Offset: 0x0001827C
		public MemoryStream ReadEntry(string name)
		{
			if (!ArchiveFile.keyVerified)
			{
				throw new ArchiveException();
			}
			MemoryStream memoryStream = new MemoryStream();
			this.Extract(name, memoryStream);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x000192B0 File Offset: 0x000182B0
		public void AddEntry(Stream dataStream)
		{
			if (!ArchiveFile.keyVerified)
			{
				throw new ArchiveException();
			}
			if (dataStream == null)
			{
				throw new ArgumentException("Null Stream");
			}
			if (!dataStream.CanRead)
			{
				throw new ArgumentException("Stream cannot Write");
			}
			byte[] array = new byte[1024];
			string text = this.index.GetCurrentName();
			MemoryStream memoryStream;
			CryptoStream cryptoStream;
			int num2;
			try
			{
				memoryStream = new MemoryStream();
				byte[] array2 = new byte[24];
				byte[] array3 = new byte[8];
				for (int i = 0; i < 24; i++)
				{
					array2[i] = (byte)ArchiveFile.keyMaterialA[i];
				}
				for (int j = 0; j < 8; j++)
				{
					array3[j] = (byte)ArchiveFile.keyMaterialB[j];
				}
				TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
				dataStream.Position = 0L;
				cryptoStream = new CryptoStream(memoryStream, tripleDESCryptoServiceProvider.CreateEncryptor(array2, array3), (CryptoStreamMode)1);
				int num;
				do
				{
					num = dataStream.Read(array, 0, 1024);
					cryptoStream.Write(array, 0, num);
				}
				while (num == 1024);
				cryptoStream.Flush();
				cryptoStream.FlushFinalBlock();
				memoryStream.Position = 0L;
				num2 = (int)memoryStream.Length;
			}
			catch (Exception)
			{
				throw new IOException("Error Encode " + text);
			}
			int num3;
			this.index.AppendEntrySize(num2, out num3);
			try
			{
				this.stream.Position = (long)num3;
				int num = 0;
				while (num + 1024 < num2)
				{
					memoryStream.Read(array, 0, 1024);
					this.stream.Write(array, 0, 1024);
					num += 1024;
				}
				memoryStream.Read(array, 0, num2 - num);
				this.stream.Write(array, 0, num2 - num);
				this.stream.Flush();
				cryptoStream.Close();
				memoryStream.Close();
			}
			catch (Exception)
			{
				throw new IOException("Error Encode " + text);
			}
			memoryStream = null;
			cryptoStream = null;
			GC.Collect();
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x000194DC File Offset: 0x000184DC
		public void AddEntry(string directory)
		{
			if (!ArchiveFile.keyVerified)
			{
				throw new ArchiveException();
			}
			string text = this.index.GetCurrentName();
			FileStream fileStream = new FileStream(string.Concat(new string[]
			{
				directory,
				"\\",
				text,
				".",
				this.index.GetExtension()
			}), (FileMode)3, (FileAccess)1);
			this.AddEntry(fileStream);
			fileStream.Close();
			GC.Collect();
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x00019558 File Offset: 0x00018558
		public void BuildArchive(string directory, string extension)
		{
			if (!ArchiveFile.keyVerified)
			{
				throw new ArchiveException();
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(directory);
			FileInfo[] files = directoryInfo.GetFiles("*." + extension);
			int num = files.Length;
			this.index = new ArchiveIndex();
			for (int i = 0; i < num; i++)
			{
				this.index.AddEntry(files[i].Name.Substring(0, files[i].Name.Length - files[i].Extension.Length));
			}
			this.index.SetExtension(extension);
			this.SaveIndex();
			for (int j = 0; j < num; j++)
			{
				this.AddEntry(directory);
			}
			this.SaveIndex();
		}

		// Token: 0x04000118 RID: 280
		private const int BufferSize = 1024;

		// Token: 0x04000119 RID: 281
		private static bool keyVerified = false;

		// Token: 0x0400011A RID: 282
		private ArchiveMode openMode;

		// Token: 0x0400011B RID: 283
		private Stream stream;

		// Token: 0x0400011C RID: 284
		private static readonly string keyMaterialA = "MullenMullenMullenMullen";

		// Token: 0x0400011D RID: 285
		private static readonly string keyMaterialB = "Yukihota";

		// Token: 0x0400011E RID: 286
		private ArchiveIndex index;
	}
}
