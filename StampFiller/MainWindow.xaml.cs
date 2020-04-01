using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace StampFiller
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			//StampTemplate = GetStampTemplate(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"Pos Service Holland B.V\Purchase - Documenten\General\Patryk\stamp.png"));
			AccountNumber = "TestTest";
		}

		private TransformedBitmap GetFilledStamp()
		{
			Bitmap Template = Properties.Resources.stamp;
			//Template.MakeTransparent(Color.White);
			WriteText(Template);

			var InitialImage = BitmapToBitmapImage(Template);

			var FinalImage = new TransformedBitmap(InitialImage, new System.Windows.Media.ScaleTransform(0.3, 0.3));
			return FinalImage;
		}

		private void WriteText(Bitmap Template)
		{
			Graphics g = Graphics.FromImage(Template);
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

			int x = 405;
			int[] y = { 120, 230, 345, 455, 565, 680 };
			string[] texts = { AccountNumber, OrderNumber, InvoiceNumber, DueDate, ApprovedBy, ShipmentNumber };

			for (int i = 0; i < y.Length; i++)
			{
				RectangleF rectangle = new RectangleF(new PointF(x, y[i]), new System.Drawing.Size(900, 100));
				StringFormat format = new StringFormat()
				{
					Alignment = StringAlignment.Near,
					LineAlignment = StringAlignment.Near
				};

				g.DrawString(texts[i], new Font("Tahoma", 48), Brushes.Black, rectangle, format);
			}

			g.Flush();
		}

		private static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
		{
			using (var memory = new MemoryStream())
			{
				bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
				memory.Position = 0;

				var bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.StreamSource = memory;
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.EndInit();
				bitmapImage.Freeze();

				return bitmapImage;
			}
		}

		//private FormattedText GetFormattedText(string text)
		//{
		//	if (text == null)
		//	{
		//		text = "";
		//	}
		//	return new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 26, Brushes.Black);
		//}

		private TransformedBitmap filledStamp;
		public TransformedBitmap FilledStamp
		{
			get { return filledStamp; }
			set
			{
				if (filledStamp != value)
				{
					filledStamp = value;
					OnPropertyChanged();
				}
			}
		}

		private string accountNumber;
		public string AccountNumber
		{
			get { return accountNumber; }
			set
			{
				if (accountNumber != value)
				{
					accountNumber = value;
					OnPropertyChanged();
				}
			}
		}

		private string orderNumber;
		public string OrderNumber
		{
			get { return orderNumber; }
			set
			{
				if (orderNumber != value)
				{
					orderNumber = value;
					OnPropertyChanged();
				}
			}
		}

		private string invoiceNumber;
		public string InvoiceNumber
		{
			get { return invoiceNumber; }
			set
			{
				if (invoiceNumber != value)
				{
					invoiceNumber = value;
					OnPropertyChanged();
				}
			}
		}

		private string dueDate;
		public string DueDate
		{
			get { return dueDate; }
			set
			{
				if (dueDate != value)
				{
					dueDate = value;
					OnPropertyChanged();
				}
			}
		}

		private string shipmentNumber;
		public string ShipmentNumber
		{
			get { return shipmentNumber; }
			set
			{
				if (shipmentNumber != value)
				{
					shipmentNumber = value;
					OnPropertyChanged();
				}
			}
		}

		private string approvedBy;
		public string ApprovedBy
		{
			get { return approvedBy; }
			set
			{
				if (approvedBy != value)
				{
					approvedBy = value;
					OnPropertyChanged();
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		private void CopyImageToClipboard(object sender, RoutedEventArgs e)
		{
			if (FilledStamp != null)
			{
				Clipboard.SetImage(FilledStamp);
			}
			else
			{
				MessageBox.Show("No picture to copy!", MessageBoxImage.Error.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ClearStamp(object sender, RoutedEventArgs e)
		{
			AccountNumber = "";
			OrderNumber = "";
			InvoiceNumber = "";
			DueDate = "";
			ShipmentNumber = "";
		}

		private void RefreshStamp(object sender, RoutedEventArgs e)
		{
			var tempStamp = GetFilledStamp();
			if (tempStamp != null)
			{
				FilledStamp = tempStamp;
			}
		}

		//private void SaveToFile(object sender, RoutedEventArgs e)
		//{
		//}
	}
}
