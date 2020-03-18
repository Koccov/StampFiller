using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
			StampTemplate = GetStampTemplate(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"Pos Service Holland B.V\Purchase - Documenten\General\Patryk\stamp.png"));
			if (StampTemplate == null)
			{
				MessageBox.Show("No stamp.png template found!\nImport manually by clicking \"Import stamp template\"");
			}
		}

		private void ImportStampTemplate()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "*.png|*.png";
			if (openFileDialog.ShowDialog() == true)
			{
				StampTemplate = GetStampTemplate(openFileDialog.FileName);
			}
		}

		private BitmapImage GetStampTemplate(string FilePath)
		{
			if (File.Exists(FilePath))
			{
				using (var stream = new FileStream(FilePath, FileMode.Open))
				{
					var image = new BitmapImage();
					image.BeginInit();
					image.CacheOption = BitmapCacheOption.OnLoad;
					image.StreamSource = stream;
					image.EndInit();
					image.Freeze();
					return image;
				}
			}
			else return null;
		}

		private RenderTargetBitmap GetFilledStamp()
		{
			var visual = new DrawingVisual();
			using (DrawingContext drawingContext = visual.RenderOpen())
			{
				drawingContext.DrawImage(stampTemplate, new Rect(0, 0, 412, 256));
				drawingContext.DrawText(GetFormattedText(AccountNumber), new Point(140, 35));
				drawingContext.DrawText(GetFormattedText(OrderNumber), new Point(140, 72));
				drawingContext.DrawText(GetFormattedText(InvoiceNumber), new Point(140, 108));
				drawingContext.DrawText(GetFormattedText(DueDate), new Point(140, 145));
				drawingContext.DrawText(GetFormattedText(ApprovedBy), new Point(140, 180));
				drawingContext.DrawText(GetFormattedText(ShipmentNumber), new Point(140, 217));
			}
			RenderTargetBitmap bitmap = new RenderTargetBitmap(412, 256, 96, 96, PixelFormats.Pbgra32);
			bitmap.Render(visual);


			return bitmap;
		}

		private FormattedText GetFormattedText(string text)
		{
			if (text == null)
			{
				text = "";
			}
			return new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 26, Brushes.Black);
		}

		private BitmapImage stampTemplate;
		public BitmapImage StampTemplate
		{
			get { return stampTemplate; }
			set
			{
				if (stampTemplate != value)
				{
					stampTemplate = value;
					OnPropertyChanged();
				}
			}
		}

		private RenderTargetBitmap filledStamp;
		public RenderTargetBitmap FilledStamp
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

		private void RefreshStamp(object sender, RoutedEventArgs e)
		{
			var tempStamp = GetFilledStamp();
			if (tempStamp != null)
			{
				FilledStamp = GetFilledStamp();
			}
		}

		private void SaveToFile(object sender, RoutedEventArgs e)
		{
		}

		private void ImportButton(object sender, RoutedEventArgs e)
		{
			ImportStampTemplate();
		}
	}
}
