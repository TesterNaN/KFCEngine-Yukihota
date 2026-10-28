using System;
using System.Collections;
using System.IO;

namespace n0001
{
	public class ArchiveIndex
	{
		private string extension;

		private ArrayList n0524;

		private ArrayList offsets;

		private ArrayList lengths;

		private bool locked;

		private int cursor;

		public ArchiveIndex()
		{
			n0524 = new ArrayList();
			offsets = new ArrayList();
			lengths = new ArrayList();
			cursor = 0;
		}

		public void n0500(Stream n0528)
		{
			byte[] array = new byte[8];
			BinaryReader binaryReader = new BinaryReader(n0528);
			binaryReader.BaseStream.Position = 0L;
			int num = binaryReader.ReadInt32();
			extension = "";
			array = binaryReader.ReadBytes(4);
			for (int i = 0; i < 3; i++)
			{
				extension += (char)array[i];
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
				lengths.Add(num2);
				offsets.Add(num3);
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
				array[i] = (byte)extension[i];
			}
			binaryWriter.Write(array, 0, 4);
			for (int j = 0; j < num; j++)
			{
				string text = (string)n0524[j];
				int value = (int)lengths[j];
				int value2 = (int)offsets[j];
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
			if (locked)
			{
				throw new AudioException();
			}
			n0524.Add(n0494);
			lengths.Add(0);
			offsets.Add(0);
		}

		public void n0532(string n0494)
		{
			if (locked)
			{
				throw new AudioException();
			}
			int num = n0524.IndexOf(n0494);
			if (num == -1)
			{
				throw new FileNotFoundException(n0494 + " is not in the Package");
			}
			n0524.RemoveAt(num);
			lengths.RemoveAt(num);
			offsets.RemoveAt(num);
		}

		public void AppendEntrySize(int n0533, out int n0534)
		{
			if (!locked)
			{
				throw new ScriptException();
			}
			if (cursor >= n0524.Count)
			{
				throw new IndexOutOfRangeException("End of the List");
			}
			lengths[cursor] = n0533;
			if (cursor == 0)
			{
				offsets[cursor] = n0536();
			}
			else
			{
				offsets[cursor] = (int)offsets[cursor - 1] + (int)lengths[cursor - 1];
			}
			n0534 = (int)offsets[cursor];
			cursor++;
		}

		public string n0517()
		{
			if (!locked)
			{
				throw new ScriptException();
			}
			if (cursor >= n0524.Count)
			{
				throw new IndexOutOfRangeException("End of the List");
			}
			return (string)n0524[cursor];
		}

		public void FindEntry(string n0494, out int n0534, out int n0533)
		{
			int num = n0524.IndexOf(n0494);
			if (num == -1)
			{
				n0534 = (n0533 = -1);
				return;
			}
			n0534 = (int)offsets[num];
			n0533 = (int)lengths[num];
		}

		public void FindEntry(string n0494, out int index)
		{
			index = n0524.IndexOf(n0494);
		}

		public void FindEntry(int index, out string n0494)
		{
			if (index >= n0524.Count || index < 0)
			{
				throw new ArgumentException("Invalid Index");
			}
			n0494 = (string)n0524[index];
		}

		public void Lock()
		{
			locked = true;
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
			extension = n0049;
		}

		public string n0513()
		{
			return extension;
		}
	}
}
