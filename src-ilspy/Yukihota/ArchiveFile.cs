using System;
using System.IO;
using System.Security.Cryptography;

namespace Yukihota
{
	public class ArchiveFile
	{
		private const int BufferSize = 1024;

		private static bool keyVerified = false;

		private ArchiveMode openMode;

		private Stream stream;

		private static readonly string keyMaterialA = "MullenMullenMullenMullen";

		private static readonly string keyMaterialB = "Yukihota";

		private ArchiveIndex index;

		public ArchiveFile(string name, ArchiveMode type)
		{
			openMode = type;
			if (openMode == ArchiveMode.Write)
			{
				stream = new FileStream(name + ".dat", FileMode.Create, FileAccess.Write);
				return;
			}
			if (openMode == ArchiveMode.Read)
			{
				stream = new FileStream(name + ".dat", FileMode.Open, FileAccess.Read);
				index = new ArchiveIndex();
				index.Load(stream);
				index.Lock();
				return;
			}
			throw new ArgumentException("Invalid CodeType");
		}

		~ArchiveFile()
		{
			if (stream != null)
			{
				stream.Close();
			}
		}

		public static void VerifyKey(string key)
		{
			keyVerified = key == keyMaterialA.Substring(0, 6);
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
		}

		public ArchiveIndex GetIndex()
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			return index;
		}

		public void SaveIndex()
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			index.Save(stream);
			index.Lock();
		}

		public void Extract(string name, Stream dataStream)
		{
			if (!keyVerified)
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
			byte[] buffer = new byte[1024];
			int offset;
			int length;
			index.FindEntry(name, out offset, out length);
			if (offset == -1)
			{
				throw new FileNotFoundException(name + " is not in the Package");
			}
			MemoryStream memoryStream;
			try
			{
				memoryStream = new MemoryStream();
				stream.Position = offset;
				int i;
				for (i = 0; i + 1024 < length; i += 1024)
				{
					stream.Read(buffer, 0, 1024);
					memoryStream.Write(buffer, 0, 1024);
				}
				stream.Read(buffer, 0, length - i);
				memoryStream.Write(buffer, 0, length - i);
				memoryStream.Flush();
			}
			catch (Exception)
			{
				throw new IOException("Error Decode " + name);
			}
			try
			{
				byte[] array = new byte[24];
				byte[] array2 = new byte[8];
				for (int j = 0; j < 24; j++)
				{
					array[j] = (byte)keyMaterialA[j];
				}
				for (int k = 0; k < 8; k++)
				{
					array2[k] = (byte)keyMaterialB[k];
				}
				TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
				memoryStream.Position = 0L;
				dataStream.Position = 0L;
				CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDESCryptoServiceProvider.CreateDecryptor(array, array2), CryptoStreamMode.Read);
				int bytesRead;
				do
				{
					bytesRead = cryptoStream.Read(buffer, 0, 1024);
					dataStream.Write(buffer, 0, bytesRead);
				}
				while (bytesRead == 1024);
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

		public void Extract(string name, string directory)
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			FileStream fileStream = new FileStream(directory + "\\" + name + "." + index.GetExtension(), FileMode.Create, FileAccess.Write);
			Extract(name, fileStream);
			fileStream.Close();
			GC.Collect();
		}

		public void ExtractAll(string directory)
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			int entryCount = index.GetCount();
			for (int i = 0; i < entryCount; i++)
			{
				string offset;
				index.FindEntry(i, out offset);
				Extract(offset, directory);
			}
		}

		public MemoryStream ReadEntry(string name)
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			MemoryStream memoryStream = new MemoryStream();
			Extract(name, memoryStream);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		public void AddEntry(Stream dataStream)
		{
			if (!keyVerified)
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
			byte[] buffer = new byte[1024];
			string entryName = index.GetCurrentName();
			int encodedLength;
			MemoryStream memoryStream;
			CryptoStream cryptoStream;
			try
			{
				memoryStream = new MemoryStream();
				byte[] array = new byte[24];
				byte[] array2 = new byte[8];
				for (int i = 0; i < 24; i++)
				{
					array[i] = (byte)keyMaterialA[i];
				}
				for (int j = 0; j < 8; j++)
				{
					array2[j] = (byte)keyMaterialB[j];
				}
				TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
				dataStream.Position = 0L;
				cryptoStream = new CryptoStream(memoryStream, tripleDESCryptoServiceProvider.CreateEncryptor(array, array2), CryptoStreamMode.Write);
				int bytesRead;
				do
				{
					bytesRead = dataStream.Read(buffer, 0, 1024);
					cryptoStream.Write(buffer, 0, bytesRead);
				}
				while (bytesRead == 1024);
				cryptoStream.Flush();
				cryptoStream.FlushFinalBlock();
				memoryStream.Position = 0L;
				encodedLength = (int)memoryStream.Length;
			}
			catch (Exception)
			{
				throw new IOException("Error Encode " + entryName);
			}
			int offset;
			index.AppendEntrySize(encodedLength, out offset);
			try
			{
				stream.Position = offset;
				int k;
				for (k = 0; k + 1024 < encodedLength; k += 1024)
				{
					memoryStream.Read(buffer, 0, 1024);
					stream.Write(buffer, 0, 1024);
				}
				memoryStream.Read(buffer, 0, encodedLength - k);
				stream.Write(buffer, 0, encodedLength - k);
				stream.Flush();
				cryptoStream.Close();
				memoryStream.Close();
			}
			catch (Exception)
			{
				throw new IOException("Error Encode " + entryName);
			}
			memoryStream = null;
			cryptoStream = null;
			GC.Collect();
		}

		public void AddEntry(string directory)
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			string entryName = index.GetCurrentName();
			FileStream fileStream = new FileStream(directory + "\\" + entryName + "." + index.GetExtension(), FileMode.Open, FileAccess.Read);
			AddEntry(fileStream);
			fileStream.Close();
			GC.Collect();
		}

		public void BuildArchive(string directory, string extension)
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(directory);
			FileInfo[] files = directoryInfo.GetFiles("*." + extension);
			int fileCount = files.Length;
			index = new ArchiveIndex();
			for (int i = 0; i < fileCount; i++)
			{
				index.AddEntry(files[i].Name.Substring(0, files[i].Name.Length - files[i].Extension.Length));
			}
			index.SetExtension(extension);
			SaveIndex();
			for (int j = 0; j < fileCount; j++)
			{
				AddEntry(directory);
			}
			SaveIndex();
		}
	}
}
