using System;
using System.Collections;
using System.IO;

namespace n0001
{
	public class ArchiveIndex
	{
		private string n0529;

		private ArrayList n0524;

		private ArrayList n0525;

		private ArrayList n0526;

		private bool n0530;

		private int n0527;

		public ArchiveIndex()
		{
			n0524 = new ArrayList();
			n0525 = new ArrayList();
			n0526 = new ArrayList();
			n0527 = 0;
		}

		public void n0500(Stream n0528)
		{
			byte[] array = new byte[8];
			BinaryReader binaryReader = new BinaryReader(n0528);
			binaryReader.BaseStream.Position = 0L;
			int num = binaryReader.ReadInt32();
			n0529 = "";
			array = binaryReader.ReadBytes(4);
			for (int i = 0; i < 3; i++)
			{
				n0529 += (char)array[i];
			}
			for (int j = 0; j < num; j++)
			{
				string text = "";
				array = binaryReader.ReadBytes(8);
				for (int k = 0; k < 8 && array[k] != 0; k++)
				{
					text += (char)array[k];
				}
				int num2 = binaryReader.ReadInt32();
				int num3 = binaryReader.ReadInt32();
				n0524.Add(text);
				n0526.Add(num2);
				n0525.Add(num3);
			}
		}

		public void n0508(Stream n0528)
		{
			int num = n0515();
			byte[] array = new byte[8];
			BinaryWriter binaryWriter = new BinaryWriter(n0528);
			binaryWriter.BaseStream.Position = 0L;
			binaryWriter.Write(num);
			for (int i = 0; i < 3; i++)
			{
				array[i] = (byte)n0529[i];
			}
			binaryWriter.Write(array, 0, 4);
			for (int j = 0; j < num; j++)
			{
				string text = (string)n0524[j];
				int value = (int)n0526[j];
				int value2 = (int)n0525[j];
				for (int k = 0; k < 8; k++)
				{
					if (k < text.Length)
					{
						array[k] = (byte)text[k];
					}
					else
					{
						array[k] = 0;
					}
				}
				binaryWriter.Write(array);
				binaryWriter.Write(value);
				binaryWriter.Write(value2);
			}
			binaryWriter.Flush();
		}

		public void n0521(string n0494)
		{
			if (n0530)
			{
				throw new AudioException();
			}
			n0524.Add(n0494);
			n0526.Add(0);
			n0525.Add(0);
		}

		public void n0532(string n0494)
		{
			if (n0530)
			{
				throw new AudioException();
			}
			int num = n0524.IndexOf(n0494);
			if (num == -1)
			{
				throw new FileNotFoundException(n0494 + " is not in the Package");
			}
			n0524.RemoveAt(num);
			n0526.RemoveAt(num);
			n0525.RemoveAt(num);
		}

		public void n0518(int n0533, out int n0534)
		{
			if (!n0530)
			{
				throw new ScriptException();
			}
			if (n0527 >= n0524.Count)
			{
				throw new IndexOutOfRangeException("End of the List");
			}
			n0526[n0527] = n0533;
			if (n0527 == 0)
			{
				n0525[n0527] = n0536();
			}
			else
			{
				n0525[n0527] = (int)n0525[n0527 - 1] + (int)n0526[n0527 - 1];
			}
			n0534 = (int)n0525[n0527];
			n0527++;
		}

		public string n0517()
		{
			if (!n0530)
			{
				throw new ScriptException();
			}
			if (n0527 >= n0524.Count)
			{
				throw new IndexOutOfRangeException("End of the List");
			}
			return (string)n0524[n0527];
		}

		public void n0510(string n0494, out int n0534, out int n0533)
		{
			int num = n0524.IndexOf(n0494);
			if (num == -1)
			{
				n0534 = (n0533 = -1);
				return;
			}
			n0534 = (int)n0525[num];
			n0533 = (int)n0526[num];
		}

		public void n0510(string n0494, out int n0309)
		{
			n0309 = n0524.IndexOf(n0494);
		}

		public void n0510(int n0309, out string n0494)
		{
			if (n0309 >= n0524.Count || n0309 < 0)
			{
				throw new ArgumentException("Invalid Index");
			}
			n0494 = (string)n0524[n0309];
		}

		public void n0501()
		{
			n0530 = true;
		}

		public int n0515()
		{
			return n0524.Count;
		}

		public int n0536()
		{
			return n0524.Count * 16 + 8;
		}

		public void n0522(string n0049)
		{
			n0529 = n0049;
		}

		public string n0513()
		{
			return n0529;
		}
	}
}
