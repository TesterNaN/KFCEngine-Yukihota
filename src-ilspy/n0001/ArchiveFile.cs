using System;
using System.IO;
using System.Security.Cryptography;

namespace n0001
{
	public class ArchiveFile
	{
		private const int n0523 = 1024;

		private static bool n0503 = false;

		private ArchiveMode n0495;

		private Stream n0497;

		private static readonly string n0504 = "MullenMullenMullenMullen";

		private static readonly string n0511 = "Yukihota";

		private ArchiveIndex n0498;

		public ArchiveFile(string n0494, ArchiveMode n0432)
		{
			n0495 = n0432;
			if (n0495 == ArchiveMode.n0496)
			{
				n0497 = new FileStream(n0494 + ".dat", FileMode.Create, FileAccess.Write);
				return;
			}
			if (n0495 == ArchiveMode.n0151)
			{
				n0497 = new FileStream(n0494 + ".dat", FileMode.Open, FileAccess.Read);
				n0498 = new ArchiveIndex();
				n0498.n0500(n0497);
				n0498.n0501();
				return;
			}
			throw new ArgumentException("Invalid CodeType");
		}

		~ArchiveFile()
		{
			if (n0497 != null)
			{
				n0497.Close();
			}
		}

		public static void n0129(string n0502)
		{
			n0503 = n0502 == n0504.Substring(0, 6);
			if (!n0503)
			{
				throw new ArchiveException();
			}
		}

		public ArchiveIndex n0506()
		{
			if (!n0503)
			{
				throw new ArchiveException();
			}
			return n0498;
		}

		public void n0507()
		{
			if (!n0503)
			{
				throw new ArchiveException();
			}
			n0498.n0508(n0497);
			n0498.n0501();
		}

		public void n0154(string n0494, Stream n0509)
		{
			if (!n0503)
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
			n0498.n0510(n0494, out n536, out n537);
			if (n536 == -1)
			{
				throw new FileNotFoundException(n0494 + " is not in the Package");
			}
			MemoryStream memoryStream;
			try
			{
				memoryStream = new MemoryStream();
				n0497.Position = n536;
				int i;
				for (i = 0; i + 1024 < n537; i += 1024)
				{
					n0497.Read(buffer, 0, 1024);
					memoryStream.Write(buffer, 0, 1024);
				}
				n0497.Read(buffer, 0, n537 - i);
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
					array[j] = (byte)n0504[j];
				}
				for (int k = 0; k < 8; k++)
				{
					array2[k] = (byte)n0511[k];
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

		public void n0154(string n0494, string n0512)
		{
			if (!n0503)
			{
				throw new ArchiveException();
			}
			FileStream fileStream = new FileStream(n0512 + "\\" + n0494 + "." + n0498.n0513(), FileMode.Create, FileAccess.Write);
			n0154(n0494, fileStream);
			fileStream.Close();
			GC.Collect();
		}

		public void n0514(string n0512)
		{
			if (!n0503)
			{
				throw new ArchiveException();
			}
			int num = n0498.n0515();
			for (int i = 0; i < num; i++)
			{
				string n536;
				n0498.n0510(i, out n536);
				n0154(n536, n0512);
			}
		}

		public MemoryStream n0137(string n0494)
		{
			if (!n0503)
			{
				throw new ArchiveException();
			}
			MemoryStream memoryStream = new MemoryStream();
			n0154(n0494, memoryStream);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		public void n0516(Stream n0509)
		{
			if (!n0503)
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
			string text = n0498.n0517();
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
					array[i] = (byte)n0504[i];
				}
				for (int j = 0; j < 8; j++)
				{
					array2[j] = (byte)n0511[j];
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
			n0498.n0518(num2, out n536);
			try
			{
				n0497.Position = n536;
				int k;
				for (k = 0; k + 1024 < num2; k += 1024)
				{
					memoryStream.Read(buffer, 0, 1024);
					n0497.Write(buffer, 0, 1024);
				}
				memoryStream.Read(buffer, 0, num2 - k);
				n0497.Write(buffer, 0, num2 - k);
				n0497.Flush();
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

		public void n0516(string n0512)
		{
			if (!n0503)
			{
				throw new ArchiveException();
			}
			string text = n0498.n0517();
			FileStream fileStream = new FileStream(n0512 + "\\" + text + "." + n0498.n0513(), FileMode.Open, FileAccess.Read);
			n0516(fileStream);
			fileStream.Close();
			GC.Collect();
		}

		public void n0519(string n0512, string n0520)
		{
			if (!n0503)
			{
				throw new ArchiveException();
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(n0512);
			FileInfo[] files = directoryInfo.GetFiles("*." + n0520);
			int num = files.Length;
			n0498 = new ArchiveIndex();
			for (int i = 0; i < num; i++)
			{
				n0498.n0521(files[i].Name.Substring(0, files[i].Name.Length - files[i].Extension.Length));
			}
			n0498.n0522(n0520);
			n0507();
			for (int j = 0; j < num; j++)
			{
				n0516(n0512);
			}
			n0507();
		}
	}
}
