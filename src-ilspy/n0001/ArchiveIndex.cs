using System;
using System.Collections;
using System.IO;

namespace Yukihota
{
	public class ArchiveIndex
	{
		private string extension;

		private ArrayList names;

		private ArrayList offsets;

		private ArrayList lengths;

		private bool locked;

		private int cursor;

		public ArchiveIndex()
		{
			names = new ArrayList();
			offsets = new ArrayList();
			lengths = new ArrayList();
			cursor = 0;
		}

		public void Load(Stream stream)
		{
			byte[] array = new byte[8];
			BinaryReader binaryReader = new BinaryReader(stream);
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
				names.Add(text);
				lengths.Add(num2);
				offsets.Add(num3);
			}
		}

		public void Save(Stream stream)
		{
			int num = GetCount();
			byte[] array = new byte[8];
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			binaryWriter.BaseStream.Position = 0L;
			binaryWriter.Write(num);
			for (int i = 0; i < 3; i++)
			{
				array[i] = (byte)extension[i];
			}
			binaryWriter.Write(array, 0, 4);
			for (int j = 0; j < num; j++)
			{
				string text = (string)names[j];
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

		public void AddEntry(string name)
		{
			if (locked)
			{
				throw new AudioException();
			}
			names.Add(name);
			lengths.Add(0);
			offsets.Add(0);
		}

		public void RemoveEntry(string name)
		{
			if (locked)
			{
				throw new AudioException();
			}
			int num = names.IndexOf(name);
			if (num == -1)
			{
				throw new FileNotFoundException(name + " is not in the Package");
			}
			names.RemoveAt(num);
			lengths.RemoveAt(num);
			offsets.RemoveAt(num);
		}

		public void AppendEntrySize(int length, out int offset)
		{
			if (!locked)
			{
				throw new ScriptException();
			}
			if (cursor >= names.Count)
			{
				throw new IndexOutOfRangeException("End of the List");
			}
			lengths[cursor] = length;
			if (cursor == 0)
			{
				offsets[cursor] = GetHeaderSize();
			}
			else
			{
				offsets[cursor] = (int)offsets[cursor - 1] + (int)lengths[cursor - 1];
			}
			offset = (int)offsets[cursor];
			cursor++;
		}

		public string GetCurrentName()
		{
			if (!locked)
			{
				throw new ScriptException();
			}
			if (cursor >= names.Count)
			{
				throw new IndexOutOfRangeException("End of the List");
			}
			return (string)names[cursor];
		}

		public void FindEntry(string name, out int offset, out int length)
		{
			int num = names.IndexOf(name);
			if (num == -1)
			{
				offset = (length = -1);
				return;
			}
			offset = (int)offsets[num];
			length = (int)lengths[num];
		}

		public void FindEntry(string name, out int index)
		{
			index = names.IndexOf(name);
		}

		public void FindEntry(int index, out string name)
		{
			if (index >= names.Count || index < 0)
			{
				throw new ArgumentException("Invalid Index");
			}
			name = (string)names[index];
		}

		public void Lock()
		{
			locked = true;
		}

		public int GetCount()
		{
			return names.Count;
		}

		public int GetHeaderSize()
		{
			return names.Count * 16 + 8;
		}

		public void SetExtension(string extensionName)
		{
			extension = extensionName;
		}

		public string GetExtension()
		{
			return extension;
		}
	}
}
