using System;
using System.Collections;
using System.IO;

namespace Yukihota
{
	// Token: 0x0200002C RID: 44
	public class ArchiveIndex
	{
		// Token: 0x06001F6C RID: 8044 RVA: 0x0001A890 File Offset: 0x00019890
		public ArchiveIndex()
		{
			this.names = new ArrayList();
			this.offsets = new ArrayList();
			this.lengths = new ArrayList();
			this.cursor = 0;
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x0001A8C0 File Offset: 0x000198C0
		public void Load(Stream stream)
		{
			byte[] array = new byte[8];
			BinaryReader binaryReader = new BinaryReader(stream);
			binaryReader.BaseStream.Position = 0L;
			int num = binaryReader.ReadInt32();
			this.extension = "";
			array = binaryReader.ReadBytes(4);
			for (int i = 0; i < 3; i++)
			{
				this.extension += (char)array[i];
			}
			int j = 0;
			while (j < num)
			{
				string text = "";
				array = binaryReader.ReadBytes(8);
				int num2 = 0;
				while (num2 < 8 && array[num2] != 0)
				{
					text += (char)array[num2];
					num2++;
				}
				int num3 = binaryReader.ReadInt32();
				int num4 = binaryReader.ReadInt32();
				this.names.Add(text);
				this.lengths.Add(num3);
				this.offsets.Add(num4);
				int num5 = j++;
			}
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x0001A9D0 File Offset: 0x000199D0
		public void Save(Stream stream)
		{
			int entryCount = this.GetCount();
			byte[] array = new byte[8];
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			binaryWriter.BaseStream.Position = 0L;
			binaryWriter.Write(entryCount);
			for (int i = 0; i < 3; i++)
			{
				array[i] = (byte)this.extension[i];
			}
			binaryWriter.Write(array, 0, 4);
			for (int j = 0; j < entryCount; j++)
			{
				string text = (string)this.names[j];
				int num = (int)this.lengths[j];
				int num2 = (int)this.offsets[j];
				int k = 0;
				while (k < 8)
				{
					if (k < text.Length)
					{
						array[k] = (byte)text[k];
					}
					else
					{
						array[k] = 0;
					}
					int num3 = k++;
				}
				binaryWriter.Write(array);
				binaryWriter.Write(num);
				binaryWriter.Write(num2);
			}
			binaryWriter.Flush();
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x0001AAEC File Offset: 0x00019AEC
		public void AddEntry(string name)
		{
			if (this.locked)
			{
				throw new ScriptException();
			}
			this.names.Add(name);
			this.lengths.Add(0);
			this.offsets.Add(0);
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x0001AB38 File Offset: 0x00019B38
		public void RemoveEntry(string name)
		{
			if (this.locked)
			{
				throw new ScriptException();
			}
			int num = this.names.IndexOf(name);
			if (num == -1)
			{
				throw new FileNotFoundException(name + " is not in the Package");
			}
			this.names.RemoveAt(num);
			this.lengths.RemoveAt(num);
			this.offsets.RemoveAt(num);
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x0001AB9C File Offset: 0x00019B9C
		public void AppendEntrySize(int length, out int offset)
		{
			if (!this.locked)
			{
				throw new AudioException();
			}
			if (this.cursor >= this.names.Count)
			{
				throw new IndexOutOfRangeException("End of the List");
			}
			this.lengths[this.cursor] = length;
			if (this.cursor == 0)
			{
				this.offsets[this.cursor] = this.GetHeaderSize();
			}
			else
			{
				this.offsets[this.cursor] = (int)this.offsets[this.cursor - 1] + (int)this.lengths[this.cursor - 1];
			}
			offset = (int)this.offsets[this.cursor];
			this.cursor++;
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x0001ACA0 File Offset: 0x00019CA0
		public string GetCurrentName()
		{
			if (!this.locked)
			{
				throw new AudioException();
			}
			if (this.cursor >= this.names.Count)
			{
				throw new IndexOutOfRangeException("End of the List");
			}
			return (string)this.names[this.cursor];
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x0001ACF0 File Offset: 0x00019CF0
		public void FindEntry(string name, out int offset, out int length)
		{
			int num = this.names.IndexOf(name);
			if (num == -1)
			{
				offset = (length = -1);
				return;
			}
			offset = (int)this.offsets[num];
			length = (int)this.lengths[num];
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x0001AD40 File Offset: 0x00019D40
		public void FindEntry(string name, out int index)
		{
			index = this.names.IndexOf(name);
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x0001AD50 File Offset: 0x00019D50
		public void FindEntry(int index, out string name)
		{
			if (index >= this.names.Count || index < 0)
			{
				throw new ArgumentException("Invalid Index");
			}
			name = (string)this.names[index];
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x0001AD84 File Offset: 0x00019D84
		public void Lock()
		{
			this.locked = true;
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x0001AD90 File Offset: 0x00019D90
		public int GetCount()
		{
			return this.names.Count;
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x0001ADA0 File Offset: 0x00019DA0
		public int GetHeaderSize()
		{
			return this.names.Count * 16 + 8;
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x0001ADB4 File Offset: 0x00019DB4
		public void SetExtension(string extensionName)
		{
			this.extension = extensionName;
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x0001ADC0 File Offset: 0x00019DC0
		public string GetExtension()
		{
			return this.extension;
		}

		// Token: 0x0400013E RID: 318
		private string extension;

		// Token: 0x0400013F RID: 319
		private ArrayList names;

		// Token: 0x04000140 RID: 320
		private ArrayList offsets;

		// Token: 0x04000141 RID: 321
		private ArrayList lengths;

		// Token: 0x04000142 RID: 322
		private bool locked;

		// Token: 0x04000143 RID: 323
		private int cursor;
	}
}
