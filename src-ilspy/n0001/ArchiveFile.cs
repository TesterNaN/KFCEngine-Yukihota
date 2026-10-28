using System;
using System.IO;
using System.Security.Cryptography;

namespace n0001
{
	public class ArchiveFile
	{
		private const int n0523 = 1024;

		private static bool keyVerified = false;

		private ArchiveMode openMode;

		private Stream stream;

		private static readonly string keyMaterialA = "MullenMullenMullenMullen";

		private static readonly string keyMaterialB = "Yukihota";

		private ArchiveIndex index;

		public ArchiveFile(string n0494, ArchiveMode n0432)
		{
			openMode = n0432;
			if (openMode == ArchiveMode.n0496)
			{
				stream = new FileStream(n0494 + ".dat", FileMode.Create, FileAccess.Write);
				return;
			}
			if (openMode == ArchiveMode.n0151)
			{
				stream = new FileStream(n0494 + ".dat", FileMode.Open, FileAccess.Read);
				index = new ArchiveIndex();
				index.n0500(stream);
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

		public static void n0129(string n0502)
		{
			keyVerified = n0502 == keyMaterialA.Substring(0, 6);
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
			index.n0508(stream);
			index.Lock();
		}

		public void Extract(string n0494, Stream n0509)
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			if (n0509 == null)
			{
				throw new ArgumentException("Null Stream");
			}
			if (!n0509.CanWrite)
			{
				throw new ArgumentException("Stream cannot Write");
			}
			byte[] buffer = new byte[1024];
			int n536;
			int n537;
			index.FindEntry(n0494, out n536, out n537);
			if (n536 == -1)
			{
				throw new FileNotFoundException(n0494 + " is not in the Package");
			}
			MemoryStream memoryStream;
			try
			{
				memoryStream = new MemoryStream();
				stream.Position = n536;
				int i;
				for (i = 0; i + 1024 < n537; i += 1024)
				{
					stream.Read(buffer, 0, 1024);
					memoryStream.Write(buffer, 0, 1024);
				}
				stream.Read(buffer, 0, n537 - i);
				memoryStream.Write(buffer, 0, n537 - i);
				memoryStream.Flush();
			}
			catch (Exception)
			{
				throw new IOException("Error Decode " + n0494);
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
				n0509.Position = 0L;
				CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDESCryptoServiceProvider.CreateDecryptor(array, array2), CryptoStreamMode.Read);
				int num;
				do
				{
					num = cryptoStream.Read(buffer, 0, 1024);
					n0509.Write(buffer, 0, num);
				}
				while (num == 1024);
				n0509.Flush();
				memoryStream.Close();
				cryptoStream.Close();
			}
			catch (Exception)
			{
				throw new IOException("Error Decode " + n0494);
			}
			memoryStream = null;
			GC.Collect();
		}

		public void Extract(string n0494, string n0512)
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			FileStream fileStream = new FileStream(n0512 + "\\" + n0494 + "." + index.n0513(), FileMode.Create, FileAccess.Write);
			Extract(n0494, fileStream);
			fileStream.Close();
			GC.Collect();
		}

		public void n0514(string n0512)
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			int num = index.n0515();
			for (int i = 0; i < num; i++)
			{
				string n536;
				index.FindEntry(i, out n536);
				Extract(n536, n0512);
			}
		}

		public MemoryStream ReadEntry(string n0494)
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			MemoryStream memoryStream = new MemoryStream();
			Extract(n0494, memoryStream);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		public void AddEntry(Stream n0509)
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			if (n0509 == null)
			{
				throw new ArgumentException("Null Stream");
			}
			if (!n0509.CanRead)
			{
				throw new ArgumentException("Stream cannot Write");
			}
			byte[] buffer = new byte[1024];
			string text = index.n0517();
			int num2;
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
				n0509.Position = 0L;
				cryptoStream = new CryptoStream(memoryStream, tripleDESCryptoServiceProvider.CreateEncryptor(array, array2), CryptoStreamMode.Write);
				int num;
				do
				{
					num = n0509.Read(buffer, 0, 1024);
					cryptoStream.Write(buffer, 0, num);
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
			int n536;
			index.AppendEntrySize(num2, out n536);
			try
			{
				stream.Position = n536;
				int k;
				for (k = 0; k + 1024 < num2; k += 1024)
				{
					memoryStream.Read(buffer, 0, 1024);
					stream.Write(buffer, 0, 1024);
				}
				memoryStream.Read(buffer, 0, num2 - k);
				stream.Write(buffer, 0, num2 - k);
				stream.Flush();
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

		public void AddEntry(string n0512)
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			string text = index.n0517();
			FileStream fileStream = new FileStream(n0512 + "\\" + text + "." + index.n0513(), FileMode.Open, FileAccess.Read);
			AddEntry(fileStream);
			fileStream.Close();
			GC.Collect();
		}

		public void n0519(string n0512, string n0520)
		{
			if (!keyVerified)
			{
				throw new ArchiveException();
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(n0512);
			FileInfo[] files = directoryInfo.GetFiles("*." + n0520);
			int num = files.Length;
			index = new ArchiveIndex();
			for (int i = 0; i < num; i++)
			{
				index.n0521(files[i].Name.Substring(0, files[i].Name.Length - files[i].Extension.Length));
			}
			index.n0522(n0520);
			SaveIndex();
			for (int j = 0; j < num; j++)
			{
				AddEntry(n0512);
			}
			SaveIndex();
		}
	}
}
