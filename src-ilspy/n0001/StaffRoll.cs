using System;
using System.Drawing;
using System.Xml;
using Microsoft.DirectX.Direct3D;
using n0062;

namespace n0001
{
	public class StaffRoll : IDisposable
	{
		protected const int n0315 = 24;

		protected const int n0556 = 15;

		private MediaPackage n0067;

		private ArchiveFile n0482;

		private ArchiveFile n0153;

		private XmlNode[] n0545;

		private XmlNode[] n0546 = new XmlNode[3];

		private XmlNode[] n0547 = new XmlNode[3];

		private Texture n0550;

		private ImageInformation n0555;

		private Texture n0551;

		private Device n0006;

		private Sprite n0144;

		private FrameCallback n0552;

		private Microsoft.DirectX.Direct3D.Font n0362;

		private ButtonLayout n0363;

		protected Texture n0135;

		protected readonly string[,] n0537;

		protected readonly string[] n0538;

		protected readonly string[,,] n0539;

		protected readonly string[] n0540;

		protected readonly Rectangle n0541;

		protected readonly Point n0542;

		protected readonly Point n0543;

		protected readonly Size n0544;

		private int n0548;

		private bool n0549;

		private int n0553;

		private int n0554;

		public StaffRoll(string n0387, ArchiveFile n0390, ArchiveFile n0385, XmlNode[] n0348)
		{
			n0537 = new string[3, 2]
			{
				{ "Ｍｅｍｏｒｉｅｓ\u3000Ｏｆｆ\u3000２ｎｄ～雪萤～", "ＣＡＳＴ" },
				{ "Ｍｅｍｏｒｉｅｓ\u3000Ｏｆｆ\u3000２ｎｄ～雪螢～", "ＣＡＳＴ" },
				{ "Ｍｅｍｏｒｉｅｓ\u3000Ｏｆｆ\u3000２ｎｄ～雪蛍～", "ＣＡＳＴ" }
			};
			n0538 = new string[6] { "SH01CMA", "IK01CMA", "TT01CMA", "SS01AMA", "NS01CMA", "IS01AMA" };
			n0539 = new string[3, 6, 2]
			{
				{
					{ "白河 萤", "水树 奈奈" },
					{ "伊波 健", "小尾 元政" },
					{ "飞世 巴", "仲西 环" },
					{ "白河 静流", "菊池 志穗" },
					{ "中森 翔太", "福山 润" },
					{ "稻穗 信", "间岛 淳司" }
				},
				{
					{ "白河 螢", "水樹 奈奈" },
					{ "伊波 健", "小尾 元政" },
					{ "飛世 巴", "仲西 環" },
					{ "白河 靜流", "菊池 志穗" },
					{ "中森 翔太", "福山 潤" },
					{ "稻穗 信", "間島 淳司" }
				},
				{
					{ "白河 ほたる", "水樹 奈々" },
					{ "伊波 健", "小尾 元政" },
					{ "飛世 巴", "仲西 環" },
					{ "白河 静流", "菊池 志穂" },
					{ "中森 翔太", "福山 潤" },
					{ "稲穂 信", "間島 淳司" }
				}
			};
			n0540 = new string[7] { "EV_HP07B", "EV_HP01A", "EV_HP02A", "EV_HP03A", "EV_HP04A", "EV_HP06A", "EV_HP07A" };
			n0541 = new Rectangle(0, 0, 300, 225);
			n0542 = new Point(1200, 480);
			n0543 = new Point(0, 560);
			n0544 = new Size(30, 40);
			n0067 = new MediaPackage("ED");
			n0067.Open(n0387);
			n0482 = n0390;
			n0153 = n0385;
			n0545 = n0348;
			for (int i = 0; i < 3; i++)
			{
				n0546[i] = n0545[i].SelectSingleNode("ed/staff");
				n0547[i] = n0545[i].SelectSingleNode("ed/music");
			}
			n0548 = n0546[0].ChildNodes.Count + 15;
			n0549 = true;
			n0550 = null;
			n0551 = null;
			n0550 = null;
		}

		public void Initialize(Device n0349, Sprite n0350, FrameCallback n0225, Texture n0351, System.Drawing.Font n0353, ButtonLayout n0046)
		{
			n0006 = n0349;
			n0144 = n0350;
			n0552 = n0225;
			n0135 = n0351;
			n0362 = new Microsoft.DirectX.Direct3D.Font(n0006, new System.Drawing.Font(n0353.FontFamily, 24f));
			n0363 = n0046;
		}

		public bool Update()
		{
			if (n0549)
			{
				n0067.Play(false);
				n0549 = false;
				n0553 = -1;
				n0554 = -1;
				if (n0550 != null)
				{
					n0550.Dispose();
				}
				if (n0551 != null)
				{
					n0551.Dispose();
				}
				n0550 = null;
				n0551 = null;
			}
			bool result = true;
			n0006.Clear(ClearFlags.Target, Color.Black, 1f, 0);
			n0006.BeginScene();
			n0144.Begin(SpriteFlags.AlphaBlend);
			double num = (double)n0067.n0269() / 1000.0;
			if (num < 8.0)
			{
				if (n0551 != null)
				{
					n0551.Dispose();
				}
				if (n0550 != null)
				{
					n0550.Dispose();
				}
				int num2 = (int)num / 4;
				double num3 = num - (double)(num2 * 4);
				int num4;
				if (num3 < 1.0)
				{
					num4 = (int)(num3 * 255.0);
				}
				else
				{
					num4 = ((!(num3 < 3.0)) ? ((int)((4.0 - num3) * 255.0)) : 255);
				}
				if (num4 == 255)
				{
					n0362.DrawText(n0144, n0537[(int)n0363, num2], 400 - n0537[(int)n0363, num2].Length * 15, 280, Color.White);
				}
				else
				{
					n0362.DrawText(n0144, n0537[(int)n0363, num2], 400 - n0537[(int)n0363, num2].Length * 15, 280, Color.FromArgb(num4, Color.White));
				}
			}
			else if (num < 38.0)
			{
				if (n0551 != null)
				{
					n0551.Dispose();
				}
				int num5 = (int)(num - 8.0) / 5;
				double num6 = num - 8.0 - (double)(num5 * 5);
				if (n0553 != num5)
				{
					if (n0550 != null)
					{
						n0550.Dispose();
					}
					n0555 = TextureLoader.ImageInformationFromStream(n0482.ReadEntry(n0538[num5]));
					n0550 = TextureLoader.FromStream(n0006, n0482.ReadEntry(n0538[num5]), n0555.Width, n0555.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
					n0553 = num5;
				}
				if (num6 < 0.5)
				{
				}
				int num7;
				if (num6 < 1.5)
				{
					num7 = (int)((num6 - 0.5) * 255.0);
				}
				else
				{
					num7 = ((num6 < 3.5) ? 255 : ((num6 < 4.5) ? ((int)((4.5 - num6) * 255.0)) : 0));
				}
				if (num7 > 0)
				{
					if (num7 == 255)
					{
						n0144.Draw2D(n0550, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, new Point(250 - n0555.Width / 2, 600 - n0555.Height), Color.White);
						n0362.DrawText(n0144, n0539[(int)n0363, num5, 0], 505 - (int)((double)n0539[(int)n0363, num5, 0].Length - 0.5) * 15, 260, Color.White);
						n0362.DrawText(n0144, n0539[(int)n0363, num5, 1], 505 - (int)((double)n0539[(int)n0363, num5, 1].Length - 0.5) * 15, 300, Color.White);
					}
					else
					{
						n0144.Draw2D(n0550, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, new Point(250 - n0555.Width / 2, 600 - n0555.Height), Color.FromArgb(num7, Color.White));
						n0362.DrawText(n0144, n0539[(int)n0363, num5, 0], 505 - (int)((double)n0539[(int)n0363, num5, 0].Length - 0.5) * 15, 260, Color.FromArgb(num7, Color.White));
						n0362.DrawText(n0144, n0539[(int)n0363, num5, 1], 505 - (int)((double)n0539[(int)n0363, num5, 1].Length - 0.5) * 15, 300, Color.FromArgb(num7, Color.White));
					}
				}
			}
			else if (num < 171.0)
			{
				int num8 = (int)(num - 38.0) / 19;
				double num9 = num - 38.0 - (double)(num8 * 19);
				if (n0554 != num8)
				{
					if (n0551 != null)
					{
						n0551.Dispose();
					}
					n0555 = TextureLoader.ImageInformationFromStream(n0153.ReadEntry(n0540[num8]));
					n0551 = TextureLoader.FromStream(n0006, n0153.ReadEntry(n0540[num8]), n0555.Width, n0555.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
					n0554 = num8;
				}
				if (n0550 != null)
				{
					n0550.Dispose();
				}
				if (num9 < 0.5)
				{
				}
				int num10;
				if (num9 < 2.5)
				{
					num10 = (int)((num9 - 0.5) / 2.0 * 255.0);
				}
				else
				{
					num10 = ((num9 < 16.5) ? 255 : ((num9 < 18.5) ? ((int)((18.5 - num9) / 2.0 * 255.0)) : 0));
				}
				if (num10 > 0)
				{
					if (num10 == 255)
					{
						n0144.Draw2D(n0551, Rectangle.Empty, n0541, Point.Empty, 0f, n0542, Color.White);
					}
					else
					{
						n0144.Draw2D(n0551, Rectangle.Empty, n0541, Point.Empty, 0f, n0542, Color.FromArgb(num10, Color.White));
					}
					n0144.Draw2D(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, Point.Empty, Color.Transparent);
				}
				if (num < 163.0)
				{
					num9 = num - 38.0;
					double num11 = num9 / 125.0 * (double)n0548;
					double num12 = num11 - 15.0;
					for (int i = (int)num12; i <= (int)num11; i++)
					{
						if (i >= 0 && i < n0548 - 15 && n0546[0].ChildNodes[i].HasChildNodes)
						{
							n0362.DrawText(n0144, n0546[(int)n0363].ChildNodes[i].FirstChild.Value, 92, (int)(((double)i - num12) * 40.0) + 2, Color.Black);
							n0362.DrawText(n0144, n0546[(int)n0363].ChildNodes[i].FirstChild.Value, 90, (int)(((double)i - num12) * 40.0), Color.White);
						}
					}
				}
			}
			else if (num * 1000.0 < (double)n0067.n0270())
			{
				if (n0550 != null)
				{
					n0550.Dispose();
				}
				if (n0551 != null)
				{
					n0551.Dispose();
				}
			}
			else
			{
				if (n0550 != null)
				{
					n0550.Dispose();
				}
				if (n0551 != null)
				{
					n0551.Dispose();
				}
				n0067.n0162();
				result = false;
			}
			n0144.Draw2D(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, n0543, Color.Black);
			for (int j = 0; j < n0547[(int)n0363].ChildNodes.Count - 1; j++)
			{
				double num13 = double.Parse(n0547[(int)n0363].ChildNodes[j].Attributes["start"].Value);
				double num14 = double.Parse(n0547[(int)n0363].ChildNodes[j].Attributes["end"].Value);
				double num15 = double.Parse(n0547[(int)n0363].ChildNodes[j + 1].Attributes["start"].Value);
				if (!(num > num13) || !(num < num15))
				{
					continue;
				}
				string value = n0547[(int)n0363].ChildNodes[j].FirstChild.Value;
				for (int k = 0; k < value.Length; k++)
				{
					n0362.DrawText(n0144, value[k].ToString(), 400 - value.Length * 15 + k * 30 + 2, 562, Color.White);
				}
				if (num < num14)
				{
					double num16 = (num - num13) / (num14 - num13) * (double)value.Length;
					for (int l = 0; l < (int)num16; l++)
					{
						n0362.DrawText(n0144, value[l].ToString(), 400 - value.Length * 15 + l * 30, 560, Color.Yellow);
					}
					n0362.DrawText(n0144, value[(int)num16].ToString(), 400 - value.Length * 15 + (int)num16 * 30, 560, Color.FromArgb((int)((num16 - (double)(int)num16) * 255.0), Color.Yellow));
				}
				else
				{
					for (int m = 0; m < value.Length; m++)
					{
						n0362.DrawText(n0144, value[m].ToString(), 400 - value.Length * 15 + m * 30, 560, Color.Yellow);
					}
				}
				break;
			}
			n0144.End();
			n0006.EndScene();
			try
			{
				n0006.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					n0006.TestCooperativeLevel();
				}
				catch (DeviceNotResetException)
				{
					n0552();
				}
				catch
				{
				}
			}
			return result;
		}

		public void n0230()
		{
			n0067.n0162();
			if (n0550 != null)
			{
				n0550.Dispose();
			}
			if (n0551 != null)
			{
				n0551.Dispose();
			}
		}

		public void SetPaused(bool n0241)
		{
			if (n0241)
			{
				n0067.SetPaused();
			}
			else
			{
				n0067.n0165();
			}
		}

		public void SetButtonLayout(ButtonLayout n0046)
		{
			n0363 = n0046;
		}

		public void Dispose()
		{
			n0067.n0162();
			GC.SuppressFinalize(this);
		}
	}
}
