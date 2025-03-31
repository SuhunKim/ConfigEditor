using APS.WPF.SHEET;
using Kornic.BlockControlFoundation;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using unvell.ReoGrid;

namespace ConfigEditor
{
	/// <summary>
	/// 
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Class constants
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_NOTE_PAD_PATH_32BIT = @"C:\Program Files\Notepad++\notepad++.exe";
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_NOTE_PAD_PATH_64BIT = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_ATTRIBUTE_KEY = "key";
		private const string DEF_ATTRIBUTE_NAME = "plcChannelName";
		private const string DEF_ATTRIBUTE_FORMAT = "format";
		private const string DEF_ATTRIBUTE_TYPE = "type";
		private const string DEF_ATTRIBUTE_UNIT = "unit";
		private const string DEF_ATTRIBUTE_RANGE = "range";
		private const string DEF_ATTRIBUTE_DOT = "dot";
		private const string DEF_ATTRIBUTE_SIGNED = "signed";
		private const string DEF_ATTRIBUTE_LOCAL = "local";
		/// <summary>
		/// 
		/// </summary>
		private const int DEF_COLUMN_KEY = 0;
		private const int DEF_COLUMN_NAME = 1;
		private const int DEF_COLUMN_FORMAT = 2;
		private const int DEF_COLUMN_TYPE = 3;
		private const int DEF_COLUMN_UNIT = 4;
		private const int DEF_COLUMN_RANGE = 5;
		private const int DEF_COLUMN_DOT = 6;
		private const int DEF_COLUMN_SIGN = 7;
		private const int DEF_COLUMN_LOCAL = 8;
		#endregion

		#region Class members

		private Configurator m_configurator;

		#endregion

		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public PLCManager PLCManager
		{
			get
			{
				return m_configurator.PLCManager;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public SvidManager SvidManager
		{
			get
			{
				return m_configurator.SvidManager;
			}
		}
		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();

			m_configurator = new Configurator(this);

			Initialize();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Initialize()
		{
			bool bSuccess = false;
			m_configurator = new Configurator(this);

			Initialize_Sheet();
			UISetSheetBorder(m_spreadUtility);
		}
		/// <summary>
		/// 
		/// </summary>
		private bool Initialize_Sheet()
		{
			try
			{
				var hashSheet = new Dictionary<int, spColDefaultStyle>();

				var sp0 = new spColDefaultStyle(m_spreadUtility.DefaultStyle);
				sp0._dataFormat = unvell.ReoGrid.DataFormat.CellDataFormatFlag.Text;
				sp0._hAlign = unvell.ReoGrid.ReoGridHorAlign.Center;
				sp0._vAlign = unvell.ReoGrid.ReoGridVerAlign.Middle;
				sp0._indent = 2;
				sp0._IsReadOnly = false;
				hashSheet.Add(-1, sp0);

				var sp1 = new spColDefaultStyle(m_spreadUtility.DefaultStyle);
				sp1._dataFormat = unvell.ReoGrid.DataFormat.CellDataFormatFlag.Text;
				sp1._hAlign = unvell.ReoGrid.ReoGridHorAlign.Left;
				sp1._vAlign = unvell.ReoGrid.ReoGridVerAlign.Middle;
				sp1._indent = 2;
				sp1._IsReadOnly = false;
				hashSheet.Add(1, sp1);


				m_spreadUtility.Initialize(hashSheet);
				//m_spreadUtility.RowCount = 0;

				return true;
			}
			catch (Exception ex)
			{
				//LogManager.ErrorWriteLog(ex.ToString());

				return false;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void UISetSheetBorder(APSSheet sheet)
		{
			sheet.SetBorders(0, 0, sheet.RowCount, sheet.ColumnCount, BorderPositions.InsideAll,RangeBorderStyle.SilverSolid);
		}
		#endregion

		#region Class private methods
		#endregion

		#region Class public methods

		#endregion

		#region Class event handler
		private void OnSelectClick(object sender, RoutedEventArgs e)
		{
			try
			{
				CommonOpenFileDialog Dialog = new CommonOpenFileDialog();

				Dialog.InitialDirectory = @"D:";
				Dialog.IsFolderPicker = true;
				Dialog.Title = "Select ConfigFile";

				if (Dialog.ShowDialog() == CommonFileDialogResult.Ok)
				{
					string m_sNewProjectFullPath = Dialog.FileName;

					if (!m_sNewProjectFullPath.Contains("ConfigFile"))
					{
						DirectoryInfo di = new DirectoryInfo(m_sNewProjectFullPath);
						DirectoryInfo sSubdi = di.GetDirectories().FirstOrDefault(x => x.Name.Contains("ConfigFile"));

						if (!(sSubdi is null) && sSubdi.Exists)
						{
							m_sNewProjectFullPath = sSubdi.FullName;
						}
					}

					if (!m_sNewProjectFullPath.Contains("ConfigFile"))
					{
						MessageBox.Show("ConfigFile을 선택해 주세요.");
						return;
					}

					bool bSuccess = m_configurator.Initialize(m_sNewProjectFullPath, "_StartUp.xml");

					if (bSuccess)
					{
						UISheetUtility();
					}
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void OnSaveClick(object sender, RoutedEventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(m_configurator.sConfigPath))
				{
					return;
				}

				//Sheet -> Xml Write
				string sConfigPath = m_configurator.sConfigPath;
				string sXPath = string.Format("{0}\\{1}\\{2}", sConfigPath, m_configurator.sProject, m_configurator.sLine);
				string sXFilePath = string.Format("{0}\\{1}", sXPath, "02_Utility.xml");
				int iBackup = 1;

				if (File.Exists(sXFilePath))
				{
					do
					{
						sXFilePath = string.Format("{0}\\{1}{2}.xml", sXPath, "02_Utility", iBackup++.ToString());

					} while (File.Exists(sXFilePath));

				}
				else
				{

				}

				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				settings.IndentChars = "\t";
				settings.NewLineChars = "\r\n"; // 줄 바꿈 문자 (기본값)
				settings.NewLineOnAttributes = true;

				XmlWriter xmlwriter = XmlWriter.Create(sXFilePath);
				xmlwriter.WriteStartDocument();
				xmlwriter.WriteWhitespace("\n");
				xmlwriter.WriteStartElement("UtilityEntries");

				string slocalCurrent = "";
				for (int i = 0; i < m_spreadUtility.RowCount; i++)
				{
					if (!slocalCurrent.Equals(m_spreadUtility.GetText(i, DEF_COLUMN_LOCAL)))
					{
						slocalCurrent = m_spreadUtility.GetText(i, DEF_COLUMN_LOCAL);
						xmlwriter.WriteWhitespace("\n\n");
					}
					xmlwriter.WriteWhitespace("\n\t");

					xmlwriter.WriteStartElement("Utility");
					xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_KEY, m_spreadUtility.GetText(i, DEF_COLUMN_KEY));
					xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_NAME, m_spreadUtility.GetText(i, DEF_COLUMN_NAME));
					xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_FORMAT, m_spreadUtility.GetText(i, DEF_COLUMN_FORMAT));
					xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_TYPE, m_spreadUtility.GetText(i, DEF_COLUMN_TYPE));
					xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_UNIT, m_spreadUtility.GetText(i, DEF_COLUMN_UNIT));
					xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_RANGE, m_spreadUtility.GetText(i, DEF_COLUMN_RANGE));
					xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_DOT, m_spreadUtility.GetText(i, DEF_COLUMN_DOT));
					xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_SIGNED, m_spreadUtility.GetText(i, DEF_COLUMN_SIGN));
					xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_LOCAL, m_spreadUtility.GetText(i, DEF_COLUMN_LOCAL));

					xmlwriter.WriteEndElement(); 
				}

				xmlwriter.WriteEndDocument();
				xmlwriter.Flush();
				xmlwriter.Close();

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
				return;
			}
		}
		private void OnNotePadClick(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(m_configurator.sConfigPath))
			{
				return;
			}

			string sPath = string.Format("{0}\\{1}\\{2}.txt", m_configurator.sConfigPath, "CIM", "Utility");

			if (!File.Exists(sPath))
			{
				//MessageBoxForm messageForm = new MessageBoxForm(new Point(this.Location.X + 80, this.Location.Y + 180));
				//messageForm.ShowForm("Type is not File or The file doesn't exist.", "Confirm");
				if (File.Exists(DEF_NOTE_PAD_PATH_32BIT))
				{
					System.Diagnostics.Process.Start(DEF_NOTE_PAD_PATH_32BIT);
				}
				else if (File.Exists(DEF_NOTE_PAD_PATH_64BIT))
				{
					System.Diagnostics.Process.Start(DEF_NOTE_PAD_PATH_64BIT);
				}
			}
			else
			{
				System.Diagnostics.Process.Start(sPath);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void UISheetUtility()
		{
			//string[] sKey = new string[SvidManager.hashSvidKeyToData.Count];
			List<string[]> listFDC = new List<string[]>();

			m_spreadUtility.RowCount = SvidManager.hashSvidKeyToData.Count;// sKey.Length;
			int iSheetIndex = 0;

			foreach (var svid in SvidManager.hashSvidKeyToData)
			{
				StringBuilder sb = new StringBuilder();
				string sKey = svid.Key.ToString();
				string sName = svid.Value.sPLC_NAME;
				string sFormat = svid.Value.sFORMAT;
				string sType = svid.Value.sTYPE;
				string sUnit = svid.Value.sUNIT;
				string sRange = svid.Value.sRANGE;
				string sDot = svid.Value.sDOT;
				string sSigned = svid.Value.sSIGNED;
				string sLocal = svid.Value.sLOCAL;

				sb.Append(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t", sKey, sName, sFormat, sType, sUnit, sRange, sDot, sSigned, sLocal));
				m_spreadUtility.SetClipValue(iSheetIndex++, 0, SvidManager.hashSvidKeyToData.Count, 9, sb.ToString());

				listFDC.Add(new string[] { svid.Key.ToString(), svid.Value.sPLC_NAME, svid.Value.sFORMAT, svid.Value.sTYPE, svid.Value.sUNIT, svid.Value.sRANGE,
					svid.Value.sDOT, svid.Value.sSIGNED, svid.Value.sLOCAL});
				//m_spreadUtility.SetClipValue(0, 0, listFDC);

			}
		}
		#endregion

		#region class utility methods
		/// <summary>
		/// 
		/// </summary>
		private void UIWindowMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				DragMove();
			}
		}
		#endregion

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
			{
				OnSelectClick(null, null);
			}
		}
	}
}
